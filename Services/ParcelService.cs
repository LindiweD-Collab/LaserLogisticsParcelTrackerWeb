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

        public Parcel GetParcelByTrackingNumber(string trackingNumber)
        {
            var parcel = new Parcel { TrackingNumber = trackingNumber };

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                var lineTracking = parts[0];
                if (lineTracking != trackingNumber) continue;

                var status = parts[1];
                var dateStr = parts[2];
                DateTime.TryParse(dateStr, out var date);

                parcel.Updates.Add(new ParcelUpdate
                {
                    Status = status,
                    Date = date
                });

               
                if (parts.Length == 4)
                    parcel.Email = parts[3];
            }

            return parcel.Updates.Any() ? parcel : null;
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
                    parcels[tracking] = new Parcel { TrackingNumber = tracking };

                parcels[tracking].Updates.Add(new ParcelUpdate { Status = status, Date = date });

                if (parts.Length == 4)
                    parcels[tracking].Email = parts[3];
            }

            return parcels.Values
                .Where(p => string.IsNullOrEmpty(statusFilter) || p.Updates.Any(u => u.Status == statusFilter))
                .Where(p => !start.HasValue || p.Updates.Any(u => u.Date >= start))
                .Where(p => !end.HasValue || p.Updates.Any(u => u.Date <= end))
                .ToList();
        }

        public void AddUpdate(string trackingNumber, string status, string email = null)
        {
            var line = $"{trackingNumber},{status},{DateTime.Now:yyyy-MM-dd}";
            if (!string.IsNullOrEmpty(email))
                line += $",{email}";

            File.AppendAllLines(filePath, new[] { line });

           
            if (!string.IsNullOrEmpty(email))
            {
                Console.WriteLine($"[MOCK EMAIL] Sent update to {email}: {status}");
            }
        }
    }
}
