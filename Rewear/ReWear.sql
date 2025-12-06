

CREATE DATABASE Rewear;
GO

USE Rewear;
GO



CREATE TABLE Users
(
    UserId INT IDENTITY(1,1)        NOT NULL PRIMARY KEY, -- PK
    FullName NVARCHAR(100)          NOT NULL,
    Email NVARCHAR(255)             NOT NULL,
    PasswordHash NVARCHAR(255)      NOT NULL,
    Phone NVARCHAR(20)              NULL,

    City NVARCHAR(100)              NOT NULL,
   
    UserType NVARCHAR(20)           NOT NULL
        CONSTRAINT CK_Users_UserType CHECK (UserType IN ('Regular', 'Admin')),

    CreatedAt DATETIME              NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT (GETDATE()),
    IsActive BIT                    NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT (1),

    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);

CREATE TABLE Store
(
    StoreId INT IDENTITY(1,1)       NOT NULL PRIMARY KEY, 
    OwnerUserId INT                 NOT NULL,             
    StoreName NVARCHAR(100)         NOT NULL,
    
    Purpose NVARCHAR(20)            NOT NULL
        CONSTRAINT CK_Stores_Purpose CHECK (Purpose IN ('Donation', 'Recycling')),
      
    City NVARCHAR(100)              NOT NULL,

    CreatedAt DATETIME              NOT NULL
        CONSTRAINT DF_Stores_CreatedAt DEFAULT (GETDATE()),
    IsActive BIT                    NOT NULL
        CONSTRAINT DF_Stores_IsActive DEFAULT (1),

    CONSTRAINT FK_Stores_OwnerUser
        FOREIGN KEY (OwnerUserId) REFERENCES Users(UserId)
);

CREATE TABLE ClothingItems
(
    ItemId INT IDENTITY(1,1)        NOT NULL PRIMARY KEY, 
    
    OwnerUserId INT                 NOT NULL,             
    StoreId INT                     NULL,                 

    Title NVARCHAR(100)             NOT NULL,
    Description NVARCHAR(500)       NULL,
    Category NVARCHAR(50)           NOT NULL,             
    Size NVARCHAR(20)               NOT NULL,
    Condition NVARCHAR(50)          NOT NULL,     
    color NVARCHAR(25)              NOT NULL,
    
    Price DECIMAL(10,2)             NULL,                 
    
    Status NVARCHAR(20)             NOT NULL
        CONSTRAINT CK_Items_Status CHECK (Status IN 
            ('Available','Reserved','Sold','Donated','Recycled','Expired')),
    
    ExpirationDate DATETIME         NULL,                 
    CreatedAt DATETIME              NOT NULL
        CONSTRAINT DF_Items_CreatedAt DEFAULT (GETDATE()),

    DestinationStoreId INT          NULL,                 

    CONSTRAINT FK_Items_OwnerUser
        FOREIGN KEY (OwnerUserId) REFERENCES Users(UserId),

    CONSTRAINT FK_Items_Store
        FOREIGN KEY (StoreId) REFERENCES Store(StoreId),

    CONSTRAINT FK_Items_DestinationStore
        FOREIGN KEY (DestinationStoreId) REFERENCES Store(StoreId)
);

CREATE TABLE Orders
(
    OrderId INT IDENTITY(1,1)       NOT NULL PRIMARY KEY, -- PK

    ItemId INT                      NOT NULL,            
    BuyerUserId INT                 NOT NULL,             
    SellerUserId INT                NOT NULL,             

    OrderDate DATETIME              NOT NULL
        CONSTRAINT DF_Orders_OrderDate DEFAULT (GETDATE()),
    
    TotalAmount DECIMAL(10,2)       NOT NULL,

    Status NVARCHAR(20)             NOT NULL
        CONSTRAINT CK_Orders_Status CHECK (Status IN 
            ('Pending','Paid','Cancelled','Completed')),

    CONSTRAINT FK_Orders_Item
        FOREIGN KEY (ItemId) REFERENCES ClothingItems(ItemId),

    CONSTRAINT FK_Orders_Buyer
        FOREIGN KEY (BuyerUserId) REFERENCES Users(UserId),

    CONSTRAINT FK_Orders_Seller
        FOREIGN KEY (SellerUserId) REFERENCES Users(UserId)
);

