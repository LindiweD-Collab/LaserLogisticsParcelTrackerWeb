using Microsoft.AspNetCore.Mvc;
using LaserLogisticsParcelTrackerWeb.Models;
using LaserLogisticsParcelTrackerWeb.Services;
using System.Linq;

namespace LaserLogisticsParcelTrackerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ParcelService _service;

        public HomeController(ParcelService parcelService)
        {
            _service = parcelService;
        }

        #region Actions

        public IActionResult Index(string trackingNumber, string statusFilter)
        {

            if (!string.IsNullOrEmpty(trackingNumber))
            {
                var parcel = _service.GetParcelByTrackingNumber(trackingNumber);

                return View("Index", parcel);
            }

            var parcels = _service.SearchParcels(statusFilter);

            ViewData["CurrentFilter"] = statusFilter;

            return View("List", parcels);
        }
        [HttpGet]
        public IActionResult Admin()
        {
            var statusCounts = _service.GetStatusAnalytics();
            var staleParcels = _service.FindStaleParcels(7);
            var allParcels = _service.GetParcelHistorySummary();

            var viewModel = new AdminDashboardViewModel
            {
                StatusCounts = statusCounts,
                StaleParcels = staleParcels,
                AllParcels = allParcels
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult UpdateEmail(string trackingNumber)
        {
            var parcel = _service.GetParcelByTrackingNumber(trackingNumber);
            if (parcel == null) { return NotFound(); }
            return View(parcel);
        }

        [HttpPost]
        public IActionResult UpdateEmail(string trackingNumber, string newEmail)
        {
            _service.UpdateParcelEmail(trackingNumber, newEmail);
            TempData["Message"] = "Email updated successfully!";
            return RedirectToAction("Index", new { trackingNumber });
        }

        [HttpPost]
        public IActionResult AddParcelUpdate(string trackingNumber, string status, string email)
        {
            if (!string.IsNullOrWhiteSpace(trackingNumber) && !string.IsNullOrWhiteSpace(status))
            {
                _service.AddUpdate(trackingNumber, status, email);
                TempData["Message"] = "Update added successfully!";
            }
            else
            {
                TempData["Error"] = "Tracking Number and Status cannot be empty.";
            }
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult Delete(string trackingNumber)
        {
            _service.DeleteParcelByTrackingNumber(trackingNumber);
            TempData["Message"] = $"Parcel {trackingNumber} has been deleted.";
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult Archive(string trackingNumber)
        {
            _service.ArchiveParcel(trackingNumber);
            TempData["Message"] = $"Parcel {trackingNumber} has been archived.";
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var analytics = _service.GetStatusAnalytics();
            var staleParcels = _service.FindStaleParcels(7);
            var dashboardViewModel = new DashboardViewModel { StatusCounts = analytics, StaleParcels = staleParcels };
            return View(dashboardViewModel);
        }

        [HttpGet]
        public IActionResult MyParcels()
        {
            return View(new MyParcelsViewModel());
        }

        [HttpPost]
        public IActionResult MyParcels(string email)
        {
            var parcels = _service.GetParcelsByEmail(email);
            var viewModel = new MyParcelsViewModel { Email = email, Parcels = parcels };
            if (!parcels.Any()) { ViewBag.Message = "No parcels found for this email address."; }
            return View(viewModel);
        }

        public IActionResult Download(string trackingNumber)
        {
            var parcel = _service.GetParcelByTrackingNumber(trackingNumber);
            if (parcel == null) { return NotFound(); }
            var content = $"Tracking Report for {parcel.TrackingNumber}\nEmail: {parcel.Email ?? "N/A"}\n---\n";
            foreach (var update in parcel.Updates) { content += $"{update.Date:yyyy-MM-dd HH:mm}: {update.Status}\n"; }
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            return File(bytes, "text/plain", $"{trackingNumber}_report.txt");
        }
        #endregion
    }
}