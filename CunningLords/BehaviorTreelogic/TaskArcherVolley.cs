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
    class TaskArcherVolley : Task
    {
        public TaskArcherVolley(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if(this.formation != null)
            {
                List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(this.formation);

                if (distances == null)
                {
                    return BTReturnEnum.failed;
                }

                bool infantryBreached = false;

                if (Utils.GetAlliedFormationsofType(this.formation, FormationClass.Infantry) != null)
                {

                    float distanceFromInfantry = this.formation.QuerySystem.AveragePosition.Distance(Utils.GetAlliedFormationsofType(this.formation, FormationClass.Infantry).QuerySystem.AveragePosition);

                    List<Formation> tooCloseForConfort = new List<Formation>();

                    foreach (Tuple<Formation, float> tup in distances)
                    {
                        if (tup.Item2 < distanceFromInfantry)
                        {
                            infantryBreached = true;
                        }

                        if (tup.Item2 < (0.5 * this.formation.QuerySystem.MissileRange))
                        {
                            tooCloseForConfort.Add(tup.Item1);
                        }
                    }

                    float infantryRatio = Utils.GetSelfFormationRatios(this.formation, FormationClass.Infantry);

                    if ((this.formation != null) && (infantryRatio >= 0.15) && (!infantryBreached) && (tooCloseForConfort.Count == 0))
                    {
                        this.formation.AI.ResetBehaviorWeights();
                        //this.formation.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(2f);

                        BehaviorArcherVanguardSkirmish behavior = this.formation.AI.SetBehaviorWeight<BehaviorArcherVanguardSkirmish>(1f);
                        behavior.Formation = this.formation;

                        return BTReturnEnum.succeeded;
                    }
                    else
                    {
                        return BTReturnEnum.failed;
                    }
                }
            }
            else
            {
                return BTReturnEnum.failed;
            }

            
        }
    }
}
