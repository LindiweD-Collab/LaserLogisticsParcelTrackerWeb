using System.Collections.Generic;

namespace LaserLogisticsParcelTrackerWeb.Models
{
    public class DashboardViewModel
    {
        public Dictionary<string, int> StatusCounts { get; set; }
        public List<Parcel> StaleParcels { get; set; }
    }
}