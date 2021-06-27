using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CunningLords.PlanDefinition
{
    public class Plan
    {
        public PlanOrderEnum infantryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum infantryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum infantryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum infantryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum archersPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum archersPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum archersPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum archersPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum cavalryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum cavalryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum cavalryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum cavalryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum horseArchersPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum horseArchersPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum horseArchersPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum horseArchersPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum skirmishersPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum skirmishersPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum skirmishersPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum skirmishersPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum heavyInfantryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyInfantryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyInfantryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyInfantryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum LightCavalryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum LightCavalryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum LightCavalryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum LightCavalryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum HeavyCavalryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum HeavyCavalryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum HeavyCavalryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum HeavyCavalryPhaseLosing = PlanOrderEnum.HoldPosition;
    }
}
