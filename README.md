# 🚚 Laser Logistics Parcel Tracker

A lightweight ASP.NET Core MVC web application to **track parcels**, **view history**, **filter by status/date**, and manage updates via a simple **admin panel**. Built with simplicity and extensibility in mind.

![Laser Logistics Banner](https://via.placeholder.com/1200x300?text=Laser+Logistics+Parcel+Tracker) <!-- Replace with actual banner if available -->

---

## ✨ Features

- 🔍 Track parcels using a **tracking number**
- 📊 View detailed **status history** with dates
- 🗂 Filter parcels by **status**, **start date**, and **end date**
- 🛠 Admin panel to **add or update parcel statuses**
- 📨 (Mock) **Email notifications** on parcel updates
- 📄 Download a **tracking report** (.txt)
- 💾 File-based data persistence (`Data/parcels.txt`)
- 📱 Mobile-friendly design using **Bootstrap** (coming soon)

---

## 📸 Screenshots

| Track Parcel | Filter Results | Admin Panel |
|--------------|----------------|-------------|
| ![Track](https://via.placeholder.com/300x200?text=Track+Parcel) | ![Filter](https://via.placeholder.com/300x200?text=Filter+Results) | ![Admin](https://via.placeholder.com/300x200?text=Admin+Panel) |

---

# 🚀 Getting Started

## 🏗 Project Structure

```
LaserLogisticsParcelTrackerWeb/
├── Controllers/
│   └── HomeController.cs
├── Models/
│   └── Parcel.cs
├── Services/
│   └── ParcelService.cs
├── Views/
│   └── Home/
│       └── Index.cshtml
├── wwwroot/
│   └── css/site.css
├── Data/
│   └── parcels.txt
├── appsettings.json
├── Program.cs
├── Startup.cs
├── LaserLogisticsParcelTrackerWeb.csproj
└── README.md

```

## 🏃 How to Run on Linux
1. Install [.NET SDK 6+](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/LaserLogisticsParcelTrackerWeb.git
   cd LaserLogisticsParcelTrackerWeb
   ```
3. Run:
```bash
dotnet restore
dotnet build
dotnet run
```
4. Open your browser and go to `http://localhost:5000`


## 🧪 Test

You can test:

http://localhost:5000/Home/Index?trackingNumber=LL12345 → shows parcel status

http://localhost:5000/Home/Download?trackingNumber=LL67890 → downloads .txt report

http://localhost:5000/Home/Admin → add a new parcel update (Admin)

---
## 📌 TODO / Coming Soon
 []Export tracking report as PDF

 []Add timeline visual for parcel history

 []Style with Bootstrap 5 for mobile-first UX

 []Add status icons / color indicators

 []Add search + sort controls to list view

 []Enable real email notifications (SMTP)

---

## 👨‍💻 Author

Created for demonstration purposes by <Lindiwe Thabsile Dlomo>
---
