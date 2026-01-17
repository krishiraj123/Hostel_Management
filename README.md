<div align="center">

# 🏨 Innap Hostel Management System
### Enterprise-Grade Housing Administration Solution

<p>
  <img src="https://img.shields.io/badge/ASP.NET%20Core%208.0-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt="ASP.NET Core" />
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server" />
  <img src="https://img.shields.io/badge/ADO.NET-0078D7?style=for-the-badge&logo=microsoft-azure&logoColor=white" alt="ADO.NET" />
</p>
<p>
  <img src="https://img.shields.io/badge/Bootstrap_5-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap" />
  <img src="https://img.shields.io/badge/AJAX-Asynchronous-yellow?style=for-the-badge&logo=jquery&logoColor=black" alt="AJAX" />
  <img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black" alt="JS" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
</p>

<p>
  <a href="#-executive-dashboard-admin-view"><b>Hostel Admin Portal</b></a> •  
  <a href="#-section-2-student-panel"><b>Student Portal</b></a> •  
</p>

</div>

---

## 📖 About The Project

**Innap** is a high-performance, dark-themed Hostel Management System engineered with **ASP.NET Core MVC**. 

Unlike standard CRUD apps that rely heavily on ORMs, this project prioritizes **raw performance and data integrity** by utilizing **ADO.NET with Stored Procedures**. This architecture provides a granular level of control over database interactions, making it suitable for high-load enterprise environments.

The system features a **Dual-Role Architecture**:
* 🛡️ **Admin Panel:** A powerful command center for wardens to manage students, rooms, and finances.
* 🎓 **Student Portal:** A mobile-responsive self-service app for residents to pay fees and raise complaints.

---

## 📸 Executive Dashboard (Admin View)

<div align="center"> 
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Dashboard.png?raw=true" width="90%" alt="Dashboard Overview">
  <br>
  <em>Central command center providing real-time insights into occupancy rates, admission trends, and pending tickets.</em>
</div>

---

## 🛡️ Section 1: Hostel Admin Panel

The Admin Panel is designed for efficiency, allowing wardens and staff to manage the entire hostel lifecycle from a single interface.

### 1. 📊 Dashboard & Analytics
The landing page provides a comprehensive overview of the hostel's health.
* **Occupancy Stats:** Real-time counter of Total Students, Vacant Rooms, and Present Students.
* **Vacancy Overview:** Visual graph showing room availability trends.
* **Recent Activity:** A feed of the latest student admissions and complaints requiring attention.

### 2. 👥 Student Management (Add & View)
Complete control over the student database.
* **Add Student:** A streamlined **multi-step wizard** for registering new students, capturing personal details, parent information, and immediate room allocation.
* **View Details:** A searchable directory of all residents. Admins can view detailed profiles, including emergency contacts and academic status.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Student_Add.png?raw=true" width="45%" alt="Admission Wizard">
  &nbsp;
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Student_Info.png?raw=true" width="45%" alt="Student Directory">
</div>

### 3. 🛏️ Room & Vacancy Management
Manage hostel capacity with precision to prevent overbooking.
* **Room Inventory:** Detailed list of all rooms with attributes like Capacity, Floor, Rent, and Type (AC/Non-AC).
* **Vacancy Indicators:** Instant visual cues (Green for Vacant, Orange for Full) allow staff to assess availability at a glance.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Room_Info.png?raw=true" width="45%" alt="Room Directory">
  &nbsp;
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Room_Add.png?raw=true" width="45%" alt="Add Room Interface">
</div>

### 4. 🎫 Complaint Resolution
A dedicated helpdesk for tracking and resolving student issues.
* **Ticket Management:** View all complaints regarding facilities (WiFi, Cleaning, Plumbing).
* **Status Tracking:** Admins can mark complaints as "In Progress" or "Resolved" and assign them to maintenance staff.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Student_Complaints.png?raw=true" width="60%" alt="Complaint Management">
</div>

### 5. 📢 Alerts & Notifications
A centralized communication hub for hostel-wide announcements.
* **Send Alerts:** Admins can broadcast notifications (e.g., "Emergency Drill" or "Fee Reminder") directly to student dashboards.
* **History Log:** View a list of all previous notices sent to students.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Alert_Notifications.png?raw=true" width="60%" alt="Notification Center">
</div>

