using System;

namespace The_Game;


public class TriggerConfig
{
    public string Action { get; set; } = string.Empty;
    public Dictionary<string, object>? Params { get; set; }
}
