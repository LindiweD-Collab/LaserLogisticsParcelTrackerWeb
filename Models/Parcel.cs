using System;
using System.Collections.Generic;


namespace LaserLogisticsParcelTrackerWeb.Models
{
    public class ParcelUpdate
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }

    public class Parcel
    {
        public string TrackingNumber { get; set; }
        public List<ParcelUpdate> Updates { get; set; } = new();
        public string Email { get; set; }
    }
}