### 6. 🍽️ Food Timetable Management
* **Upload Schedule:** Admins can bulk-upload the weekly mess menu using a standard **Excel template**.
* **View Timetable:** Check the current active meal plan for Breakfast, Lunch, and Dinner.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Upload_FoodTimetable.png?raw=true" width="60%" alt="Menu Upload">
</div>

### 7. 💳 Payment Status & Profile
* **Payment Oversight:** Admins can check the fee payment status of any student to identify defaulters.
* **Hostel Profile:** Manage general hostel settings and details.
  
<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Payment.png?raw=true" width="60%" alt="Menu Upload">
</div>

---

## 🎓 Section 2: Student Panel

A self-service portal empowering students to manage their stay without visiting the warden's office.

### 1. 🏠 Student Dashboard & Notices
A personalized home screen for residents.
* **Live Notifications:** Students can view all active notices and alerts sent by the Hostel Admin.
* **Room Details:** Quick access to their assigned room number and details.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Student_DashBoard.png?raw=true" width="85%" alt="Student Dashboard">
</div>

### 2. 📝 Complaints & Facilities
* **Raise Complaint:** Students can file complaints regarding hostel facilities (e.g., "Fan not working").
* **History:** View a log of previous complaints and track their resolution status.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Complaint_Studentside.png?raw=true" width="60%" alt="Add Complaint">
</div>

### 3. 💳 Fee Payments & Receipts
A secure financial module for rent management.
* **Pay Fees:** Integrated gateway to pay monthly hostel rent online.
* **Download Receipt:** Automatically generates a downloadable PDF receipt after payment.
* **Payment History:** View a ledger of all past transactions.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Pay_fees.png?raw=true" width="45%" alt="Fee Payments">
  &nbsp;
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Payment_History.png?raw=true" width="45%" alt="Payment History">
</div>

### 4. 🍽️ Digital Mess Menu
"What's for dinner?" — Students can check the full weekly breakfast/lunch/dinner schedule anytime to plan their meals.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/FoodTimetable_student.png?raw=true" width="60%" alt="Weekly Timetable">
</div>

### 5. 🫂 Roommate Information
Safety and community feature.
* **Emergency Contacts:** Students can view details about their roommates.
* **Contact Info:** Useful for medical emergencies or coordination.

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/Roommate_info.png?raw=true" width="60%" alt="Roommate Info">
</div>

---

## ⚙️ Technical Architecture

This project was built to demonstrate proficiency in **low-level data access** and **secure API design**.

| Component | Tech Stack | Description |
| :--- | :--- | :--- |
| **Backend** | ASP.NET Core MVC 8.0 | Robust server-side rendering and logic. |
| **Data Access** | **ADO.NET** | Chosen over EF Core to demonstrate SQL optimization and Stored Proc mastery. |
| **Database** | SQL Server | Relational data integrity with complex constraints. |
| **Frontend** | Bootstrap 5 | Custom Dark Theme implementation using SASS. |
| **API** | RESTful API | Fully documented endpoints for mobile integration. |

<div align="center">
  <img src="https://github.com/krishiraj123/Hostel_Management/blob/main/Pictures/API.png?raw=true" width="85%" alt="API Swagger Documentation">
</div>

---

## 🛠️ Getting Started

Follow these steps to set up the project locally.

### Prerequisites
* .NET SDK 8.0 or later
* SQL Server (LocalDB or Express)
* Visual Studio 2022

### Installation

1.  **Clone the Repository**
    ```bash
    git clone [https://github.com/krishiraj123/Hostel_Management.git](https://github.com/krishiraj123/Hostel_Management.git)
    ```

2.  **Database Configuration (Easy Setup)**
    * Navigate to the `Database/` folder in the project.
    * Open **SQL Server Management Studio (SSMS)**.
    * Right-click **Databases** > **Attach...**
    * Select `HostelDB.mdf` from the `Database/` folder.

3.  **App Configuration**
    * Open `appsettings.json`.
    * Update the connection string if necessary:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.\\SQLEXPRESS;Database=HostelDB;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

4.  **Run the Application**
    ```bash
    dotnet run
    ```

---

<div align="center">
  <p>Designed & Developed by <strong>Krishirajsinh Vansia</strong> © 2026</p>
  <p>
    <a href="https://github.com/krishiraj123">GitHub Profile</a>
  </p>
</div>
