﻿using System;
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
    class TaskStop : Task
    {
        public TaskStop(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if ((this.formation != null))
            {
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorStop>(2f);

                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
