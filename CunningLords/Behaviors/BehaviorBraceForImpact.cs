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
    class BehaviorBraceForImpact : BehaviorDefend
    {
        public Formation Formation;

        private Formation mainFormation;
        public BehaviorBraceForImpact(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
        }
        /*
        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions()
        {
            //InformationManager.DisplayMessage(new InformationMessage(this.Formation.FormationIndex + ": Brace For Impact"));

            List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(this.Formation);

            float currentMinDistance = 100000f;
            Formation currentClosestEnemy = null;

            foreach (Tuple<Formation, float> tup in distances)
            {
                if (tup.Item2 < (currentMinDistance))
                {
                    currentClosestEnemy = tup.Item1;
                }
            }

            Vec2 escapeVector;

            Vec2 targetPosition = this.mainFormation.QuerySystem.AveragePosition;

            Vec2 targetDirection = (currentClosestEnemy.QuerySystem.AveragePosition - this.mainFormation.QuerySystem.AveragePosition).Normalized();

            escapeVector = targetPosition;

            WorldPosition position = this.Formation.QuerySystem.MedianPosition;
            position.SetVec2(escapeVector);
            this.Formation.MovementOrder = MovementOrder.MovementOrderMove(position);

            this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(targetDirection);
            MaintainPersistentOrders();
        }

        private void MaintainPersistentOrders()
        {
            if (this.Formation.QuerySystem.HasShield)
            {
                this.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderShieldWall;
            }
            else
            {
                this.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLine;
            }
            this.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.Formation.FormOrder = FormOrder.FormOrderDeep;
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
        }*/
    }
}
