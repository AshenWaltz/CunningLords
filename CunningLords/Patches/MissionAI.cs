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

        [HarmonyPatch(typeof(TeamAIComponent))]
        [HarmonyPatch("MakeDecision")]
        class TeamAIOverride
        {
            //Make Decision original logic
            // 1. lists all avilable tactics
            // 2. Checks if current state of mission is continue, if there are tactics available or if there are teams
            // 3. checks if there are enemy teams. If not, attributes tactic charge if it has it, if not, return first tactic
            // 4. Verify if defense is applicable - Check this function for more data on Teams
            // 5. See which tactics has the highest weight. Current tactic is multiplied by 1.5.

            //ToDo

            static void Postfix(TeamAIComponent __instance)
            {

            }
        }

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

        
        [HarmonyPatch(typeof(TacticFullScaleAttack))]
        class OverrideTacticFullScaleAttack
        {
            [HarmonyPostfix]
            [HarmonyPatch("Advance")]
            static void PostfixAdvance(ref Formation ____mainInfantry, ref Formation ____archers, 
                ref Formation ____rightCavalry, ref Formation ____leftCavalry)
            {
                bool infantryNotNull = ____mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ____rightCavalry != null;
                bool leftCavalryNotNull = ____leftCavalry != null;

                if (infantryNotNull)
                {
                    ____mainInfantry.AI.ResetBehaviorWeights();
                    ____mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ____rightCavalry.AI.ResetBehaviorWeights();
                    ____rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ____leftCavalry.AI.ResetBehaviorWeights();
                    ____leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("Attack")]
            static void PostfixAttack(ref Formation ____mainInfantry, ref Formation ____archers,
                ref Formation ____rightCavalry, ref Formation ____leftCavalry)
            {
                bool infantryNotNull = ____mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ____rightCavalry != null;
                bool leftCavalryNotNull = ____leftCavalry != null;

                if (infantryNotNull)
                {
                    ____mainInfantry.AI.ResetBehaviorWeights();
                    ____mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ____rightCavalry.AI.ResetBehaviorWeights();
                    ____rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ____leftCavalry.AI.ResetBehaviorWeights();
                    ____leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }
        }

        
        [HarmonyPatch(typeof(TacticDefensiveEngagement))]
        class OverrideTacticDefensiveEngagement
        {
            [HarmonyPostfix]
            [HarmonyPatch("Defend")]
            private static void PostfixDefend(ref Formation ____mainInfantry, ref Formation ____archers,
                ref Formation ____rightCavalry, ref Formation ____leftCavalry)
            {
                bool infantryNotNull = ____mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ____rightCavalry != null;
                bool leftCavalryNotNull = ____leftCavalry != null;

                if (infantryNotNull)
                {
                    ____mainInfantry.AI.ResetBehaviorWeights();
                    ____mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ____rightCavalry.AI.ResetBehaviorWeights();
                    ____rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ____leftCavalry.AI.ResetBehaviorWeights();
                    ____leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("Engage")]
            private static void PostfixEngage(ref Formation ____mainInfantry, ref Formation ____archers,
                ref Formation ____rightCavalry, ref Formation ____leftCavalry)
            {
                bool infantryNotNull = ____mainInfantry != null;
                bool archersNotNull = ____archers != null;
                bool rightCavalryNotNull = ____rightCavalry != null;
                bool leftCavalryNotNull = ____leftCavalry != null;

                if (infantryNotNull)
                {
                    ____mainInfantry.AI.ResetBehaviorWeights();
                    ____mainInfantry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (archersNotNull)
                {
                    ____archers.AI.ResetBehaviorWeights();
                    ____archers.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (rightCavalryNotNull)
                {
                    ____rightCavalry.AI.ResetBehaviorWeights();
                    ____rightCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }

                if (leftCavalryNotNull)
                {
                    ____leftCavalry.AI.ResetBehaviorWeights();
                    ____leftCavalry.AI.SetBehaviorWeight<BehaviorStop>(2f);
                }
            }
        }
    }
}
