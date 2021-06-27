using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.Tactics;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using CunningLords.Interaction;
using TaleWorlds.CampaignSystem;
using CunningLords.PlanDefinition;

namespace CunningLords.Patches
{
    class MissionAI
    {
        public static bool missionAiActive = false;
        public static BattleSideEnum PlayerBattleSide { get; set; } = BattleSideEnum.None;

        [HarmonyPatch(typeof(MissionCombatantsLogic))]
        [HarmonyPatch("EarlyStart")]
        //This class is used to load tactics into the AI Teams, the tactics themselves determine the behaviour of each Formation within a Team
        class TeamTacticsInitializer
        {
            static void Postfix(MissionCombatantsLogic __instance)
            {
                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                string finalPath = Path.Combine(path, "ModuleData", "configData.json");

                CunningLordsConfigData data;
                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer deserializer = new JsonSerializer();
                    data = (CunningLordsConfigData)deserializer.Deserialize(file, typeof(CunningLordsConfigData));
                }

                if (data.AIActive)
                {
                    //MissionAI.PlayerBattleSide = __instance.Mission.MainAgent.Team.Side; //Crashes

                    List<Team> enemyTeams = Utils.GetAllEnemyTeams(__instance.Mission);

                    if (__instance.Mission.MissionTeamAIType == Mission.MissionTeamAITypeEnum.FieldBattle)
                    {
                        foreach (Team team in enemyTeams)
                        {
                            int tacticSkill = data.TacticSill;
                            string culture = data.Culture;
                            InformationManager.DisplayMessage(new InformationMessage("PASSOU AQUI!!!"));
                            if (CampaignInteraction.isCustomBattle)
                            {
                                InformationManager.DisplayMessage(new InformationMessage("Custom Battle"));

                                /*string path2 = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                                string finalPath2 = Path.Combine(path2, "ModuleData", "DecisionTree.json");

                                Plan plan = new Plan();


                                var serializer = new JsonSerializer();
                                using (var sw = new StreamWriter(finalPath2))
                                using (JsonWriter writer = new JsonTextWriter(sw))
                                {
                                    serializer.Serialize(writer, plan);
                                }

                                Plan planFromJson;

                                using (StreamReader file = File.OpenText(finalPath))
                                {
                                    JsonSerializer serializer2 = new JsonSerializer();
                                    planFromJson = (Plan)serializer2.Deserialize(file, typeof(Plan));
                                }*/

                                team.ClearTacticOptions();
                                team.AddTacticOption(new DTTacticHold(team));
                            }  
                            else if (/*hasGeneral ||*/ (tacticSkill <= 25)) //nearly no tactic level. Just charge and hope
                            {
                                InformationManager.DisplayMessage(new InformationMessage("nearly no tactic level"));

                                team.ClearTacticOptions();
                                team.AddTacticOption(new DTTacticLevelZero(team));
                            }
                            else if ((tacticSkill > 25) && (tacticSkill < 75)) //Minimal tactic level. there is somewhat of a plan
                            {
                                team.ClearTacticOptions();
                                team.AddTacticOption(new DTTacticLevelOne(team));
                            }
                            else if ((tacticSkill > 75) && (tacticSkill <= 200)) //Good tactic level. I know my culture and my army. I know how to use them
                            {
                                InformationManager.DisplayMessage(new InformationMessage("Good tactic level"));
                                team.ClearTacticOptions();
                                switch (culture)
                                {
                                    case "Empire":
                                        team.AddTacticOption(new DTTacticLevelTwoEmpire(team));
                                        break;
                                    case "Battania":
                                        team.AddTacticOption(new DTTacticLevelTwoBattania(team));
                                        break;
                                    case "Vlandia":
                                        team.AddTacticOption(new DTTacticLevelTwoVlandia(team));
                                        break;
                                    case "Sturgia":
                                        team.AddTacticOption(new DTTacticLevelTwoSturgia(team));
                                        break;
                                    case "Aserai":
                                        team.AddTacticOption(new DTTacticLevelTwoAserai(team));
                                        break;
                                    case "Khuzait":
                                        team.AddTacticOption(new DTTacticLevelTwoKhuzait(team));
                                        break;
                                    default:
                                        team.AddTacticOption(new DTTacticLevelTwoEmpire(team));
                                        break;
                                }
                            }
                            else //Excelent tactic level. Not only do I know my culture and my army, but I also recognize the strengths and weaknesses of my enemies
                            {
                                InformationManager.DisplayMessage(new InformationMessage("Excelent tactic level"));
                                team.ClearTacticOptions();
                                switch (culture)
                                {
                                    case "Empire":
                                        team.AddTacticOption(new DTTacticLevelThreeEmpire(team));
                                        break;
                                    case "Battania":
                                        team.AddTacticOption(new DTTacticLevelThreeBattania(team));
                                        break;
                                    case "Vlandia":
                                        team.AddTacticOption(new DTTacticLevelThreeVlandia(team));
                                        break;
                                    case "Sturgia":
                                        team.AddTacticOption(new DTTacticLevelThreeSturgia(team));
                                        break;
                                    case "Aserai":
                                        team.AddTacticOption(new DTTacticLevelThreeAserai(team));
                                        break;
                                    case "Khuzait":
                                        team.AddTacticOption(new DTTacticLevelThreeKhuzait(team));
                                        break;
                                    default:
                                        team.AddTacticOption(new DTTacticLevelTwoEmpire(team));
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
