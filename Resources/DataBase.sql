--CREATE DATABASE CosmeticShop;

use CosmeticShop;

CREATE TABLE Brands (
    BrandId INT PRIMARY KEY,
    BrandName VARCHAR(100),
    BrandDescription nvarchar(MAX)
);


INSERT INTO Brands (BrandId, BrandName, BrandDescription) VALUES
(1, 'Essence', 'Essence — немецкий косметический бренд, ориентированный на молодых потребителей и тех, кто ценит креативность, доступность и тренды. Компания предлагает широкий ассортимент декоративной косметики, включая туши, тени, хайлайтеры и лаки для ногтей, при этом акцентируя внимание на высоком качестве продукции по очень доступным ценам. Essence известен своим подходом без жестокости — вся продукция не тестируется на животных.'),
(2, 'Maybelline', 'Maybelline — один из самых узнаваемых и старейших американских косметических брендов, входящий в концерн L’Oréal. Бренд предлагает широкий спектр продуктов для макияжа: от тональных основ и тушей до губных помад и пудр. Maybelline ориентирован на создание модных, инновационных продуктов, которые легко использовать в повседневной жизни, при этом сохраняя профессиональное качество.'),
(3, 'NYX', 'NYX Professional Makeup — популярный американский бренд декоративной косметики, особенно любимый визажистами и блогерами. NYX предлагает яркие, пигментированные и инновационные продукты по доступным ценам, сохраняя высокое качество. Особое внимание бренд уделяет разнообразию оттенков и средств, включая веганские формулы, подходящие для разных типов кожи и макияжа любого стиля — от повседневного до арт-выражения.'),
(4, 'L''Oréal', 'L''Oréal — ведущий мировой бренд в индустрии красоты, основанный во Франции. Бренд предлагает не только декоративную косметику, но и средства по уходу за кожей и волосами, а также парфюмерию. Слоган компании “Потому что вы этого достойны” отражает миссию бренда — делать качественную косметику доступной для женщин по всему миру. L''Oréal активно инвестирует в научные исследования и устойчивое развитие.'),
(5, 'Revolution', 'Revolution (или Makeup Revolution) — британский косметический бренд, известный своими доступными по цене аналогами люксовой косметики и активной позицией в вопросах инклюзивности. Продукция бренда отличается креативной упаковкой, частыми релизами и широкой палитрой оттенков, что делает его популярным среди молодых потребителей и поклонников трендового макияжа. Все продукты бренда — cruelty-free и часто веганские.'),
(6, 'Catrice', 'Catrice — немецкий косметический бренд, принадлежащий той же компании, что и Essence. Бренд сочетает в себе трендовость, высокое качество и доступные цены, предлагая потребителям как базовые продукты для макияжа, так и инновационные новинки сезона. Catrice активно работает над устойчивым развитием, снижая использование пластика и предлагая экологичные альтернативы упаковки.'),
(7, 'Sephora', 'Sephora — международный бренд магазинов косметики, принадлежащий французскому холдингу LVMH. Помимо широкого ассортимента товаров разных брендов, Sephora выпускает продукцию под собственной маркой, которая охватывает все категории — от ухода за кожей до декоративной косметики. Компания известна инновационными подходами в ритейле, акцентом на разнообразие и поддержку новых независимых брендов по всему миру.'),
(8, 'The Ordinary', 'The Ordinary — канадский бренд, входящий в компанию DECIEM, который произвел революцию в индустрии ухода за кожей благодаря прозрачному подходу к ингредиентам, минималистичному дизайну и доступным ценам. Продукты бренда нацелены на решение конкретных проблем кожи с помощью высококонцентрированных активных компонентов, таких как ниацинамид, ретинол и витамин С.'),
(9, 'Clinique', 'Clinique — престижный американский бренд, входящий в концерн Estée Lauder, специализирующийся на гипоаллергенной косметике и уходе за кожей. Clinique стал первым брендом, предлагающим дерматологически протестированные средства, не содержащие отдушек. Продукция разрабатывается с учетом различных типов кожи и подходит даже для самой чувствительной.'),
(10, 'Fenty Beauty', 'Fenty Beauty — инновационный косметический бренд, созданный певицей Рианной, который стал символом инклюзивности и разнообразия. Fenty Beauty вошел в историю индустрии красоты, предложив 40 оттенков тонального крема в дебютной коллекции. Продукция бренда сочетает в себе высокую пигментацию, креативный подход к упаковке и актуальные тренды.');

