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
    class Sequence : Task
    {
        public Sequence(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if (this.children.Count() > 0)
            {
                foreach (Task c in this.children)
                {
                    if (c.run() == BTReturnEnum.failed)
                    {
                        return BTReturnEnum.failed;
                    }
                }
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.isEmpty;
            }
        }
    }
}