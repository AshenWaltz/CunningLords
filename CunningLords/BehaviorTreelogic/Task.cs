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
    public abstract class Task
    {
        protected Formation formation;

        protected List<Task> children;

        public Task(Formation f = null)
        {
            this.formation = f;
            this.children = new List<Task>();
        }

        public abstract BTReturnEnum run();

        public void addTask(Task t)
        {
            this.children.Add(t);
        }

        public void removeTask(Task t)
        {
            this.children.Remove(t);
        }
    }
}
