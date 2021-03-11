using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CunningLords.Patches
{
    class MissionAI
    {
        
        public static BattleSideEnum PlayerBattleSide { get; set; } = BattleSideEnum.None;

        [HarmonyPatch(typeof(MissionCombatantsLogic))]
        [HarmonyPatch("EarlyStart")]
        //This class is used to load tactics into the AI Teams, the tactics themselves determine the behaviour of each Formation within a Team
        class TeamTacticsInitializer
        {
            static void Postfix(MissionCombatantsLogic __instance)
            {
                //MissionAI.PlayerBattleSide = __instance.Mission.MainAgent.Team.Side; //Crashes

                List<Team> enemyTeams = Utilities.GetAllEnemyTeams(__instance.Mission);

                if (__instance.Mission.MissionTeamAIType == Mission.MissionTeamAITypeEnum.FieldBattle)
                {
                    foreach (Team team in enemyTeams)
                    {
                        if(team.Side == BattleSideEnum.Attacker)
                        {
                            team.ClearTacticOptions();
                            team.AddTacticOption(new TacticFullScaleAttack(team));
                        }
                        else if(team.Side == BattleSideEnum.Defender)
                        {
                            team.ClearTacticOptions();
                            team.AddTacticOption(new TacticDefensiveEngagement(team));
                        }
                    }
                }
            }
        }

        /*
        [HarmonyPatch(typeof(TacticFullScaleAttack))]
        class OverrideTacticFullScaleAttack
        {
            [HarmonyPostfix]
            [HarmonyPatch("Advance")]
            private static void PostfixAdvance(ref Formation ___mainInfantry, ref Formation ____archers, 
                ref Formation ___rightCavalry, ref Formation ___leftCavalry)
            {
                bool infantryNotNull = ___mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ___rightCavalry != null;
                bool leftCavalryNotNull = ___leftCavalry != null;

                if (infantryNotNull)
                {
                    ___mainInfantry.AI.ResetBehaviorWeights();
                    ___mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ___rightCavalry.AI.ResetBehaviorWeights();
                    ___rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ___leftCavalry.AI.ResetBehaviorWeights();
                    ___leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("Attack")]
            private static void PostfixAttack(ref Formation ___mainInfantry, ref Formation ____archers,
                ref Formation ___rightCavalry, ref Formation ___leftCavalry)
            {
                bool infantryNotNull = ___mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ___rightCavalry != null;
                bool leftCavalryNotNull = ___leftCavalry != null;

                if (infantryNotNull)
                {
                    ___mainInfantry.AI.ResetBehaviorWeights();
                    ___mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ___rightCavalry.AI.ResetBehaviorWeights();
                    ___rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ___leftCavalry.AI.ResetBehaviorWeights();
                    ___leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }
        }

        [HarmonyPatch(typeof(TacticDefensiveEngagement))]
        class OverrideTacticDefensiveEngagement
        {
            [HarmonyPostfix]
            [HarmonyPatch("Defend")]
            private static void PostfixDefend(ref Formation ___mainInfantry, ref Formation ____archers,
                ref Formation ___rightCavalry, ref Formation ___leftCavalry)
            {
                bool infantryNotNull = ___mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ___rightCavalry != null;
                bool leftCavalryNotNull = ___leftCavalry != null;

                if (infantryNotNull)
                {
                    ___mainInfantry.AI.ResetBehaviorWeights();
                    ___mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ___rightCavalry.AI.ResetBehaviorWeights();
                    ___rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ___leftCavalry.AI.ResetBehaviorWeights();
                    ___leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("Engage")]
            private static void PostfixEngage(ref Formation ___mainInfantry, ref Formation ____archers,
                ref Formation ___rightCavalry, ref Formation ___leftCavalry)
            {
                bool infantryNotNull = ___mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ___rightCavalry != null;
                bool leftCavalryNotNull = ___leftCavalry != null;

                if (infantryNotNull)
                {
                    ___mainInfantry.AI.ResetBehaviorWeights();
                    ___mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ___rightCavalry.AI.ResetBehaviorWeights();
                    ___rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ___leftCavalry.AI.ResetBehaviorWeights();
                    ___leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }
        }*/
    }
}
