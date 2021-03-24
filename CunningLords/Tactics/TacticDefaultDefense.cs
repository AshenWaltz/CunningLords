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

		/*protected Formation _maininfantry;
        protected Formation _archers;
        protected Formation _leftCavalry;
        protected Formation _rightCavalry;
        protected Formation _rangedCavalry;*/

		private static int tickCounter = 0;

        public TacticDefaultDefense(Team team) : base(team)
        {
            _hasBattleBeenJoined = false;
            _AIControlledFormationCount = base.Formations.Count((Formation f) => f.IsAIControlled);
        }

		private void Defend()
        {
			InformationManager.DisplayMessage(new InformationMessage("Has entered: Defend"));
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				SetDefaultBehaviorWeights(this._mainInfantry);
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorHoldHighGround>(1f);
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				SetDefaultBehaviorWeights(this._archers);
				this._archers.AI.SetBehaviorWeight<BehaviorSkirmishLine>(1f);
				this._archers.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(1f);
			}
			if (this._leftCavalry != null)
			{
				this._leftCavalry.AI.ResetBehaviorWeights();
				SetDefaultBehaviorWeights(this._leftCavalry);
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorProtectFlank>(1f).FlankSide = FormationAI.BehaviorSide.Left;
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorCavalryScreen>(1f);
			}
			if (this._rightCavalry != null)
			{
				this._rightCavalry.AI.ResetBehaviorWeights();
				SetDefaultBehaviorWeights(this._rightCavalry);
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorProtectFlank>(1f).FlankSide = FormationAI.BehaviorSide.Right;
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorCavalryScreen>(1f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				SetDefaultBehaviorWeights(this._rangedCavalry);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorMountedSkirmish>(1f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(1f);
			}
		}

		private void Engage()
        {
			InformationManager.DisplayMessage(new InformationMessage("Has entered: Engage"));
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				this._archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._leftCavalry != null)
			{
				this._leftCavalry.AI.ResetBehaviorWeights();
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._rightCavalry != null)
			{
				this._rightCavalry.AI.ResetBehaviorWeights();
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
		}

		private void ThirdState()
		{
			InformationManager.DisplayMessage(new InformationMessage("Has entered: Third State"));
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				this._mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._archers != null)
			{
				this._archers.AI.ResetBehaviorWeights();
				this._archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._leftCavalry != null)
			{
				this._leftCavalry.AI.ResetBehaviorWeights();
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._rightCavalry != null)
			{
				this._rightCavalry.AI.ResetBehaviorWeights();
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
			if (this._rangedCavalry != null)
			{
				this._rangedCavalry.AI.ResetBehaviorWeights();
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
			}
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

			//Infantry Hold Line
			TaskHoldLine MIHold = new TaskHoldLine(this._mainInfantry);

			//Archers Volley
			TaskInitialVolley AInitialVolley = new TaskInitialVolley(this._archers);
			TaskBehindVolley ABehindVolley = new TaskBehindVolley(this._archers);
			Selector archerSelector = new Selector();
			archerSelector.addTask(AInitialVolley);
			archerSelector.addTask(ABehindVolley);

			//Horse Archers
			TaskRangedHarrassment HARangedHar = new TaskRangedHarrassment(this._rangedCavalry);
			TaskRangedRearHarrassment HARangedRearHar = new TaskRangedRearHarrassment(this._rangedCavalry);
			Selector horseArcherSelector = new Selector();
			horseArcherSelector.addTask(HARangedHar);
			horseArcherSelector.addTask(HARangedRearHar);

			//Right Cavalry
			TaskAttackFlank RCAttackFlank = new TaskAttackFlank(this._rightCavalry);
			TaskProtectFlank RCProtectFlank = new TaskProtectFlank(this._rightCavalry);
			Selector rightCavalrySelector = new Selector();
			rightCavalrySelector.addTask(RCAttackFlank);
			rightCavalrySelector.addTask(RCProtectFlank);

			//Left Cavalry
			TaskAttackFlank LCAttackFlank = new TaskAttackFlank(this._leftCavalry);
			TaskProtectFlank LCProtectFlank = new TaskProtectFlank(this._leftCavalry);
			Selector leftCavalrySelector = new Selector();
			leftCavalrySelector.addTask(LCAttackFlank);
			leftCavalrySelector.addTask(LCProtectFlank);

			//Final Tree
			Sequence tree = new Sequence(null);
			tree.addTask(MIHold);
			tree.addTask(archerSelector);
			tree.addTask(horseArcherSelector);
			tree.addTask(rightCavalrySelector);
			tree.addTask(leftCavalrySelector);


			tree.run();
			/*if((tickCounter / 10) == 0)
            {
				Defend();
            }
			else if((tickCounter / 10) == 1)
            {
				Engage();
            }
			else if((tickCounter / 10) == 2)
            {
				ThirdState();
            }
            else
            {
				tickCounter = 0;
            }

			tickCounter++;*/
			/*if (!base.AreFormationsCreated)
			{
				return;
			}
			if (CheckAndSetAvailableFormationsChanged())
			{
				ManageFormationCounts();
				if (_hasBattleBeenJoined)
				{
					Defend();
				}
				else
				{
					Engage();
				}
			}
			bool flag = hasBattleBeenJoined();
			if (flag != _hasBattleBeenJoined || IsTacticReapplyNeeded)
			{
				_hasBattleBeenJoined = flag;
				if (_hasBattleBeenJoined)
				{
					Engage();
				}
				else
				{
					Defend();
				}
				IsTacticReapplyNeeded = false;
			}*/
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
