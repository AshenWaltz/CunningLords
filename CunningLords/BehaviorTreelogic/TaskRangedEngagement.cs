using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.Patches;
using CunningLords.Behaviors;

namespace CunningLords.BehaviorTreelogic
{
    class TaskRangedEngagement : Task
    {
        private float engageDistance;

        private FormationClass formationFocus;

        public TaskRangedEngagement(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run() //If allied units are engaged in melee, charge
        {                                  //
            if ((this.formation != null))
            {
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);

                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.succeeded;
            }
        }
    }
}
