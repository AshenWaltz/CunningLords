using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace CunningLords.DecisionTreeLogic
{
    class ActionHorseArcherSkirmish : DecisionTreeNode
    {
        public ActionHorseArcherSkirmish(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override void makeDecision()
        {
            InformationManager.DisplayMessage(new InformationMessage(this.formation.FormationIndex.ToString() + " HORSE ARCHER SKIRMISH!"));
            this.formation.AI.ResetBehaviorWeights();
            this.formation.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(2f);
        }
    }
}
