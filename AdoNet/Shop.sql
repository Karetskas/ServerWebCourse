--USE master;
--GO

--ALTER DATABASE Shop
--SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
--GO

--DROP DATABASE Shop;
--GO

CREATE DATABASE Shop;
GO

USE Shop;
GO

CREATE TABLE ProductCategory
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL CHECK(Name <> N'')
);
GO

CREATE TABLE Product
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL CHECK(Name <> N''),
	Price DECIMAL(20, 2) NOT NULL CHECK(Price >= 0.00),
	CategoryId INT NOT NULL,
	FOREIGN KEY (CategoryId) REFERENCES ProductCategory(Id) 
		ON DELETE CASCADE
		ON UPDATE CASCADE
);
GO

INSERT INTO ProductCategory(Name)
VALUES (N'Meat'),
	(N'Fruit'),
	(N'Vegetables'),
	(N'Dairy foods'),
	(N'Fish');
GO

INSERT INTO Product(Name, Price, CategoryId)
VALUES (N'Sausage', 342.43, 1),
	(N'Potato', 1.94, 3),
	(N'Flatfish', 774.21, 5),
	(N'Milk', 21.17, 4),
	(N'Lemon', 833.00, 2),
	(N'Apple', 11.12, 5),
	(N'Carrot', 11123.00, 1),
	(N'Lemon', 603.11, 2),
	(N'Potato', 2.33, 3),
	(N'Milk', 20.19, 4),
	(N'Lemon', 345.33, 2);
GO