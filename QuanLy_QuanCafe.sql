CREATE DATABASE QL_QUANCAFE

CREATE TABLE tbl_Members
(	
	MemberID NVARCHAR(6) NOT NULL Primary Key,
	MemberName NVARCHAR(100) NOT NULL,
	MemberPNumber NVARCHAR(50) NOT NULL,
	MemberRank NVARCHAR(50) NOT NULL DEFAULT 'Bronze',
)

CREATE TABLE tbl_Account
(
	Username NVARCHAR(100) NOT NULL Primary Key,	
	DisplayName NVARCHAR(100) NOT NULL,
	Pass NVARCHAR(100) NOT NULL DEFAULT 0,
	AccountType INT NOT NULL  DEFAULT 0 -- 0 = staff, 1 = ad
)

CREATE TABLE tbl_Staff
(	
	StaffID NVARCHAR(6) NOT NULL,
	StaffName NVARCHAR(100) NOT NULL,	
	PNumber NVARCHAR(50) NOT NULL,
	Position NVARCHAR(50) NOT NULL,
	Username NVARCHAR(100) NOT NULL,
	primary key(StaffID, Username)
)

CREATE TABLE tbl_FCategory
(
	FCategoryID nvarchar(6) NOT NULL PRIMARY KEY,
	FCategoryName NVARCHAR(100) NOT NULL,
)

CREATE TABLE tbl_Food
(
	FoodID nvarchar(6) NOT NULL PRIMARY KEY,
	FoodName NVARCHAR(100) NOT NULL,
	FCategoryID nvarchar(6) NOT NULL,
	Price int NOT NULL DEFAULT 0
)

CREATE TABLE tbl_Bill
(
	BillID nvarchar(6) NOT NULL PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	StaffDName NVARCHAR(100) NOT NULL,
	CustomerName NVARCHAR(100) NOT NULL DEFAULT 'Guest',
	Discount int NOT NULL,
)

CREATE TABLE tbl_BillInfo
(
	BillID nvarchar(6) NOT NULL,
	FoodID nvarchar(6) NOT NULL,
	Amount INT,
	primary key(BillID,FoodID)
)

Alter table tbl_BillInfo
Add constraint fk_bi_b foreign key (BillID) references tbl_Bill (BillID)
Alter table tbl_BillInfo
Add constraint fk_bi_f foreign key (FoodID) references tbl_Food (FoodID)
Alter table tbl_Food
Add constraint fk_f_fc foreign key (FCategoryID) references tbl_FCategory(FCategoryID)
Alter table tbl_Staff
Add constraint fk_s_a foreign key (Username) references tbl_Account(Username)


Select top 4 * from tbl_Bill order by BillID desc
select * from tbl_FCategory

INSERT INTO tbl_FCategory Values ('FC01','Milk Tea')
INSERT INTO tbl_FCategory Values ('FC02','Fruit Tea')
INSERT INTO tbl_FCategory Values ('FC03','Hot Tea')
INSERT INTO tbl_FCategory Values ('FC04','Smoothie')
INSERT INTO tbl_FCategory Values ('FC05','Classic Coffee')
INSERT INTO tbl_FCategory Values ('FC06','Vietnamese Coffee')
INSERT INTO tbl_FCategory Values ('FC07','Blended Beverage')
INSERT INTO tbl_FCategory Values ('FC08','Food & Cake')
INSERT INTO tbl_FCategory Values ('FC09','Others')

INSERT INTO tbl_Food Values ('F101','Milk Tea','FC01','40000')
INSERT INTO tbl_Food Values ('F102','Black Milk Tea','FC01','45000')
INSERT INTO tbl_Food Values ('F103','Olong Milk Tea','FC01','45000')
INSERT INTO tbl_Food Values ('F104','Peach Milk Tea','FC01','50000')
INSERT INTO tbl_Food Values ('F105','Matcha Latte','FC01','45000')

