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
    [HarmonyPatch(typeof(Mission), "OnTick")]
    public class MissionAI
    {
        public static BattleSideEnum PlayerBattleSide {get; set; } = BattleSideEnum.None;

        private static int FrameCounter = 0;
        private static void Postfix(Mission __instance)
        {
            MissionAI.PlayerBattleSide = __instance.MainAgent.Team.Side;

            if(FrameCounter == 0)
            {
                InformationManager.DisplayMessage(new InformationMessage("Within Battle Mission"));

                foreach(Team t in MissionAI.GetAllEnemyTeams(__instance))
                {
                    InformationManager.DisplayMessage(new InformationMessage("Enemy Team: " + t.ToString()));
                }

                foreach (Formation f in MissionAI.GetAllEnemyFormations(__instance))
                {
                    InformationManager.DisplayMessage(new InformationMessage("Enemy Formations: " + f.ToString()));
                }

                foreach (Formation f in MissionAI.GetAllFormationsOfTypeAndSide(__instance, FormationClass.Infantry, "Enemy"))
                {
                    InformationManager.DisplayMessage(new InformationMessage("Enemy Formations of type infantry: " + f.ToString()));
                }
            }
            

        }

        private static List<Team> GetAllEnemyTeams(Mission __instance)
        {
            return (from t in __instance.Teams where t.Side != MissionAI.PlayerBattleSide select t).ToList<Team>();
        }

        private static List<Team> GetAllAllyTeams(Mission __instance)
        {
            return (from t in __instance.Teams where t.Side == MissionAI.PlayerBattleSide select t).ToList<Team>();
        }

        private static List<Formation> GetAllEnemyFormations(Mission __instance)
        {
            List<Formation> list = new List<Formation>();
            List<Team> allEnemyTeams = MissionAI.GetAllEnemyTeams(__instance);
            bool notNullorZeroVerifier = allEnemyTeams != null && allEnemyTeams.Count > 0;
            if (notNullorZeroVerifier)
            {
                foreach(Team t in allEnemyTeams)
                {
                    foreach(Formation f in t.FormationsIncludingSpecial)
                    {
                        list.Add(f);
                    }
                }
            }
            return list;
        }

        private static List<Formation> GetAllAllyFormations(Mission __instance)
        {
            List<Formation> list = new List<Formation>();
            List<Team> allAllyTeams = MissionAI.GetAllAllyTeams(__instance);
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

        private static List<Formation> GetAllFormationsOfTypeAndSide(Mission __instance, FormationClass formationClass, string side = "Ally")
        {
            List<Formation> list = new List<Formation>();
            List<Formation> allFormations;
            if (side == "Ally")
            {
                allFormations = MissionAI.GetAllAllyFormations(__instance);
            }
            else{
                allFormations = MissionAI.GetAllEnemyFormations(__instance);
            }
            bool notNullorZeroVerifier = allFormations != null && allFormations.Count > 0;
            if (notNullorZeroVerifier)
            {
                foreach (Formation f in allFormations)
                {
                    if(f.FormationIndex == formationClass)
                    {
                        list.Add(f);
                    }
                }
            }
            return list;
        }
    }
}