create table Client
(
	ClientId int,
	Processed bit
)

create table Metadata
(
	LegacyDocumentCodeId int identity,
	LegacyDocumentId int,
	LegacyDocumentName nvarchar(50),
	LegacyClientId int,
	EffectiveDate datetime2,
	LegacyDocumentCategory nvarchar(50),
	PathToPdfFile nvarchar(100)
)