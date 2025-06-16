using System.Collections.Generic;

namespace LaserLogisticsParcelTrackerWeb.Models
{
    public class AdminDashboardViewModel
    {
        public Dictionary<string, int> StatusCounts { get; set; }

        public List<Parcel> StaleParcels { get; set; }

        public List<Parcel> AllParcels { get; set; }
    }
}