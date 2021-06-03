using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace CunningLords.DecisionTreeLogic
{
    public class Decision : DecisionTreeNode
    {
        protected DecisionTreeNode trueNode;

        protected DecisionTreeNode falseNode;

        public Decision(Formation f, ref DecisionTreeNode tn, ref DecisionTreeNode fn) : base(f)
        {
            this.formation = f;
            this.trueNode = tn;
            this.falseNode = fn;
        }

        private DecisionTreeNode getBranch()
        {
            return this.trueNode;
        }

        public override void makeDecision()
        {
            DecisionTreeNode chosen = this.getBranch();
            chosen.makeDecision();
        }
    }
}
