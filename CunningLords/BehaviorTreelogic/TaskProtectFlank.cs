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
using CunningLords.Patches;

namespace CunningLords.BehaviorTreelogic
{
    class TaskProtectFlank : Task
    {
        public FormationAI.BehaviorSide flankSide;

        public TaskProtectFlank(Formation f, FormationAI.BehaviorSide side) : base(f)
        {
            this.formation = f;
            this.flankSide = side;
        }

        public override BTReturnEnum run()
        {
            if ((this.formation != null))
            {
                InformationManager.DisplayMessage(new InformationMessage(Utils.GetPlayerFormationRatios(this.formation, FormationClass.Cavalry).ToString()));

                if (Utils.GetPlayerFormationRatios(this.formation, FormationClass.Cavalry) > 0.1f)
                {
                    this.formation.AI.ResetBehaviorWeights();

                    this.formation.AI.SetBehaviorWeight<BehaviorProtectFlank>(2f).FlankSide = this.flankSide;

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