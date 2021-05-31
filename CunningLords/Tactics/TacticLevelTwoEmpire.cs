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
    class TacticLevelTwoEmpire : TacticComponent
    {
		private bool _hasBattleBeenJoined;

		private int _AIControlledFormationCount;

		private Task tree;

		private int tickCounter = 0;

		public TacticLevelTwoEmpire(Team team) : base(team)
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
				BehaviorConfig MISlowAdvanceConfig = new BehaviorConfig(ArrangementOrder.ArrangementOrderSquare,
																		FiringOrder.FiringOrderFireAtWill,
																		FormOrder.FormOrderDeep,
																		WeaponUsageOrder.WeaponUsageOrderUseAny,
																		RidingOrder.RidingOrderFree,
																		FacingOrder.FacingOrderLookAtEnemy);
				float infantryEngageDistance = 30f;
				TaskSlowAdvance MISlowAdvance = new TaskSlowAdvance(this._mainInfantry, MISlowAdvanceConfig, infantryEngageDistance, FormationClass.Infantry);
				TaskProximityCharge MITargetedCharge = new TaskProximityCharge(this._mainInfantry, MISlowAdvanceConfig, infantryEngageDistance);
				Selector mainInfantryAgressiveSelector = new Selector(null);
				mainInfantryAgressiveSelector.addTask(MISlowAdvance);
				mainInfantryAgressiveSelector.addTask(MITargetedCharge);
				//Archers Agressive
				BehaviorConfig AHideBehindConfig = new BehaviorConfig(ArrangementOrder.ArrangementOrderLoose,
																		FiringOrder.FiringOrderFireAtWill,
																		FormOrder.FormOrderWide,
																		WeaponUsageOrder.WeaponUsageOrderUseAny,
																		RidingOrder.RidingOrderFree,
																		FacingOrder.FacingOrderLookAtEnemy);
				TaskHideBehind ABehindVolley = new TaskHideBehind(this._archers, AHideBehindConfig, FormationClass.Infantry);
				Selector archerAgressiveSelector = new Selector(null);
				archerAgressiveSelector.addTask(ABehindVolley);
				//Horse Archers Agressive
				TaskArcherBehindVolley HABehindVolley = new TaskArcherBehindVolley(this._rangedCavalry);
				TaskArcherVolley HAVolley = new TaskArcherVolley(this._rangedCavalry);
				Selector horseArcherAgressiveSelector = new Selector(null);
				horseArcherAgressiveSelector.addTask(HABehindVolley);
				horseArcherAgressiveSelector.addTask(HAVolley);
				//Right Cavalry Agressive
				TaskAttackFlank RCAttackFlank = new TaskAttackFlank(this._rightCavalry);
				TaskFlankCharge RCFlankCharge = new TaskFlankCharge(this._rightCavalry, 10f);
				Selector rightCavalryAgressiveSelector = new Selector(null);
				rightCavalryAgressiveSelector.addTask(RCFlankCharge);
				rightCavalryAgressiveSelector.addTask(RCAttackFlank);
				//Left Cavalry Agressive
				TaskAttackFlank LCAttackFlank = new TaskAttackFlank(this._leftCavalry);
				TaskFlankCharge LCFlankCharge = new TaskFlankCharge(this._leftCavalry, 10f);
				Selector leftCavalryAgressiveSelector = new Selector(null);
				leftCavalryAgressiveSelector.addTask(LCFlankCharge);
				leftCavalryAgressiveSelector.addTask(LCAttackFlank);
				//Agressive Node Assembly
				taskAgressiveStance.addTask(mainInfantryAgressiveSelector);
				taskAgressiveStance.addTask(archerAgressiveSelector);
				taskAgressiveStance.addTask(horseArcherAgressiveSelector);
				taskAgressiveStance.addTask(rightCavalryAgressiveSelector);
				taskAgressiveStance.addTask(leftCavalryAgressiveSelector);

				//Defensive Stance
				TaskDefensiveStance taskDefensiveStance = new TaskDefensiveStance(this._mainInfantry);
				//Infantry Defensive
				TaskBraceForImpact MIBraceForImpact = new TaskBraceForImpact(this._mainInfantry);
				Selector mainInfantryDefensiveSelector = new Selector(null);
				mainInfantryDefensiveSelector.addTask(MIBraceForImpact);
				//Archers Defensive
				Selector archerDefensiveSelector = new Selector(null);
				archerDefensiveSelector.addTask(ABehindVolley);
				//Horse Archers Defensive
				Selector horseArcherDefensiveSelector = new Selector(null);
				horseArcherDefensiveSelector.addTask(HABehindVolley);
				horseArcherDefensiveSelector.addTask(HAVolley);
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
