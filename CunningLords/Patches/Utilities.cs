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
    }
}
