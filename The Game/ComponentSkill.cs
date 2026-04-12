using System;

namespace The_Game;


public enum Skill
{
    Strength,
    Dexterity,
    Charisma,
    Intelligence,
    Wisdom,
    Constitution
}
public class SkillComponent
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Charisma { get; set; }
    public int Wisdom { get; set; }
    public int Intelligence { get; set; }
    public SkillComponent Clone()
    {
        var SkillComponent = new SkillComponent
        {
            Strength = this.Strength,
            Dexterity = this.Dexterity,
            Constitution = this.Constitution,
            Charisma = this.Charisma,
            Wisdom = this.Wisdom,
            Intelligence = this.Intelligence
        };
        return SkillComponent;
    }
}
