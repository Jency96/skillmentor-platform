
SkillMentor is a full-stack mentorship platform that connects students with professional mentors for learning sessions.
Students can browse mentors and book sessions, while administrators manage mentors, subjects, and session bookings.

The platform is built using a React frontend and a Spring Boot backend, with authentication handled by Clerk, and deployed using Vercel and Render.

###  Public Features

  * Browse available mentors

  * View mentor details and expertise

  * Explore mentorship subjects

  * Responsive landing page

### Student Features

  * Secure authentication using Clerk

  * Enroll in mentorship sessions

  * View upcoming sessions

  * View session history

  * Access meeting links for sessions

### Admin Features

  * Admin users can access the Admin Dashboard to manage platform operations.

### Subject Management

  * Create new subjects

  * View subject list

### Mentor Management

  * Create mentor profiles

  * Store mentor professional information

  * Assign mentors to subjects

### Booking Management

  * View all session bookings

  * Filter sessions by status

  * Confirm session payments

  * Add meeting links

  * Mark sessions as completed


🛠 Tech Stack

🟠 Frontend

 * React

 * TypeScript

 * Vite

 * React Router

 * Tailwind CSS

 * Clerk Authentication


🟠 Backend

 * Java

 * Spring Boot

 * Spring Security

 * Spring Data JPA

 * Hibernate


🟠 Database

 * PostgreSQL


🟠 Deployment

 * Frontend: Vercel

 * Backend: Render

 * Database: Render PostgreSQL
 


⚙️ Getting Started (Local Development)


1️⃣ FrontEnd Clone the repository

git clone (https://github.com/Jency96/skillmentor-frontend-final.git)
cd skillmentor-frontend-final
idea .

## install dependencies
💠npm install


## Start development server:
💠npm run dev

## Frontend will run at:
💠http://localhost:3001


2️⃣ BackEnd Clone the repository

git clone (https://github.com/Jency96/skillmentor-backend-final.git)
cd skillmentor-backend-final
idea .


## Run the application , if the project use maven:
💠./mvnw spring-boot:run


## Backend will run at:
💠http://localhost:8081


## Swagger documentation:
http://localhost:8081/swagger-ui.html


🔐 Environment Variables

♦️Frontend (.env)
 💠VITE_API_BASE_URL=http://localhost:8081
 💠VITE_CLERK_PUBLISHABLE_KEY=your_clerk_publishable_key

♦️Backend (application.properties)
 💠spring.application.name=skillmentor
 💠spring.profiles.active=${PROFILE:prod}
 💠spring.datasource.driver-class-name=org.postgresql.Driver
 💠spring.jpa.database-platform=org.hibernate.dialect.PostgreSQLDialect
 💠spring.datasource.url=${DATABASE_URL}
 💠spring.datasource.username=${DB_USERNAME}
 💠spring.datasource.password=${DB_PASSWORD}
 💠spring.datasource.hikari.maximum-pool-size=1
 💠spring.jpa.hibernate.ddl-auto=none


 📘 API Documentation

♦️Swagger UI is available at:
💠/swagger-ui.html


♦️Deployed Swagger:
💠https://skillmentor-backend-final-v1.onrender.com/swagger-ui.html



🧑‍💻Mentor APIs

🟩Retrieve paginated mentors
💠GET /api/v1/mentors

🟩Create mentor (Admin only).
💠POST /api/v1/mentors

🟩Update mentor.
💠PUT /api/v1/mentors/{id}

🟩Delete mentor.
💠DELETE /api/v1/mentors/{id}



📕Subject APIs

🟪Retrieve subjects.
💠GET /api/v1/subjects

🟪Create subject (Admin only).
💠POST /api/v1/subjects

🟪Update subject.
💠PUT /api/v1/subjects/{id}

🟪Delete subject.
💠DELETE /api/v1/subjects/{id}



🧑‍🏫Session APIs

🟨Student enrolls in mentorship session.
💠POST /api/v1/sessions/enroll

🟨Retrieve student sessions.
💠GET /api/v1/sessions/my-sessions



🧑‍🏫Admin Booking APIs

⬜Retrieve all bookings.
💠GET /api/v1/admin/sessions

⬜Confirm payment.
💠PATCH /api/v1/admin/sessions/{id}/confirm-payment

⬜Add meeting link.
💠PATCH /api/v1/admin/sessions/{id}/meeting-link

⬜Mark session as completed.
💠PATCH /api/v1/admin/sessions/{id}/complete



🌐 Deployed Links

🟥Frontend (Vercel)
💠https://skillmentor-frontend-final-opal.vercel.app

🟥Backend (Render)
💠https://skillmentor-backend-final-v1.onrender.com

🟥Swagger Documentation
💠https://skillmentor-backend-final-v1.onrender.com/swagger-ui.html



skillmentor
│
├── frontend
│   ├── src
|   |   |
|   |   |__assestS
|   |   |
│   │   ├── components
|   |   |    |
|   |   |    |___hooks
|   |   |    |___ui
|   |   |    |__ AdminLayout
|   |   |    |__AdminSideBar
|   |   |    |__Footer
|   |   |    |__Layout
|   |   |    |__MentorCard
|   |   |    |__Navigation
|   |   |    |__SchedulinModel
|   |   |    |__SignUpDialog
|   |   |    |__StatusPill
|   |   |
|   |   |___lib
|   |   |    |__api
|   |   |    |__utils
|   |   |
|   |   |  
│   │   ├── pages
│   │   │   ├── HomePage
│   │   │   ├── DashboardPage
|   |   |   |__LoginPage
|   |   |   |__PaymentPage
|   |   |   |
│   │   │   └── admin
│   │   │       ├── AdminOverviewPage
│   │   │       ├── CreateSubjectPage
│   │   │       ├── SubjectsPage
│   │   │       ├── CreateMentorPage
│   │   │       └── ManageBookingsPage
│   │   ├
│   │   │
│   │   |__App
|   |   |__index
|   |   |__main
|   |   |__types
|   |   |__vite-env.d.ts
|   |   |__env
|   
│
├── backend
│   ├── main
|   |   |__java
|   |   |  |__com
|   |   |  |  |__stemlink
|   |   |  |  |  |__skillmentor
|   |   |  |  |  |  |
|   |   |  |  |  |  |__config
|   |   |  |  |  |  |__constants
|   |   |  |  |  |  |__controllers
|   |   |  |  |  |  |__dtos
|   |   |  |  |  |  |__entities
|   |   |  |  |  |  |__exceptions
|   |   |  |  |  |  |__repositories
|   |   |  |  |  |  |__security
|   |   |  |  |  |  |__service
|   |   |  |  |  |  |__utils
|   |   |  |  |  |  |__skillmentor application  
│   
│   
│   
│
└── README.md












