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
    class TacticLevelTwoVladian : TacticComponent
    {
		private bool _hasBattleBeenJoined;

		private int _AIControlledFormationCount;

		private Task tree;

		private int tickCounter = 0;

		public TacticLevelTwoVladian(Team team) : base(team)
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
				BehaviorConfig MISlowAdvanceConfig = new BehaviorConfig(ArrangementOrder.ArrangementOrderLoose,
																		FiringOrder.FiringOrderFireAtWill,
																		FormOrder.FormOrderWide,
																		WeaponUsageOrder.WeaponUsageOrderUseAny,
																		RidingOrder.RidingOrderFree,
																		FacingOrder.FacingOrderLookAtEnemy);
				TaskSlowAdvance MISlowAdvance = new TaskSlowAdvance(this._mainInfantry, MISlowAdvanceConfig, infantryEngageDistance, FormationClass.Infantry);
				TaskProximityCharge MIProximityCharge = new TaskProximityCharge(this._mainInfantry, 30);
				Selector mainInfantryAgressiveSelector = new Selector(null);
				mainInfantryAgressiveSelector.addTask(MISlowAdvance);
				mainInfantryAgressiveSelector.addTask(MIProximityCharge);
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
				TaskCharge RCCharge = new TaskCharge(this._rightCavalry);
				Selector rightCavalryAgressiveSelector = new Selector(null);
				rightCavalryAgressiveSelector.addTask(RCCharge);
				//Left Cavalry Agressive
				TaskCharge LCCharge = new TaskCharge(this._leftCavalry);
				Selector leftCavalryAgressiveSelector = new Selector(null);
				leftCavalryAgressiveSelector.addTask(LCCharge);
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
				TaskCycleCharge RCCycleCharge = new TaskCycleCharge(this._rightCavalry, 50, 50);
				Selector rightCavalryDefensiveSelector = new Selector(null);
				rightCavalryDefensiveSelector.addTask(RCProtectFlank);
				rightCavalryDefensiveSelector.addTask(RCCycleCharge);
				//Left Cavalry Protect and Attack Flank
				TaskProtectFlank LCProtectFlank = new TaskProtectFlank(this._leftCavalry, FormationAI.BehaviorSide.Left);
				TaskCycleCharge LCCycleCharge = new TaskCycleCharge(this._leftCavalry, 50, 50);
				Selector leftCavalryDefensiveSelector = new Selector(null);
				leftCavalryDefensiveSelector.addTask(LCProtectFlank);
				leftCavalryDefensiveSelector.addTask(LCCycleCharge);
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
