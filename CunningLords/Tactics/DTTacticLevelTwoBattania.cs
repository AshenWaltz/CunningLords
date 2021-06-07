using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.DecisionTreeLogic;

namespace CunningLords.Tactics 
{
    class DTTacticLevelTwoBattania : TacticComponent
	{
		private bool _hasBattleBeenJoined;

		private int _AIControlledFormationCount;

		private DecisionTreeNode infantryTree;
		private DecisionTreeNode archersTree;
		private DecisionTreeNode leftCavalryTree;
		private DecisionTreeNode rightCavalryTree;
		private DecisionTreeNode HorseArchersTree;


		private int tickCounter = 0;

		public DTTacticLevelTwoBattania(Team team) : base(team)
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
				//Infantry
				ActionCharge infantryCharge = new ActionCharge(this._mainInfantry);
				ActionPullBack infantryPullback = new ActionPullBack(this._mainInfantry);
				ActionDontExist infantryDontExist = new ActionDontExist(this._mainInfantry);

				DecisionCycleCharge infantryCycleCharge = new DecisionCycleCharge(this._mainInfantry, infantryCharge, infantryPullback, 20, 0);

				DecisionClosestEnemyCloserThan decisionCloserThan = new DecisionClosestEnemyCloserThan(this._mainInfantry, infantryCycleCharge, infantryCharge, 30f);

				DecisionIsFormationNotNull decisionInfantryNotNull = new DecisionIsFormationNotNull(this._mainInfantry, decisionCloserThan, infantryDontExist);

				//Archers
				ActionDontExist archersDontExist = new ActionDontExist(this._archers);

				DecisionIsFormationNotNull decisionArchersNotNull = new DecisionIsFormationNotNull(this._archers, archersDontExist, archersDontExist);
			
				//Left Cavalry
				ActionDontExist leftCavalryDontExist = new ActionDontExist(this._leftCavalry);


				DecisionIsFormationNotNull IsLeftCavalryNotNull = new DecisionIsFormationNotNull(this._leftCavalry, leftCavalryDontExist, leftCavalryDontExist);
			
				//Right Cavalry
				ActionDontExist rightCavalryCavalryDontExist = new ActionDontExist(this._rightCavalry);


				DecisionIsFormationNotNull IsrightCavalryCavalryNotNull = new DecisionIsFormationNotNull(this._rightCavalry, rightCavalryCavalryDontExist, rightCavalryCavalryDontExist);

				//Horse Archers
				ActionDontExist horseArcherDontExist = new ActionDontExist(this._rangedCavalry);

				DecisionIsFormationNotNull IsHorseArchersNotNull = new DecisionIsFormationNotNull(this._rangedCavalry, horseArcherDontExist, horseArcherDontExist);

				//Final Trees
				this.infantryTree = decisionInfantryNotNull;
				this.archersTree = decisionArchersNotNull;
				this.leftCavalryTree = IsLeftCavalryNotNull;
				this.rightCavalryTree = IsrightCavalryCavalryNotNull;
				this.HorseArchersTree = IsHorseArchersNotNull;

				this.infantryTree.makeDecision();
				this.archersTree.makeDecision();
				this.leftCavalryTree.makeDecision();
				this.rightCavalryTree.makeDecision();
				this.HorseArchersTree.makeDecision();

				this.tickCounter++;
			}
			else if (base.AreFormationsCreated && this.tickCounter != 0)
			{
				this.infantryTree.makeDecision();
				this.archersTree.makeDecision();
				this.leftCavalryTree.makeDecision();
				this.rightCavalryTree.makeDecision();
				this.HorseArchersTree.makeDecision();
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