CREATE TABLE Goods (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    Brand NVARCHAR(MAX),
    ImagePath NVARCHAR(MAX),
    Price FLOAT,
    BrandId INT,
    Composition NVARCHAR(MAX),
    CONSTRAINT FK_Goods_Brands FOREIGN KEY (BrandId) REFERENCES Brands(BrandId)
);

--DELETE FROM Goods
--WHERE Id IN (
--    SELECT TOP 2 Id
--    FROM Goods
--    ORDER BY Id DESC
--);


INSERT INTO Goods(Name, Description, Brand, ImagePath, Price, BrandId, Composition)
VALUES
('Essence gelly-grip primer', 'Праймер', 'Essence', 'D:/лабораторные работы/ооп/lab4_5/pics/essence-primer-jelly-grip.jpg', 123, 1, 'Aqua, Glycerin, Propylene Glycol, Carbomer, Panthenol, Sodium Hydroxide, Phenoxyethanol'),
('L''OREAL PARIS panorama', 'Тушь для ресниц', 'L''OREAL', 'D:/лабораторные работы/ооп/lab4_5/pics/L''OREAL-PARIS-panorama.jpg', 245, 4, 'Water, Paraffin, Cera Alba, Stearic Acid, Acacia Senegal Gum, Triethanolamine, Phenoxyethanol'),
('Maybelline Fit Me Matte', 'Праймер', 'Maybelline', 'D:/лабораторные работы/ооп/lab4_5/pics/Maybelline-Fit-Me-Matte.jpg', 189, 2, 'Dimethicone, Isododecane, Cyclopentasiloxane, Silica, Disteardimonium Hectorite, Propylene Carbonate'),
('NYX HD Studio Photogenic', 'Консилер', 'NYX', 'D:/лабораторные работы/ооп/lab4_5/pics/NYX HD Studio Photogenic.jpg', 210, 3, 'Water, Cyclopentasiloxane, Glycerin, Dimethicone, Talc, Sodium Chloride, Tocopheryl Acetate'),
('Revolution Banana Powder', 'Пудра', 'Revolution', 'D:/лабораторные работы/ооп/lab4_5/pics/Revolution Banana Powder.jpg', 275, 5, 'Talc, Mica, Silica, Kaolin, Magnesium Stearate, Dimethicone, Ethylhexylglycerin'),
('L''OREAL PARIS Infallible 24H', 'Тональный крем', 'L''OREAL', 'D:/лабораторные работы/ооп/lab4_5/pics/L''OREAL PARIS Infallible 24H Fresh Wear Foundation.jpg', 390, 4, 'Cyclopentasiloxane, Aqua, Alcohol Denat, PEG-10 Dimethicone, Nylon-12, Butylene Glycol, Tocopherol'),
('Maybelline Lash Sensational Sky High', 'Тушь для ресниц', 'Maybelline', 'D:/лабораторные работы/ооп/lab4_5/pics/Maybelline Lash Sensational Sky High.jpg', 198, 2, 'Aqua, Synthetic Beeswax, Styrene/Acrylates Copolymer, Paraffin, Glycerin, Polyvinyl Alcohol, Panthenol'),
('Essence Stay All Day 16h Foundation', 'Тональный крем', 'Essence', 'D:/лабораторные работы/ооп/lab4_5/pics/Essence Stay All Day 16h Foundation.jpg', 165, 1, 'Aqua, Cyclopentasiloxane, Talc, Glycerin, PEG-10 Dimethicone, Disteardimonium Hectorite, Phenoxyethanol'),
('NYX Can''t Stop Won''t Stop Setting Powder', 'Фиксирующая пудра', 'NYX', 'D:/лабораторные работы/ооп/lab4_5/pics/NYX Can''t Stop Won''t Stop Setting Powder.jpg', 223, 3, 'Silica, Talc, Dimethicone, Caprylyl Glycol, Hexylene Glycol, Tocopheryl Acetate'),
('Revolution Conceal & Define Concealer', 'Консилер', 'Revolution', 'D:/лабораторные работы/ооп/lab4_5/pics/Revolution Conceal & Define Concealer.jpg', 155, 5, 'Aqua, Cyclopentasiloxane, Propylene Glycol, Talc, PEG-10 Dimethicone, Phenoxyethanol'),
('Maybelline SuperStay Matte Ink', 'Жидкая матовая помада', 'Maybelline', 'D:/лабораторные работы/ооп/lab4_5/pics/Maybelline SuperStay Matte Ink.jpg', 199, 2, 'Dimethicone, Isododecane, Trimethylsiloxysilicate, Silica Silylate, Disteardimonium Hectorite'),
('Essence Lash Princess Mascara', 'Тушь для ресниц', 'Essence', 'D:/лабораторные работы/ооп/lab4_5/pics/Essence Lash Princess False Lash Effect Mascara.jpg', 145, 1, 'Aqua, Cera Alba, Acacia Senegal Gum, Stearic Acid, Palmitic Acid, Aminomethyl Propanol, Phenoxyethanol');

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Role NVARCHAR(50) default 'Client',
    Username NVARCHAR(50) DEFAULT 'User',
    Login NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100),
    Pfp NVARCHAR(255) DEFAULT 'D:\лабораторные работы\ооп\lab4_5\lab4_5\Resources\DefaultPfp.png'
);


