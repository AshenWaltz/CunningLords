using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace CunningLords.DecisionTreeLogic
{
    class ActionSparseSkirmish : DecisionTreeNode
    {
        public ActionSparseSkirmish(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override void makeDecision()
        {
            InformationManager.DisplayMessage(new InformationMessage(this.formation.FormationIndex.ToString() + " SPARSE SKIRMISH!"));
            this.formation.AI.ResetBehaviorWeights();
            this.formation.AI.SetBehaviorWeight<BehaviorSparseSkirmish>(2f);
        }
    }
}
