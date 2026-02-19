CREATE DATABASE DelhiMetroRouteFinderDb;
GO

USE DelhiMetroRouteFinderDb;
GO

CREATE TABLE Lines (
    LineId INT PRIMARY KEY,
    LineName NVARCHAR(100) NOT NULL,
    Color NVARCHAR(20) NOT NULL
);

CREATE TABLE Stations (
    StationId INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    LineId INT NOT NULL,
    Latitude DECIMAL(9,6) NOT NULL,
    Longitude DECIMAL(9,6) NOT NULL,
    FirstMetroTime TIME NOT NULL,
    LastMetroTime TIME NOT NULL,
    FOREIGN KEY (LineId) REFERENCES Lines(LineId)
);

CREATE TABLE Connections (
    ConnectionId INT IDENTITY(1,1) PRIMARY KEY,
    FromStationId INT NOT NULL,
    ToStationId INT NOT NULL,
    DistanceKm DECIMAL(5,2) NOT NULL,
    TimeMinutes INT NOT NULL,
    FOREIGN KEY (FromStationId) REFERENCES Stations(StationId),
    FOREIGN KEY (ToStationId) REFERENCES Stations(StationId),
    CONSTRAINT UQ_Connection UNIQUE (FromStationId, ToStationId)
);

CREATE TABLE FareRules (
    FareRuleId INT IDENTITY(1,1) PRIMARY KEY,
    MinStations INT NOT NULL,
    MaxStations INT NOT NULL,
    Fare DECIMAL(10,2) NOT NULL
);
