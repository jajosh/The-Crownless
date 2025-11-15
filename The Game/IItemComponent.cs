using System;

public interface IItemComponent { }
public record WeaponComponent(int DamageDie, Skill DamageBonusSkill, DamageTypes DamageType) : IItemComponent;

public record MeleeComponent(bool Versatile) : IItemComponent;

public record RangedComponent(int Range) : IItemComponent;

public record ConsumableComponent(int HealthToHeal) : IItemComponent;

public record CraftingComponent(bool IsCookable, int CookingDifficulty) : IItemComponent;

public record ArmorComponent(EquipmentSlots SlotType, List<EquipmentSlots>? Invalidated) : IItemComponent;
public record DurabilityComponent(int MaxDurability, int CurrentDurability) : IItemComponent
