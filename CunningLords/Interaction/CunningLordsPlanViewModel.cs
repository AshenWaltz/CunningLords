﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.Core;
using CunningLords.Interaction;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using TaleWorlds.MountAndBlade;
using CunningLords.Patches;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.ObjectSystem;
using SandBox;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade.MissionSpawnHandlers;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;
using SandBox.Source.Missions.Handlers;
using SandBox.Source.Missions;
using Helpers;
using CunningLords.PlanDefinition;

namespace CunningLords.Interaction
{
    class CunningLordsPlanViewModel : ViewModel
    {
        public TextViewModel InfantryPrepareText { get; }
        public SelectorVM<SelectorItemVM> InfantryPrepare { get; }

        public TextViewModel InfantryRangedText { get; }
        public SelectorVM<SelectorItemVM> InfantryRanged { get; }

        public TextViewModel InfantryEngageText { get; }
        public SelectorVM<SelectorItemVM> InfantryEngage { get; }

        public TextViewModel InfantryWinningText { get; }
        public SelectorVM<SelectorItemVM> InfantryWinning { get; }

        public TextViewModel InfantryLosingText { get; }
        public SelectorVM<SelectorItemVM> InfantryLosing { get; }

        public TextViewModel ArchersPrepareText { get; }
        public SelectorVM<SelectorItemVM> ArchersPrepare { get; }

        public TextViewModel ArchersRangedText { get; }
        public SelectorVM<SelectorItemVM> ArchersRanged { get; }

        public TextViewModel ArchersEngageText { get; }
        public SelectorVM<SelectorItemVM> ArchersEngage { get; }

        public TextViewModel ArchersWinningText { get; }
        public SelectorVM<SelectorItemVM> ArchersWinning { get; }

        public TextViewModel ArchersLosingText { get; }
        public SelectorVM<SelectorItemVM> ArchersLosing { get; }

        public TextViewModel CavalryPrepareText { get; }
        public SelectorVM<SelectorItemVM> CavalryPrepare { get; }

        public TextViewModel CavalryRangedText { get; }
        public SelectorVM<SelectorItemVM> CavalryRanged { get; }

        public TextViewModel CavalryEngageText { get; }
        public SelectorVM<SelectorItemVM> CavalryEngage { get; }

        public TextViewModel CavalryWinningText { get; }
        public SelectorVM<SelectorItemVM> CavalryWinning { get; }

        public TextViewModel CavalryLosingText { get; }
        public SelectorVM<SelectorItemVM> CavalryLosing { get; }

        public TextViewModel HorseArchersPrepareText { get; }
        public SelectorVM<SelectorItemVM> HorseArchersPrepare { get; }

        public TextViewModel HorseArchersRangedText { get; }
        public SelectorVM<SelectorItemVM> HorseArchersRanged { get; }

        public TextViewModel HorseArchersEngageText { get; }
        public SelectorVM<SelectorItemVM> HorseArchersEngage { get; }

        public TextViewModel HorseArchersWinningText { get; }
        public SelectorVM<SelectorItemVM> HorseArchersWinning { get; }

        public TextViewModel HorseArchersLosingText { get; }
        public SelectorVM<SelectorItemVM> HorseArchersLosing { get; }

        public TextViewModel SkirmishersPrepareText { get; }
        public SelectorVM<SelectorItemVM> SkirmishersPrepare { get; }

        public TextViewModel SkirmishersRangedText { get; }
        public SelectorVM<SelectorItemVM> SkirmishersRanged { get; }

        public TextViewModel SkirmishersEngageText { get; }
        public SelectorVM<SelectorItemVM> SkirmishersEngage { get; }

        public TextViewModel SkirmishersWinningText { get; }
        public SelectorVM<SelectorItemVM> SkirmishersWinning { get; }

        public TextViewModel SkirmishersLosingText { get; }
        public SelectorVM<SelectorItemVM> SkirmishersLosing { get; }

        public TextViewModel HeavyInfantryPrepareText { get; }
        public SelectorVM<SelectorItemVM> HeavyInfantryPrepare { get; }

        public TextViewModel HeavyInfantryRangedText { get; }
        public SelectorVM<SelectorItemVM> HeavyInfantryRanged { get; }

