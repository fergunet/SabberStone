﻿using SabberStoneCore.Actions;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using System.Collections.Generic;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class DamageTask : SimpleTask
	{
		public DamageTask(int amount, int randAmount, EntityType entityType, bool spellDmg = false)
		{
			Amount = amount;
			RandAmount = randAmount;
			Type = entityType;
			SpellDmg = spellDmg;
		}

		public DamageTask(int amount, EntityType entityType, bool spellDmg = false)
		{
			Amount = amount;
			RandAmount = 0;
			Type = entityType;
			SpellDmg = spellDmg;
		}

		public int Amount { get; set; }

		public int RandAmount { get; set; }

		public EntityType Type { get; set; }

		public bool SpellDmg { get; set; }

		public override TaskState Process()
		{
			if (Amount < 1 && RandAmount < 1)
				return TaskState.STOP;

			IList<IPlayable> entities = IncludeTask.GetEntities(Type, Controller, Source, Target, Playables);

			for (int i = 0; i < entities.Count; i++)
			{
				int randAmount = 0;
				if (RandAmount > 0)
				{
					randAmount = Random.Next(0, RandAmount + 1);
					Game.OnRandomHappened(true);
				}

				int amount = Amount + randAmount;

				Game.Log(LogLevel.WARNING, BlockType.ACTION, "DamageTask", !Game.Logging? "":$"Amount is {amount} damage of {Source}.");

				Generic.DamageCharFunc.Invoke(Source as IPlayable, entities[i] as ICharacter, amount, SpellDmg);
			};
			return TaskState.COMPLETE;
		}

		public override ISimpleTask Clone()
		{
			var clone = new DamageTask(Amount, RandAmount, Type, SpellDmg);
			clone.Copy(this);
			return clone;
		}
	}
}
