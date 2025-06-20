IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MEETEA')
BEGIN
  CREATE DATABASE MEETEA;
END;
GO

USE MEETEA
GO


CREATE TABLE category(
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	title NVARCHAR(100)
)
GO

CREATE TABLE product(
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	title NVARCHAR(100),
	img NVARCHAR(200),
	price INT,
	rate FLOAT,
	createAt DATETIME,
	updateAt DATETIME,
	categoryId int,
	FOREIGN KEY (categoryId) REFERENCES category(id)
)
GO

CREATE TABLE customer(
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	firstName NVARCHAR(100),
	lastName NVARCHAR(100),
	address NVARCHAR(500),
	phone NVARCHAR(15),
	email NVARCHAR(50),
	img NVARCHAR(200),
	registeredAt DATETIME,
	updateAt DATETIME,
	dateOfBirth DATE null,	
	password NVARCHAR(200),
	randomKey varchar (50) null,
	isActive BIT,
	role INT  --  0: user, 1: admin,
)
GO

CREATE TABLE  cart (
	id INT  NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	customerId INT,
	createAt DATETIME,
	productId INT,
	quantity INT,
	FOREIGN KEY (customerId) REFERENCES customer(id),
	FOREIGN KEY (productId) REFERENCES product(id)
)
GO

CREATE TABLE  payment (
	id INT  NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	customerId INT,
	firstName NVARCHAR(100),
	lastName NVARCHAR(100),
	phone NVARCHAR(15),
	email NVARCHAR(50),
	createAt DATETIME,
	total FLOAT,
	FOREIGN KEY (customerId) REFERENCES customer(id)
)
GO

CREATE TABLE paymentDetail (
	productId INT,
	paymentId INT,
	price INT,
	quantity INT,
	total FLOAT,
	createAt DATETIME,
	FOREIGN KEY (productId) REFERENCES product(id),
	FOREIGN KEY (paymentId) REFERENCES payment(id)
)
GO

CREATE TABLE menu(
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	parentId INT NULL,
	title NVARCHAR(100),
	menuUrl NVARCHAR(100),
	menuIndex int,
	isVisible BIT DEFAULT 1, --1: DISPLAY || 0: HIDDEN
)
GO


CREATE TABLE order_detail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT, -- dùng để nhóm các dòng thuộc cùng 1 đơn hàng
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT N'Pending',

    -- Thông tin khách hàng
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Address NVARCHAR(500),
    Phone NVARCHAR(15),

    -- Thông tin sản phẩm
    ProductId INT,
    ProductTitle NVARCHAR(100),
    Quantity INT,
    UnitPrice FLOAT,
    Total FLOAT
);




---------------------- INSERT category -----------------
SET IDENTITY_INSERT [dbo].[category] ON 

INSERT [dbo].[category] ([id], [title]) VALUES (1, N'TRÀ SỮA MEE TEA')
INSERT [dbo].[category] ([id], [title]) VALUES (2, N'SỮA TƯƠI')
INSERT [dbo].[category] ([id], [title]) VALUES (3, N'TRÀ TRÁI CÂY')
INSERT [dbo].[category] ([id], [title]) VALUES (4, N'SỮA CHUA')
INSERT [dbo].[category] ([id], [title]) VALUES (5, N'CÀ PHÊ')
INSERT [dbo].[category] ([id], [title]) VALUES (6, N'SODA')
INSERT [dbo].[category] ([id], [title]) VALUES (7, N'ĂN VẶT')
SET IDENTITY_INSERT [dbo].[category] OFF
GO


