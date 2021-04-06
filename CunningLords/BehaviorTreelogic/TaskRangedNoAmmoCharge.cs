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

namespace CunningLords.BehaviorTreelogic
{
    class TaskRangedNoAmmoCharge : Task
    {
        public TaskRangedNoAmmoCharge(Formation f) : base(f)
        {
            this.formation = f;
        }

        public override BTReturnEnum run()
        {
            if ((this.formation != null) && Utils.HasAmmoRatio(this.formation) < 0.4f)
            {
                if (this.formation.FormationIndex == FormationClass.HorseArcher)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Horse Archers: Ammo Depleted. Charging!"));
                }
                else if(this.formation.FormationIndex == FormationClass.Ranged)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Archers: Ammo Depleted. Charging!"));
                }
                this.formation.AI.ResetBehaviorWeights();
                this.formation.AI.SetBehaviorWeight<BehaviorCharge>(2f);
                return BTReturnEnum.succeeded;
            }
            else
            {
                return BTReturnEnum.failed;
            }
        }
    }
}
