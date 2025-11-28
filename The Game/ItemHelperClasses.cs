using System;

public class ItemProperty
{
    public int ITemObjectID { get; set; }
    public ItemObject ItemDefinition { get; set; }

    public Properties Property { get; set; }  // ← your enum!
}

public class ItemPrimaryMaterial
{
    public int ITemObjectID { get; set; }
    public PrimaryMaterial Material { get; set; }
}
public class ItemSecondaryMaterial
{
    public int ITemObjectID { get; set; }
    public SecondaryMaterial Material { get; set; }
}