---------------------- INSERT product -----------------
SET IDENTITY_INSERT [dbo].[product] ON 
INSERT [dbo].[product] ([id], [title], [img], [price], [rate], [createAt], [updateAt], [categoryId]) VALUES 
(1, N'Trà Sữa Thạch Phô Mai Tươi', N'TraSuaThachPhoMai.jpg', 30000, 4.9, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(2, N'Trà Sữa Phô Mai', N'TraSuaPhoMai.jpg', 19000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(3, N'Trà Sữa Socola', N'TraSuaSocola.jpg', 35000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(4, N'Trà Sữa Oreo Kem Trứng', N'TraSuaOreoKemTrung.jpg', 35000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(5, N'Trà Sữa Matcha', N'TraSuaMatcha.jpg', 30000, 4.8, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(6, N'Trà Sữa Khúc Bạch', N'TraSuaKhucBach.jpg', 19000, 4.5 , N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(7, N'Trà Sữa Khoai Môn Kem Macchiato', N'TraSuaKhoaiMon.jpg', 38000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(8, N'Trà Sữa Keo Bông', N'TraSuaPhoMai.jpg', 32000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(9, N'Trà Sữa Kem Trứng Bánh Milo', N'TraSuaKemTrung.jpg', 35000, 4.9, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),
(10, N'Trà Sữa Đặc Biệt', N'TraSuaDacBiet.jpg', 30000, 4.8, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 1),

(11, N'Coffee Latte Sữa Gấu', N'CoffeeLatte.jpg', 28000, 5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 2),
(12, N'Choco Latte Sữa Gấu', N'ChocoLatte.jpg', 25000, 5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 2),
(13, N'Socola Latte Sữa Gấu', N'SocolaLatte.jpg', 28000, 5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 2),
(14, N'Matcha Latte Sữa Gấu', N'MatchaLatte.jpg', 25000, 5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 2),
(15, N'Matcha Dâu Latte Sữa Gấu', N'MatchaDauLatte.jpg', 25000, 5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 2),
(16, N'Sữa Tươi Milo', N'SuaTuoiMilo.jpg', 25000, 5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 2),
(17, N'Sữa Tưới Trân Châu Đường Đen', N'SuaTuoiDuongDen.jpg', 28000, 5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 2),

(18, N'Trà Ổi Hồng', N'TraOi.jpg', 19000, 4.3, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(19, N'Trà Trái Cây Nhiệt Đới', N'TraNhietDoi.jpg', 25000, 4.8, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(20, N'Trà Me Đác Thơm', N'TraMeDacThom.jpg', 20000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(21, N'Trà Mãng Cầu', N'TraMangCau.jpg', 25000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(22, N'Trà Mãng Cầu Dâu', N'TraMangCauDau.jpg', 28000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(23, N'Trà Lựu Anh Đào ', N'TraLuu.jpg', 25000, 4.9,  N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(24, N'Trà Chanh Dây', N'TraChanhDay.jpg', 19000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(25, N'Trà Dứa Thạch Trái Cây', N'TraDua.jpg', 19000, 4.3, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(26, N'Trà Đào', N'TraDao.jpg', 20000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(27, N'Trà Chanh Nha Đam', N'TraChanhNhaDam.jpg', 19000, 4.9, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),
(28, N'Trà Việt Quất', N'TraVietQuat.jpg', 19000, 4.9, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 3),

(29, N'Sữa Chua Việt Quất', N'SuaChuaVietQuat.jpg', 30000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 4),
(30, N'Sữa Chua Phúc Bồn Tử', N'SuaChuaPhucBonTu.jpg', 32000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 4),
(31, N'Sữa Chua Trái Cây', N'SuaChuaTraiCay.jpg', 35000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 4),
(32, N'Sữa Chua Dâu', N'SuaChuaDau.jpg', 30000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 4),
(33, N'Sữa Chua Cam', N'SuaChuaCam.jpg', 30000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 4),
(34, N'Sữa Chua Đào', N'SuaChuaDao.jpg', 30000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 4),

(35, N'Bạc Xỉu', N'BacXiu.jpg', 19000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 5),
(36, N'Bạc Xỉu Kem Muối', N'BacXiuKemMuoi.jpg', 22000, 4.8, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 5),
(37, N'Bạc Xỉu Kem Trứng', N'BacXiukemTrung.jpg', 25000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 5),

(38, N'Soda Ocean', N'SodaOcean.jpg', 25000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 6),
(39, N'Soda Bạc Hà', N'SodaBacHa.jpg', 25000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 6),
(40, N'Soda Táo', N'SodaTao.jpg', 25000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 6),
(41, N'Soda Đào', N'SodaDao.jpg', 25000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 6),

(42, N'Bánh Que Sốt Kem Trứng', N'BanhQueSotKemTrung.jpg', 15000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 7),
(43, N'Bánh Tráng Chấm', N'BanhTrangCham.jpg', 15000, 4.5, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 7),
(44, N'Bánh Tráng Cuốn Trộn', N'BanhTrangCuonTron.jpg', 25000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 7),
(45, N'Bánh Tráng Ủ Bơ Trộn Mỡ Hành', N'BanhTrangMoHanh.jpg', 20000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 7),
(46, N'Panna Cotta Dâu', N'PannaCottaDau.jpg', 20000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 7),
(47, N'Panna Cotta Xoài', N'PannaCottaXoai.jpg', 20000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 7),
(48, N'Panna Cotta Việt Quất', N'PannaCottaVietQuat.jpg', 20000, 5.0, N'2025-06-07 06:47:22.870', N'2025-06-07 06:47:22.870', 7)

SET IDENTITY_INSERT [dbo].[product] OFF
GO


---------------------- INSERT customer -----------------
SET IDENTITY_INSERT [dbo].[customer] ON 

INSERT [dbo].[customer] ([id], [firstName], [lastName], [address], [phone], [email], [img], [registeredAt], [updateAt], [dateOfBirth], [password], [randomKey], [isActive], [role]) 
VALUES (1, N'Tran', N'User', N'50 Ngô Gia Tự, Nha Trang, Khánh Hòa', N'09631047349', N'user123@gmail.com', N'avatar-default.jpg', CAST(N'2025-06-02 18:04:22.963' AS DateTime), CAST(N'2025-06-02 18:04:22.963' AS DateTime), CAST(N'2000-03-02' AS Date), N'773dbb14478480bfb4599607dea0b4cb', N'sKGgv', 1, 0),
	   (2, N'Admin', N'Nguyen', N'104 Nguyễn Trãi, Nha Trang, Khánh Hòa', N'0385247684', N'admin@gmail.com', N'avatar-default.jpg', CAST(N'2025-06-05 16:41:37.290' AS DateTime), CAST(N'2025-06-05 16:41:37.290' AS DateTime), CAST(N'1995-01-01' AS Date), N'ba1523dd75f80c51ebdb837823090351', N'G*kY*', 1, 1)

SET IDENTITY_INSERT [dbo].[customer] OFF
GO
---------------------- INSERT cart -----------------

---------------------- INSERT payment -----------------

---------------------- INSERT paymentDetail -----------------

---------------------- INSERT menu -----------------
SET IDENTITY_INSERT [dbo].[menu] ON 

INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (1, NULL, N'TRANG CHỦ', N'/', 1, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (2, NULL, N'CỬA HÀNG', N'/Product', 2, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (3, NULL, N'DANH MỤC SẢN PHẨM', NULL, 3, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (4, 3, N'TRÀ SỮA MEE TEA', N'/Product?idcategory=1', 4, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (5, 3, N'SỮA TƯƠI', N'/Product?idcategory=2', 5, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (6, 3, N'TRÀ TRÁI CÂY', N'/Product?idcategory=3', 6, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (7, 3, N'SỮA CHUA', N'/Product?idcategory=4', 7, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (8, 3, N'CÀ PHÊ', N'/Product?idcategory=5', 8, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (9, 3, N'SODA', N'/Product?idcategory=6', 9, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (10, 3, N'ĂN VẶT', N'/Product?idcategory=7', 10, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (11, NULL, N'LIÊN HỆ', N'/contact', 11, 1)
INSERT [dbo].[menu] ([id], [parentId], [title], [menuUrl], [menuIndex], [isVisible]) VALUES (12, NULL, N'TRANG QUẢN TRỊ', N'/Admin/ProductAdmin', 12, 1)
SET IDENTITY_INSERT [dbo].[menu] OFF
GO

SELECT GETDATE() AS nowDateTime
DROP TABLE IF EXISTS category;
DROP TABLE IF EXISTS product;
DROP TABLE IF EXISTS customer;
DROP TABLE IF EXISTS cart;
DROP TABLE IF EXISTS payment;
DROP TABLE IF EXISTS paymentDetail;
DROP TABLE IF EXISTS menu;
DROP TABLE IF EXISTS order_detail;



-- Select * from customer;
 Select * from order_detail;

