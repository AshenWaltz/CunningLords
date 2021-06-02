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
    class TacticLevelTwoBattania : TacticComponent
    {
		private bool _hasBattleBeenJoined;

		private int _AIControlledFormationCount;

		private Task tree;

		private int tickCounter = 0;

		public TacticLevelTwoBattania(Team team) : base(team)
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
				//Agressive Stance
				TaskAgressiveStance taskAgressiveStance = new TaskAgressiveStance(this._mainInfantry);
				//Infantry Agressive
				float infantryEngageDistance = 50f;
				int duration = 50;
				TaskCycleCharge MICycleCharge = new TaskCycleCharge(this._mainInfantry, infantryEngageDistance, duration);
				TaskCharge MICharge = new TaskCharge(this._mainInfantry);
				Selector mainInfantryAgressiveSelector = new Selector(null);
				mainInfantryAgressiveSelector.addTask(MICycleCharge);
				mainInfantryAgressiveSelector.addTask(MICharge);
				//Archers Agressive
				BehaviorConfig AHideBehindConfig = new BehaviorConfig(ArrangementOrder.ArrangementOrderLoose,
																		FiringOrder.FiringOrderFireAtWill,
																		FormOrder.FormOrderWide,
																		WeaponUsageOrder.WeaponUsageOrderUseAny,
																		RidingOrder.RidingOrderFree,
																		FacingOrder.FacingOrderLookAtEnemy);
				TaskRangedEngagement ARangedEngagement = new TaskRangedEngagement(this._archers);
				Selector archerAgressiveSelector = new Selector(null);
				archerAgressiveSelector.addTask(ARangedEngagement);
				//Horse Archers Agressive
				TaskRangedEngagement HARangedEngagement = new TaskRangedEngagement(this._rangedCavalry);
				Selector horseArcherAgressiveSelector = new Selector(null);
				horseArcherAgressiveSelector.addTask(HARangedEngagement);
				//Right Cavalry Agressive
				float cavalryEngageDistance = 50f;
				TaskCycleCharge RCCycleCharge = new TaskCycleCharge(this._rightCavalry, cavalryEngageDistance, duration);
				Selector rightCavalryAgressiveSelector = new Selector(null);
				rightCavalryAgressiveSelector.addTask(RCCycleCharge);
				//Left Cavalry Agressive
				TaskCycleCharge LCCycleCharge = new TaskCycleCharge(this._leftCavalry, cavalryEngageDistance, duration);
				Selector leftCavalryAgressiveSelector = new Selector(null);
				leftCavalryAgressiveSelector.addTask(LCCycleCharge);
				//Agressive Node Assembly
				taskAgressiveStance.addTask(mainInfantryAgressiveSelector);
				taskAgressiveStance.addTask(archerAgressiveSelector);
				taskAgressiveStance.addTask(horseArcherAgressiveSelector);
				taskAgressiveStance.addTask(rightCavalryAgressiveSelector);
				taskAgressiveStance.addTask(leftCavalryAgressiveSelector);

				//Defensive Stance
				TaskDefensiveStance taskDefensiveStance = new TaskDefensiveStance(this._mainInfantry);
				//Infantry Defensive
				float engageDistance = 30f;
				TaskHoldLine MIHoldLine = new TaskHoldLine(this._mainInfantry);
				TaskProximityCharge MIproximityCharge = new TaskProximityCharge(this._mainInfantry, engageDistance);
				Selector mainInfantryDefensiveSelector = new Selector(null);
				mainInfantryDefensiveSelector.addTask(MIHoldLine);
				mainInfantryDefensiveSelector.addTask(MIproximityCharge);
				//Archers Defensive
				TaskHoldLine ArcherHoldLine = new TaskHoldLine(this._archers);
				Selector archerDefensiveSelector = new Selector(null);
				archerDefensiveSelector.addTask(ArcherHoldLine);
				//Horse Archers Defensive
				Selector horseArcherDefensiveSelector = new Selector(null);
				horseArcherDefensiveSelector.addTask(HARangedEngagement);
				//Right Cavalry Protect and Attack Flank
				TaskProtectFlank RCProtectFlank = new TaskProtectFlank(this._rightCavalry, FormationAI.BehaviorSide.Right);
				Selector rightCavalryDefensiveSelector = new Selector(null);
				rightCavalryDefensiveSelector.addTask(RCProtectFlank);
				//Left Cavalry Protect and Attack Flank
				TaskProtectFlank LCProtectFlank = new TaskProtectFlank(this._leftCavalry, FormationAI.BehaviorSide.Left);
				Selector leftCavalryDefensiveSelector = new Selector(null);
				leftCavalryDefensiveSelector.addTask(LCProtectFlank);
				//Agressive Node Assembly
				taskDefensiveStance.addTask(mainInfantryDefensiveSelector);
				taskDefensiveStance.addTask(archerDefensiveSelector);
				taskDefensiveStance.addTask(horseArcherDefensiveSelector);
				taskDefensiveStance.addTask(rightCavalryDefensiveSelector);
				taskDefensiveStance.addTask(leftCavalryDefensiveSelector);

				//Final Tree
				this.tree = new Sequence(null);
				this.tree.addTask(taskAgressiveStance);
				this.tree.addTask(taskDefensiveStance);

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
