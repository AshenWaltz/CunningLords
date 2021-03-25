using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CunningLords.BehaviorTreelogic
{
    class TaskHoldLine : Task
    {
        public TaskHoldLine(Formation f) : base(f)
        {
            /*if(f != null)
            {
                InformationManager.DisplayMessage(new InformationMessage("f: " + f.ToString()));
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("f: null" ));
            }*/
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if((this.formation != null))
            {
                InformationManager.DisplayMessage(new InformationMessage("Infantry: Hold Line"));
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorDefend>(2f);
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
