using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace CunningLords.DecisionTreeLogic
{
    class ActionProtectFlank : DecisionTreeNode
    {
        public FormationAI.BehaviorSide flankSide;

        public ActionProtectFlank(Formation f, FormationAI.BehaviorSide side) : base(f)
        {
            this.formation = f;
            this.flankSide = side;
        }

        public override void makeDecision()
        {
            InformationManager.DisplayMessage(new InformationMessage(this.formation.FormationIndex.ToString() + " PROTECT FLANK!"));
            this.formation.AI.ResetBehaviorWeights();
            this.formation.AI.SetBehaviorWeight<BehaviorProtectFlank>(2f).FlankSide = this.flankSide;
        }
    }
}
