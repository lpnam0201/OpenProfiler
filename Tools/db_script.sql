CREATE DATABASE OpenProfilerSampleDatabase;
GO

USE OpenProfilerSampleDatabase;

CREATE TABLE Customer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100),
    Birthday DATETIME
);
GO

INSERT INTO Customer (Name, Birthday) VALUES
('Alice', '1990-05-12'),
('Bob', '1985-09-23'),
('Charlie', '2000-01-15'),
('Diana', '1992-07-30'),
('Ethan', '1988-11-05');
GO