CREATE TABLE Payments
(
    PaymentId INT IDENTITY(1,1)     NOT NULL PRIMARY KEY, 

    OrderId INT                     NOT NULL,             
    Amount DECIMAL(10,2)            NOT NULL,
    PaymentDate DATETIME            NOT NULL
        CONSTRAINT DF_Payments_PaymentDate DEFAULT (GETDATE()),

    PaymentMethod NVARCHAR(20)      NOT NULL
        CONSTRAINT CK_Payments_Method CHECK (PaymentMethod IN 
            ('CreditCard','Cash','PayPal')),
    
    Status NVARCHAR(20)             NOT NULL
        CONSTRAINT CK_Payments_Status CHECK (Status IN 
            ('Pending','Paid','Failed','Refunded')),

    CONSTRAINT FK_Payments_Order
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

CREATE TABLE ShippingCompanies
(
    ShippingCompanyId INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 

    Name NVARCHAR(100)            NOT NULL,
    Phone NVARCHAR(20)            NULL,
    IsActive BIT                  NOT NULL
        CONSTRAINT DF_ShippingCompanies_IsActive DEFAULT (1)
);

CREATE TABLE Deliveries
(
    DeliveryId INT IDENTITY(1,1)    NOT NULL PRIMARY KEY, 

    OrderId INT                     NOT NULL,            
    ShippingCompanyId INT           NULL,                 

    DeliveryMethod NVARCHAR(20)     NOT NULL
        CONSTRAINT CK_Deliveries_Method CHECK (DeliveryMethod IN
            ('Pickup','Shipping','Locker')),

    City NVARCHAR(100)              NOT NULL,
    StreetAddress NVARCHAR(255)     NOT NULL,

    TrackingNumber NVARCHAR(100)    NULL,

    Status NVARCHAR(20)             NOT NULL
        CONSTRAINT CK_Deliveries_Status CHECK (Status IN
            ('Pending','Shipped','Delivered','Cancelled')),

    CreatedAt DATETIME              NOT NULL
        CONSTRAINT DF_Deliveries_CreatedAt DEFAULT (GETDATE()),

    CONSTRAINT FK_Deliveries_Order
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),

    CONSTRAINT FK_Deliveries_ShippingCompany
        FOREIGN KEY (ShippingCompanyId) REFERENCES ShippingCompanies(ShippingCompanyId)
);

--------------------------------------------------
-- שלב 2: הכנסת נתונים (תואם בדיוק לטבלאות האלה)
--------------------------------------------------

-- Users
INSERT INTO Users
    (FullName, Email, PasswordHash, Phone, City, UserType)
VALUES
    (N'נועה כהן',    N'noa@example.com',    N'hashed_pw_1', N'050-1111111', N'תל אביב',      N'Regular'), -- 1
    (N'דניאל לוי',   N'daniel@example.com', N'hashed_pw_2', N'052-2222222', N'חיפה',          N'Regular'), -- 2
    (N'יואב ישראלי', N'yoav@example.com',   N'hashed_pw_3', N'053-3333333', N'ירושלים',       N'Regular'), -- 3
    (N'מיכל ברק',    N'michal@example.com', N'hashed_pw_4', N'054-4444444', N'ראשון לציון',  N'Regular'), -- 4
    (N'עדי רוזן',    N'adi@example.com',    N'hashed_pw_5', N'055-5555555', N'באר שבע',       N'Regular'), -- 5
    (N'אורן אדמינוב',N'admin@example.com',  N'admin_pw_1',  N'050-0000000', N'תל אביב',      N'Admin');   -- 6

