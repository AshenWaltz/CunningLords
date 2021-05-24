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
    class TaskProximityCharge : Task
    {
        private BehaviorConfig behaviorConfig;

        private float engageDistance;

        private FormationClass formationFocus;

        public TaskProximityCharge(Formation f, BehaviorConfig config, float distance) : base(f)
        {
            this.formation = f;
            this.behaviorConfig = config;
            this.engageDistance = distance;
        }

        public override BTReturnEnum run() //If the target or any other formation is closer than "engageDistance"
        {                                  //Otherwise, continue to advance towards focus.
            if ((this.formation != null))
            {
                Formation closestFormation = Utils.GetClosestPlayerFormation(this.formation);

                if (closestFormation != null && (this.formation.QuerySystem.AveragePosition.Distance(closestFormation.QuerySystem.AveragePosition) < engageDistance))
                {
                    this.formation.AI.ResetBehaviorWeights();
                    BehaviorProximityCharge behavior = this.formation.AI.SetBehaviorWeight<BehaviorProximityCharge>(2f);
                    behavior.Formation = this.formation;
                    behavior.config = this.behaviorConfig;

                    return BTReturnEnum.succeeded;
                }
                else
                {
                    return BTReturnEnum.failed;
                }
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
