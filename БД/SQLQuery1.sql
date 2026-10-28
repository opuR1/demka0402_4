create table Roles(
	RoleId int primary key identity(1,1),
	RoleName nvarchar(50) not null
);

create table Users(
	UserId int primary key identity(1,1),
	RoleId int foreign key references Roles(RoleId) not null,
	LastName nvarchar(100) not null,
	FirstName nvarchar(100) not null,
	MiddleName nvarchar(100),
	Login nvarchar(255) not null,
	Password nvarchar(255) not null
);

create table Cities(
	CityId int primary key identity(1,1),
	CityName nvarchar(50) not null
);

create table PickupPoints(
	PointId int primary key identity(1,1),
	PointIndex nvarchar(100),
	CityId int foreign key references Cities(CityId) not null,
	Street nvarchar(255), 
	House nvarchar(100)
);

create table Units(
	UnitId int primary key identity(1,1),
	UnitName nvarchar(50) not null
);

create table Suppliers(
	SupplierId int primary key identity(1,1),
	SupplierName nvarchar(50) not null
);

create table Producers(
	ProducerId int primary key identity(1,1),
	ProducerName nvarchar(50) not null
);

create table Categories(
	CategoryId int primary key identity(1,1),
	CategoryName nvarchar(50) not null
);

create table Products(
	ProductId int primary key identity(1,1),
	ProductArtycle nvarchar(100),
	ProductName nvarchar(50) not null,
	UnitId int foreign key references Units(UnitId) not null,
	Price decimal(10,2),
	SupplierId int foreign key references Suppliers(SupplierId) not null,
	ProducerId int foreign key references Producers(ProducerId) not null,
	CategoryId int foreign key references Categories(CategoryId) not null,
	Discount int,
	StorageCount int,
	Desciption nvarchar(500),
	Photo nvarchar(255)
);

create table OrderStatuses(
	StatusId int primary key identity(1,1),
	StatusName nvarchar(50) not null
);

create table Orders(
	OrderId int primary key identity(1,1),
	OrderDate date,
	DeliveryDate date,
	PointId int foreign key references PickupPoints(PointId) not null,
	UserId int foreign key references Users(UserId) not null,
	Code nvarchar(100),
	StatusId int foreign key references OrderStatuses(StatusId) not null
);

create table OrdersInfo(
	OrderInfoId int primary key identity(1,1),
	OrderId int foreign key references Orders(OrderId) not null,
	ProductId int foreign key references Products(ProductId) not null,
	ProductCount int
);

