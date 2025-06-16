using System.Collections.Generic;

namespace LaserLogisticsParcelTrackerWeb.Models
{
    public class MyParcelsViewModel
    {
        public string Email { get; set; }
        public List<Parcel> Parcels { get; set; } = new List<Parcel>();
    }
}