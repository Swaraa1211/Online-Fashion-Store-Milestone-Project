select * from AspNetUsers
select * from AspNetUserRoles
select * from AspNetRoles

delete from AspNetUserRoles
delete from AspNetRoles

Insert into AspNetRoles values('admin1', 'Admin', 'ADMIN', '2023-05-02 14:30:00')
Insert into AspNetUserRoles values('eba9ba51-d26e-4609-9d80-a0ce93865034', 'admin1')


CREATE TABLE Colors (
  Color_Id INT PRIMARY KEY IDENTITY(1,1),
  Color_Name VARCHAR(255)
);

CREATE TABLE Size (
  Size_Id INT PRIMARY KEY IDENTITY(1,1),
  Size_Name VARCHAR(255)
);

CREATE TABLE Products (
  Product_Id INT PRIMARY KEY IDENTITY(1,1),
  Product_name VARCHAR(255) NOT NULL,
  Product_description VARCHAR(255),
  Product_Image VARCHAR(255),
  Color VARCHAR(255) NOT NULL,
  Size VARCHAR(20),
  Price INT
);

CREATE TABLE Carts (
 Cart_Id INT PRIMARY KEY IDENTITY(1,1),
 Product_Id INT,
 Product_name VARCHAR(255),
 Quantity INT,
 Price INT
 FOREIGN KEY (Product_Id) REFERENCES Products(Product_Id)
);

CREATE TABLE Orders(
	Order_Id INT PRIMARY KEY IDENTITY(1,1),
	Order_Date DATETIME,
	Total_Amount INT,
	User_Email VARCHAR(255)

);


ALTER TABLE Orders ADD Is_Paid VARCHAR(255)



CREATE TABLE OrderItem(
	OrderItem_Id INT PRIMARY KEY IDENTITY(1,1),
	Order_Id INT,
	Product_Id INT,
	Product_name VARCHAR(255),
	Color VARCHAR(255) NOT NULL,
    Size VARCHAR(20), 
	Quantity INT,
	Price INT,
	User_Email VARCHAR(255),
	FOREIGN KEY (Product_Id) REFERENCES Products(Product_Id),
	FOREIGN KEY (Order_Id) REFERENCES Orders(Order_Id)
);

DROP TABLE Carts
DROP TABLE Products
DROP TABLE Orders
DROP TABLE OrderItem

DELETE FROM Carts
DELETE FROM Orders
DELETE FROM OrderItem


select * from Colors
select * from Size
select * from Products
select * from Carts
select * from Orders
select * from OrderItem

Update Orders set Is_Paid = 'Paid' where Order_Id = 1

Select * from Products where Product_Id =1

CREATE OR ALTER PROCEDURE Update_Product
@Product_Id INT,
@Product_name VARCHAR(255) ,
  @Product_description VARCHAR(255),
  @Product_Image VARCHAR(255),
  @Color VARCHAR(255),
  @Size VARCHAR(20),
  @Price DECIMAL(10, 2)
AS
Update Products set Product_name=@Product_name, Product_description=@Product_description, Product_Image = @Product_Image,
	Color=@Color,Size=@Size, Price=@Price where Product_Id=@Product_Id


CREATE OR ALTER PROCEDURE Update_Product_User
@Product_Id INT,
  @Color VARCHAR(255),
  @Size VARCHAR(20)
AS
Update Products set Color=@Color,Size=@Size where Product_Id=@Product_Id




