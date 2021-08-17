using HarmonyLib;
using SandBox.ViewModelCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;
using CunningLords.Interaction;

namespace CunningLords.Patches
{
    class TestBattleEndScreen
    {
        private static readonly TextObject _goldStr = new TextObject("{=WAKz9xX8}You gained {A0} gold.", null);

        [HarmonyPatch(typeof(SPScoreboardVM))]
        [HarmonyPatch("GetBattleRewards")]
        class ResetOverride
        {
            static void Prefix(SPScoreboardVM __instance, ref bool playerVictory)
            {
                if (CampaignInteraction.isCustomBattle)
                {
                    playerVictory = false;
                }
            }
        }
    }
}
