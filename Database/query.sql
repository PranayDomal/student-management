CREATE DATABASE StudentDB;
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL
);

INSERT INTO Users (Username, Password)
VALUES 
('admin', '1234'),
('user1', '1234');

CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    CreatedBy INT,
    CreatedAt DATETIME,
    UpdatedAt DATETIME,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

select * from Students;
select * from Users;