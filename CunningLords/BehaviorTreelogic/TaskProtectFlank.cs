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
            if ((this.formation != null) && true/*&& this.formation.Team.QuerySystem.EnemyCavalryRatio > 0.1f*/)
            {

                this.formation.AI.ResetBehaviorWeights();

                BehaviorProtectAroundAnchor behavior = this.formation.AI.SetBehaviorWeight<BehaviorProtectAroundAnchor>(1f);
                behavior.Formation = this.formation;
                behavior.FlankSide = this.flankSide;

                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}