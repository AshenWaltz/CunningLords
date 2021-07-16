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

        public static bool isCustomBattle = false;

        public static int healthPreTest = 50;

        public static bool hadMission = false;

        public static Hero characterToHeal = null;

        [HarmonyPatch(typeof(Campaign))]
        [HarmonyPatch("RealTick")]
        class CampaignHourlyTickOverride
        {
            static void Postfix(Campaign __instance)
            {
                if (Input.IsKeyDown(InputKey.LeftAlt) && Input.IsKeyDown(InputKey.E) && !CampaignInteraction._inMenu)
                {
                    CampaignInteraction._inMenu = true;
                    //InformationManager.DisplayMessage(new InformationMessage("PRESSED!"));
                    ScreenManager.PushScreen(new CunningLordsOptionScreen());
                }
                if (Input.IsKeyDown(InputKey.LeftAlt) && Input.IsKeyDown(InputKey.R) && !CampaignInteraction._inMenu)
                {
                    CampaignInteraction.isCustomBattle = true;
                    CunningLordsMenuViewModel vm = new CunningLordsMenuViewModel();
                    vm.StartMission();
                }

                //CampaignInteraction.isCustomBattle = false;
                MissionOverride.FrameCounter = 0;
                CampaignInteraction.isCampaign = true;

                /*if (characterToHeal != null && hadMission)
                {
                    characterToHeal.HitPoints = healthPreTest;
                    hadMission = false;
                }*/
            }
        }

        [HarmonyPatch(typeof(PlayerEncounter))]
        [HarmonyPatch("UpdateInternal")]
        class UpdateInternalOverride
        {
            static void Postfix(PlayerEncounter __instance)
            {
                if (CampaignInteraction.isCustomBattle)
                {
                    if (PlayerEncounter.Current != null)
                    {
                        if (PlayerEncounter.Current.EncounterState == PlayerEncounterState.Wait)
                        {
                            PlayerEncounter.Finish(false);
                        }
                    }

                    CampaignInteraction.isCustomBattle = false;
                }
                
            }
        }
            //Campaign.MapSate
        }
}
