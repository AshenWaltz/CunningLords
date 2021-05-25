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
				//Infantry Slow Advance
				BehaviorConfig MISlowAdvanceConfig = new BehaviorConfig(ArrangementOrder.ArrangementOrderSquare,
																		FiringOrder.FiringOrderFireAtWill,
																		FormOrder.FormOrderDeep,
																		WeaponUsageOrder.WeaponUsageOrderUseAny,
																		RidingOrder.RidingOrderFree,
																		FacingOrder.FacingOrderLookAtEnemy);
				float infantryEngageDistance = 30f;
				TaskSlowAdvance MISlowAdvance = new TaskSlowAdvance(this._mainInfantry, MISlowAdvanceConfig, infantryEngageDistance, FormationClass.Infantry);
				TaskProximityCharge MITargetedCharge = new TaskProximityCharge(this._mainInfantry, MISlowAdvanceConfig, infantryEngageDistance);
				Selector mainInfantrySelector = new Selector(null);
				mainInfantrySelector.addTask(MISlowAdvance);
				mainInfantrySelector.addTask(MITargetedCharge);

				//Archers Hide Behind Infantry
				BehaviorConfig AHideBehindConfig = new BehaviorConfig(ArrangementOrder.ArrangementOrderLoose,
																		FiringOrder.FiringOrderFireAtWill,
																		FormOrder.FormOrderWide,
																		WeaponUsageOrder.WeaponUsageOrderUseAny,
																		RidingOrder.RidingOrderFree,
																		FacingOrder.FacingOrderLookAtEnemy);
				TaskHideBehind ABehindVolley = new TaskHideBehind(this._archers, AHideBehindConfig, FormationClass.Infantry);
				Selector archerSelector = new Selector(null);
				archerSelector.addTask(ABehindVolley);

				//Horse Archers Vanguard and Behind Volley
				TaskArcherBehindVolley HABehindVolley = new TaskArcherBehindVolley(this._rangedCavalry);
				TaskArcherVolley HAVolley = new TaskArcherVolley(this._rangedCavalry);
				Selector horseArcherSelector = new Selector(null);
				horseArcherSelector.addTask(HABehindVolley);
				horseArcherSelector.addTask(HAVolley);

				//Right Cavalry Charge
				TaskStop RCCharge = new TaskStop(this._rightCavalry);
				Selector rightCavalrySelector = new Selector(null);
				rightCavalrySelector.addTask(RCCharge);

				//Left Cavalry Charge
				TaskStop LCCharge = new TaskStop(this._leftCavalry);
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