-- Store
INSERT INTO Store
    (OwnerUserId, StoreName, Purpose, City)
VALUES
    (1, N'לב העיר - תרומות',   N'Donation',  N'תל אביב'),  -- 1
    (2, N'ירוק בחיפה - מחזור', N'Recycling', N'חיפה'),      -- 2
    (5, N'לב הדרום - תרומות',  N'Donation',  N'באר שבע');  -- 3

-- ShippingCompanies
INSERT INTO ShippingCompanies
    (Name, Phone)
VALUES
    (N'אקספרס שילוח',  N'03-9000000'),  -- 1
    (N'דואר מהיר פלוס', N'03-8000000'); -- 2

-- ClothingItems
INSERT INTO ClothingItems
    (OwnerUserId, StoreId, Title, Description, Category, Size, Condition, color,
     Price, Status, ExpirationDate, DestinationStoreId)
VALUES
    (3, NULL,
     N'ג׳ינס סקיני כחול', N'ג׳ינס במצב מצוין, נלבש מעט',
     N'מכנסיים', N'M', N'כמו חדש', N'כחול',
     80.00, N'Available', '2025-12-31', 1),

    (4, NULL,
     N'שמלת ערב שחורה', N'שמלה אלגנטית לאירועים',
     N'שמלה', N'S', N'חדש', N'שחור',
     150.00, N'Available', '2025-11-30', 1),

    (1, NULL,
     N'טי-שירט לבנה', N'טי בסיסית, כותנה 100%',
     N'חולצה', N'M', N'מצב טוב', N'לבן',
     40.00, N'Sold', '2025-10-01', 1),

    (2, 1,
     N'מעיל חורף אפור', N'מעיל עבה לחורף, מחמם מאוד',
     N'מעיל', N'L', N'מצב טוב', N'אפור',
     NULL, N'Donated', '2025-09-01', 1),

    (5, 2,
     N'סניקרס ישנות', N'סניקרס משומשות, מתאימות למחזור',
     N'נעליים', N'42', N'בלוי', N'אפור',
     NULL, N'Recycled', '2025-08-01', 2);

-- Orders
INSERT INTO Orders
    (ItemId, BuyerUserId, SellerUserId, TotalAmount, Status)
VALUES
    (3, 2, 1, 40.00,  N'Completed'),
    (1, 4, 3, 80.00,  N'Paid'),
    (2, 5, 4, 150.00, N'Pending');

-- Payments
INSERT INTO Payments
    (OrderId, Amount, PaymentMethod, Status)
VALUES
    (1, 40.00,  N'CreditCard', N'Paid'),
    (2, 80.00,  N'CreditCard', N'Paid'),
    (3, 150.00, N'CreditCard', N'Failed');

-- Deliveries
INSERT INTO Deliveries
    (OrderId, ShippingCompanyId, DeliveryMethod,
     City, StreetAddress, TrackingNumber, Status)
VALUES
    (1, 1, N'Shipping',
     N'חיפה', N'הרצל 5', N'TRK123456', N'Delivered'),

    (2, NULL, N'Pickup',
     N'תל אביב', N'דיזנגוף 100', NULL, N'Pending'),

    (3, 2, N'Shipping',
     N'באר שבע', N'רגר 10', N'TRK999999', N'Shipped');


-- כל המשתמשים
CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT 
        UserId,
        FullName,
        Email,
        Phone,
        City,
        UserType,
        CreatedAt,
        IsActive
    FROM Users;
END
GO

-- משתמש לפי מזהה
CREATE PROCEDURE GetUserById
    @UserId INT
AS
BEGIN
    SELECT 
        UserId,
        FullName,
        Email,
        Phone,
        City,
        UserType,
        CreatedAt,
        IsActive
    FROM Users
    WHERE UserId = @UserId;
