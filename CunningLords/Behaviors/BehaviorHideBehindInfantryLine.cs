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
    class BehaviorHideBehindInfantryLine : BehaviorDefend
    {
        public Formation Formation;

        private Formation mainFormation;
        public BehaviorHideBehindInfantryLine(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
        }

        /*
        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions()
        {
            //InformationManager.DisplayMessage(new InformationMessage(this.Formation.FormationIndex + ": Hiding Behind Infantry"));

            Vec2 escapeVector;

            if(this.mainFormation != null)
            {
                Vec2 infantryPosition = this.mainFormation.QuerySystem.AveragePosition;

                Vec2 infantryDirection = this.mainFormation.Direction.Normalized();

                escapeVector = infantryPosition - (infantryDirection * 4 * (this.mainFormation.Depth + this.Formation.Depth));

                WorldPosition position = this.Formation.QuerySystem.MedianPosition;
                position.SetVec2(escapeVector);
                this.Formation.MovementOrder = MovementOrder.MovementOrderMove(position);

                this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(infantryDirection);
            }
            else
            {
                this.Formation.MovementOrder = MovementOrder.MovementOrderStop;
                this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtEnemy;
            }
            MaintainPersistentOrders();
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
        }*/
    }
}