INSERT INTO tbl_Food Values ('F201','Peach Black Tea','FC02','55000')
INSERT INTO tbl_Food Values ('F202','Lychee Jasmine Tea','FC02','55000')
INSERT INTO tbl_Food Values ('F203','Lychee Lotus Tea','FC02','55000')
INSERT INTO tbl_Food Values ('F204','Longan Jasmine Tea','FC02','55000')
INSERT INTO tbl_Food Values ('F205','Longan Lotus Tea','FC02','55000')
INSERT INTO tbl_Food Values ('F206','Lemon Black Tea ','FC02','40000')
INSERT INTO tbl_Food Values ('F207','Lucky Tea','FC02','50000')
INSERT INTO tbl_Food Values ('F208','Forest Black Tea','FC02','70000')
INSERT INTO tbl_Food Values ('F209','Forest Jasmine Tea','FC02','50000')
INSERT INTO tbl_Food Values ('F210','Rose Tea','FC02','50000')
INSERT INTO tbl_Food Values ('F211','Strawberry Oolong Tea','FC02','50000')

INSERT INTO tbl_Food Values ('F301','Hot Cinnamon Cirtrus Tea','FC03','45000')
INSERT INTO tbl_Food Values ('F302','Hot Oolong Tea','FC03','40000')
INSERT INTO tbl_Food Values ('F303','Hot Lotus Tea','FC03','40000')
INSERT INTO tbl_Food Values ('F304','Hot Jasmine Tea','FC03','40000')

INSERT INTO tbl_Food Values ('F401','Snowy Lemon','FC04','45000')
INSERT INTO tbl_Food Values ('F402','Orange Tea Freeze','FC04','55000')
INSERT INTO tbl_Food Values ('F403','Red Sunset','FC04','55000')
INSERT INTO tbl_Food Values ('F404','Love Autaumn','FC04','55000')

INSERT INTO tbl_Food Values ('F501','Esspresso','FC05','45000')
INSERT INTO tbl_Food Values ('F502','Esspresso (Ice)','FC05','45000')
INSERT INTO tbl_Food Values ('F503','Cappuchino','FC05','45000')
INSERT INTO tbl_Food Values ('F504','Cappuchino (Ice)','FC05','45000')
INSERT INTO tbl_Food Values ('F505','Latte','FC05','55000')
INSERT INTO tbl_Food Values ('F506','Latte (Ice)','FC05','55000')
INSERT INTO tbl_Food Values ('F507','Cold Brew','FC05','45000')
INSERT INTO tbl_Food Values ('F508','Milk Cold Brew','FC05','45000')
INSERT INTO tbl_Food Values ('F509','Americano','FC05','45000')
INSERT INTO tbl_Food Values ('F510','Americano (Ice)','FC05','45000')

INSERT INTO tbl_Food Values ('F601','Black Coffee','FC06','35000')
INSERT INTO tbl_Food Values ('F602','Black Coffee (Ice)','FC06','35000')
INSERT INTO tbl_Food Values ('F603','Milk Coffee','FC06','35000')
INSERT INTO tbl_Food Values ('F604','Milk Coffee (Ice)','FC06','35000')
INSERT INTO tbl_Food Values ('F605','Bac Xiu','FC06','35000')

INSERT INTO tbl_Food Values ('F701','Matcha Ice Blended','FC07','60000')
INSERT INTO tbl_Food Values ('F702','Choco-Almond Crunch','FC07','60000')
INSERT INTO tbl_Food Values ('F703','Cappuchino Blast','FC07','60000')
INSERT INTO tbl_Food Values ('F704','Oreo Cappuchino Blast','FC07','60000')
INSERT INTO tbl_Food Values ('F705','Rich Caramel','FC07','60000')
INSERT INTO tbl_Food Values ('F706','Choco Lover','FC07','60000')
INSERT INTO tbl_Food Values ('F707','Peach Tea Freezee','FC07','60000')

INSERT INTO tbl_Food Values ('F801','Banh Mi','FC08','25000')
INSERT INTO tbl_Food Values ('F802','Mousse Passion Cheese','FC08','35000')
INSERT INTO tbl_Food Values ('F803','Mousse Red Velvet','FC08','35000')
INSERT INTO tbl_Food Values ('F804','Mousse Tiramisu','FC08','35000')
INSERT INTO tbl_Food Values ('F805','Mousse Cocoa','FC08','35000')

INSERT INTO tbl_Food Values ('F901','Dasani Water','FC09','10000')