END
GO

-- הוספת משתמש
CREATE PROCEDURE AddUser
    @FullName      NVARCHAR(100),
    @Email         NVARCHAR(255),
    @PasswordHash  NVARCHAR(255),
    @Phone         NVARCHAR(20) = NULL,
    @City          NVARCHAR(100),
    @UserType      NVARCHAR(20) = N'Regular'  -- 'Regular' / 'Admin'
AS
BEGIN
    INSERT INTO Users
    (
        FullName,
        Email,
        PasswordHash,
        Phone,
        City,
        UserType
    )
    VALUES
    (
        @FullName,
        @Email,
        @PasswordHash,
        @Phone,
        @City,
        @UserType
    );

    SELECT SCOPE_IDENTITY() AS NewUserId;
END
GO

-- עדכון פרטי משתמש
CREATE PROCEDURE UpdateUser
    @UserId       INT,
    @FullName     NVARCHAR(100),
    @Phone        NVARCHAR(20) = NULL,
    @City         NVARCHAR(100),
    @UserType     NVARCHAR(20),
    @IsActive     BIT
AS
BEGIN
    UPDATE Users
    SET 
        FullName = @FullName,
        Phone    = @Phone,
        City     = @City,
        UserType = @UserType,
        IsActive = @IsActive
    WHERE UserId = @UserId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- כל החנויות
CREATE PROCEDURE GetAllStores
AS
BEGIN
    SELECT 
        StoreId,
        OwnerUserId,
        StoreName,
        Purpose,
        City,
        CreatedAt,
        IsActive
    FROM Store;
END
GO

-- חנות לפי מזהה
CREATE PROCEDURE GetStoreById
    @StoreId INT
AS
BEGIN
    SELECT 
        StoreId,
        OwnerUserId,
        StoreName,
        Purpose,
        City,
        CreatedAt,
        IsActive
    FROM Store
    WHERE StoreId = @StoreId;
END
GO

-- כל החנויות של בעל מסוים (כמו GetBooksByAuthor)
CREATE PROCEDURE GetStoresByOwnerUserId
    @OwnerUserId INT
AS
BEGIN
    SELECT 
        StoreId,
        OwnerUserId,
        StoreName,
        Purpose,
        City,
        CreatedAt,
        IsActive
    FROM Store
    WHERE OwnerUserId = @OwnerUserId;
END
GO

-- הוספת חנות
CREATE PROCEDURE AddStore
    @OwnerUserId INT,
    @StoreName   NVARCHAR(100),
    @Purpose     NVARCHAR(20),
    @City        NVARCHAR(100)
AS
BEGIN
    INSERT INTO Store
    (
        OwnerUserId,
        StoreName,
        Purpose,
        City
    )
    VALUES
    (
        @OwnerUserId,
        @StoreName,
        @Purpose,
        @City
    );

    SELECT SCOPE_IDENTITY() AS NewStoreId;
END
GO

-- עדכון חנות
CREATE PROCEDURE UpdateStore
    @StoreId     INT,
    @StoreName   NVARCHAR(100),
    @Purpose     NVARCHAR(20),
    @City        NVARCHAR(100),
    @IsActive    BIT
AS
BEGIN
    UPDATE Store
    SET
        StoreName = @StoreName,
        Purpose   = @Purpose,
        City      = @City,
        IsActive  = @IsActive
    WHERE StoreId = @StoreId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- כל פרטי הלבוש + הצגת בעלים וחנויות
