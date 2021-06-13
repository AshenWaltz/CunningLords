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
    class BehaviorSlowAdvance : BehaviorDefend
    {
        public Formation Formation;
        public BehaviorConfig config;
        public FormationClass focus;

        private Formation mainFormation;
        public BehaviorSlowAdvance(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
        }
        /*
        protected override void CalculateCurrentOrder()
        {
        }

        private void ExecuteActions()
        {
            if ((this.focus != FormationClass.Unset) && (this.focus != null))
            {
                List<Formation> formationsOfFocus = Utils.GetPlayerFormationsofType(this.Formation, this.focus);

                if (formationsOfFocus.Count > 0)
                {
                    Formation focusedFormation = formationsOfFocus.First();

                    this.Formation.MovementOrder = MovementOrder.MovementOrderMove(focusedFormation.QuerySystem.MedianPosition);

                    Vec2 focusDirection = (focusedFormation.QuerySystem.MedianPosition.AsVec2 - this.Formation.QuerySystem.MedianPosition.AsVec2);

                    this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(focusDirection);
                }
                else
                {
                    Formation closestPlayerFormation = Utils.GetClosestPlayerFormation(this.Formation);

                    if (closestPlayerFormation != null)
                    {
                        this.Formation.MovementOrder = MovementOrder.MovementOrderMove(closestPlayerFormation.QuerySystem.MedianPosition);

                        Vec2 focusDirection = (closestPlayerFormation.QuerySystem.MedianPosition.AsVec2 - this.Formation.QuerySystem.MedianPosition.AsVec2);

                        this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(focusDirection);
                    }
                    else
                    {
                        this.Formation.MovementOrder = MovementOrder.MovementOrderCharge;
                        this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtEnemy;
                    }
                }
            }
            else
            {
                Formation closestPlayerFormation = Utils.GetClosestPlayerFormation(this.Formation);

                if (closestPlayerFormation != null)
                {
                    this.Formation.MovementOrder = MovementOrder.MovementOrderMove(closestPlayerFormation.QuerySystem.MedianPosition);

                    Vec2 focusDirection = (closestPlayerFormation.QuerySystem.MedianPosition.AsVec2 - this.Formation.QuerySystem.MedianPosition.AsVec2);

                    this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(focusDirection);
                }
                else
                {
                    this.Formation.MovementOrder = MovementOrder.MovementOrderCharge;
                    this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtEnemy;
                }
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
        }*/
    }
}
