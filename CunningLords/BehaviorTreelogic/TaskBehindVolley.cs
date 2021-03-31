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
    class TaskBehindVolley : Task
    {
        public TaskBehindVolley(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            //InformationManager.DisplayMessage(new InformationMessage("Archers: Volley from Behind"));
            if ((this.formation != null) && (this.formation.QuerySystem.AveragePosition.Distance(this.formation.QuerySystem.ClosestEnemyFormation.AveragePosition)
                <= (this.formation.QuerySystem.MissileRange / 3)))
            {
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorSkirmishLine>(2f);
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
