# 🚚 Laser Logistics Parcel Tracker

A lightweight ASP.NET Core MVC web application to **track parcels**, **view history**, **filter by status/date**, and manage updates via a simple **admin panel**. Built with simplicity and extensibility in mind.

---

## Technologies Used

* **ASP.NET Core MVC**: For building the web application.
* **Bootstrap 5**: For responsive and modern UI styling.
* **C#**: The primary programming language.
---

##  Features

-  **Parcel Tracking:** Track any parcel using its unique tracking number.
-  **Admin Dashboard:** A central hub for administrators showing:
    -   **Status Analytics:** See counts of parcels for each status (e.g., "In Transit," "Delivered").
    -   **Stale Parcel Detection:** Automatically flags parcels that haven't been updated recently.
    -   **Parcel Management:** A list of all active parcels with quick-action buttons.
-  **User-Specific Search:** Users can find all parcels associated with their email address.
-  **Full CRUD Operations:**
    -   **Create:** Add new parcel updates.
    -   **Read:** View detailed parcel history with dates.
    -   **Update:** Change the email address associated with a parcel.
    -   **Delete:** Permanently delete a parcel and its entire history.
-  **Parcel Archiving:** Archive completed or old parcels to a separate file (`parcels_archive.txt`) to keep the active database clean.
-  **Download Reports:** Download a simple text-based tracking report for any parcel.
-  **Styled with Bootstrap & Font Awesome:** A clean UI with icons for a better user experience.
-  **File-Based Persistence:** All data is stored in simple, human-readable `.txt` files in the `Data/` directory.


---

##  Screenshots

| Track Parcel | Filter Results | Admin Panel |
|--------------|----------------|-------------|
| ![ParcelTracker](https://github.com/user-attachments/assets/c5944009-10d0-4798-84b0-bc6f5a2c6068) | ![FilteredResults](https://github.com/user-attachments/assets/bbbdb6a6-f135-4d73-9dd7-61c8f17d0aa7) | ![AdminPanel](https://github.com/user-attachments/assets/63aca8a6-6c0c-49c8-95e0-ac2afd1e2421)|


#  Getting Started

##  Project Structure

```
LaserLogisticsParcelTrackerWeb/
├── Controllers/
│   └── HomeController.cs         
├── Data/
│   ├── parcels.txt               
│   └── parcels_archive.txt       
├── Models/
│   ├── Parcel.cs                 
│   ├── DashboardViewModel.cs     
│   └── MyParcelsViewModel.cs     
├── Services/
│   └── ParcelService.cs          
├── Views/
│   ├── Home/
│   │   ├── Admin.cshtml
│   │   ├── Index.cshtml
│   │   ├── List.cshtml
│   │   ├── MyParcels.cshtml
│   │   └── UpdateEmail.cshtml
│   └── _ViewImports.cshtml       
├── wwwroot/
│   └── css   └── style.css             
├── appsettings.json
├── Program.cs                    
├── LaserLogisticsParcelTrackerWeb.csproj
└── README.md
```

##  How to Run 
1. Install [.NET SDK 6+](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Clone the repository:

   ```bash
   git clone https://github.com/LindiweD-Collab/LaserLogisticsParcelTrackerWeb.git
   cd LaserLogisticsParcelTrackerWeb
   ```
3. Run:
```bash
dotnet restore
dotnet build
dotnet run
```
4. Open your browser and go to `http://localhost:5000`


##  Test

Once the application is running, you can try the following URLs:

-   **`http://localhost:5000/`**: Main tracking page.
-   **`http://localhost:5000/Home/Admin`**: The admin dashboard.
-   **`http://localhost:5000/Home/MyParcels`**: The page to search for parcels by email.

---
##  TODO / Coming Soon
-   [ ] **Implement a Database:** Replace the file-based storage with a proper database like SQLite or PostgreSQL for better performance and scalability.
-   [ ] **User Authentication:** Add a login system for administrators and users.
-   [ ] **Visual Timeline:** Enhance the parcel history view with a graphical timeline.
-   [ ] **Real Email Notifications:** Integrate an email service (like SendGrid) to send actual notifications.
-   [ ] **Unit & Integration Tests:** Add a test project to ensure code quality and prevent regressions.
-   [ ] **Export to PDF/CSV:** Add more formats for downloading reports.



##  Author

Created for demonstration purposes by [Lindiwe Thabsile Dlomo](https://github.com/LindiweD-Collab)

