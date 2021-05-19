using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.BehaviorTreelogic;

namespace CunningLords.Tactics
{
	public class TacticRecklessCharge : TacticComponent
	{

		private bool _hasBattleBeenJoined;

		private int _AIControlledFormationCount;

		private Task tree;

		/*protected Formation _maininfantry;
        protected Formation _archers;
        protected Formation _leftCavalry;
        protected Formation _rightCavalry;
        protected Formation _rangedCavalry;*/

		private int tickCounter = 0;

		public TacticRecklessCharge(Team team) : base(team)
		{
			_hasBattleBeenJoined = false;
			_AIControlledFormationCount = base.Formations.Count((Formation f) => f.IsAIControlled);

			AssignTacticFormations1121();
		}

		private bool hasBattleBeenJoined()
		{
			return false;
		}

		protected override void ManageFormationCounts()
		{
			base.AssignTacticFormations1121();
		}
		protected override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			else if (base.AreFormationsCreated && this.tickCounter == 0)
			{
				//Infantry Charge
				TaskCharge MICharge = new TaskCharge(this._mainInfantry);
				Selector mainInfantrySelector = new Selector(null);
				mainInfantrySelector.addTask(MICharge);

				//Archers Charge
				TaskCharge ACharge = new TaskCharge(this._archers);
				Selector archerSelector = new Selector(null);
				archerSelector.addTask(ACharge);

				//Horse Archers Charge
				TaskCharge HACharge = new TaskCharge(this._rangedCavalry);
				Selector horseArcherSelector = new Selector(null);
				horseArcherSelector.addTask(HACharge);

				//Right Cavalry Charge
				TaskCharge RCCharge = new TaskCharge(this._rightCavalry);
				Selector rightCavalrySelector = new Selector(null);
				rightCavalrySelector.addTask(RCCharge);

				//Left Cavalry Charge
				TaskCharge LCCharge = new TaskCharge(this._leftCavalry);
				Selector leftCavalrySelector = new Selector(null);
				leftCavalrySelector.addTask(LCCharge);

				//Final Tree
				this.tree = new Sequence(null);
				this.tree.addTask(mainInfantrySelector);
				this.tree.addTask(archerSelector);
				this.tree.addTask(horseArcherSelector);
				this.tree.addTask(rightCavalrySelector);
				this.tree.addTask(leftCavalrySelector);

				this.tree.run();
				this.tickCounter++;
			}
			else if (base.AreFormationsCreated && this.tickCounter != 0)
			{
				this.tree.run();
			}

			base.TickOccasionally();
		}

		internal float GetTacticWeight()
		{
			return 10f;
		}

		internal static void SetDefaultBehaviorWeights(Formation formation)
		{
			formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
			formation.AI.SetBehaviorWeight<BehaviorPullBack>(1f);
			formation.AI.SetBehaviorWeight<BehaviorStop>(1f);
			formation.AI.SetBehaviorWeight<BehaviorReserve>(1f);
		}

	}
}