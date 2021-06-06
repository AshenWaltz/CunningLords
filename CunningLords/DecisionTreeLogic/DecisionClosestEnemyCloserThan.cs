﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using CunningLords.Patches;

namespace CunningLords.DecisionTreeLogic
{
    class DecisionClosestEnemyCloserThan : DecisionTreeNode
    {
        protected DecisionTreeNode trueNode;

        protected DecisionTreeNode falseNode;

        protected float engageDistance;

        public DecisionClosestEnemyCloserThan(Formation f, DecisionTreeNode tn, DecisionTreeNode fn, float distance) : base(f)
        {
            this.formation = f;
            this.trueNode = tn;
            this.falseNode = fn;
            this.engageDistance = distance;
        }

        private DecisionTreeNode getBranch()
        {
            Formation closestFormation = Utils.GetClosestPlayerFormation(this.formation);
            float closestDistance = this.formation.QuerySystem.AveragePosition.Distance(closestFormation.QuerySystem.AveragePosition);
            if (closestDistance < this.engageDistance)
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
