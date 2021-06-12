using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using CunningLords.Patches;

namespace CunningLords.DecisionTreeLogic
{
    class DecisionEnemyFormationWeaker : DecisionTreeNode
    {
        protected DecisionTreeNode trueNode;

        protected DecisionTreeNode falseNode;

        protected FormationClass targetFormation;

        protected int powerMultiplier;

        public DecisionEnemyFormationWeaker(Formation f, DecisionTreeNode tn, DecisionTreeNode fn, FormationClass formClass, int pm) : base(f)
        {
            this.formation = f;
            this.trueNode = tn;
            this.falseNode = fn;
            this.targetFormation = formClass;
            this.powerMultiplier = pm;
        }

        private DecisionTreeNode getBranch()
        {
            bool powerComparison = Utils.PowerComparison(this.targetFormation, this.formation, this.powerMultiplier);
            if (powerComparison)
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
