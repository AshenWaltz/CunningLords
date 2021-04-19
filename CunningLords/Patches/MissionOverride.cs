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

namespace CunningLords.Patches
{
    public class MissionOverride
    {
        public static BattleSideEnum PlayerBattleSide {get; set; } = BattleSideEnum.None;

        private static int FrameCounter = 0;

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
                    Utils.OnStartPositioning(__instance);
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