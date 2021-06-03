using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace CunningLords.DecisionTreeLogic
{
    public abstract class DecisionTreeNode
    {
        protected Formation formation;
        public DecisionTreeNode(Formation f)
        {
            this.formation = f;
        }

        public abstract void makeDecision();
    }
}
