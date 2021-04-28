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

namespace CunningLords.Patches
{
    public class MissionOverride
    {
        public static BattleSideEnum PlayerBattleSide {get; set; } = BattleSideEnum.None;

        public static int FrameCounter = 0;

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
                    /*StartingOrderData orders = new StartingOrderData()
                    {
                        InfantryOrder = OrderType.Advance,
                        ArcherOrder = OrderType.Advance,
                        CavalryOrder = OrderType.Advance,
                        HorseArcherOrder = OrderType.Advance,
                        SkirmisherOrder = OrderType.Advance,
                        HeavyInfantryOrder = OrderType.Advance,
                        LightCavalryOrder = OrderType.Advance,
                        HeavyCavalryOrder = OrderType.Advance
                    };

                    string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                    string finalPath = Path.Combine(path, "ModuleData", "startdata.json");

                    var serializer = new JsonSerializer();
                    using (var sw = new StreamWriter(finalPath))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, orders);
                    }*/

                    Utils.OnStartOrders(__instance);
                    MissionOverride.FrameCounter++;
                }

                Utils.ManageInputKeys(__instance);
            }
        }

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("AfterStart")]
        class AfterStartOverride
        {
            static void Postfix(Mission __instance)
            {
                //Utils.OnStartPositioning(__instance);
            }
        }
    }
}