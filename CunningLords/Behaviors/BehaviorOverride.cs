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

        [HarmonyPatch(typeof(BehaviorHorseArcherSkirmish))]
        class OverrideBehaviorMountedSkirmish
        {
            [HarmonyPostfix]
            [HarmonyPatch("CalculateCurrentOrder")]
            private static void Postfix(ref Formation ___formation, BehaviorHorseArcherSkirmish __instance, ref MovementOrder ____currentOrder)
            {
                //1. Only do if it is enemy
                //2. See biggest threat to army. Movement speed and numbers.
                //3. Harrass the defined enemy
                //4. Try to maintain a minimum of 1/2 to 3/4 * MissileRange Distance from all enemies. Kite as enemy draws near

                if (___formation == null || __instance == null || ____currentOrder == null)
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

                    Vec2 escapeVector = new Vec2(0, 0);

                    if (tooCloseForConfort.Count > 1) //Too close from more than 1 formation
                    {
                        //InformationManager.DisplayMessage(new InformationMessage("Horse Archers: Kiting " + tooCloseForConfort.Count.ToString() + " enemies"));

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
                        //InformationManager.DisplayMessage(new InformationMessage("Horse Archers: Kiting one enemy"));

                        escapeVector = ___formation.QuerySystem.AveragePosition - tooCloseForConfort.First().QuerySystem.AveragePosition;

                        escapeVector = Utils.MultVec2(5, escapeVector);

                        escapeVector = Utils.AddVec2(escapeVector, ___formation.QuerySystem.AveragePosition);

                    }
                    else //No formations close, must approach
                    {
                        //InformationManager.DisplayMessage(new InformationMessage("Horse Archers: approach enemy"));

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

        
        [HarmonyPatch(typeof(BehaviorScreenedSkirmish))]
        class OverrideBehaviorScreenedSkirmish
        {
            [HarmonyPostfix]
            [HarmonyPatch("CalculateCurrentOrder")]
            private static void Postfix(Formation ___formation, ref MovementOrder ____currentOrder, ref FacingOrder ___CurrentFacingOrder)
            {
                if (___formation == null || ____currentOrder == null || ___CurrentFacingOrder == null)
                {
                    return;
                }
                
                if (!___formation.Team.IsPlayerTeam) //Only works if there are only 2 teams, the player's and the AI's
                {
                    List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(___formation);

                    List<Formation> tooCloseForConfort = new List<Formation>();

                    Formation closestEnemy = null;

                    float currentMinimumDistance = 100000f;

                    foreach (Tuple<Formation, float> tup in distances)
                    {
                        if (tup.Item2 < (0.5 * ___formation.QuerySystem.MissileRange))
                        {
                            tooCloseForConfort.Add(tup.Item1);
                        }

                        if (tup.Item2 < currentMinimumDistance)
                        {
                            closestEnemy = tup.Item1;
                            currentMinimumDistance = tup.Item2;
                        }
                    }

                    Vec2 targetPosition = new Vec2(0, 0);

                    if (tooCloseForConfort.Count > 0)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Archers: " + tooCloseForConfort.Count.ToString() + " enemies approaching"));

                        Vec2 infantryPosition = Utils.GetAlliedFormationsofType(___formation, FormationClass.Infantry).QuerySystem.AverageAllyPosition;

                        foreach (Formation f in tooCloseForConfort)
                        {
                            targetPosition = Utils.AddVec2(targetPosition, infantryPosition - f.QuerySystem.AveragePosition);
                        }

                        targetPosition = targetPosition.Normalized();

                        targetPosition = Utils.MultVec2(10, targetPosition);

                        targetPosition = Utils.AddVec2(targetPosition, infantryPosition);
                    }
                    else
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Archers: forward volley"));

                        Vec2 infantryPosition = Utils.GetAlliedFormationsofType(___formation, FormationClass.Infantry).QuerySystem.AveragePosition;

                        if(closestEnemy == null)
                        {
                            targetPosition = Utils.AddVec2(targetPosition, ___formation.QuerySystem.AveragePosition);
                        }
                        else
                        {
                            targetPosition = Utils.AddVec2(targetPosition, closestEnemy.QuerySystem.AveragePosition - infantryPosition);

                            targetPosition = targetPosition.Normalized();

                            targetPosition = Utils.MultVec2(10, targetPosition);

                            targetPosition = Utils.AddVec2(targetPosition, infantryPosition);
                        }
                    }

                    if (closestEnemy != null)
                    {
                        ___CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(closestEnemy.QuerySystem.AveragePosition - ___formation.QuerySystem.AveragePosition);
                    }


                    WorldPosition position = ___formation.QuerySystem.MedianPosition;
                    position.SetVec2(targetPosition);
                    ____currentOrder = MovementOrder.MovementOrderMove(position);
                }
            }
        }

        
        [HarmonyPatch(typeof(BehaviorSkirmishLine))]
        class OverrideBehaviorSkirmishLine
        {
            [HarmonyPostfix]
            [HarmonyPatch("CalculateCurrentOrder")]
            private static void Postfix(ref Formation ___formation, BehaviorSkirmishLine __instance, ref MovementOrder ____currentOrder)
            {

                if (___formation == null || __instance == null || ____currentOrder == null)
                {
                    return;
                }

                Formation mainThreat;

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

                    Vec2 escapeVector = new Vec2(0, 0);

                    if (tooCloseForConfort.Count > 1) //Too close from more than 1 formation
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Archer Skirmishing: Kiting " + tooCloseForConfort.Count.ToString() + " enemies"));

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
                        InformationManager.DisplayMessage(new InformationMessage("Archer Skirmishing: Kiting one enemy"));

                        escapeVector = ___formation.QuerySystem.AveragePosition - tooCloseForConfort.First().QuerySystem.AveragePosition;

                        escapeVector = Utils.MultVec2(5, escapeVector);

                        escapeVector = Utils.AddVec2(escapeVector, ___formation.QuerySystem.AveragePosition);

                    }
                    else //No formations close, must approach
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Archer Skirmishing: Stand in wait"));

                        WorldPosition medianPosition = ___formation.QuerySystem.MedianPosition;
                        medianPosition.SetVec2(___formation.QuerySystem.AveragePosition);
                        ____currentOrder = MovementOrder.MovementOrderMove(medianPosition);
                        return;
                    }

                    WorldPosition position = ___formation.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    ____currentOrder = MovementOrder.MovementOrderMove(position);
                }

            }
        }


    }
}
