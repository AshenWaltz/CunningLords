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
    class TaskRangedHarrassment : Task
    {
        public TaskRangedHarrassment(Formation f = null) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if ((this.formation != null) && this.formation.Team.QuerySystem.EnemyCavalryRatio > 0.2f)
            {
                InformationManager.DisplayMessage(new InformationMessage("Horse Archers: Harrassing"));
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorMountedSkirmish>(2f);
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
