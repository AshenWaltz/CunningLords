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
    class Selector : Task
    {
        public Selector(Formation f = null) : base(f)
        {
        }

        public override BTReturnEnum run()
        {
            if(this.children.Count() > 0)
            {
                foreach (Task c in this.children)
                {
                    if (c.run() == BTReturnEnum.succeeded)
                    {
                        return BTReturnEnum.succeeded;
                    }
                }
                return BTReturnEnum.failed;
            }
            else
            {
                return BTReturnEnum.isEmpty;
            }
        }
    }
}
