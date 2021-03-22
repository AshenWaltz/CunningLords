using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

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

        public TacticDefaultDefense(Team team) : base(team)
        {
            _hasBattleBeenJoined = false;
            _AIControlledFormationCount = base.Formations.Count((Formation f) => f.IsAIControlled);
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
			if (CheckAndSetAvailableFormationsChanged())
			{
				ManageFormationCounts();
				if (_hasBattleBeenJoined)
				{
					//Attack();
				}
				else
				{
					//Advance();
				}
			}
			bool flag = hasBattleBeenJoined();
			if (flag != _hasBattleBeenJoined || IsTacticReapplyNeeded)
			{
				_hasBattleBeenJoined = flag;
				if (_hasBattleBeenJoined)
				{
					//Attack();
				}
				else
				{
					//Advance();
				}
				IsTacticReapplyNeeded = false;
			}
			base.TickOccasionally();
		}
    }
}
