using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace CunningLords.DecisionTreeLogic
{
    class DecisionIsFormationNotNull : DecisionTreeNode
    {
        protected DecisionTreeNode trueNode;

        protected DecisionTreeNode falseNode;

        public DecisionIsFormationNotNull(Formation f, DecisionTreeNode tn, DecisionTreeNode fn) : base(f)
        {
            this.formation = f;
            this.trueNode = tn;
            this.falseNode = fn;
        }

        private DecisionTreeNode getBranch()
        {
            if (this.formation != null)
            {
                return this.trueNode;
            }
            else
            {
                return this.falseNode;
            }
        }

        public override void makeDecision()
        {
            DecisionTreeNode chosen = this.getBranch();
            chosen.makeDecision();
        }
    }
}
