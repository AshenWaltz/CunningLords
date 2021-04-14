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
    class BehaviorLooselyWaitOrders : BehaviorDefend
    {
        public Formation Formation;

        private Formation mainFormation;
        public BehaviorLooselyWaitOrders(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
        }

        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions()
        {
            InformationManager.DisplayMessage(new InformationMessage(this.Formation.FormationIndex + ": Loosely Waiting Orders"));

            Vec2 escapeVector = this.Formation.QuerySystem.AveragePosition;

            Vec2 infantryDirection = this.Formation.Direction;

            WorldPosition position = this.Formation.QuerySystem.MedianPosition;
            position.SetVec2(escapeVector);
            this.Formation.MovementOrder = MovementOrder.MovementOrderMove(position);

            this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(infantryDirection);
            MaintainPersistentOrders();
        }

        private void MaintainPersistentOrders()
        {
            this.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLoose;
            this.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.Formation.FormOrder = FormOrder.FormOrderWider;
            this.Formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
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
