using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace CunningLords.DecisionTreeLogic
{
    class ActionCavalryScreen : DecisionTreeNode
    {
        public ActionCavalryScreen(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override void makeDecision()
        {
            InformationManager.DisplayMessage(new InformationMessage(this.formation.FormationIndex.ToString() + " CAvALRY SCREEN!"));
            this.formation.AI.ResetBehaviorWeights();
            this.formation.AI.SetBehaviorWeight<BehaviorCavalryScreen>(2f);
        }
    }
}
