CREATE TABLE Product (
    Id INT PRIMARY KEY,
    Title NVARCHAR(100),
    Price DECIMAL(18, 2),
    Description NVARCHAR(100),
    Category NVARCHAR(100)
);

CREATE TABLE Customer (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100)
);
