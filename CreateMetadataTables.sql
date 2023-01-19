create table Client
(
	ClientId int,
	Processed bit
)

create table Metadata
(
	LegacyDocumentId int identity,
	LegacyDocumentCodeId int,	
	LegacyDocumentName nvarchar(100),
	LegacyClientId int,
	EffectiveDate datetime2,
	LegacyDocumentCategory nvarchar(50),
	PathToPdfFile nvarchar(100)
)

