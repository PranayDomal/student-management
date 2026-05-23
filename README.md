# Student Management API

## Introduction

This project is a **Student Management API** built using **ASP.NET Core Web API**. It allows users to manage student records through a set of secure endpoints.

The main focus of this project is to implement a clean backend structure with authentication and user-based access. It includes features like login with JWT tokens, performing CRUD operations on student data, and ensuring that users can only access the data they have created.

Through this project, I have worked on:

* Designing secure APIs
* Implementing user-based access control
* Performing CRUD operations using ADO.NET
* Handling authentication using JWT tokens

---

## Features

### Authentication

* Users can log in using their username and password
* A JWT token is generated after successful login
* All protected APIs require this token for access

### Student Management (CRUD)

* Add a new student
* View all students created by the logged-in user
* Retrieve details of a specific student
* Update student information
* Delete a student

### Access Control

* Each student record is associated with the user who created it
* Users can only view and manage their own data
 
 ---

## Tech Stack

* ASP.NET Core Web API
* ADO.NET (SQL Server)
* JWT Authentication
* Swagger (API Testing)

---

## Database Structure

### Users Table

* Id (int)
* Username (varchar)
* Password (varchar)

### Students Table

* Id (int)
* Name (varchar)
* Email (varchar)
* Phone (varchar)
* CreatedBy (int)
* CreatedAt (datetime)
* UpdatedAt (datetime)

---

## Setup Instructions

### 1. Clone Repository

```bash
git clone https://github.com/PranayDomal/student-management.git
```

### 2. Setup Database

* Open SQL Server
* Run the provided `database.sql` file

### 3. Configure Application

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "your_connection_string"
},
"Jwt": {
  "Key": "your_secret_key",
  "Issuer": "your_issuer",
  "Audience": "your_audience"
}
```

### 4. Run Application

* Open in Visual Studio
* Press **F5** or run:

```bash
dotnet run
```

### 5. Access Swagger

```
https://localhost:{port}/swagger
```

---

## 🔑 API Endpoints

### Auth

* `POST /api/Auth/login` → Generate JWT Token

### Students (Protected)

* `GET /api/Student` → Get all students
* `POST /api/Student` → Add student
* `PUT /api/Student/{id}` → Update student
* `DELETE /api/Student/{id}` → Delete student

---

## 🔐 Authorization Usage

1. Login using `/api/Auth/login`
2. Copy the token
3. Click **Authorize** in Swagger
4. Enter:

```
Bearer YOUR_TOKEN
```

---

## 📄 Notes

* All student operations require authentication
* SQL queries are parameterized to prevent SQL injection
* JWT token expires after a fixed duration

---

## 📬 Contact

For any queries or clarification, feel free to reach out.

- Email: domalpranay@gmail.com
- LinkedIn: https://www.linkedin.com/in/pranay-domal/
- GitHub: https://github.com/PranayDomal/ 
