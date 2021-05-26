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

namespace CunningLords.BehaviorTreelogic
{
    class TaskAttackFlank : Task
    {
        public TaskAttackFlank(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if ((this.formation != null))
            {
                InformationManager.DisplayMessage(new InformationMessage(Utils.GetPlayerFormationRatios(this.formation, FormationClass.Cavalry).ToString()));
                if (Utils.GetPlayerFormationRatios(this.formation, FormationClass.Cavalry) > 0.1f)
                {
                    this.formation.AI.ResetBehaviorWeights();

                    this.formation.AI.SetBehaviorWeight<BehaviorFlank>(2f);

                    return BTReturnEnum.succeeded;
                }
                else
                {
                    return BTReturnEnum.failed;
                }

                /*BehaviorProtectAroundAnchor behavior = this.formation.AI.SetBehaviorWeight<BehaviorProtectAroundAnchor>(1f);
                behavior.Formation = this.formation;
                behavior.FlankSide = this.flankSide;

                return BTReturnEnum.succeeded;*/
            }
            else
            {
                return BTReturnEnum.succeeded;
            }
        }
    }
}