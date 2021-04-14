using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.Patches;
using CunningLords.Behaviors;

namespace CunningLords.BehaviorTreelogic
{
    class TaskArcherSkirmish : Task
    {
        public TaskArcherSkirmish(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(this.formation);

            bool infantryBreached = false;

            float distanceFromInfantry = this.formation.QuerySystem.AveragePosition.Distance(Utils.GetAlliedFormationsofType(this.formation, FormationClass.Infantry).QuerySystem.AveragePosition);

            foreach (Tuple<Formation, float> tup in distances)
            {
                if (tup.Item2 < distanceFromInfantry)
                {
                    infantryBreached = true;
                }
            }

            float infantryRatio = Utils.GetSelfFormationRatios(this.formation, FormationClass.Infantry);

            if ((this.formation != null) && ((infantryRatio < 0.15) || (infantryBreached)))
            {
                this.formation.AI.ResetBehaviorWeights();
                BehaviorSkirmishMode behavior = this.formation.AI.SetBehaviorWeight<BehaviorSkirmishMode>(1f);
                behavior.Formation = this.formation;
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