CREATE PROCEDURE GetAllClothingItems
AS
BEGIN
    SELECT 
        CI.ItemId,
        CI.OwnerUserId,
        U.FullName AS OwnerFullName,
        CI.StoreId,
        S.StoreName AS StoreName,
        CI.Title,
        CI.Description,
        CI.Category,
        CI.Size,
        CI.Condition,
        CI.color,
        CI.Price,
        CI.Status,
        CI.ExpirationDate,
        CI.CreatedAt,
        CI.DestinationStoreId,
        DS.StoreName AS DestinationStoreName
    FROM ClothingItems CI
    INNER JOIN Users U
        ON CI.OwnerUserId = U.UserId
    LEFT JOIN Store S
        ON CI.StoreId = S.StoreId
    LEFT JOIN Store DS
        ON CI.DestinationStoreId = DS.StoreId;
END
GO

-- פריט לבוש לפי מזהה
CREATE PROCEDURE GetClothingItemById
    @ItemId INT
AS
BEGIN
    SELECT 
        CI.ItemId,
        CI.OwnerUserId,
        U.FullName AS OwnerFullName,
        CI.StoreId,
        S.StoreName AS StoreName,
        CI.Title,
        CI.Description,
        CI.Category,
        CI.Size,
        CI.Condition,
        CI.color,
        CI.Price,
        CI.Status,
        CI.ExpirationDate,
        CI.CreatedAt,
        CI.DestinationStoreId,
        DS.StoreName AS DestinationStoreName
    FROM ClothingItems CI
    INNER JOIN Users U
        ON CI.OwnerUserId = U.UserId
    LEFT JOIN Store S
        ON CI.StoreId = S.StoreId
    LEFT JOIN Store DS
        ON CI.DestinationStoreId = DS.StoreId
    WHERE CI.ItemId = @ItemId;
END
GO

-- כל הפריטים של משתמש מסוים (כמו GetBooksByAuthor)
CREATE PROCEDURE GetClothingItemsByOwnerUserId
    @OwnerUserId INT
AS
BEGIN
    SELECT 
        ItemId,
        OwnerUserId,
        StoreId,
        Title,
        Description,
        Category,
        Size,
        Condition,
        color,
        Price,
        Status,
        ExpirationDate,
        CreatedAt,
        DestinationStoreId
    FROM ClothingItems
    WHERE OwnerUserId = @OwnerUserId;
END
GO

-- כל הפריטים בחנות מסוימת
CREATE PROCEDURE GetClothingItemsByStoreId
    @StoreId INT
AS
BEGIN
    SELECT 
        ItemId,
        OwnerUserId,
        StoreId,
        Title,
        Description,
        Category,
        Size,
        Condition,
        color,
        Price,
        Status,
        ExpirationDate,
        CreatedAt,
        DestinationStoreId
    FROM ClothingItems
    WHERE StoreId = @StoreId;
END
GO

-- הוספת פריט לבוש
CREATE PROCEDURE AddClothingItem
    @OwnerUserId        INT,
    @StoreId            INT             = NULL,
    @Title              NVARCHAR(100),
    @Description        NVARCHAR(500)   = NULL,
    @Category           NVARCHAR(50),
    @Size               NVARCHAR(20),
    @Condition          NVARCHAR(50),
    @Color              NVARCHAR(25),
    @Price              DECIMAL(10,2)   = NULL,
    @Status             NVARCHAR(20),
    @ExpirationDate     DATETIME        = NULL,
    @DestinationStoreId INT             = NULL
AS
BEGIN
    INSERT INTO ClothingItems
    (
        OwnerUserId,
        StoreId,
        Title,
        Description,
        Category,
        Size,
        Condition,
        color,
        Price,
        Status,
        ExpirationDate,
        DestinationStoreId
    )
    VALUES
    (
        @OwnerUserId,
        @StoreId,
        @Title,
        @Description,
        @Category,
        @Size,
        @Condition,
        @Color,
        @Price,
        @Status,
        @ExpirationDate,
        @DestinationStoreId
    );

    SELECT SCOPE_IDENTITY() AS NewItemId;
END
GO

