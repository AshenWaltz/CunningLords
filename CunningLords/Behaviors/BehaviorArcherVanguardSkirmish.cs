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

        internal BehaviorArcherVanguardSkirmish(Formation formation) : base(formation)
        {
            this.CalculateCurrentOrder();
        }

        protected override void CalculateCurrentOrder()
        {
            base.CalculateCurrentOrder();
        }

        protected override void TickOccasionally()
        {
            this.CalculateCurrentOrder();
            //this.formation.MovementOrder = base.CurrentFacingOrder;
        }

        protected override void OnBehaviorActivatedAux()
        {
            this.CalculateCurrentOrder();
            /*this.formation.MovementOrder = base.CurrentOrder;
            this.formation.FacingOrder = this.CurrentFacingOrder;
            this.formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLine;
            this.formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.formation.FormOrder = FormOrder.FormOrderDeep;
            this.formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;*/
        }

        protected override float GetAiWeight()
        {
            /*if (this.formation.QuerySystem.ClosestEnemyFormation == null)
            {
                return 0f;
            }*/
            return 1f;
        }
    }
}
