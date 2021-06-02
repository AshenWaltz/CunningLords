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
    class TaskCycleCharge : Task
    {
        private float engageDistance;

        private FormationClass formationFocus;

        private int cycleChargeCounter;

        private int retreatCounter;

        private int chargeDuration;
        public TaskCycleCharge(Formation f, float distance, int duration) : base(f)
        {
            this.formation = f;
            this.engageDistance = distance;
            this.cycleChargeCounter = 0;
            this.chargeDuration = duration;
            this.retreatCounter = duration;
        }

        public override BTReturnEnum run() //If allied units are engaged in melee, charge
        {                                  //
            if ((this.formation != null))
            {
                Formation closestFormation = Utils.GetClosestPlayerFormation(this.formation);
                if (this.formation.QuerySystem.AveragePosition.Distance(closestFormation.QuerySystem.AveragePosition) < engageDistance)
                {
                    if (this.cycleChargeCounter < this.chargeDuration)
                    {
                        this.cycleChargeCounter++;
                        this.formation.AI.ResetBehaviorWeights();

                        this.formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
                    }
                    else if (this.cycleChargeCounter >= this.chargeDuration)
                    {
                        this.retreatCounter = 0;
                        this.retreatCounter++;
                        this.formation.AI.ResetBehaviorWeights();

                        this.formation.AI.SetBehaviorWeight<BehaviorRetreat>(1f);
                    }
                    else if (this.retreatCounter < this.chargeDuration)
                    {
                        this.retreatCounter++;
                        this.formation.AI.ResetBehaviorWeights();

                        this.formation.AI.SetBehaviorWeight<BehaviorRetreat>(1f);
                    }
                    else if (this.retreatCounter >= this.chargeDuration)
                    {
                        this.cycleChargeCounter = 0;
                        this.cycleChargeCounter++;
                        this.formation.AI.ResetBehaviorWeights();

                        this.formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
                    }

                    return BTReturnEnum.succeeded;
                }
                else
                {
                    this.cycleChargeCounter = 0;
                    this.retreatCounter = 0;
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