-- עדכון פריט לבוש
CREATE PROCEDURE UpdateClothingItem
    @ItemId            INT,
    @OwnerUserId       INT,
    @StoreId           INT             = NULL,
    @Title             NVARCHAR(100),
    @Description       NVARCHAR(500)   = NULL,
    @Category          NVARCHAR(50),
    @Size              NVARCHAR(20),
    @Condition         NVARCHAR(50),
    @Color             NVARCHAR(25),
    @Price             DECIMAL(10,2)   = NULL,
    @Status            NVARCHAR(20),
    @ExpirationDate    DATETIME        = NULL,
    @DestinationStoreId INT            = NULL
AS
BEGIN
    UPDATE ClothingItems
    SET
        OwnerUserId        = @OwnerUserId,
        StoreId            = @StoreId,
        Title              = @Title,
        Description        = @Description,
        Category           = @Category,
        Size               = @Size,
        Condition          = @Condition,
        color              = @Color,
        Price              = @Price,
        Status             = @Status,
        ExpirationDate     = @ExpirationDate,
        DestinationStoreId = @DestinationStoreId
    WHERE ItemId = @ItemId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO


-- כל ההזמנות + פרטי פריט וקונים/מוכרים
CREATE PROCEDURE GetAllOrders
AS
BEGIN
    SELECT 
        O.OrderId,
        O.ItemId,
        CI.Title AS ItemTitle,
        O.BuyerUserId,
        BU.FullName AS BuyerFullName,
        O.SellerUserId,
        SU.FullName AS SellerFullName,
        O.OrderDate,
        O.TotalAmount,
        O.Status
    FROM Orders O
    INNER JOIN ClothingItems CI
        ON O.ItemId = CI.ItemId
    INNER JOIN Users BU
        ON O.BuyerUserId = BU.UserId
    INNER JOIN Users SU
        ON O.SellerUserId = SU.UserId;
END
GO

-- הזמנה לפי מזהה
CREATE PROCEDURE GetOrderById
    @OrderId INT
AS
BEGIN
    SELECT 
        O.OrderId,
        O.ItemId,
        CI.Title AS ItemTitle,
        O.BuyerUserId,
        BU.FullName AS BuyerFullName,
        O.SellerUserId,
        SU.FullName AS SellerFullName,
        O.OrderDate,
        O.TotalAmount,
        O.Status
    FROM Orders O
    INNER JOIN ClothingItems CI
        ON O.ItemId = CI.ItemId
    INNER JOIN Users BU
        ON O.BuyerUserId = BU.UserId
    INNER JOIN Users SU
        ON O.SellerUserId = SU.UserId
    WHERE O.OrderId = @OrderId;
END
GO

-- הזמנות של קונה מסוים
CREATE PROCEDURE GetOrdersByBuyerUserId
    @BuyerUserId INT
AS
BEGIN
    SELECT 
        OrderId,
        ItemId,
        BuyerUserId,
        SellerUserId,
        OrderDate,
        TotalAmount,
        Status
    FROM Orders
    WHERE BuyerUserId = @BuyerUserId;
END
GO

-- הזמנות של מוכר מסוים
CREATE PROCEDURE GetOrdersBySellerUserId
    @SellerUserId INT
AS
BEGIN
    SELECT 
        OrderId,
        ItemId,
        BuyerUserId,
        SellerUserId,
        OrderDate,
        TotalAmount,
        Status
    FROM Orders
    WHERE SellerUserId = @SellerUserId;
END
GO

-- הוספת הזמנה
CREATE PROCEDURE AddOrder
    @ItemId        INT,
    @BuyerUserId   INT,
    @SellerUserId  INT,
    @TotalAmount   DECIMAL(10,2),
    @Status        NVARCHAR(20) = N'Pending'
AS
BEGIN
    INSERT INTO Orders
    (
        ItemId,
        BuyerUserId,
        SellerUserId,
        TotalAmount,
        Status
    )
    VALUES
    (
        @ItemId,
        @BuyerUserId,
        @SellerUserId,
        @TotalAmount,
        @Status
    );

    SELECT SCOPE_IDENTITY() AS NewOrderId;
