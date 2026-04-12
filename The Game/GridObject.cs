using System.Text.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace The_Game
{
    // Used the dehydrated tiles. 
    public class GridObject
    {
        public int GridID { get; set; }
        public List<string> GridMapKey { get; set; }
        [JsonIgnore]
        public int LocationID { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        public GridBiomeType Biome { get; set; } // E.G. BorelForest, TemperateBroadleadForest
        public GridBiomeSubType SubBiome { get; set; } // E.G. Town, HighTower, Farm, Forest
        public int RandomEventChance { get; set; }
        [NotMapped]
        public List<DescriptionEntry> DescriptionEntries { get; set; } = new();
        [NotMapped]
        public Dictionary<char, TileAddData>? TileAdds { get; set; } = new(); // Adds desriptions to the tiles
        [NotMapped]
        public List<TileObject> GridMap { get; set; } // used for cell rendering
        public GridObject() { }

        /// <summary>
        /// Adds a description entry to the grid.
        /// Null values act as "any".
        /// </summary>
        public void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null)
        {
            DescriptionEntries.Add(new DescriptionEntry
            (
                text, weight, biome, subBiome, season, weather)
            );
        }
    }
    public class TileAddData
    {
        public int Weight { get; set; }
        public string Text { get; set; }
        public GridBiomeType Biome { get; set; }
        public GridBiomeSubType SubBiome { get; set; }
        public SeasonData Season { get; set; }
        public WeatherData Weather { get; set; }
    }
}