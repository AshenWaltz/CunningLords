using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using CunningLords.Patches;

namespace CunningLords.Interaction
{
    class CampaignInteraction
    {
        public static bool _inMenu = false;

        public static bool isCampaign = false;

        [HarmonyPatch(typeof(Campaign))]
        [HarmonyPatch("RealTick")]
        class CampaignHourlyTickOverride
        {
            static void Postfix(Campaign __instance)
            {
                if (Input.IsKeyDown(InputKey.LeftAlt) && Input.IsKeyDown(InputKey.A) && !CampaignInteraction._inMenu)
                {
                    CampaignInteraction._inMenu = true;
                    InformationManager.DisplayMessage(new InformationMessage("PRESSED!"));
                    ScreenManager.PushScreen(new CunningLordsOptionScreen());
                }

                MissionOverride.FrameCounter = 0;
                CampaignInteraction.isCampaign = true;
            }
        }

        //Campaign.MapSate
    }
}
