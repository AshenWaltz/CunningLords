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

namespace CunningLords.BehaviorTreelogic
{
    class TaskArcherVolley : Task
    {
        public TaskArcherVolley(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            //InformationManager.DisplayMessage(new InformationMessage("Archers: Forward Volley"));

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

            if ((this.formation != null) && (infantryRatio >= 0.15) && (!infantryBreached))
            {
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(2f);
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
