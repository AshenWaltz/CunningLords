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

namespace CunningLords.Behaviors
{
    class BehaviorOverride
    {

        [HarmonyPatch(typeof(BehaviorMountedSkirmish))]
        [HarmonyPatch("")]
        class TeamAIOverride
        {
            private static void Postfix(ref Formation ___formation, BehaviorMountedSkirmish __instance, ref bool ____engaging, ref MovementOrder ____currentOrder)
            {
                //1. Only do if it is enemy
                //2. See biggest threat to army. Movement speed and numbers.
                //3. Harrass the defined enemy
                //4. Try to maintain a minimum of 1/2 to 3/4 * MissileRange Distance from all enemies. Kite as enemy draws near

                if(___formation == null || __instance == null || ____currentOrder == null)
                {
                    return;
                }

                FormationClass mainThreat = FormationClass.Unset;

                if (!___formation.Team.IsPlayerTeam)
                {
                    //mainThreat = Utilities.GetSkirmishersGreatestEnemy(___formation);
                }
            }
        }
    }
}
