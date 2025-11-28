create table Tile (
RootComponentId TEXT PRIMARY KEY, --- serialized or grid of RootComponent
GridX INTEGER,
GridY INTEGER,
LocalX INTEGER,
LocalY INTEGER,
TileType TEXT, -- Enum conversion
Cover TEXT, -- Enum conversion
IseWalkable INTEGER, -- 0/1 FALSE/TRUE
IsRoofed INTEGER,
BurnAmount INTEGER,
);

CREATE TABLE TileComponents(
TileObjectRoot Text Primary Key -- Reference RootID to tile object
TileComponent TEXT -- enum conversion
);

Create Table TileProperties (
TileObjectRoot Text Primary Key, -- Reference RootID to tile object
TileProperty TEXT
);

create table DescriptionEntry (
ID integer Primary Key,
TextEntry Text,
DescriptionType TEXT, -- Refereence Type, I.E. Tile, Grid, Item
TypeID integer,
DescriptionWeight integer,
Biome TEXT, -- enum conversion
SubBiome TEXT, -- enum conversion
Season TEXT, -- enum conversion
Weather TEXT -- enum conversion
)

