using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using CunningLords.Patches;

namespace CunningLords.PlanDefinition
{
    public class PlanGenerator
    {
        public Plan plan;

        public Formation infantry;
        public Formation archers;
        public Formation cavalry;
        public Formation horseArchers;
        public Formation skirmishers;
        public Formation heavyInfantry;
        public Formation lightCavalry;
        public Formation heavyCavalry;

        public PlanGenerator()
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "DecisionTree.json");

            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer serializer2 = new JsonSerializer();
                this.plan = (Plan)serializer2.Deserialize(file, typeof(Plan));
            }

            if (Mission.Current != null)
            {
                if (Mission.Current.MainAgent != null)
                {
                    Team playerTeam = Mission.Current.MainAgent.Team;

                    foreach (Formation f in playerTeam.Formations)
                    {
                        f.IsAIControlled = true;
                        switch (f.FormationIndex)
                        {
                            case FormationClass.Infantry:
                                this.infantry = f;
                                break;
                            case FormationClass.Ranged:
                                this.archers = f;
                                break;
                            case FormationClass.Cavalry:
                                this.cavalry = f;
                                break;
                            case FormationClass.HorseArcher:
                                this.horseArchers = f;
                                break;
                            case FormationClass.Skirmisher:
                                this.skirmishers = f;
                                break;
                            case FormationClass.HeavyInfantry:
                                this.heavyCavalry = f;
                                break;
                            case FormationClass.LightCavalry:
                                this.lightCavalry = f;
                                break;
                            case FormationClass.HeavyCavalry:
                                this.heavyCavalry = f;
                                break;
                        }
                    }
                }
            }

        }

        public void Run()
        {
            PlanStateEnum state = GetMissionState();

            InformationManager.DisplayMessage(new InformationMessage(state.ToString()));

            if (Mission.Current != null)
            {
                if (Mission.Current.MainAgent != null)
                {
                    Team playerTeam = Mission.Current.MainAgent.Team;

                    foreach (Formation f in playerTeam.Formations)
                    {
                        f.IsAIControlled = true;
                        switch (f.FormationIndex)
                        {
                            case FormationClass.Infantry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.infantryPhasePrepare);
                                }
                                else if(state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.infantryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.infantryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.infantryPhaseLosing);
                                }
                                break;
                            case FormationClass.Ranged:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.archersPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.archersPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.archersPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.archersPhaseLosing);
                                }
                                break;
                            case FormationClass.Cavalry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.cavalryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.cavalryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.cavalryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.cavalryPhaseLosing);
                                }
                                break;
                            case FormationClass.HorseArcher:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhaseLosing);
                                }
                                break;
                            case FormationClass.Skirmisher:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhaseLosing);
                                }
                                break;
                            case FormationClass.HeavyInfantry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhaseLosing);
                                }
                                break;
                            case FormationClass.LightCavalry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.LightCavalryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.LightCavalryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.LightCavalryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.LightCavalryPhaseLosing);
                                }
                                break;
                            case FormationClass.HeavyCavalry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.HeavyCavalryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.HeavyCavalryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.HeavyCavalryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.HeavyCavalryPhaseLosing);
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void ApplyBehavior(Formation f, PlanOrderEnum order)
        {
            switch (order)
            {
                case PlanOrderEnum.Charge:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorCharge>(2f);
                    break;
                case PlanOrderEnum.HoldPosition:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorDefend>(2f);
                    break;
                case PlanOrderEnum.Flank:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorFlank>(2f);
                    break;
                case PlanOrderEnum.Skirmish:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorSkirmish>(2f);
                    break;
                case PlanOrderEnum.HideBehind:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorScreenedSkirmish>(2f);
                    break;
                case PlanOrderEnum.ProtectFlank:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorProtectFlank>(2f);
                    break;
                case PlanOrderEnum.Advance:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorAdvance>(2f);
                    break;
                case PlanOrderEnum.CautiousAdvance:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorCautiousAdvance>(2f);
                    break;
            }
        }

        public PlanStateEnum GetMissionState()
        {
            PlanStateEnum result = PlanStateEnum.Prepare;

            if (Mission.Current != null)
            {
                if (Mission.Current.MainAgent != null)
                {
                    float alliedCasualityRatio = 0.0f;

                    int numberOfFormations = 0;

                    foreach(Formation f in Mission.Current.MainAgent.Team.Formations)
                    {
                        Formation closestsFormation;
                        if (f.QuerySystem.ClosestEnemyFormation != null)
                        {
                            closestsFormation = f.QuerySystem.ClosestEnemyFormation.Formation;
                        }
                        else
                        {
                            return PlanStateEnum.Prepare;
                        }

                        if (closestsFormation != null)
                        {
                            float distance = f.QuerySystem.AveragePosition.Distance(closestsFormation.QuerySystem.AveragePosition);

                            if (distance < 30.0f)
                            {
                                return PlanStateEnum.Engage;
                            }

                            alliedCasualityRatio += f.QuerySystem.CasualtyRatio;
                            numberOfFormations++;
                        }
                        else
                        {
                            return PlanStateEnum.Prepare;
                        } 
                    }

                    List<Team> enemyTeams = (from t in Mission.Current.Teams where t.Side != Mission.Current.MainAgent.Team.Side select t).ToList<Team>();

                    float enemyCasualityRatio = 0.0f;

                    int numberOfEnemyFormations = 0;

                    foreach (Team te in enemyTeams)
                    {
                        if (te != null)
                        {
                            foreach (Formation fe in te.Formations)
                            {
                                enemyCasualityRatio += fe.QuerySystem.CasualtyRatio;
                                numberOfEnemyFormations++;
                            }
                        }
                        else
                        {
                            return PlanStateEnum.Prepare;
                        }
                    }

                    float averageCasualities = alliedCasualityRatio / numberOfFormations;

                    float averageEnemyCasualities = enemyCasualityRatio / numberOfEnemyFormations;
                    /*
                    if (averageCasualities > averageEnemyCasualities)
                    {
                        return PlanStateEnum.Losing;
                    }
                    else
                    {
                        return PlanStateEnum.Winning;
                    }*/
                } 
            }

            return result;
        }

    }
}
