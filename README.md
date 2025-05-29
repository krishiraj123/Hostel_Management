# 🏨 Hostel Management System (ASP.NET Core MVC)

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-5C2D91?style=for-the-badge&logo=.net)
![MVC](https://img.shields.io/badge/MVC-FF6A00?style=for-the-badge&logo=.net)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap)

A comprehensive hostel management solution built with **ASP.NET Core MVC** featuring dual dashboards for students and staff, dynamic room allocation, complaint management, and real-time alerts. Includes both robust API endpoints and responsive frontend interfaces.

---

## 🚀 Features

### 👥 Dual Dashboard System
- **Student Dashboard**  
  ✓ View room allocation status  
  ✓ Submit maintenance complaints  
  ✓ Receive real-time alerts  
  ✓ Check payment history  
  ✓ View hostel announcements
  
- **Staff Dashboard**  
  ✓ Manage student registrations  
  ✓ Dynamic room allocation  
  ✓ Complaint resolution tracking  
  ✓ Generate occupancy reports  
  ✓ Broadcast notifications

---

### 🛏 Dynamic Room Allocation
- **Intelligent Assignment Engine**  
  Automatically assigns rooms based on availability, gender, and preferences
- **Manual Override Control**  
  Staff can manually adjust allocations with conflict detection
- **Room Transfer Management**  
  Seamless room switching with history tracking
- **Vacancy Forecasting**  
  Predictive analytics for upcoming vacancies

---

### 🛠 Complaint Management
- **Ticket Lifecycle Tracking**  
  `Open → In Progress → Resolved` with timestamps
- **Priority Tagging System**  
  Critical/High/Medium/Low priority classification
- **Staff Assignment**  
  Automatic routing to relevant department
- **Resolution Analytics**  
  Performance metrics and SLA tracking

---

### 🔔 Real-Time Alert System
- **Automated Reminders**  
  Payment dues, complaint updates, curfew alerts
- **Emergency Broadcasts**  
  Instant notifications for emergencies
- **Announcement Center**  
  Pinned messages and event notifications
- **Mobile-Ready Alerts**  
  Responsive design for all devices

---

## ⚙️ Tech Stack

### Backend
- **Framework**: ASP.NET Core 7.0 MVC
- **API**: RESTful Web API with JWT Authentication
- **Database**: SQL Server + Entity Framework Core
- **ORM**: Entity Framework Core (Code-First)
- **Authentication**: ASP.NET Core Identity

### Frontend
- **UI**: Razor Pages + Bootstrap 5
- **Client Scripting**: JavaScript + jQuery
- **Charts**: Chart.js
- **Notifications**: Toastr.js

---

## 🛠️ Installation

```bash
# Clone repository
git clone https://github.com/your-username/hostel-management-system.git

# Navigate to project
cd HostelManagement

# Restore packages
dotnet restore

# Apply database migrations
dotnet ef database update

# Run application
dotnet run
