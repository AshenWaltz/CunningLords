using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CunningLords.Patches
{
    class Utilities
    {
        public static List<Team> GetAllEnemyTeams(Mission __instance)
        {
            return (from t in __instance.Teams where t.Side != MissionAIHelloWorld.PlayerBattleSide select t).ToList<Team>();
        }

        public static List<Team> GetAllAllyTeams(Mission __instance)
        {
            return (from t in __instance.Teams where t.Side == MissionAIHelloWorld.PlayerBattleSide select t).ToList<Team>();
        }

        public static List<Formation> GetAllEnemyFormations(Mission __instance)
        {
            List<Formation> list = new List<Formation>();
            List<Team> allEnemyTeams = Utilities.GetAllEnemyTeams(__instance);
            bool notNullorZeroVerifier = allEnemyTeams != null && allEnemyTeams.Count > 0;
            if (notNullorZeroVerifier)
            {
                foreach (Team t in allEnemyTeams)
                {
                    foreach (Formation f in t.FormationsIncludingSpecial)
                    {
                        list.Add(f);
                    }
                }
            }
            return list;
        }

        public static List<Formation> GetAllAllyFormations(Mission __instance)
        {
            List<Formation> list = new List<Formation>();
            List<Team> allAllyTeams = Utilities.GetAllAllyTeams(__instance);
            bool notNullorZeroVerifier = allAllyTeams != null && allAllyTeams.Count > 0;
            if (notNullorZeroVerifier)
            {
                foreach (Team t in allAllyTeams)
                {
                    foreach (Formation f in t.FormationsIncludingSpecial)
                    {
                        list.Add(f);
                    }
                }
            }
            return list;
        }

        public static List<Formation> GetAllFormationsOfTypeAndSide(Mission __instance, FormationClass formationClass, string side = "Ally")
        {
            List<Formation> list = new List<Formation>();
            List<Formation> allFormations;
            if (side == "Ally")
            {
                allFormations = Utilities.GetAllAllyFormations(__instance);
            }
            else
            {
                allFormations = Utilities.GetAllEnemyFormations(__instance);
            }
            bool notNullorZeroVerifier = allFormations != null && allFormations.Count > 0;
            if (notNullorZeroVerifier)
            {
                foreach (Formation f in allFormations)
                {
                    if (f.FormationIndex == formationClass)
                    {
                        list.Add(f);
                    }
                }
            }
            return list;
        }

        public static void ManageInputKeys(Mission mission)
        {
            if(mission != null && Input.IsKeyDown(InputKey.LeftControl))
            {
                if (Input.IsKeyDown(InputKey.E))
                {
                    Utilities.PrintRelevantData(mission);
                }
            }
        }

        public static void PrintRelevantData(Mission mission)
        {
            BattleSideEnum playerSide = BattleSideEnum.None;

            if (mission != null && mission.MainAgent != null)
            {
                playerSide = mission.MainAgent.Team.Side;
            }
            else
            {
                return;
            }

            InformationManager.DisplayMessage(new InformationMessage("Within Battle Mission"));

            foreach (Team t in Utilities.GetAllEnemyTeams(mission))
            {
                InformationManager.DisplayMessage(new InformationMessage("Enemy Team: " + t.ToString()));
            }

            foreach (Formation f in Utilities.GetAllEnemyFormations(mission))
            {
                InformationManager.DisplayMessage(new InformationMessage("Enemy Formations: " + f.FormationIndex.ToString()));
            }

            foreach (Formation f in Utilities.GetAllFormationsOfTypeAndSide(mission, FormationClass.Infantry, "Enemy"))
            {
                InformationManager.DisplayMessage(new InformationMessage("Enemy Formations of type infantry: " + f.FormationIndex.ToString()));
            }

            Utilities.PrintInfluenceMapDataForFormationOfType(FormationClass.Infantry, mission);

        }

        public static void PrintInfluenceMapDataForFormationOfType(FormationClass formationClass, Mission mission)
        {
            //List<Formation> alliedFormations = Utilities.GetAllAllyFormations(mission);
            //List<Formation> enemyFormations = Utilities.GetAllEnemyFormations(mission);
            List<Formation> formation = Utilities.GetAllFormationsOfTypeAndSide(mission, formationClass, "Enemy");

            if(formation.Count <= 1)
            {
                Tuple<float, float, float, FormationClass> influenceMap = GetinfluenceMapDataForFormation(formation[0], mission);
                InformationManager.DisplayMessage(new InformationMessage("Threat Risk: " + influenceMap.Item1.ToString()));
                InformationManager.DisplayMessage(new InformationMessage("Threat Distance: " + influenceMap.Item2.ToString()));
                InformationManager.DisplayMessage(new InformationMessage("Threat Power: " + influenceMap.Item3.ToString()));
                InformationManager.DisplayMessage(new InformationMessage("Threat Class: " + influenceMap.Item4.ToString()));
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("There is more than one formation of type and side"));
            }

        }

        public static Tuple<float, float, float, FormationClass> GetinfluenceMapDataForFormation(Formation formation, Mission mission)
        {
            List<Formation> alliedFormations = Utilities.GetAllAllyFormations(mission);

            //Return Values
            float riskFactor = -1.0f;
            float distanceThreat = -1.0f;
            float powerThreat = -1.0f;
            FormationClass classThreat = FormationClass.Unset;


            foreach (Formation f in alliedFormations)
            {
                //Distance Between Formations
                float distance = formation.QuerySystem.MedianPosition.AsVec2.Distance(f.QuerySystem.MedianPosition.AsVec2);

                //Dot product between formations
                Vec2 enemyDirection = formation.QuerySystem.EstimatedDirection;
                Vec2 aliedDirection = f.QuerySystem.EstimatedDirection;

                float dotProduct = formation.QuerySystem.EstimatedDirection.DotProduct(f.QuerySystem.EstimatedDirection);

                //Count difference between formations
                int countAllyUnits = f.CountOfUnits;
                int countEnemyUnits = formation.CountOfUnits;

                int countUnitsDifference = countAllyUnits - countEnemyUnits;

                //Local Ally power
                float localAllyPower = f.QuerySystem.LocalAllyPower;
                float localEnemyPower = formation.QuerySystem.LocalAllyPower;

                float localPowerDifference = localAllyPower - localEnemyPower;
                //Is Shielded
                float allyShieldRatio = 0.0f;
                float enemyShieldRatio = 0.0f;

                if (formation.QuerySystem.HasShield)
                {
                    enemyShieldRatio = formation.QuerySystem.HasShieldUnitRatio;
                }
                if (f.QuerySystem.HasShield)
                {
                    allyShieldRatio = f.QuerySystem.HasShieldUnitRatio;
                }

                //Formation Type
                FormationClass allyType = f.QuerySystem.MainClass;
                FormationClass enemyType = formation.QuerySystem.MainClass;

                //Formation Disperseness
                float allyDispersedness = f.QuerySystem.FormationDispersedness;
                float enemyDispersedness = formation.QuerySystem.FormationDispersedness;

                //Formation Power
                float allyFormationPower = f.QuerySystem.FormationPower;
                float enemyFormationPower = formation.QuerySystem.FormationPower;

                //Formation Missile Range
                float allyMissileRange = f.QuerySystem.MissileRange;
                float enemyMissileRange = formation.QuerySystem.MissileRange;

                //Formation Speed
                float allyMovementSpeed = f.QuerySystem.MovementSpeed;
                float enemyMovementSpeed = formation.QuerySystem.MovementSpeed;

                //Risk Calculation
                float tempRiskValue = 1.0f;
                if (formation.FormationIndex == FormationClass.Infantry)
                {
                    //ToDO
                    tempRiskValue = ((1 / distance) + dotProduct + ((float)countUnitsDifference) + localPowerDifference);
                }
                else if (formation.FormationIndex == FormationClass.Ranged)
                {
                    //ToDO
                    tempRiskValue = ((1 / distance) + dotProduct + ((float)countUnitsDifference) + localPowerDifference);
                }
                else if (formation.FormationIndex == FormationClass.Cavalry)
                {
                    //ToDO
                    tempRiskValue = ((1 / distance) + dotProduct + ((float)countUnitsDifference) + localPowerDifference);
                }
                else if (formation.FormationIndex == FormationClass.HorseArcher)
                {
                    //ToDO
                    tempRiskValue = ((1 / distance) + dotProduct + ((float)countUnitsDifference) + localPowerDifference);
                }

                if (tempRiskValue > riskFactor)
                {
                    riskFactor = tempRiskValue;
                    distanceThreat = distance;
                    powerThreat = allyFormationPower;
                    classThreat = allyType;
                }

                //InformationManager.DisplayMessage(new InformationMessage("Distance " + formation.FormationIndex.ToString() + " to " + f.FormationIndex.ToString() + ": " + distance));
                //InformationManager.DisplayMessage(new InformationMessage("Direction " + enemyDirection.ToString() + " to " + aliedDirection.ToString()));
                //InformationManager.DisplayMessage(new InformationMessage("Dot Product " + formation.FormationIndex.ToString() + " to " + f.FormationIndex.ToString() + ": " + dotProduct));
            }

            return Tuple.Create(riskFactor, distanceThreat, powerThreat, classThreat);
        }
    }
}
