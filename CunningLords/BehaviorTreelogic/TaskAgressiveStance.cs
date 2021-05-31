using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.Patches;

namespace CunningLords.BehaviorTreelogic
{
    class TaskAgressiveStance : Sequence
    {
        public TaskAgressiveStance(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if (this.formation.Team.Side == BattleSideEnum.Attacker)
            {
                BTReturnEnum res = base.run();

                if(res == BTReturnEnum.succeeded)
                {
                    return BTReturnEnum.succeeded;
                }
                else if(res == BTReturnEnum.failed)
                {
                    return BTReturnEnum.failed;
                }
                else
                {
                    return BTReturnEnum.isEmpty;
                }
            }
            else
            {
                return BTReturnEnum.failed;
            }
            
        }
    }
}