        public TextViewModel HeavyInfantryEngageText { get; }
        public SelectorVM<SelectorItemVM> HeavyInfantryEngage { get; }

        public TextViewModel HeavyInfantryWinningText { get; }
        public SelectorVM<SelectorItemVM> HeavyInfantryWinning { get; }

        public TextViewModel HeavyInfantryLosingText { get; }
        public SelectorVM<SelectorItemVM> HeavyInfantryLosing { get; }

        public TextViewModel LightCavalryPrepareText { get; }
        public SelectorVM<SelectorItemVM> LightCavalryPrepare { get; }

        public TextViewModel LightCavalryRangedText { get; }
        public SelectorVM<SelectorItemVM> LightCavalryRanged { get; }

        public TextViewModel LightCavalryEngageText { get; }
        public SelectorVM<SelectorItemVM> LightCavalryEngage { get; }

        public TextViewModel LightCavalryWinningText { get; }
        public SelectorVM<SelectorItemVM> LightCavalryWinning { get; }

        public TextViewModel LightCavalryLosingText { get; }
        public SelectorVM<SelectorItemVM> LightCavalryLosing { get; }

        public TextViewModel HeavyCavalryPrepareText { get; }
        public SelectorVM<SelectorItemVM> HeavyCavalryPrepare { get; }

        public TextViewModel HeavyCavalryRangedText { get; }
        public SelectorVM<SelectorItemVM> HeavyCavalryRanged { get; }

        public TextViewModel HeavyCavalryEngageText { get; }
        public SelectorVM<SelectorItemVM> HeavyCavalryEngage { get; }

        public TextViewModel HeavyCavalryWinningText { get; }
        public SelectorVM<SelectorItemVM> HeavyCavalryWinning { get; }

        public TextViewModel HeavyCavalryLosingText { get; }
        public SelectorVM<SelectorItemVM> HeavyCavalryLosing { get; }

        public CunningLordsPlanViewModel()
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "Decisiontree.json");

