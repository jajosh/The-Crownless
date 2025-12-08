using System;

public record LootTableObject
{
	public Dictionary<ItemStackComponent, int> /*item, weight*/ LootTable { get; set; }
	public List<ItemStackComponent> ConstantDrops { get; set; }
}
