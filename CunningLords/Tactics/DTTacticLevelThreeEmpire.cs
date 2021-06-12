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
using CunningLords.Patches;

namespace CunningLords.Tactics
{
    class DTTacticLevelThreeEmpire : TacticComponent
    {
		private bool _hasBattleBeenJoined;

		private int _AIControlledFormationCount;

		private DecisionTreeNode infantryTree;
		private DecisionTreeNode archersTree;
		private DecisionTreeNode leftCavalryTree;
		private DecisionTreeNode rightCavalryTree;
		private DecisionTreeNode HorseArchersTree;


		private int tickCounter = 0;

		public DTTacticLevelThreeEmpire(Team team) : base(team)
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
				ActionAdvance infantryAdvance = new ActionAdvance(this._mainInfantry);
				ActionHoldLine infantryHoldLine = new ActionHoldLine(this._mainInfantry);
				ActionDontExist infantryDontExist = new ActionDontExist(this._mainInfantry);

				DecisionClosestEnemyCloserThan decisionCloserThan = new DecisionClosestEnemyCloserThan(this._mainInfantry, infantryCharge, infantryAdvance, 30f);
				DecisionIsAttacker decisionIsAttacker = new DecisionIsAttacker(this._mainInfantry, decisionCloserThan, infantryHoldLine);

				DecisionIsFormationNotNull decisionInfantryNotNull = new DecisionIsFormationNotNull(this._mainInfantry, decisionIsAttacker, infantryDontExist);

				//Archers
				ActionScreenedSkirmish archersScreenedSkirmish = new ActionScreenedSkirmish(this._archers);
				ActionDontExist archersDontExist = new ActionDontExist(this._archers);
				ActionHoldLine archersHoldLine = new ActionHoldLine(this._archers);

				DecisionIsAttacker decisionArcherIsAttacker = new DecisionIsAttacker(this._archers, archersScreenedSkirmish, archersHoldLine);

				DecisionIsFormationNotNull decisionArchersNotNull = new DecisionIsFormationNotNull(this._archers, decisionArcherIsAttacker, archersDontExist);

				//Left Cavalry
				ActionFlank leftCavalryFlank = new ActionFlank(this._leftCavalry);
				ActionCharge leftCavalryCharge = new ActionCharge(this._leftCavalry);
				ActionProtectFlank leftCavalryProtectFlank = new ActionProtectFlank(this._leftCavalry, FormationAI.BehaviorSide.Left);
				ActionDontExist leftCavalryDontExist = new ActionDontExist(this._leftCavalry);
				List<Formation> enemyCavalry = Utils.PlayerFormationsOfType(FormationClass.Cavalry, this._leftCavalry);
				Formation target = null;
				if (enemyCavalry != null && enemyCavalry.Count >= 0)
                {
					target = enemyCavalry.First();
                }

				DecisionClosestEnemyCloserThan IsInfantryEngaged = new DecisionClosestEnemyCloserThan(this._mainInfantry, leftCavalryCharge, leftCavalryFlank, 30f);

				DecisionIsFormationNotNull IsInfantryNotNull = new DecisionIsFormationNotNull(this._mainInfantry, IsInfantryEngaged, leftCavalryCharge);

				DecisionClosestEnemyCloserThan IsLeftCavalryDevensiveAttacking = new DecisionClosestEnemyCloserThan(this._leftCavalry, leftCavalryCharge, leftCavalryFlank, 50f);

				DecisionEnemyFormationWeaker isLeftCavalryStronger = new DecisionEnemyFormationWeaker(this._leftCavalry, IsLeftCavalryDevensiveAttacking, leftCavalryProtectFlank, FormationClass.Cavalry, 2);

				DecisionIsFormationNotNull IsEnemyCavalryNotNull = new DecisionIsFormationNotNull(target, isLeftCavalryStronger, IsLeftCavalryDevensiveAttacking);
				
				DecisionIsAttacker IsLeftCavalryAttacker = new DecisionIsAttacker(this._leftCavalry, IsInfantryNotNull, IsEnemyCavalryNotNull);

				DecisionIsFormationNotNull IsLeftCavalryNotNull = new DecisionIsFormationNotNull(this._leftCavalry, IsLeftCavalryAttacker, leftCavalryDontExist);

				//Right Cavalry
				ActionFlank rightCavalryFlank = new ActionFlank(this._rightCavalry);
				ActionCharge rightCavalryCharge = new ActionCharge(this._rightCavalry);
				ActionProtectFlank rightCavalryCavalryProtectFlank = new ActionProtectFlank(this._rightCavalry, FormationAI.BehaviorSide.Right);
				ActionDontExist rightCavalryCavalryDontExist = new ActionDontExist(this._rightCavalry);

				DecisionClosestEnemyCloserThan IsInfantryEngagedII = new DecisionClosestEnemyCloserThan(this._mainInfantry, rightCavalryCharge, rightCavalryFlank, 30f);

				DecisionIsFormationNotNull IsInfantryNotNullII = new DecisionIsFormationNotNull(this._mainInfantry, IsInfantryEngagedII, rightCavalryCharge);

				DecisionClosestEnemyCloserThan IsRightCavalryDevensiveAttacking = new DecisionClosestEnemyCloserThan(this._rightCavalry, leftCavalryCharge, leftCavalryFlank, 50f);

				DecisionEnemyFormationWeaker isRightCavalryStronger = new DecisionEnemyFormationWeaker(this._rightCavalry, IsRightCavalryDevensiveAttacking, rightCavalryCavalryProtectFlank, FormationClass.Cavalry, 2);

				DecisionIsFormationNotNull IsEnemyCavalryNotNullII = new DecisionIsFormationNotNull(target, isRightCavalryStronger, IsRightCavalryDevensiveAttacking);

				DecisionIsAttacker IsrightCavalryCavalryAttacker = new DecisionIsAttacker(this._rightCavalry, IsInfantryNotNullII, IsEnemyCavalryNotNullII);

				DecisionIsFormationNotNull IsrightCavalryCavalryNotNull = new DecisionIsFormationNotNull(this._rightCavalry, IsrightCavalryCavalryAttacker, rightCavalryCavalryDontExist);

				//Horse Archers
				ActionCharge horseArcherCharge = new ActionCharge(this._rangedCavalry);
				ActionHorseArcherSkirmish horseArcherSkirmish = new ActionHorseArcherSkirmish(this._rangedCavalry);
				ActionScreenedSkirmish horseArcherScreenedSkirmish = new ActionScreenedSkirmish(this._rangedCavalry);
				ActionDontExist horseArcherDontExist = new ActionDontExist(this._rangedCavalry);

				DecisionClosestEnemyCloserThan IsInfantryEngagedIII = new DecisionClosestEnemyCloserThan(this._mainInfantry, horseArcherScreenedSkirmish, horseArcherSkirmish, 30f);

				DecisionIsFormationNotNull IsInfantryNotNullIII = new DecisionIsFormationNotNull(this._mainInfantry, IsInfantryEngagedIII, horseArcherCharge);

				DecisionIsAttacker IsHorseArchersAttacker = new DecisionIsAttacker(this._rangedCavalry, IsInfantryNotNullIII, horseArcherSkirmish);

				DecisionIsFormationNotNull IsHorseArchersNotNull = new DecisionIsFormationNotNull(this._rangedCavalry, IsHorseArchersAttacker, horseArcherDontExist);

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