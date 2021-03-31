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
        class OverrideBehaviorMountedSkirmish
        {
            [HarmonyPatch(typeof(BehaviorMountedSkirmish))]
            [HarmonyPatch("CalculateCurrentOrder")]
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

                Formation mainThreat = null;

                if (!___formation.Team.IsPlayerTeam) //Only works if there are only 2 teams, the player's and the AI's
                {
                    mainThreat = Utils.GetSkirmishersGreatestEnemy(___formation); //Careful if null

                    List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(___formation);

                    List<Formation> tooCloseForConfort = new List<Formation>();

                    foreach (Tuple<Formation, float> tup in distances)
                    {
                        if (tup.Item2 < (0.6 * ___formation.QuerySystem.MissileRange))
                        {
                            tooCloseForConfort.Add(tup.Item1);
                        }
                    }

                    Vec2 escapeVector = new Vec2(0,0);

                    if (tooCloseForConfort.Count > 1) //Too close from more than 1 formation
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Horse Archers: Kiting " + tooCloseForConfort.Count.ToString() + " enemies"));

                        foreach (Formation f in tooCloseForConfort)
                        {
                            escapeVector = Utils.AddVec2(escapeVector, ___formation.QuerySystem.AveragePosition - f.QuerySystem.AveragePosition);
                        }

                        escapeVector = escapeVector.Normalized();

                        escapeVector = Utils.MultVec2(5, escapeVector);

                        escapeVector = Utils.AddVec2(escapeVector, ___formation.QuerySystem.AveragePosition);

                    }
                    else if (tooCloseForConfort.Count == 1) //Too close too one formation
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Horse Archers: Kiting one enemy"));

                        escapeVector = ___formation.QuerySystem.AveragePosition - tooCloseForConfort.First().QuerySystem.AveragePosition;

                        escapeVector = Utils.MultVec2(5, escapeVector);

                        escapeVector = Utils.AddVec2(escapeVector, ___formation.QuerySystem.AveragePosition);

                    }
                    else //No formations close, must approach
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Horse Archers: approach enemy"));

                        escapeVector = ___formation.QuerySystem.AveragePosition - mainThreat.QuerySystem.AveragePosition;

                        escapeVector = escapeVector.Normalized();

                        escapeVector = Utils.MultVec2((___formation.QuerySystem.MissileRange * 0.75f), escapeVector);

                        escapeVector = Utils.AddVec2(mainThreat.QuerySystem.AveragePosition, escapeVector);

                    }

                    WorldPosition position = ___formation.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    ____currentOrder = MovementOrder.MovementOrderMove(position);
                }
            }
        }
    }
}
