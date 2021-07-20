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
using CunningLords.Interaction;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using CunningLords.PlanDefinition;

namespace CunningLords.Patches
{
    public class MissionOverride
    {
        public static BattleSideEnum PlayerBattleSide {get; set; } = BattleSideEnum.None;

        public static int FrameCounter = 0;

        public static bool IsPlanActive = false;

        private static PlanGenerator Generator = null;

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("OnTick")]
        class OnTickOverride
        {
            static void Postfix(Mission __instance)
            {
                if (__instance != null && __instance.MainAgent != null)
                {
                    MissionOverride.PlayerBattleSide = __instance.MainAgent.Team.Side;
                }
                else
                {
                    return;
                }

                if (MissionOverride.FrameCounter == 0)
                {
                    MissionOverride.IsPlanActive = false;
                    MissionOverride.Generator = new PlanGenerator();
                    Utils.OnStartOrders(__instance);
                }
                MissionOverride.FrameCounter++;
                Utils.ManageInputKeys(__instance);

                if (MissionOverride.IsPlanActive && __instance.MainAgent != null)
                {
                    MissionOverride.Generator.Run();
                }

                if (!MissionOverride.IsPlanActive && __instance.MainAgent != null)
                {
                    foreach (Formation f in __instance.MainAgent.Team.Formations)
                    {
                        f.IsAIControlled = false;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("AfterStart")]
        class AfterStartOverride
        {
            static void Postfix(Mission __instance)
            {
                MissionOverride.FrameCounter = 0;




                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                string finalPath = Path.Combine(path, "ModuleData", "configData.json");

                CunningLordsConfigData data;
                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer deserializer = new JsonSerializer();
                    data = (CunningLordsConfigData)deserializer.Deserialize(file, typeof(CunningLordsConfigData));
                }

                GameMetricsController GMC= new GameMetricsController();

                GMC.WriteToJson(GameMetricEnum.TotalBattles);

                if (CampaignInteraction.isCustomBattle)
                {
                    GMC.WriteToJson(GameMetricEnum.NumberOfTestBattles);
                }
                else
                {
                    if (data.AIActive)
                    {
                        GMC.WriteToJson(GameMetricEnum.BattlesUsingAI);
                    }
                    else
                    {
                        GMC.WriteToJson(GameMetricEnum.BattlesUsingNative);
                    }
                }
            }
        }
    }
}