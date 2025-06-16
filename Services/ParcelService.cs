using LaserLogisticsParcelTrackerWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LaserLogisticsParcelTrackerWeb.Services
{
    public class ParcelService
    {
        private readonly string filePath = "Data/parcels.txt";
        private readonly string archiveFilePath = "Data/parcels_archive.txt";
        
        #region Existing Methods (from previous response)
        public Parcel GetParcelByTrackingNumber(string trackingNumber)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber))
                return null;

            var allLines = File.ReadAllLines(filePath);
            var parcelLines = allLines.Where(line => line.StartsWith($"{trackingNumber},")).ToList();

            if (!parcelLines.Any())
                return null;

            var parcel = new Parcel { TrackingNumber = trackingNumber };

            foreach (var line in parcelLines)
            {
                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                var status = parts[1];
                if (DateTime.TryParse(parts[2], out var date))
                {
                    parcel.Updates.Add(new ParcelUpdate
                    {
                        Status = status,
                        Date = date
                    });
                }

                if (parts.Length == 4 && !string.IsNullOrWhiteSpace(parts[3]))
                {
                    parcel.Email = parts[3];
                }
            }

            parcel.Updates = parcel.Updates.OrderByDescending(u => u.Date).ToList();

            return parcel;
        }
        
        public List<Parcel> SearchParcels(string statusFilter = null, DateTime? start = null, DateTime? end = null)
        {
            var parcels = new Dictionary<string, Parcel>();
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                var tracking = parts[0];
                var status = parts[1];
                if (!DateTime.TryParse(parts[2], out var date)) continue;

                if (!parcels.ContainsKey(tracking))
                {
                    parcels[tracking] = new Parcel { TrackingNumber = tracking };
                }

                parcels[tracking].Updates.Add(new ParcelUpdate { Status = status, Date = date });

                if (parts.Length == 4)
                {
                    parcels[tracking].Email = parts[3];
                }
            }

            return parcels.Values
                .Where(p => string.IsNullOrEmpty(statusFilter) || p.Updates.Any(u => u.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase)))
                .Where(p => !start.HasValue || p.Updates.Any(u => u.Date >= start))
                .Where(p => !end.HasValue || p.Updates.Any(u => u.Date <= end))
                .ToList();
        }

        public void AddUpdate(string trackingNumber, string status, string email = null)
        {
            var line = $"{trackingNumber},{status},{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            if (!string.IsNullOrEmpty(email))
            {
                line += $",{email}";
            }

            File.AppendAllLines(filePath, new[] { line });

            if (!string.IsNullOrEmpty(email))
            {
                Console.WriteLine($"[MOCK EMAIL] Sent update to {email}: {status}");
            }
        }

        public void DeleteParcelByTrackingNumber(string trackingNumber)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber)) return;

            var allLines = File.ReadAllLines(filePath).ToList();
            var updatedLines = allLines.Where(line => !line.StartsWith($"{trackingNumber},")).ToList();

            File.WriteAllLines(filePath, updatedLines);
        }

        public void UpdateParcelEmail(string trackingNumber, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber) || string.IsNullOrWhiteSpace(newEmail)) return;

            var allLines = File.ReadAllLines(filePath);
            var updatedLines = new List<string>();

            foreach (var line in allLines)
            {
                var parts = line.Split(',');
                if (parts.Length > 0 && parts[0] == trackingNumber)
                {
                    updatedLines.Add($"{parts[0]},{parts[1]},{parts[2]},{newEmail}");
                }
                else
                {
                    updatedLines.Add(line);
                }
            }

            File.WriteAllLines(filePath, updatedLines);
        }

        public List<Parcel> GetParcelHistorySummary()
        {
            var parcels = new Dictionary<string, Parcel>();
            var allLines = File.ReadAllLines(filePath);

            foreach (var line in allLines)
            {
                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                var tracking = parts[0];
                if (!DateTime.TryParse(parts[2], out var date)) continue;

                if (!parcels.ContainsKey(tracking) || date > parcels[tracking].Updates.First().Date)
                {
                    var parcel = new Parcel
                    {
                        TrackingNumber = tracking,
                        Email = parts.Length == 4 ? parts[3] : string.Empty,
                        Updates = new List<ParcelUpdate>
                        {
                            new ParcelUpdate { Status = parts[1], Date = date }
                        }
                    };
                    parcels[tracking] = parcel;
                }
            }
            return parcels.Values.ToList();
        }

        public void ClearAllData()
        {
            File.WriteAllText(filePath, string.Empty);
        }
        #endregion

        #region New Feature Methods

        public List<Parcel> GetParcelsByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return new List<Parcel>();

            var allLines = File.ReadAllLines(filePath);
            var relevantTrackingNumbers = allLines
                .Select(line => line.Split(','))
                .Where(parts => parts.Length == 4 && parts[3].Equals(email, StringComparison.OrdinalIgnoreCase))
                .Select(parts => parts[0])
                .Distinct()
                .ToList();
            
            return relevantTrackingNumbers.Select(tn => GetParcelByTrackingNumber(tn)).Where(p => p != null).ToList();
        }

        public Dictionary<string, int> GetStatusAnalytics()
        {
            var summary = GetParcelHistorySummary();
            return summary
                .Select(p => p.Updates.First().Status)
                .GroupBy(status => status)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public void ArchiveParcel(string trackingNumber)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber)) return;

            var allLines = File.ReadAllLines(filePath).ToList();
            
            var linesToArchive = allLines.Where(line => line.StartsWith($"{trackingNumber},")).ToList();
            if (!linesToArchive.Any()) return;

            var remainingLines = allLines.Except(linesToArchive).ToList();

            File.AppendAllLines(archiveFilePath, linesToArchive);

            File.WriteAllLines(filePath, remainingLines);
        }

        public List<Parcel> FindStaleParcels(int daysInactive)
        {
            var summary = GetParcelHistorySummary();
            var thresholdDate = DateTime.Now.AddDays(-daysInactive);

            return summary
                .Where(p => p.Updates.First().Date < thresholdDate)
                .ToList();
        }

        #endregion
    }
}





