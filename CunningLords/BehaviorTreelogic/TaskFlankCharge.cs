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
    class TaskFlankCharge : Task
    {

        private float engageDistance;

        private FormationClass formationFocus;

        public TaskFlankCharge(Formation f, float distance) : base(f)
        {
            this.formation = f;
            this.engageDistance = distance;
        }

        public override BTReturnEnum run() //If allied units are engaged in melee, charge
        {                                  //
            if ((this.formation != null))
            {
                float distance = 1000f;

                foreach (Formation form in this.formation.Team.Formations)
                {
                    Formation closestFormation = Utils.GetClosestPlayerFormation(form);
                    if (form.QuerySystem.AveragePosition.Distance(closestFormation.QuerySystem.AveragePosition) < distance)
                    {
                        distance = form.QuerySystem.AveragePosition.Distance(closestFormation.QuerySystem.AveragePosition);
                    }
                }

                if (distance < this.engageDistance)
                {
                    this.formation.AI.ResetBehaviorWeights();
                    /*BehaviorProximityCharge behavior = this.formation.AI.SetBehaviorWeight<BehaviorProximityCharge>(2f);
                    behavior.Formation = this.formation;
                    behavior.config = this.behaviorConfig;*/

                    this.formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);

                    return BTReturnEnum.succeeded;
                }
                else
                {
                    return BTReturnEnum.failed;
                }
            }
            else
            {
                return BTReturnEnum.succeeded;
            }
        }
    }
}
