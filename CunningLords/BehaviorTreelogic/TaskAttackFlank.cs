using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CunningLords.BehaviorTreelogic
{
    class TaskAttackFlank : Task
    {
        public TaskAttackFlank(Formation f = null) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if ((this.formation != null) && this.formation.Team.QuerySystem.EnemyCavalryRatio <= 0.3f)
            {
                InformationManager.DisplayMessage(new InformationMessage("Cavalry: Flanking"));
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorFlank>(2f);
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}