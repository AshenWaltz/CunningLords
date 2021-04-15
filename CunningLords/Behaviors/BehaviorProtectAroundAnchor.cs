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
    class BehaviorProtectAroundAnchor : BehaviorDefend
    {
        public Formation Formation;

        private Formation mainFormation;

        public FormationAI.BehaviorSide FlankSide;

        private BehaviorProtectAroundAnchor.BehaviorState protectFlankSate = BehaviorProtectAroundAnchor.BehaviorState.HoldingFlank;
        public BehaviorProtectAroundAnchor(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
        }

        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions() //Doesn't work if there is no mainformation
        {
            if (this.mainFormation == null || this.Formation == null)
            {
                this.Formation.MovementOrder = MovementOrder.MovementOrderStop;
                this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtEnemy;

                return;
            }

            float engageDistance = 50f;

            Formation archerFormation = Utils.GetAlliedFormationsofType(this.Formation, FormationClass.Ranged);

            Vec2 anchorPoint = new Vec2();

            if (archerFormation == null)
            {

                Vec2 infantryDirection = this.mainFormation.Direction.Normalized();

                if (this.FlankSide == FormationAI.BehaviorSide.Left)
                {
                    anchorPoint = this.mainFormation.CurrentPosition + (this.mainFormation.Width * Utils.PerpLeft(infantryDirection));
                }
                else if (this.FlankSide == FormationAI.BehaviorSide.Right)
                {
                    anchorPoint = this.mainFormation.CurrentPosition + (this.mainFormation.Width * Utils.PerpRight(infantryDirection));
                }
            }
            else
            {
                float archerInfantryDistance = this.mainFormation.CurrentPosition.Distance(archerFormation.CurrentPosition);

                //Vec2 midPoint = ____mainFormation.CurrentPosition + ((archerFormation.CurrentPosition - ____mainFormation.CurrentPosition).Normalized() * (archerInfantryDistance / 2));

                Vec2 archerInfantryDirection = (archerFormation.CurrentPosition - this.mainFormation.CurrentPosition).Normalized();

                float dotProduct = archerInfantryDirection.DotProduct(this.mainFormation.CurrentPosition);

                bool archersInFront = false;

                if (dotProduct > 0.6)
                {
                    archersInFront = true;
                }
                else
                {
                    archersInFront = false;
                }

                if (archersInFront)
                {
                    Vec2 archersDirection = archerFormation.Direction.Normalized();

                    if (this.FlankSide == FormationAI.BehaviorSide.Left)
                    {
                        anchorPoint = archerFormation.CurrentPosition + (archerFormation.Width * Utils.PerpLeft(archersDirection));
                    }
                    else if (this.FlankSide == FormationAI.BehaviorSide.Right)
                    {
                        anchorPoint = archerFormation.CurrentPosition + (archerFormation.Width * Utils.PerpRight(archersDirection));
                    }
                }
                else
                {
                    Vec2 infantryDirection = this.mainFormation.Direction.Normalized();

                    if (this.FlankSide == FormationAI.BehaviorSide.Left)
                    {
                        anchorPoint = this.mainFormation.CurrentPosition + (this.mainFormation.Width * Utils.PerpLeft(infantryDirection));
                    }
                    else if (this.FlankSide == FormationAI.BehaviorSide.Right)
                    {
                        anchorPoint = this.mainFormation.CurrentPosition + (this.mainFormation.Width * Utils.PerpRight(infantryDirection));
                    }
                }
            }

            List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemiesToPoint(this.Formation, anchorPoint);

            float minDistance = 100000f;

            Formation closestEnemy = null;

            foreach (Tuple<Formation, float> tup in distances)
            {
                if (tup.Item2 < minDistance)
                {
                    minDistance = tup.Item2;
                    closestEnemy = tup.Item1;
                }
            }

            float distanceFromAnchorPoint = this.Formation.CurrentPosition.Distance(anchorPoint);


            if (minDistance > engageDistance && distanceFromAnchorPoint < 5)
            {
                this.protectFlankSate = BehaviorState.HoldingFlank;
            }
            else if (minDistance > engageDistance && distanceFromAnchorPoint > 10)
            {
                this.protectFlankSate = BehaviorState.Returning;
            }
            else if (minDistance < engageDistance)
            {
                this.protectFlankSate = BehaviorState.Charging;
            }

            Vec2 targetPosition = anchorPoint;
            Vec2 targetDirection = this.mainFormation.Direction;

            if (this.protectFlankSate == BehaviorState.Returning || this.protectFlankSate == BehaviorState.HoldingFlank)
            {
                InformationManager.DisplayMessage(new InformationMessage("Cavalry Protecting Flank - Returning"));

                targetPosition = anchorPoint;
                targetDirection = this.mainFormation.Direction;
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("Cavalry Engaging"));

                if (closestEnemy == null)
                {
                    targetPosition = anchorPoint;
                    targetDirection = this.mainFormation.Direction;
                }
                else
                {
                    targetPosition = closestEnemy.CurrentPosition;
                    targetDirection = (closestEnemy.CurrentPosition - this.Formation.CurrentPosition).Normalized();
                }
            }

            WorldPosition position = this.Formation.QuerySystem.MedianPosition;
            position.SetVec2(targetPosition);
            this.Formation.MovementOrder = MovementOrder.MovementOrderMove(position);

            this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(targetDirection);
            MaintainPersistentOrders();
        }

        private void MaintainPersistentOrders()
        {
            this.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLine;
            this.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.Formation.FormOrder = FormOrder.FormOrderWide;
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

        private enum BehaviorState
        {
            // Token: 0x04000016 RID: 22
            HoldingFlank,
            // Token: 0x04000017 RID: 23
            Charging,
            // Token: 0x04000018 RID: 24
            Returning
        }
    }
}
