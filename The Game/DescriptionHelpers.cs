using System;

public class DescriptionEntryFlag
{
    public int DescriptionEntryID { get; set; }
    public DescriptionFlag Flag { get; set; }
    public bool Matches(DescriptionFlag flag)
    {
        if (flag == Flag)
            return true;
        return false;
    }
}
