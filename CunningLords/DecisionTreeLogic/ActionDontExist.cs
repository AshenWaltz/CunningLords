using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace CunningLords.DecisionTreeLogic
{
    class ActionDontExist : DecisionTreeNode
    {
        public ActionDontExist(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override void makeDecision()
        {

        }
    }
}