            Plan data;

            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                data = (Plan)serializer.Deserialize(file, typeof(Plan));
            }



            List<String> orders = new List<String>()
            {
                "Charge",
                "HoldPosition",
                "Flank",
                "Skirmish",
                "HideBehind",
                "ProtectFlank",
                "Advance",
                "CautiousAdvance"
            };

            //Infantry

            this.InfantryPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.InfantryPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.InfantryPrepare.SelectedIndex = GetIndex(data.infantryPhasePrepare);

            this.InfantryRangedText = new TextViewModel(new TextObject("Hello", null));
            this.InfantryRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.InfantryRanged.SelectedIndex = GetIndex(data.infantryPhaseRanged);

            this.InfantryEngageText = new TextViewModel(new TextObject("Hello", null));
            this.InfantryEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.InfantryEngage.SelectedIndex = GetIndex(data.infantryPhaseEngage);

            this.InfantryWinningText = new TextViewModel(new TextObject("Hello", null));
            this.InfantryWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.InfantryWinning.SelectedIndex = GetIndex(data.infantryPhaseWinning);

            this.InfantryLosingText = new TextViewModel(new TextObject("Hello", null));
            this.InfantryLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.InfantryLosing.SelectedIndex = GetIndex(data.infantryPhaseLosing);

            //Archers

            this.ArchersPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.ArchersPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.ArchersPrepare.SelectedIndex = GetIndex(data.archersPhasePrepare);

            this.ArchersRangedText = new TextViewModel(new TextObject("Hello", null));
            this.ArchersRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.ArchersRanged.SelectedIndex = GetIndex(data.archersPhaseRanged);

            this.ArchersEngageText = new TextViewModel(new TextObject("Hello", null));
            this.ArchersEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.ArchersEngage.SelectedIndex = GetIndex(data.archersPhaseEngage);

            this.ArchersWinningText = new TextViewModel(new TextObject("Hello", null));
            this.ArchersWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.ArchersWinning.SelectedIndex = GetIndex(data.archersPhaseWinning);

            this.ArchersLosingText = new TextViewModel(new TextObject("Hello", null));
            this.ArchersLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.ArchersLosing.SelectedIndex = GetIndex(data.archersPhaseLosing);

            //Cavalry

            this.CavalryPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.CavalryPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.CavalryPrepare.SelectedIndex = GetIndex(data.cavalryPhasePrepare);

            this.CavalryRangedText = new TextViewModel(new TextObject("Hello", null));
            this.CavalryRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.CavalryRanged.SelectedIndex = GetIndex(data.cavalryPhaseRanged);

            this.CavalryEngageText = new TextViewModel(new TextObject("Hello", null));
            this.CavalryEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.CavalryEngage.SelectedIndex = GetIndex(data.cavalryPhaseEngage);

            this.CavalryWinningText = new TextViewModel(new TextObject("Hello", null));
            this.CavalryWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.CavalryWinning.SelectedIndex = GetIndex(data.cavalryPhaseWinning);

            this.CavalryLosingText = new TextViewModel(new TextObject("Hello", null));
            this.CavalryLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.CavalryLosing.SelectedIndex = GetIndex(data.cavalryPhaseLosing);

            //HorseArchers

            this.HorseArchersPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.HorseArchersPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HorseArchersPrepare.SelectedIndex = GetIndex(data.horseArchersPhasePrepare);

            this.HorseArchersRangedText = new TextViewModel(new TextObject("Hello", null));
            this.HorseArchersRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HorseArchersRanged.SelectedIndex = GetIndex(data.horseArchersPhaseRanged);

            this.HorseArchersEngageText = new TextViewModel(new TextObject("Hello", null));
            this.HorseArchersEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HorseArchersEngage.SelectedIndex = GetIndex(data.horseArchersPhaseEngage);

            this.HorseArchersWinningText = new TextViewModel(new TextObject("Hello", null));
            this.HorseArchersWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HorseArchersWinning.SelectedIndex = GetIndex(data.horseArchersPhaseWinning);

            this.HorseArchersLosingText = new TextViewModel(new TextObject("Hello", null));
            this.HorseArchersLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HorseArchersLosing.SelectedIndex = GetIndex(data.horseArchersPhaseLosing);

            //Skirmishers

            this.SkirmishersPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.SkirmishersPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.SkirmishersPrepare.SelectedIndex = GetIndex(data.skirmishersPhasePrepare);

            this.SkirmishersRangedText = new TextViewModel(new TextObject("Hello", null));
            this.SkirmishersRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.SkirmishersRanged.SelectedIndex = GetIndex(data.skirmishersPhaseRanged);

            this.SkirmishersEngageText = new TextViewModel(new TextObject("Hello", null));
            this.SkirmishersEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.SkirmishersEngage.SelectedIndex = GetIndex(data.skirmishersPhaseEngage);

            this.SkirmishersWinningText = new TextViewModel(new TextObject("Hello", null));
            this.SkirmishersWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.SkirmishersWinning.SelectedIndex = GetIndex(data.skirmishersPhaseWinning);

            this.SkirmishersLosingText = new TextViewModel(new TextObject("Hello", null));
            this.SkirmishersLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.SkirmishersLosing.SelectedIndex = GetIndex(data.skirmishersPhaseLosing);

            //HeavyInfantry

            this.HeavyInfantryPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyInfantryPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyInfantryPrepare.SelectedIndex = GetIndex(data.heavyInfantryPhasePrepare);

            this.HeavyInfantryRangedText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyInfantryRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyInfantryRanged.SelectedIndex = GetIndex(data.heavyInfantryPhaseRanged);

            this.HeavyInfantryEngageText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyInfantryEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyInfantryEngage.SelectedIndex = GetIndex(data.heavyInfantryPhaseEngage);

            this.HeavyInfantryWinningText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyInfantryWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyInfantryWinning.SelectedIndex = GetIndex(data.heavyInfantryPhaseWinning);

            this.HeavyInfantryLosingText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyInfantryLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyInfantryLosing.SelectedIndex = GetIndex(data.heavyInfantryPhaseLosing);

            //LightCavalry

            this.LightCavalryPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.LightCavalryPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.LightCavalryPrepare.SelectedIndex = GetIndex(data.lightCavalryPhasePrepare);

            this.LightCavalryRangedText = new TextViewModel(new TextObject("Hello", null));
            this.LightCavalryRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.LightCavalryRanged.SelectedIndex = GetIndex(data.lightCavalryPhaseRanged);

            this.LightCavalryEngageText = new TextViewModel(new TextObject("Hello", null));
            this.LightCavalryEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.LightCavalryEngage.SelectedIndex = GetIndex(data.lightCavalryPhaseEngage);

            this.LightCavalryWinningText = new TextViewModel(new TextObject("Hello", null));
            this.LightCavalryWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.LightCavalryWinning.SelectedIndex = GetIndex(data.lightCavalryPhaseWinning);

            this.LightCavalryLosingText = new TextViewModel(new TextObject("Hello", null));
            this.LightCavalryLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.LightCavalryLosing.SelectedIndex = GetIndex(data.lightCavalryPhaseLosing);

            //HeavyCavalry

            this.HeavyCavalryPrepareText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyCavalryPrepare = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyCavalryPrepare.SelectedIndex = GetIndex(data.heavyCavalryPhasePrepare);

            this.HeavyCavalryRangedText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyCavalryRanged = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyCavalryRanged.SelectedIndex = GetIndex(data.heavyCavalryPhaseRanged);

            this.HeavyCavalryEngageText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyCavalryEngage = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyCavalryEngage.SelectedIndex = GetIndex(data.heavyCavalryPhaseEngage);

            this.HeavyCavalryWinningText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyCavalryWinning = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyCavalryWinning.SelectedIndex = GetIndex(data.heavyCavalryPhaseWinning);

            this.HeavyCavalryLosingText = new TextViewModel(new TextObject("Hello", null));
            this.HeavyCavalryLosing = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.HeavyCavalryLosing.SelectedIndex = GetIndex(data.heavyCavalryPhaseLosing);

        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            //this._sliderValueText = this._sliderValue.ToString();

        }

        private void ExecuteDone()
        {
            Plan orders = new Plan()
            {
                infantryPhasePrepare = GetOrderType(InfantryPrepare.SelectedIndex),
                infantryPhaseRanged = GetOrderType(InfantryRanged.SelectedIndex),
                infantryPhaseEngage = GetOrderType(InfantryEngage.SelectedIndex),
                infantryPhaseWinning = GetOrderType(InfantryWinning.SelectedIndex),
                infantryPhaseLosing = GetOrderType(InfantryLosing.SelectedIndex),
                infantryPhasePrepare = GetOrderType(InfantryPrepare.SelectedIndex),
                infantryPhaseRanged = GetOrderType(InfantryRanged.SelectedIndex),
                infantryPhaseEngage = GetOrderType(InfantryEngage.SelectedIndex),
                infantryPhaseWinning = GetOrderType(InfantryWinning.SelectedIndex),
                infantryPhaseLosing = GetOrderType(InfantryLosing.SelectedIndex),
            };

            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "startdata.json");

            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(finalPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, orders);
            }

            ScreenManager.PopScreen();
        }

        public int GetIndex(PlanOrderEnum order)
        {
            switch (order)
            {
                case PlanOrderEnum.Charge:
                    return 0;
                case PlanOrderEnum.HoldPosition:
                    return 1;
                case PlanOrderEnum.Flank:
                    return 2;
                case PlanOrderEnum.Skirmish:
                    return 3;
                case PlanOrderEnum.HideBehind:
                    return 4;
                case PlanOrderEnum.ProtectFlank:
                    return 5;
                case PlanOrderEnum.Advance:
                    return 6;
                case PlanOrderEnum.CautiousAdvance:
                    return 7;
                default:
                    return 0;
            }
        }

        public PlanOrderEnum GetOrderType(int value)
        {
            if (value == 0)
            {
                return PlanOrderEnum.Charge;
            }
            else if (value == 1)
            {
                return PlanOrderEnum.HoldPosition;
            }
            else if (value == 2)
            {
                return PlanOrderEnum.Flank;
            }
            else if (value == 3)
            {
                return PlanOrderEnum.Skirmish;
            }
            else if (value == 4)
            {
                return PlanOrderEnum.HideBehind;
            }
            else if (value == 5)
            {
                return PlanOrderEnum.ProtectFlank;
            }
            else if (value == 6)
            {
                return PlanOrderEnum.Advance;
            }
            else if (value == 7)
            {
                return PlanOrderEnum.CautiousAdvance;
            }
            return PlanOrderEnum.HoldPosition;
        }
    }
}
