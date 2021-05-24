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
    public class TacticDefaultDefense : TacticComponent
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

        public TacticDefaultDefense(Team team) : base(team)
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
			else if(base.AreFormationsCreated && this.tickCounter == 0)
            {
				//Infantry Hold Line
				TaskHoldLine MIHold = new TaskHoldLine(this._mainInfantry);
				TaskBraceForImpact MIBraceImpact = new TaskBraceForImpact(this._mainInfantry);
				Selector mainInfantrySelector = new Selector(null);
				mainInfantrySelector.addTask(MIHold);
				mainInfantrySelector.addTask(MIBraceImpact);

				//Archers Volley
				TaskArcherVolley AInitialVolley = new TaskArcherVolley(this._archers);
				TaskArcherSkirmish ASkirmish = new TaskArcherSkirmish(this._archers);
				TaskArcherBehindVolley ABehindVolley = new TaskArcherBehindVolley(this._archers);
				TaskRangedNoAmmoCharge ACharge = new TaskRangedNoAmmoCharge(this._archers); //Does nothing new yet
				Selector archerSelector = new Selector(null);
				archerSelector.addTask(AInitialVolley);
				archerSelector.addTask(ABehindVolley);
				archerSelector.addTask(ASkirmish);
				archerSelector.addTask(ACharge);

				//Horse Archers
				TaskRangedHarrassment HARangedHar = new TaskRangedHarrassment(this._rangedCavalry);
				TaskRangedRearHarrassment HARangedRearHar = new TaskRangedRearHarrassment(this._rangedCavalry); //Does nothing new yet
				TaskRangedNoAmmoCharge HACharge = new TaskRangedNoAmmoCharge(this._rangedCavalry); //Does nothing new yet
				Selector horseArcherSelector = new Selector(null);
				horseArcherSelector.addTask(HARangedHar);
				horseArcherSelector.addTask(HARangedRearHar);
				horseArcherSelector.addTask(HACharge);

				//Right Cavalry
				TaskAttackFlank RCAttackFlank = new TaskAttackFlank(this._rightCavalry); //Does nothing new yet
				TaskProtectFlank RCProtectFlank = new TaskProtectFlank(this._rightCavalry, FormationAI.BehaviorSide.Right); //Does nothing new yet
				Selector rightCavalrySelector = new Selector(null);
				rightCavalrySelector.addTask(RCAttackFlank);
				rightCavalrySelector.addTask(RCProtectFlank);

				//Left Cavalry
				TaskAttackFlank LCAttackFlank = new TaskAttackFlank(this._leftCavalry); //Does nothing new yet
				TaskProtectFlank LCProtectFlank = new TaskProtectFlank(this._leftCavalry, FormationAI.BehaviorSide.Left); //Does nothing new yet
				Selector leftCavalrySelector = new Selector(null);
				leftCavalrySelector.addTask(LCAttackFlank);
				leftCavalrySelector.addTask(LCProtectFlank);

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
