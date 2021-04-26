using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CunningLords.Interaction
{
    class StartingOrderData
    {
        public OrderType InfantryOrder { get; set; }

        public OrderType ArcherOrder { get; set; }

        public OrderType CavalryOrder { get; set; }

        public OrderType HorseArcherOrder { get; set; }

        public OrderType SkirmisherOrder { get; set; }

        public OrderType HeavyInfantryOrder { get; set; }

        public OrderType LightCavalryOrder { get; set; }

        public OrderType HeavyCavalryOrder { get; set; }
    }
}
