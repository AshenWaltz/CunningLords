﻿using System;
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
    class TaskInitialVolley : Task
    {
        public TaskInitialVolley(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            //InformationManager.DisplayMessage(new InformationMessage("Archers: Forward Volley"));
            if ((this.formation != null) && (this.formation.QuerySystem.AveragePosition.Distance(this.formation.QuerySystem.ClosestEnemyFormation.AveragePosition) 
                > (this.formation.QuerySystem.MissileRange / 3)))
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
