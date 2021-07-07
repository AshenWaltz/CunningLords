﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.DecisionTreeLogic;
using CunningLords.Patches;

namespace CunningLords.Tactics
{
    class DTTacticLevelTwoSturgia : TacticComponent
    {
		private bool _hasBattleBeenJoined;

		private int _AIControlledFormationCount;

		private DecisionTreeNode infantryTree;
		private DecisionTreeNode archersTree;
		private DecisionTreeNode leftCavalryTree;
		private DecisionTreeNode rightCavalryTree;
		private DecisionTreeNode HorseArchersTree;


		public int tickCounter = 0;

		public Utils util;

		public DTTacticLevelTwoSturgia(Team team) : base(team)
		{
			_hasBattleBeenJoined = false;
			_AIControlledFormationCount = base.Formations.Count((Formation f) => f.IsAIControlled);
			this.util = new Utils();

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
				InformationManager.DisplayMessage(new InformationMessage("Level 2 Sturgia"));

				//Infantry
				ActionCharge infantryCharge = new ActionCharge(this._mainInfantry);
				ActionDontExist infantryDontExist = new ActionDontExist(this._mainInfantry);
				ActionHoldLine infantryHoldline = new ActionHoldLine(this._mainInfantry);

				DecisionIsAttacker decisionIsIfantryAttacker = new DecisionIsAttacker(this._mainInfantry, infantryCharge, infantryHoldline);

				DecisionIsFormationNotNull decisionInfantryNotNull = new DecisionIsFormationNotNull(this._mainInfantry, decisionIsIfantryAttacker, infantryDontExist);

				//Archers
				ActionScreenedSkirmish archersSkirmish = new ActionScreenedSkirmish(this._archers);
				ActionHoldLine archersHoldLine = new ActionHoldLine(this._archers);
				ActionDontExist archersDontExist = new ActionDontExist(this._archers);

				DecisionIsAttacker decisionIsArchersAttacker = new DecisionIsAttacker(this._archers, archersSkirmish, archersHoldLine);

				DecisionIsFormationNotNull decisionArchersNotNull = new DecisionIsFormationNotNull(this._archers, decisionIsArchersAttacker, archersDontExist);

				//Left Cavalry
				ActionCharge leftCavalryCharge = new ActionCharge(this._leftCavalry);
				ActionPullBack leftCavalryPullback = new ActionPullBack(this._leftCavalry);
				ActionAdvance leftCavalryAdvance = new ActionAdvance(this._leftCavalry);
				ActionProtectFlank leftCavalryProtectFlank = new ActionProtectFlank(this._leftCavalry, FormationAI.BehaviorSide.Left);
				ActionDontExist leftCavalryDontExist = new ActionDontExist(this._leftCavalry);

				DecisionCycleCharge decisionLeftCavalryCycleCharge = new DecisionCycleCharge(this._leftCavalry, leftCavalryCharge, leftCavalryPullback, 10, 0, ref this.util);

				DecisionClosestEnemyCloserThan decisionLeftCavalryEnemyCloser = new DecisionClosestEnemyCloserThan(this._leftCavalry, decisionLeftCavalryCycleCharge, leftCavalryAdvance, 30f);

				DecisionIsAttacker decisionLeftCavalryIsAttacker = new DecisionIsAttacker(this._leftCavalry, decisionLeftCavalryEnemyCloser, leftCavalryProtectFlank);

				DecisionIsFormationNotNull IsLeftCavalryNotNull = new DecisionIsFormationNotNull(this._leftCavalry, decisionLeftCavalryIsAttacker, leftCavalryDontExist);

				//Right Cavalry
				ActionCharge rightCavalryCharge = new ActionCharge(this._rightCavalry);
				ActionPullBack rightCavalryPullback = new ActionPullBack(this._rightCavalry);
				ActionAdvance rightCavalryAdvance = new ActionAdvance(this._rightCavalry);
				ActionProtectFlank rightCavalryProtectFlank = new ActionProtectFlank(this._rightCavalry, FormationAI.BehaviorSide.Right);
				ActionDontExist rightCavalryCavalryDontExist = new ActionDontExist(this._rightCavalry);

				DecisionCycleCharge decisionRightCavalryCycleCharge = new DecisionCycleCharge(this._rightCavalry, rightCavalryCharge, rightCavalryPullback, 10, 0, ref this.util);

				DecisionClosestEnemyCloserThan decisionRightCavalryEnemyCloser = new DecisionClosestEnemyCloserThan(this._rightCavalry, decisionRightCavalryCycleCharge, rightCavalryAdvance, 30f);

				DecisionIsAttacker decisionRightCavalryIsAttacker = new DecisionIsAttacker(this._rightCavalry, decisionRightCavalryEnemyCloser, rightCavalryProtectFlank);

				DecisionIsFormationNotNull IsrightCavalryCavalryNotNull = new DecisionIsFormationNotNull(this._rightCavalry, decisionRightCavalryIsAttacker, rightCavalryCavalryDontExist);

				//Horse Archers
				ActionHorseArcherSkirmish horseAcherSkirmish = new ActionHorseArcherSkirmish(this._rangedCavalry);
				ActionDontExist horseArcherDontExist = new ActionDontExist(this._rangedCavalry);

				DecisionIsFormationNotNull IsHorseArchersNotNull = new DecisionIsFormationNotNull(this._rangedCavalry, horseAcherSkirmish, horseArcherDontExist);

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
				this.util.IncreaseTickCounter();
			}
			else if (base.AreFormationsCreated && this.tickCounter != 0)
			{
				this.infantryTree.makeDecision();
				this.archersTree.makeDecision();
				this.leftCavalryTree.makeDecision();
				this.rightCavalryTree.makeDecision();
				this.HorseArchersTree.makeDecision();

				this.tickCounter++;
				this.util.IncreaseTickCounter();
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
