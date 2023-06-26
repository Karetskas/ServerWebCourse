--USE master;
--GO

--ALTER DATABASE shop
--SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
--GO

--DROP DATABASE shop;
--GO

CREATE DATABASE shop;
GO

USE shop;
GO

CREATE TABLE productCategories
(
	id INT IDENTITY(1, 1) PRIMARY KEY,
	name NVARCHAR(255) NOT NULL DEFAULT 'add name category' CHECK(name <> '')
);
GO

CREATE TABLE product
(
	id INT IDENTITY(1, 1) PRIMARY KEY,
	name NVARCHAR(255) NOT NULL DEFAULT 'add name product' CHECK(name <> ''),
	price DECIMAL(20, 2) NOT NULL DEFAULT 0.00 CHECK(price >= 0.00),
	categoryId INT NOT NULL,
	FOREIGN KEY (categoryId) REFERENCES productCategories(id) 
	ON DELETE CASCADE
	ON UPDATE CASCADE
);
GO

INSERT INTO productCategories(name)
VALUES ('Meat'),
	('Fruit'),
	('Vegetables'),
	('Dairy foods'),
	('Fish');
GO

INSERT INTO product(name, price, categoryId)
VALUES ('Sausage', 342.43, 1),
	('Potato', 1.94, 3),
	('Flatfish', 774.21, 5),
	('Milk', 21.17, 4),
	('Lemon', 833.00, 2),
	('Apple', 11.12, 5),
	('Carrot', 11123.00, 1),
	('Lemon', 603.11, 2),
	('Potato', 2.33, 3),
	('Milk', 20.19, 4),
	('Lemon', 345.33, 2);
GO