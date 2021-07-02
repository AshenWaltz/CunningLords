using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace CunningLords.DecisionTreeLogic
{
    class ActionHoldLine : DecisionTreeNode
    {
        public ActionHoldLine(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override void makeDecision()
        {
            //InformationManager.DisplayMessage(new InformationMessage(this.formation.FormationIndex.ToString() + " HOLD LINE!"));
            this.formation.AI.ResetBehaviorWeights();
            this.formation.AI.SetBehaviorWeight<BehaviorDefend>(2f);
        }
    }
}