END
GO

-- עדכון הזמנה
CREATE PROCEDURE UpdateOrder
    @OrderId       INT,
    @ItemId        INT,
    @BuyerUserId   INT,
    @SellerUserId  INT,
    @TotalAmount   DECIMAL(10,2),
    @Status        NVARCHAR(20)
AS
BEGIN
    UPDATE Orders
    SET
        ItemId       = @ItemId,
        BuyerUserId  = @BuyerUserId,
        SellerUserId = @SellerUserId,
        TotalAmount  = @TotalAmount,
        Status       = @Status
    WHERE OrderId = @OrderId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO


-- כל התשלומים
CREATE PROCEDURE GetAllPayments
AS
BEGIN
    SELECT 
        PaymentId,
        OrderId,
        Amount,
        PaymentDate,
        PaymentMethod,
        Status
    FROM Payments;
END
GO

-- תשלום לפי מזהה
CREATE PROCEDURE GetPaymentById
    @PaymentId INT
AS
BEGIN
    SELECT 
        PaymentId,
        OrderId,
        Amount,
        PaymentDate,
        PaymentMethod,
        Status
    FROM Payments
    WHERE PaymentId = @PaymentId;
END
GO

-- כל התשלומים להזמנה מסוימת
CREATE PROCEDURE GetPaymentsByOrderId
    @OrderId INT
AS
BEGIN
    SELECT 
        PaymentId,
        OrderId,
        Amount,
        PaymentDate,
        PaymentMethod,
        Status
    FROM Payments
    WHERE OrderId = @OrderId;
END
GO

-- הוספת תשלום
CREATE PROCEDURE AddPayment
    @OrderId       INT,
    @Amount        DECIMAL(10,2),
    @PaymentMethod NVARCHAR(20),
    @Status        NVARCHAR(20) = N'Pending'
AS
BEGIN
    INSERT INTO Payments
    (
        OrderId,
        Amount,
        PaymentMethod,
        Status
    )
    VALUES
    (
        @OrderId,
        @Amount,
        @PaymentMethod,
        @Status
    );

    SELECT SCOPE_IDENTITY() AS NewPaymentId;
END
GO

-- עדכון תשלום
CREATE PROCEDURE UpdatePayment
    @PaymentId     INT,
    @OrderId       INT,
    @Amount        DECIMAL(10,2),
    @PaymentMethod NVARCHAR(20),
    @Status        NVARCHAR(20)
AS
BEGIN
    UPDATE Payments
    SET
        OrderId       = @OrderId,
        Amount        = @Amount,
        PaymentMethod = @PaymentMethod,
        Status        = @Status
    WHERE PaymentId = @PaymentId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO


-- כל חברות המשלוח
CREATE PROCEDURE GetAllShippingCompanies
AS
BEGIN
    SELECT 
        ShippingCompanyId,
        Name,
        Phone,
        IsActive
    FROM ShippingCompanies;
END
GO

-- חברת משלוח לפי מזהה
CREATE PROCEDURE GetShippingCompanyById
    @ShippingCompanyId INT
AS
BEGIN
    SELECT 
        ShippingCompanyId,
        Name,
        Phone,
        IsActive
    FROM ShippingCompanies
    WHERE ShippingCompanyId = @ShippingCompanyId;
END
GO

-- הוספת חברת משלוח
CREATE PROCEDURE AddShippingCompany
    @Name     NVARCHAR(100),
    @Phone    NVARCHAR(20) = NULL,
    @IsActive BIT          = 1
AS
BEGIN
    INSERT INTO ShippingCompanies
    (
        Name,
        Phone,
        IsActive
    )
    VALUES
    (
        @Name,
        @Phone,
        @IsActive
    );

    SELECT SCOPE_IDENTITY() AS NewShippingCompanyId;
