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
    class TaskHideBehind : Task
    {
        private BehaviorConfig behaviorConfig;
        private FormationClass formationFocus;
        public TaskHideBehind(Formation f, BehaviorConfig config, FormationClass focus) : base(f)
        {
            this.formation = f;
            this.behaviorConfig = config;
            this.formationFocus = focus;
        }

        public override BTReturnEnum run() //This task simply forces a formation to always maintain itself behind another
        {                                  //If no proper focuses are found, this returns failed
            if ((this.formation != null))
            {
                bool isFocusNotNull = (this.formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == this.formationFocus) != null);

                if (isFocusNotNull)
                {
                    this.formation.AI.ResetBehaviorWeights();
                    BehaviorHideBehind behavior = this.formation.AI.SetBehaviorWeight<BehaviorHideBehind>(2f);
                    behavior.Formation = this.formation;
                    behavior.config = this.behaviorConfig;
                    behavior.focus = this.formationFocus;

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