INSERT INTO Users (Role, Username, Login, Password, Pfp)
VALUES
('Admin', 'User', 'qwerty', '1234', 'D:\лабораторные работы\ооп\lab4_5\lab4_5\Resources\DefaultPfp.png'),
('Client', 'User', 'йцукен', '1234', 'D:\лабораторные работы\ооп\lab4_5\lab4_5\Resources\DefaultPfp.png');

CREATE TABLE Carts (
    CartId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Carts_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE CartItems (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT DEFAULT 1,
    AddedAt DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_CartItems_Carts FOREIGN KEY (CartId) REFERENCES Carts(CartId),
    CONSTRAINT FK_CartItems_Goods FOREIGN KEY (ProductId) REFERENCES Goods(Id)
);


-- Таблица заказов
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
    DeliveryMethod NVARCHAR(50) NOT NULL, 
	PaymentMethod NVARCHAR(100) NOT NULL,

    City NVARCHAR(100),
    Street NVARCHAR(100),
    Apartment NVARCHAR(50),
    Building NVARCHAR(50),
    PickupPoint NVARCHAR(255),

    Comment NVARCHAR(MAX),
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Ожидает подтверждения',

    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);


-- Таблица товаров в заказе
CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT DEFAULT 1,
    PriceAtPurchase FLOAT, 

    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderItems_Goods FOREIGN KEY (ProductId) REFERENCES Goods(Id)
);


CREATE TABLE Reviews (
    ReviewId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Rating INT CHECK (Rating >= 1 AND Rating <= 5),
    ReviewText NVARCHAR(MAX),
    ReviewDate DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_Reviews_Goods FOREIGN KEY (ProductId) REFERENCES Goods(Id)
);

--DROP TABLE IF EXISTS OrderItems;
--DROP TABLE IF EXISTS Reviews;
--DROP TABLE IF EXISTS CartItems;
--DROP TABLE IF EXISTS Orders;
--DROP TABLE IF EXISTS Carts;

--DROP TABLE IF EXISTS Goods;
--DROP TABLE IF EXISTS Users;
--DROP TABLE IF EXISTS Brands;

