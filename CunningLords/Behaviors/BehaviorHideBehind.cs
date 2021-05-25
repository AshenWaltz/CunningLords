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
    class BehaviorHideBehind : BehaviorDefend
    {
        public Formation Formation;
        public BehaviorConfig config;
        public FormationClass focus;

        public BehaviorHideBehind(Formation formation) : base(formation)
        {
        }

        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions()
        {
            Formation focusedFormation = this.Formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == this.focus);

            if (focusedFormation != null)
            {
                Vec2 escapeVector;

                Vec2 focusedPosition = focusedFormation.QuerySystem.AveragePosition;

                Vec2 focusedDirection = focusedFormation.Direction.Normalized();

                escapeVector = focusedPosition - (focusedDirection * 4 * (focusedFormation.Depth + focusedFormation.Depth));

                WorldPosition position = this.Formation.QuerySystem.MedianPosition;
                position.SetVec2(escapeVector);
                this.Formation.MovementOrder = MovementOrder.MovementOrderMove(position);

                this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(focusedDirection);
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
