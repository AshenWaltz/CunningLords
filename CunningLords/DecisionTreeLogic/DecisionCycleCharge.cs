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
    class DecisionCycleCharge : DecisionTreeNode
    {
        protected DecisionTreeNode trueNode;

        protected DecisionTreeNode falseNode;

        protected int engageDuration;

        protected int lastFrame;

        protected int startFrame;

        protected Utils util;

        public DecisionCycleCharge(Formation f, DecisionTreeNode tn, DecisionTreeNode fn, int duration, int frame, ref Utils ut) : base(f)
        {
            this.formation = f;
            this.trueNode = tn;
            this.falseNode = fn;
            this.engageDuration = duration;
            this.startFrame = frame;
            this.lastFrame = frame;
            this.util = ut;
        }

        private DecisionTreeNode getBranch()
        {
            int currentFrame = util.GetMissionTickCounter();

            if (currentFrame == (this.lastFrame + 1) && currentFrame < (this.startFrame + this.engageDuration))
            {
                //InformationManager.DisplayMessage(new InformationMessage("charging"));
                this.lastFrame = currentFrame;
                return this.trueNode;
            }
            else if (currentFrame == (this.lastFrame + 1) && currentFrame >= (this.startFrame + this.engageDuration) && currentFrame < (this.startFrame + this.engageDuration + this.engageDuration))
            {
                //InformationManager.DisplayMessage(new InformationMessage("pulling back"));
                this.lastFrame = currentFrame;
                return this.falseNode;
            }
            else
            {
                //InformationManager.DisplayMessage(new InformationMessage("else" + currentFrame.ToString()));
                this.startFrame = currentFrame;
                this.lastFrame = currentFrame;
                return this.trueNode;
            }
        }

        public override void makeDecision()
        {
            DecisionTreeNode chosen = this.getBranch();
            chosen.makeDecision();
        }
    }
}
