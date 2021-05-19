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
using TaleWorlds.MountAndBlade.CustomBattle;

namespace CunningLords.Patches
{
    class GameDataForAI
    {
        public int tacticsSkill;

        [HarmonyPatch(typeof(PlayerEncounter), "DoMeetingInternal")]
        public class PlayerEncounterDoMeetingInternalPatch
        {
            private static void Postfix(PartyBase ____encounteredParty)
            {
                Hero hero = ____encounteredParty.MobileParty.LeaderHero;
                int tactics = -1;
                if (hero != null)
                {
                    tactics = hero.GetSkillValue(DefaultSkills.Tactics);
                }
                //GameDataForAI.tacticsSkill = tactics; see communication!!!!
                InformationManager.DisplayMessage(new InformationMessage("Encountered enemy with " + tactics + " tactics skill"));
            }
        }

        [HarmonyPatch(typeof(CustomBattleState), "GetCustomBattleParties")]
        public class CustomBattleStateGetCustomBattlePartiesPatch
        {
            private static void Postfix(CustomBattleCombatant[] __result)
            {
                foreach (var result in __result)
                {
                    var character = result.Characters.First();
                    int tactics = character.GetSkillValue(DefaultSkills.Tactics);
                    InformationManager.DisplayMessage(new InformationMessage("Party Leader " + result.Name + " has "+ tactics + " tactics skill"));
                    FileLog.Log($"{result.Name}: {character} tactics: {result.GetTacticsSkillAmount()}");
                }
            }
        }
    }
}
