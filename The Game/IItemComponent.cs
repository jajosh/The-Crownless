using System;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;

namespace The_Game 
{
    public interface IItemComponent
    {
    }
    public record WeaponComponent(
        int DamageDie,
        Skill DamageBonusSkill,
        DamageTypes DamageType) : IItemComponent;

    public record MeleeComponent(
        bool Versatile) : IItemComponent;

    public record RangedComponent(
        int Range) : IItemComponent;

    public record ConsumableComponent(
        TriggerConfig Trigger) : IItemComponent;

    public record CraftingComponent(
        bool IsCookable,
        int CookingDifficulty) : IItemComponent;

    public record ArmorComponent(
        EquipmentSlots SlotType,
        List<EquipmentSlots>? Invalidated) : IItemComponent;
    public record DurabilityComponent(
        int MaxDurability,
        int CurrentDurability) : IItemComponent;
    public record EnchantableComponent(
        List<Enchantment> ActiveEnchantments,
        int EnchantmentSlots) : IItemComponent;
    public record EquipedableComponent(
        // Single slot items
        SingleEquipmentSlots? SingleSlot,
        // Multi slot items (bitmask)
        MultiEquipmentSlots? MultiSlots) : IItemComponent;

    public static class ComponentLoader
    {
        private static readonly Dictionary<string, Type> ComponentTypes = new()
    {
        { "WeaponComponent", typeof(WeaponComponent) },
        { "MeleeComponent", typeof(MeleeComponent) },
        { "RangedComponent", typeof(RangedComponent) },
        { "ConsumableComponent", typeof(ConsumableComponent) },
        { "CraftingComponent", typeof(CraftingComponent) },
        { "ArmorComponent", typeof(ArmorComponent) },
        { "DurabilityComponent", typeof(DurabilityComponent) }
    };

        public static IItemComponent LoadComponent(JsonObject obj)
        {
            string typeName = obj["Type"]!.GetValue<string>();

            if (!ComponentTypes.TryGetValue(typeName, out var targetType))
                throw new Exception($"Unknown component type '{typeName}'");

            return (IItemComponent)obj.Deserialize(targetType)!;
        }
        
    }
}