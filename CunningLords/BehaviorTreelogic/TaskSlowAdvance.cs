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
    class TaskSlowAdvance : Task
    {
        private BehaviorConfig behaviorConfig;

        private float engageDistance;

        private FormationClass formationFocus;

        public TaskSlowAdvance(Formation f, BehaviorConfig config, float distance, FormationClass focus) : base(f)
        {
            this.formation = f;
            this.behaviorConfig = config;
            this.engageDistance = distance;
            this.formationFocus = focus;
        }

        public override BTReturnEnum run() //verifies if there is a formation closer than the engage distance, if so stop sow advance
        {                                  //Otherwise, continue to advance towards focus.
            if ((this.formation != null))
            {
                List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(this.formation);

                foreach (Tuple<Formation, float> tup in distances)
                {
                    if (tup.Item2 < this.engageDistance)
                    {
                        return BTReturnEnum.failed;
                    }
                }

                this.formation.AI.ResetBehaviorWeights();
                BehaviorSlowAdvance behavior = this.formation.AI.SetBehaviorWeight<BehaviorSlowAdvance>(2f);
                behavior.Formation = this.formation;
                behavior.config = this.behaviorConfig;
                behavior.focus = this.formationFocus;

                InformationManager.DisplayMessage(new InformationMessage("Slow Advance!"));
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.succeeded;
            }
        }
    }
}
