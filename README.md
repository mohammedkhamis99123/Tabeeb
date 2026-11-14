# Tabeeb (طبيب) - Multi-Tenant Medical Clinic SaaS Platform

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-18%2B-DD0031?logo=angular&logoColor=white)](https://angular.io/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blueviolet)](https://github.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**Tabeeb** is an enterprise-grade, Multi-Tenant SaaS platform designed for modern medical clinics, multi-doctor practices, and healthcare centers. It provides robust patient data isolation, seamless appointment scheduling, financial transaction processing via global and regional gateways, and automated professional medical invoice generation.

---

## 🚀 Key Features

*   **Multi-Tenant SaaS Architecture:** Complete data isolation per clinic/tenant with a scalable database strategy and shared/isolated tenant routing.
*   **Role-Based Access Control (RBAC):** Granular permissions for Super Admins, Clinic Owners, Doctors, Receptionists, and Patients.
*   **Appointment & Schedule Management:** Real-time slot booking, doctor availability tracking, and calendar management.
*   **Secure Payment Integration:** Integrated payment gateways supporting both **Stripe** (International) and **MyFatoorah** (Regional / GCC) for consultation and service billing.
*   **Automated PDF Generation:** Built using **QuestPDF** to generate professional medical reports, prescriptions, and ZATCA-compliant invoices with Right-to-Left (RTL) Arabic text shaping support.
*   **Real-time Notifications:** SignalR-powered live updates for queue management, appointment reminders, and clinic alerts.
*   **Robust Backend Pipeline:** CQRS pattern implemented via **MediatR**, pipeline validation using **FluentValidation**, structured logging with **Serilog & Seq**, and background job processing with **Hangfire**.

---

## 🛠️ Technology Stack

### **Backend (.NET Core)**
*   **Framework:** ASP.NET Core (.NET 10) / Web API
*   **Architecture:** Clean Architecture (Domain, Application, Infrastructure, Presentation)
*   **Data Access:** Entity Framework Core (EF Core), LINQ Performance Optimization, SQL Server
*   **Messaging & CQRS:** MediatR
*   **Validation & Logging:** FluentValidation, Serilog, Seq
*   **Background Jobs:** Hangfire
*   **Real-time:** ASP.NET Core SignalR
*   **Security:** JWT Authentication, Refresh Token Rotation, Multi-Tenant Middleware
*   **Document Generation:** QuestPDF

### **Frontend (Angular)**
*   **Framework:** Angular (TypeScript)
*   **State Management:** NgRx Store & RxJS Reactive Programming
*   **Forms & UI:** Angular Reactive Forms , Tailwind CSS
*   **Icons & Assets: FontAwesome

---

## 📂 Project Structure (Clean Architecture)

```text
Tabeeb.SaaS/
│
├── src/
│   ├── Tabeeb.Domain/          # Core business entities, value objects, and domain events
│   ├── Tabeeb.Application/     # Use cases, CQRS handlers, interfaces, DTOs, and Validators
│   ├── Tabeeb.Infrastructure/  # EF Core DbContext, Migrations, External Services (Stripe, MyFatoorah, QuestPDF)
│   └── Tabeeb.API/          # Controllers, Middlewares, Dependency Injection, and Program.cs
│
└── frontend/                   # Angular Client Application
    ├── src/app/
    │   ├── core/               # Interceptors, Guards, Global Services
    │   ├── features/           # Modularized features (Clinics, Doctors, Patients, Billing)
    │   └── shared/             # Reusable UI components and pipes
    └── angular.json
```

---

## ⚙️ Getting Started

### **Prerequisites**
*   [.NET 10 SDK](https://dotnet.microsoft.com/download)
*   [Node.js (LTS)](https://nodejs.org/) & Angular CLI
*   SQL Server (LocalDB or SQL Server Express/Enterprise)

### **1. Clone the Repository**
```bash
https://github.com/mohammedkhamis99123/Tabeeb.git
cd tabeeb-saas
```

### **2. Configure Backend Settings**
Navigate to `src/Tabeeb.WebApi/appsettings.json` and configure your connection strings and secret keys:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TabeebDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "YOUR_SUPER_SECRET_KEY_HERE_MIN_32_BYTES",
    "Issuer": "TabeebApi",
    "Audience": "TabeebClient",
    "DurationInMinutes": 60
  },

}
```

### **3. Run Database Migrations**
```bash
dotnet ef database update --project src/Tabeeb.Infrastructure --startup-project src/Tabeeb.API
```

### **4. Run the Backend API**
```bash
cd src/Tabeeb.API
dotnet run
```
The API will run on `https://localhost:7090` (Swagger UI available at `/swagger`).

### **5. Run the Angular Frontend**

Repo : https://github.com/mohammedkhamis99123/Tabeeb-angular.git
```bash
cd frontend
npm install
ng serve
```
Open your browser and navigate to `http://localhost:4200`.

---

## 📄 Document Generation (QuestPDF)

Tabeeb utilizes QuestPDF to create high-performance invoices and medical reports. To test or customize invoice layouts, check the service implementation under:
`src/Tabeeb.Infrastructure/Services/PdfInvoiceService.cs`

---

## 🛡️ Security & Multi-Tenancy

*   **Tenant Resolution:** Tenants are identified dynamically via HTTP Headers (`X-Tenant-Id`) or Subdomains.
*   **Data Isolation:** Global Query Filters are applied automatically via EF Core to guarantee that tenant data never leaks across clinic boundaries.

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the [issues page](../../issues).

---

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).
