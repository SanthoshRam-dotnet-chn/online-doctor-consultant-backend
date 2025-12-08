Doc2U - Healthcare Platform 

Backend (Microservices Architecture)

This repository contains the backend services for a secure, scalable Healthcare Appointment Platform built using a microservices architecture.
The system includes authentication, doctor management, patient appointments, prescription handling, real-time notifications, and secure video consultations.

Architecture Overview

The backend is composed of multiple independent microservices, all connected through an API Gateway.
Each service has its own database and isolated business logic.

🧩 Microservices Included
Service	Responsibility

API Gateway         -	Routes client requests to appropriate microservices
UserAuthService     -	User authentication, JWT, cookies, roles
DoctorService       -	Doctor profiles, availability slots, prescriptions
PatientService      -	Patient profiles, appointments, doctor selection
NotificationService -	Email notifications (registration, bookings, etc.)
VideoService	      - Jitsi-based secure video meeting management

🏗️ Technology Stack

1. .NET 7 / .NET 8 Web API

2. Entity Framework Core

3. SQL Server

4. JWT Authentication (HttpOnly Cookies)

5. Microservices Architecture

6. REST API

7. Jitsi Meet for Video Calls

8. Email Provider (SMTP / SendGrid / etc.)

9. Grpc for service to service communication (notification service)

10. HttpClient for service to service communication

===================================================
🔐 1. UserAuthService
Handles all authentication and authorization workflows.
===================================================
Features

* User Registration & Login

* Generates JWT Token

* JWT is sent to frontend as a HttpOnly cookie

* Role-based Access Control (doctor/patient/admin)

* Profile Management

===================================================
🧑‍⚕️ 2. DoctorService
Manages doctor data and medical operations.
===================================================

Features

* Create and manage doctor availability slots

* Doctor can prescribe medicines to patients

* Fetch appointments for a doctor

* Secure data handled using JWT validation

* Create a Meeting room using and allow patient to join

===================================================
🧑‍⚕️ 3. PatientService
Responsible for patient-related workflows.
===================================================

Features

* View doctors by experience and specializations

* Book appointment with a doctor

* Maintain patient profile

* View prescriptions & slot schedules

* Join Consultation Call with respective Doctor

===================================================
🔔 4. NotificationService
Triggers email notifications for important events.
===================================================

Sends Emails For:

* New User Registration

* Appointment Booked

* Appointment Cancelled

* Prescription Added

* Reminder Notifications
* 
===================================================
🎥 5. VideoService (Jitsi Integration)
Handles secure video consultations between doctor and patient.
===================================================

Features

* Stores Jitsi meeting URL for each appointment

* Generates secure access token

* Backend validates token → redirects user to correct meeting

* Prevents unauthorized meeting access

* Guarantees doctor-only room access and patient-only room entry

<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
-- Final Flow --

* Appointment created → VideoService stores meeting link

* Patient clicks “Join Meeting”

* Frontend sends request to backend

* Backend validates JWT + meeting token

* Redirects to Jitsi meeting link securely

#############################################
DEMO
▶️ Running the Services
#############################################

1.  Clone the repo
    git clone https://github.com/SanthoshRam-dotnet-chn/online-doctor-consultant-backend.git
    cd online-doctor-consultant-backend

2. Build each service
    dotnet build --project UserAuthService
    dotnet build --project DoctorService
    dotnet build --project PatientService
    dotnet build --project NotificationService
    dotnet build --project VideoService
    dotnet build --project APIGateway
   
3.  Run each service
    dotnet run --project UserAuthService
    dotnet run --project DoctorService
    dotnet run --project PatientService
    dotnet run --project NotificationService
    dotnet run --project VideoService
    dotnet run --project APIGateway

   (Skip this, if you are using vs code).
   Note: If you are using visual studio, then you can set the solution properties to run multiple projects at a time.
   1. Right click on solution name
   2. Click properties
   3. Select multiple projects
   4. Change None -> start what ever projects you want to run.
   5. Click Apply and ok (finish)


🤝 Contributing
Pull requests are welcome! Follow the microservice folder structure and coding standards.
