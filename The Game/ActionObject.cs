using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace The_Game
{
	public enum ActionType { Action, BonusAction, FreeAction, Ritual }
	public enum ActionEffectType { Heal, Attack, Buff, Debuff }
	public enum ActionRangeType { TileEffect, SingleTarget, AreaOfEffect, Self, Multiple }
	public abstract class ActionObject : ICloneable
	{
		public string Name { get; }
		public ActionType Type { get; protected set; }
		public ActionRangeType RangeType { get; protected set; }
		public int Range { get; protected set; }
		protected ActionObject(string name)
		{
			Name = name;
			RangeType = ActionRangeType.SingleTarget;
			Range = 1;
		}
		public abstract void Execute(ICharacter caster, IActionable Target);
		public abstract Object Clone();
		
	}
}
