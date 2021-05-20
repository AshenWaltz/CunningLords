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
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.ObjectSystem;
using CunningLords.Interaction;

namespace CunningLords.Patches
{
    class GameDataForAI
    {
        public int tacticsSkill;

        public static void writeToData(int tactics)
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "configData.json");

            CunningLordsConfigData data;
            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer deserializer = new JsonSerializer();
                data = (CunningLordsConfigData)deserializer.Deserialize(file, typeof(CunningLordsConfigData));
            }

            data.TacticSill = tactics;

            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(finalPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data);
            }
        }

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

                GameDataForAI.writeToData(tactics);
            }
        }

        [HarmonyPatch(typeof(CustomBattleState), "GetCustomBattleParties")]
        public class CustomBattleStateGetCustomBattlePartiesPatch
        {
            private static void Postfix(CustomBattleCombatant[] __result)
            {
                int tactics = -1;
                foreach (var result in __result)
                {
                    if (result.Side != MissionOverride.PlayerBattleSide)
                    {
                        var character = result.Characters.First();
                        tactics = character.GetSkillValue(DefaultSkills.Tactics);
                        InformationManager.DisplayMessage(new InformationMessage("Party Leader " + result.Name + " has " + tactics + " tactics skill"));
                    }

                }

                GameDataForAI.writeToData(tactics);
            }
        }
    }
}
