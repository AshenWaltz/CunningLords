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
    class TaskHoldLine : Task
    {
        public TaskHoldLine(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if((this.formation != null))
            {
                float braceDistance = 20f;

                List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(this.formation);

                bool brace = false;

                foreach (Tuple<Formation, float> tup in distances)
                {
                    if (tup.Item2 < braceDistance)
                    {
                        brace = true;
                    }
                }
                if((this.formation != null) && (!brace))
                {
                    BehaviorLooselyWaitOrders behavior = this.formation.AI.SetBehaviorWeight<BehaviorLooselyWaitOrders>(1f);
                    behavior.Formation = this.formation;

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
