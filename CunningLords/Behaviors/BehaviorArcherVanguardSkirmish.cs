using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace CunningLords.Behaviors
{
    public class BehaviorArcherVanguardSkirmish : BehaviorComponent
    {
        Formation formation;
        internal BehaviorArcherVanguardSkirmish(Formation f) : base(/*formation*/)
        {
            this.CalculateCurrentOrder();
            this.formation = f;
        }

        protected override void CalculateCurrentOrder()
        {
            base.CalculateCurrentOrder();
        }

        protected override void TickOccasionally()
        {
            this.CalculateCurrentOrder();
            this.formation.MovementOrder = base.CurrentOrder;
        }

        protected override void OnBehaviorActivatedAux()
        {
            this.CalculateCurrentOrder();
            this.formation.MovementOrder = base.CurrentOrder;
            this.formation.FacingOrder = this.CurrentFacingOrder;
            this.formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLine;
            this.formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.formation.FormOrder = FormOrder.FormOrderDeep;
            this.formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
        }

        protected override float GetAiWeight()
        {
            if (this.formation.QuerySystem.ClosestEnemyFormation == null)
            {
                return 0f;
            }
            return 1f;
        }
    }
}
