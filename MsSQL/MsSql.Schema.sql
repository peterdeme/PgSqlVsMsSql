IF OBJECT_ID('dbo.Persons', 'U') IS NOT NULL 
  DROP TABLE dbo.Persons; 

CREATE TABLE Persons (
    PersonID int IDENTITY PRIMARY KEY,
    LastName varchar(255) NOT NULL,
    FirstName varchar(255) NOT NULL,
    AddressLine1 varchar(255) NOT NULL,
	AddressLine2 varchar(255) NULL,
    City varchar(255)  NOT NULL,
	ZipCode varchar(20) NOT NULL
);
