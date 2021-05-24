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
using CunningLords.BehaviorTreelogic;

namespace CunningLords.Behaviors
{
    class BehaviorProximityCharge : BehaviorDefend
    {
        public Formation Formation;
        public BehaviorConfig config;

        private Formation mainFormation;
        public BehaviorProximityCharge(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
        }

        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions()
        {
            Formation closestFormation = Utils.GetClosestPlayerFormation(this.Formation);

            this.Formation.MovementOrder = MovementOrder.MovementOrderChargeToTarget(closestFormation);

            Vec2 focusDirection = (closestFormation.QuerySystem.MedianPosition.AsVec2 - this.Formation.QuerySystem.MedianPosition.AsVec2);

            this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(focusDirection);

            MaintainPersistentOrders();
        }

        private void MaintainPersistentOrders()
        {
            this.Formation.ArrangementOrder = this.config.arrangementOrder;
            this.Formation.FiringOrder = this.config.firingOrder;
            this.Formation.FormOrder = this.config.formOrder;
            this.Formation.WeaponUsageOrder = this.config.weaponusageOrder;
            this.Formation.RidingOrder = this.config.ridingOrder;
        }

        protected override void TickOccasionally()
        {
            ExecuteActions();
        }

        protected override void OnBehaviorActivatedAux()
        {
        }

        protected override float GetAiWeight()
        {
            return 1f;
        }
    }
}
