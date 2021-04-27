using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using CunningLords.Patches;

namespace CunningLords.Interaction
{
    class CampaignInteraction
    {

        [HarmonyPatch(typeof(Campaign))]
        [HarmonyPatch("RealTick")]
        class CampaignHourlyTickOverride
        {
            static void Postfix(Campaign __instance)
            {
                if (Input.IsKeyDown(InputKey.Numpad7))
                {
                    InformationManager.DisplayMessage(new InformationMessage("PRESSED!"));
                }
            }
        }

    }
}