INSERT INTO tbl_Account Values ('acc1','M-John Doe','811165451677',1)
INSERT INTO tbl_Account Values ('acc2','S-Layla','811165451677',0)

INSERT INTO tbl_Staff Values ('NV01','John Doe','2093482203','Manager','acc1')
INSERT INTO tbl_Staff Values ('NV02','Layla Morgan','2092042168','Cashier','acc2')

INSERT INTO tbl_Members Values ('M001','Vincent Valentino','2092021403',DEFAULT)
INSERT INTO tbl_Members Values ('M002','Konpeki','2092021031',DEFAULT)

insert into tbl_Bill values('B001',GETDATE(),'S-Layla',DEFAULT)

select * from tbl_Members where MemberName ='Vincent Valentino' and MemberPNumber ='2092021403'

create proc sp_binfoToListV
@BillID nvarchar(6)
as
begin
select FoodName, bi.Amount,Price, Price*bi.Amount as 'Total'
from tbl_Food f inner join tbl_BillInfo bi on f.FoodID = bi.FoodID
	inner join tbl_Bill b on b.BillID = bi.BillID
where b.BillID = @BillID
end

exec sp_binfoToListV 'B001'



create proc sp_Income
as
begin
select b.BillID, DateCheckIn, StaffDName, Discount,Sum(Price*bi.Amount) - Sum(Price*bi.Amount)*Discount/100 as 'Total'
from tbl_Food f inner join tbl_BillInfo bi on f.FoodID = bi.FoodID
	inner join tbl_Bill b on b.BillID = bi.BillID
group by b.BillID, DateCheckIn, StaffDName, Discount
end

exec sp_Income


create proc sp_SearchBill
@FromDate Date,
@ToDate Date
as
begin
select b.BillID, DateCheckIn, StaffDName, Discount,  Sum(Price*bi.Amount) - Sum(Price*bi.Amount)*Discount/100 as 'Total'
from tbl_Food f inner join tbl_BillInfo bi on f.FoodID = bi.FoodID
	inner join tbl_Bill b on b.BillID = bi.BillID
where Day(DateCheckIn) <= Day(@ToDate) and Month(DateCheckIn) <= Month(@ToDate) and Year(DateCheckIn) <= Year(@ToDate)
and Day(DateCheckIn) >= Day(@FromDate) and Month(DateCheckIn) >= Month(@FromDate) and Year(DateCheckIn) >= Year(@FromDate)
group by b.BillID, DateCheckIn, StaffDName, Discount
end

exec sp_SearchBill '11-14-2022', '11-14-2022'


create proc sp_SearchFood
@FoodName nvarchar(100)
as
begin
select * from tbl_Food where FoodName like @FoodName
end

exec sp_SearchFood N'%Coffee%'


create proc sp_TodayBill
@Date Date
as
begin
select BillID, DateCheckIn, StaffDName as 'Responsible'
from tbl_Bill
where Day(DateCheckIn) = Day(@Date) and Month(DateCheckIn) = Month(@Date) and Year(DateCheckIn) = Year(@Date)
order by BillID desc
end

exec sp_TodayBill ''

create proc sp_MemberTotalSpend
@CustomerName nvarchar(100)
as
begin
select CustomerName, Sum(Price*bi.Amount) - Sum(Price*bi.Amount)*Discount/100 as 'Total'
from tbl_Food f inner join tbl_BillInfo bi on f.FoodID = bi.FoodID
	inner join tbl_Bill b on b.BillID = bi.BillID
where CustomerName = @CustomerName
group by CustomerName,Discount
end

exec sp_MemberTotalSpend 'Konpeki'


create proc sp_MemberBill
@CustomerName nvarchar(100)
as
begin
select b.BillID, CustomerName, Sum(Price*bi.Amount) - Sum(Price*bi.Amount)*Discount/100 as 'Total'
from tbl_Food f inner join tbl_BillInfo bi on f.FoodID = bi.FoodID
	inner join tbl_Bill b on b.BillID = bi.BillID
where CustomerName = @CustomerName
group by b.BillID, CustomerName,Discount
end

exec sp_MemberBill 'Konpeki'

select * from tbl_Members
select * from tbl_Account where Username='acc1'

