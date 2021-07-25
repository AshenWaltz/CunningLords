using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.Behaviors;

namespace CunningLords.BehaviorTreelogic
{
    class TaskRangedRearHarrassment : Task
    {
        public TaskRangedRearHarrassment(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if ((this.formation != null) && this.formation.Team.QuerySystem.EnemyCavalryRatio <= 0.2f)
            {
                this.formation.AI.ResetBehaviorWeights();
                BehaviorSkirmishMode behavior = this.formation.AI.SetBehaviorWeight<BehaviorSkirmishMode>(1f);
                //behavior.Formation = this.formation;
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.succeeded;
            }
        }
    }
}