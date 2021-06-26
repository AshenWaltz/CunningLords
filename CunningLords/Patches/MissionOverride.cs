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
using CunningLords.DecisionTreeLogic;

namespace CunningLords.Patches
{
    public class MissionOverride
    {
        public static BattleSideEnum PlayerBattleSide {get; set; } = BattleSideEnum.None;

        public static int FrameCounter = 0;

        public static bool IsPlanActive = false;

        private static DecisionTreeGenerator Generator = null;

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
                    MissionOverride.Generator = null;
                    Utils.OnStartOrders(__instance);
                }
                MissionOverride.FrameCounter++;
                Utils.ManageInputKeys(__instance);

                if (MissionOverride.IsPlanActive && __instance.MainAgent != null)
                {
                    MissionOverride.Generator = new DecisionTreeGenerator();
                    MissionOverride.Generator.Run();
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
            }
        }
    }
}