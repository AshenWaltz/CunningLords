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
using CunningLords.Behaviors;
using TaleWorlds.Library;

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

        private int engageCounterStart = -1;

        private bool isEngaged = false;

        private PlanStateEnum previousState = PlanStateEnum.Prepare;

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
                        //f.IsAIControlled = true;
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

            if (this.previousState != state)
            {
                InformationManager.DisplayMessage(new InformationMessage("Mission has entered " + state.ToString() + " State"));
                this.previousState = state;
            }
            
            if (Mission.Current != null)
            {
                if (Mission.Current.MainAgent != null)
                {
                    Team playerTeam = Mission.Current.MainAgent.Team;

                    foreach (Formation f in playerTeam.Formations)
                    {
                        //f.IsAIControlled = true;
                        if (f.PlayerOwner != null)
                        {
                            switch (f.FormationIndex)
                            {
                                case FormationClass.Infantry:
                                    if (state == PlanStateEnum.Prepare)
                                    {
                                        ApplyBehavior(f, plan.infantryPhasePrepare);
                                    }
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.infantryPhaseRanged);
                                    }
                                    else if (state == PlanStateEnum.Engage)
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
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.archersPhaseRanged);
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
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.cavalryPhaseRanged);
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
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.horseArchersPhaseRanged);
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
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.skirmishersPhaseRanged);
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
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.heavyInfantryPhaseRanged);
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
                                        ApplyBehavior(f, plan.lightCavalryPhasePrepare);
                                    }
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.lightCavalryPhaseRanged);
                                    }
                                    else if (state == PlanStateEnum.Engage)
                                    {
                                        ApplyBehavior(f, plan.lightCavalryPhaseEngage);
                                    }
                                    else if (state == PlanStateEnum.Winning)
                                    {
                                        ApplyBehavior(f, plan.lightCavalryPhaseWinning);
                                    }
                                    else if (state == PlanStateEnum.Losing)
                                    {
                                        ApplyBehavior(f, plan.lightCavalryPhaseLosing);
                                    }
                                    break;
                                case FormationClass.HeavyCavalry:
                                    if (state == PlanStateEnum.Prepare)
                                    {
                                        ApplyBehavior(f, plan.heavyCavalryPhasePrepare);
                                    }
                                    else if (state == PlanStateEnum.Ranged)
                                    {
                                        ApplyBehavior(f, plan.heavyCavalryPhaseRanged);
                                    }
                                    else if (state == PlanStateEnum.Engage)
                                    {
                                        ApplyBehavior(f, plan.heavyCavalryPhaseEngage);
                                    }
                                    else if (state == PlanStateEnum.Winning)
                                    {
                                        ApplyBehavior(f, plan.heavyCavalryPhaseWinning);
                                    }
                                    else if (state == PlanStateEnum.Losing)
                                    {
                                        ApplyBehavior(f, plan.heavyCavalryPhaseLosing);
                                    }
                                    break;
                            }
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
                    Formation mainFormation = GetFormationPriority(Mission.Current);
                    float offset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    if (offset > 0)
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorFlank>(2f);
                        f.AI.Side = FormationAI.BehaviorSide.Right;
                    }
                    else
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorFlank>(2f);
                        f.AI.Side = FormationAI.BehaviorSide.Left;
                    }
                    break;
                case PlanOrderEnum.Skirmish:
                    if(f.FormationIndex == FormationClass.HorseArcher)
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(2f);
                    }
                    else
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorSkirmishMode>(2f);
                    }
                    break;
                case PlanOrderEnum.HideBehind:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorHideBehind>(2f);
                    break;
                case PlanOrderEnum.ProtectFlank:
                    Formation mainFormation2 = GetFormationPriority(Mission.Current);
                    float offset2 = CalculateXYOffsetsBasedOnWorldPosition(mainFormation2, f).X;
                    if (offset2 > 0)
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorProtectFlank>(2f);
                        f.AI.Side = FormationAI.BehaviorSide.Right;
                    }
                    else
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorProtectFlank>(2f);
                        f.AI.Side = FormationAI.BehaviorSide.Left;
                    }
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
                case PlanOrderEnum.Retreat:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorRetreat>(2f);
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

                    if (!isEngaged)
                    {
                        foreach (Formation f in Mission.Current.MainAgent.Team.Formations)
                        {
                            Formation closestsFormation;
                            if (f.QuerySystem.ClosestEnemyFormation != null)
                            {
                                closestsFormation = f.QuerySystem.ClosestEnemyFormation.Formation;
                            }
                            else
                            {
                                return this.previousState;
                            }

                            if (closestsFormation != null)
                            {
                                float distance = f.QuerySystem.AveragePosition.Distance(closestsFormation.QuerySystem.AveragePosition);

                                float res = ProximityPercentage(Mission.Current, f, 30);

                                if (res > 0.10f)
                                {
                                    if (engageCounterStart == -1 && isEngaged == false)
                                    {
                                        engageCounterStart = MissionOverride.FrameCounter;
                                        isEngaged = true;
                                    }
                                    return PlanStateEnum.Engage;
                                }
                                else if (distance >= 30.0f && distance < 100.0f)
                                {
                                    return PlanStateEnum.Ranged;
                                }
                                else
                                {
                                    return this.previousState;
                                }
                            }
                            else
                            {
                                return this.previousState;
                            }
                        }
                    }
                    else if (isEngaged && (MissionOverride.FrameCounter - engageCounterStart) > 1000)
                    {
                        List<Team> enemyTeams = (from t in Mission.Current.Teams where t.Side != Mission.Current.MainAgent.Team.Side select t).ToList<Team>();

                        List<Team> alliedTeams = (from t in Mission.Current.Teams where t.Side == Mission.Current.MainAgent.Team.Side select t).ToList<Team>();

                        float enemyPowerRatio = 0.0f;

                        float alliedPowerRatio = 0.0f;

                        foreach (Team te in enemyTeams)
                        {
                            if (te != null)
                            {
                                enemyPowerRatio += te.QuerySystem.TeamPower;
                            }
                        }

                        foreach (Team te in alliedTeams)
                        {
                            if (te != null)
                            {
                                alliedPowerRatio += te.QuerySystem.TeamPower;
                            }
                        }

                        float averageAlliedPower = alliedPowerRatio / alliedTeams.Count;

                        float averageEnemyPower = enemyPowerRatio / enemyTeams.Count;

                        if ((averageEnemyPower * 0.5) > averageAlliedPower)
                        {
                            return PlanStateEnum.Losing;
                        }
                        else if ((averageAlliedPower * 0.5) > averageEnemyPower)
                        {
                            return PlanStateEnum.Winning;
                        }
                        else
                        {
                            return PlanStateEnum.Engage;
                        }
                    }
                    else if (this.isEngaged)
                    {
                        return PlanStateEnum.Engage;
                    }
                    else
                    {
                        return PlanStateEnum.Prepare;
                    }
                } 
            }

            return result;
        }

        private static Vec2 CalculateXYOffsetsBasedOnWorldPosition(Formation mainFormation, Formation formation)
        {
            double num = (double)formation.CurrentPosition.X;
            double num2 = (double)formation.CurrentPosition.Y;
            double num3 = (double)mainFormation.CurrentPosition.X;
            double num4 = (double)mainFormation.CurrentPosition.Y;
            double num5 = Math.Atan2((double)mainFormation.Direction.Y, (double)mainFormation.Direction.X);
            num -= num3;
            num2 -= num4;
            double num6 = num * Math.Sin(num5) - num2 * Math.Cos(num5);
            double num7 = num * Math.Cos(num5) + num2 * Math.Sin(num5);
            return new Vec2((float)num6, (float)num7);
        }

        public Formation GetFormationPriority(Mission mission)
        {
            Formation infantry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
            Formation archers = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Ranged);
            Formation cavalry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Cavalry);
            Formation horseArcher = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HorseArcher);
            Formation skirmisher = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Skirmisher);
            Formation heavyInfantry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HeavyInfantry);
            Formation lightCavalry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.LightCavalry);
            Formation heavyCavalry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HeavyInfantry);

            List<Formation> formations = new List<Formation>();

            formations.Add(infantry);
            formations.Add(archers);
            formations.Add(cavalry);
            formations.Add(horseArcher);
            formations.Add(skirmisher);
            formations.Add(heavyInfantry);
            formations.Add(lightCavalry);
            formations.Add(heavyCavalry);

            foreach (Formation f in formations)
            {
                if (f != null)
                {
                    return f;
                }
            }

            return null;
        }

        public static float ProximityPercentage(Mission __instance, Formation formation, float distance)
        {
            List<Formation> list = new List<Formation>();
            List<Team> allEnemyTeams = (from t in __instance.Teams where t.Side != MissionOverride.PlayerBattleSide select t).ToList<Team>();
            bool notNullorZeroVerifier = allEnemyTeams != null && allEnemyTeams.Count > 0;

            float armyNumber = 1.0f;
            float closestTroops = 0.0f;

            if (notNullorZeroVerifier)
            {
                foreach (Team t in allEnemyTeams)
                {
                    armyNumber += t.QuerySystem.MemberCount;
                    foreach (Formation f in t.FormationsIncludingSpecial)
                    {
                        float dist = formation.QuerySystem.AveragePosition.Distance(f.QuerySystem.AveragePosition);

                        

                        if (dist < distance)
                        {
                            closestTroops += f.CountOfUnits;
                        }
                    }
                }
            }

            float res = (closestTroops / armyNumber);

            return res;
        }

    }
}