END
GO

-- עדכון חברת משלוח
CREATE PROCEDURE UpdateShippingCompany
    @ShippingCompanyId INT,
    @Name              NVARCHAR(100),
    @Phone             NVARCHAR(20) = NULL,
    @IsActive          BIT
AS
BEGIN
    UPDATE ShippingCompanies
    SET
        Name     = @Name,
        Phone    = @Phone,
        IsActive = @IsActive
    WHERE ShippingCompanyId = @ShippingCompanyId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO


-- כל המשלוחים
CREATE PROCEDURE GetAllDeliveries
AS
BEGIN
    SELECT 
        D.DeliveryId,
        D.OrderId,
        D.ShippingCompanyId,
        SC.Name AS ShippingCompanyName,
        D.DeliveryMethod,
        D.City,
        D.StreetAddress,
        D.TrackingNumber,
        D.Status,
        D.CreatedAt
    FROM Deliveries D
    LEFT JOIN ShippingCompanies SC
        ON D.ShippingCompanyId = SC.ShippingCompanyId;
END
GO

-- משלוח לפי מזהה
CREATE PROCEDURE GetDeliveryById
    @DeliveryId INT
AS
BEGIN
    SELECT 
        D.DeliveryId,
        D.OrderId,
        D.ShippingCompanyId,
        SC.Name AS ShippingCompanyName,
        D.DeliveryMethod,
        D.City,
        D.StreetAddress,
        D.TrackingNumber,
        D.Status,
        D.CreatedAt
    FROM Deliveries D
    LEFT JOIN ShippingCompanies SC
        ON D.ShippingCompanyId = SC.ShippingCompanyId
    WHERE D.DeliveryId = @DeliveryId;
END
GO

-- כל המשלוחים לפי הזמנה
CREATE PROCEDURE GetDeliveriesByOrderId
    @OrderId INT
AS
BEGIN
    SELECT 
        DeliveryId,
        OrderId,
        ShippingCompanyId,
        DeliveryMethod,
        City,
        StreetAddress,
        TrackingNumber,
        Status,
        CreatedAt
    FROM Deliveries
    WHERE OrderId = @OrderId;
END
GO

-- הוספת משלוח
CREATE PROCEDURE AddDelivery
    @OrderId           INT,
    @ShippingCompanyId INT            = NULL,
    @DeliveryMethod    NVARCHAR(20),
    @City              NVARCHAR(100),
    @StreetAddress     NVARCHAR(255),
    @TrackingNumber    NVARCHAR(100)  = NULL,
    @Status            NVARCHAR(20)   = N'Pending'
AS
BEGIN
    INSERT INTO Deliveries
    (
        OrderId,
        ShippingCompanyId,
        DeliveryMethod,
        City,
        StreetAddress,
        TrackingNumber,
        Status
    )
    VALUES
    (
        @OrderId,
        @ShippingCompanyId,
        @DeliveryMethod,
        @City,
        @StreetAddress,
        @TrackingNumber,
        @Status
    );

    SELECT SCOPE_IDENTITY() AS NewDeliveryId;
END
GO

-- עדכון משלוח
CREATE PROCEDURE UpdateDelivery
    @DeliveryId        INT,
    @ShippingCompanyId INT            = NULL,
    @DeliveryMethod    NVARCHAR(20),
    @City              NVARCHAR(100),
    @StreetAddress     NVARCHAR(255),
    @TrackingNumber    NVARCHAR(100)  = NULL,
    @Status            NVARCHAR(20)
AS
BEGIN
    UPDATE Deliveries
    SET
        ShippingCompanyId = @ShippingCompanyId,
        DeliveryMethod    = @DeliveryMethod,
        City              = @City,
        StreetAddress     = @StreetAddress,
        TrackingNumber    = @TrackingNumber,
        Status            = @Status
    WHERE DeliveryId = @DeliveryId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO




	