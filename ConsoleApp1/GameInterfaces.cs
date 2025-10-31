using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Transactions;
using Windows.ApplicationModel.Background;
using static System.Net.Mime.MediaTypeNames;
public enum ActionNames
{
    CorruptingTouch,
    HorrifiyingVisage,
    Wail,
    Heal
}

public enum CombatRelation { Ally, Enemy, Neutral }
public enum TriggerEnum
{
    Trade,
    Attack,
    Flee,
    RequestHelp,
    GiveQuest,
    Steal,
    Bribe,
    Talk,
}
// Used to connect any object that can serve as caster or target of an action
public interface ICharacter : IActionable, IHealth  { }
public interface IActionable
{
    string Name { get; }
    Root Root { get; }

    (int GridX, int GridY, int LocalX, int LocalY) GetPosition();

    int TileSpeed { get; }          // Movement speed, default 6
    int ActionCount { get; }        // Base 1, overrideable
    int BonusActionCount { get; }   // Base 1, overrideable

    bool CanAttack(IActionable target);
    void PerformAttack(IActionable target);
    bool CanUseBonus();
    void PerformBonus(IActionable target);
    CombatRelation GetCombatRelation(SaveGame saveGame);
    List<GameAction> PossibleAttacks(ICharacter Target, List<ICharacter> allies);
}
public interface IHealth
{
    HealthComponent Health { get; }
}
public interface IHasMoney
{
    Dictionary<CoinType, int> Money { get; set; }
}
public interface IInventory
{
    bool AddItem(Item item, int quantity = 1);
    bool RemoveItem(Item item, int quantity = 1);
    bool EquipItem(Item item);
    List<Item>? GetEquippedItem(EquipmentSlots slot);
    bool HasItem(Item Item, int minQuantity = 1);
    ItemStack? GetSlot(int slot);
}
public interface ISkills
{
    int Modifier(Skill skill);
    void PrintSkills();
}

#region === Entitiy helper classes ===
public class HealthComponent
{
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int CurrentMP { get; set; }
    public int MaxMP { get; set; }
    public bool IsAlive { get; set; }
    public void DamageHP(int amount)
    {
        CurrentHP = CurrentHP - amount;
    }
    public void DamageMP(int amount)
    {
        CurrentMP = CurrentMP - amount;
    }
    public void HealHP(int amount)
    {
        CurrentHP = CurrentHP - amount;
    }
    public void HealMP(int amount)
    {
        CurrentMP = CurrentMP - amount;
    }
    public HealthComponent Clone()
    {
        var HealthComponent = new HealthComponent
        {
            CurrentHP = this.CurrentHP,
            MaxHP = this.MaxHP,
            CurrentMP = this.CurrentMP,
            MaxMP = this.MaxMP
        };
        return HealthComponent;
    }
}
public class SkillsCompanent : ISkills
{
    public Dictionary<Skill, int> Skills { get; set; } = new Dictionary<Skill, int>();
    public int Modifier(Skill skill)
    {
        int value = Skills.ContainsKey(skill) ? Skills[skill] : 10;
        return (value - 10) / 2;
    }
    public void PrintSkills()
    {
        foreach (Skill skill in Enum.GetValues(typeof(Skill)))
        {
            int value = Skills.ContainsKey(skill) ? Skills[skill] : 0;
            EngineText.Write($"{skill}: {value}");
            EngineText.Read("Press enter to continue");
        }
    }
}
public class ItemStack
{
    public Item Item { get; set; }
    public int stackSize { get; set; }
    public ItemStack(Item item, int quantity = 1)
    {
        Item = item;
        stackSize = quantity;
    }
    public ItemStack()
    {
        Item = new Item();
    }

    public void Add(int amount)
    {
        stackSize += amount;
    }

    public bool Remove(int amount)
    {
        if (amount > stackSize) return false;
        stackSize -= amount;
        return true;
    }
    public ItemStack Clone()
    {
        return new ItemStack
        {
            Item = this.Item,
            stackSize = this.stackSize
        };
    }
}
public class TriggerConfig
{
    public string Action { get; set; } = string.Empty;
    public Dictionary<string, object>? Params { get; set; }
}
#endregion