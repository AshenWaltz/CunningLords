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

namespace CunningLords.BehaviorTreelogic
{
    public class BehaviorConfig
    {
        public ArrangementOrder arrangementOrder = ArrangementOrder.ArrangementOrderLoose;
        public FiringOrder firingOrder = FiringOrder.FiringOrderFireAtWill;
        public FormOrder formOrder = FormOrder.FormOrderWider;
        public WeaponUsageOrder weaponusageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
        public RidingOrder ridingOrder = RidingOrder.RidingOrderFree;
        public FacingOrder facingOrder = FacingOrder.FacingOrderLookAtEnemy;

        public BehaviorConfig(ArrangementOrder arrangement, FiringOrder firing, FormOrder form, WeaponUsageOrder weapon,
                              RidingOrder riding, FacingOrder facing)
        {
            this.arrangementOrder = arrangement;
            this.firingOrder = firing;
            this.formOrder = form;
            this.weaponusageOrder = weapon;
            this.ridingOrder = riding;
            this.facingOrder = facing;
        }
    }
}
