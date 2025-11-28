using System;
public enum ItemTag { }
public class ItemComponentDictionaryEntry
{
	public string Key { get; set; }	
	public IItemComponent Value { get; set; }
}
public class ItemTags
{
    public int ItemID { get; set; }
	public ItemTag Tag { get; set; }
}

