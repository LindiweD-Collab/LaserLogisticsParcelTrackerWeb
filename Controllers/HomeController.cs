using System;

using Microsoft.AspNetCore.Mvc;
using LaserLogisticsParcelTrackerWeb.Models;
using LaserLogisticsParcelTrackerWeb.Services;

namespace LaserLogisticsParcelTrackerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ParcelService _service = new ParcelService();

        public IActionResult Index(string trackingNumber, string statusFilter, DateTime? startDate, DateTime? endDate)
        {
            if (!string.IsNullOrEmpty(trackingNumber))
            {
                var parcel = _service.GetParcelByTrackingNumber(trackingNumber);
                return View(parcel);
            }

            var filtered = _service.SearchParcels(statusFilter, startDate, endDate);



            return View("List", filtered);
        }

        [HttpGet]
        public IActionResult Admin() => View();

        [HttpPost]
        public IActionResult Admin(string trackingNumber, string status, string email)
        {
            _service.AddUpdate(trackingNumber, status, email);
            ViewBag.Message = "Update added!";
            return View();
        }

        public IActionResult Download(string trackingNumber)
        {
            var parcel = _service.GetParcelByTrackingNumber(trackingNumber);
            var content = $"Tracking Report for {trackingNumber}\n";
            foreach (var update in parcel.Updates)
                content += $"{update.Date:yyyy-MM-dd}: {update.Status}\n";

            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            return File(bytes, "application/octet-stream", $"{trackingNumber}_report.txt");
        }
    }
}
