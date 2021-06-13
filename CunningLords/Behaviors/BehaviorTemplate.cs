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
    class BehaviorTemplate : BehaviorDefend
    {
        public Formation Formation;

        private Formation mainFormation;
        public BehaviorTemplate(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
        }

        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions()
        {
            /*InformationManager.DisplayMessage(new InformationMessage("Custom Behavior"));
            this.Formation.MovementOrder = base.CurrentOrder;
            this.Formation.FacingOrder = this.CurrentFacingOrder;
            this.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLine;
            this.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.Formation.FormOrder = FormOrder.FormOrderDeep;
            this.Formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
            InformationManager.DisplayMessage(new InformationMessage("DID SET MOVEMENTORDER"));*/
        }

        private void MaintainPersistentOrders()
        {
            this.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLine;
            this.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.Formation.FormOrder = FormOrder.FormOrderWide;
            this.Formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
        }

        public override void TickOccasionally()
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
