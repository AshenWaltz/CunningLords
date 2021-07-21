using System;
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

namespace CunningLords.Interaction
{
    internal class CunningLordsMenuViewModel : ViewModel
    {

        public bool isCampaign = false;

        public TextViewModel FormationIText { get; }
        public SelectorVM<SelectorItemVM> FormationI { get; }

        public TextViewModel FormationIIText { get; }
        public SelectorVM<SelectorItemVM> FormationII { get; }

        public TextViewModel FormationIIIText { get; }
        public SelectorVM<SelectorItemVM> FormationIII { get; }

        public TextViewModel FormationIVText { get; }
        public SelectorVM<SelectorItemVM> FormationIV { get; }

        public TextViewModel FormationVText { get; }
        public SelectorVM<SelectorItemVM> FormationV { get; }

        public TextViewModel FormationVIText { get; }
        public SelectorVM<SelectorItemVM> FormationVI { get; }

        public TextViewModel FormationVIIText { get; }
        public SelectorVM<SelectorItemVM> FormationVII { get; }

        public TextViewModel FormationVIIIText { get; }
        public SelectorVM<SelectorItemVM> FormationVIII { get; }

        public CunningLordsMenuViewModel()
        {
            this._doneText = new TextObject("{=ATDone}Done", null).ToString();
            this._cancelText = new TextObject("{=ATCancel}Cancel", null).ToString();
            this._pressButton1 = new TextObject("{=ATPressButton1}PressButton1", null).ToString();
            this._tab1Text = new TextObject("{=ATTab1Text}Main Interface", null).ToString();
            this._planTabText = new TextObject("{=ATPlanTabText}Plan Definition", null).ToString();
            this._tab2Text = new TextObject("{=ATTab2Text}Sub Interface", null).ToString();
            this._sliderText = new TextObject("{=ATSlideText}Slider Example", null).ToString();
            this._sliderValue = 10.0f;
            this._sliderValueText = this._sliderValue.ToString();
            this._booleanValue = true;
            this.RefreshValues();

            //!!!!!!!!!!!!!!
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "startdata.json");

            StartingOrderData data;

            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                data = (StartingOrderData)serializer.Deserialize(file, typeof(StartingOrderData));
            }



            List<String> orders = new List<String>()
            {
                "Charge",
                "Follow Me",
                "Hold",
                "Advance",
                "FallBack"
            };

            this.FormationIText = new TextViewModel(new TextObject("Hello", null));
            this.FormationI = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationI.SelectedIndex = GetIndex(data.InfantryOrder);

            this.FormationIIText = new TextViewModel(new TextObject("Hello", null));
            this.FormationII = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationII.SelectedIndex = GetIndex(data.ArcherOrder);

            this.FormationIIIText = new TextViewModel(new TextObject("Hello", null));
            this.FormationIII = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationIII.SelectedIndex = GetIndex(data.CavalryOrder);

            this.FormationIVText = new TextViewModel(new TextObject("Hello", null));
            this.FormationIV = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationIV.SelectedIndex = GetIndex(data.HorseArcherOrder);

            this.FormationVText = new TextViewModel(new TextObject("Hello", null));
            this.FormationV = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationV.SelectedIndex = GetIndex(data.SkirmisherOrder);

            this.FormationVIText = new TextViewModel(new TextObject("Hello", null));
            this.FormationVI = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationVI.SelectedIndex = GetIndex(data.HeavyInfantryOrder);

            this.FormationVIIText = new TextViewModel(new TextObject("Hello", null));
            this.FormationVII = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationVII.SelectedIndex = GetIndex(data.LightCavalryOrder);

            this.FormationVIIIText = new TextViewModel(new TextObject("Hello", null));
            this.FormationVIII = new SelectorVM<SelectorItemVM>(orders, 0, null);
            this.FormationVIII.SelectedIndex = GetIndex(data.HeavyCavalryOrder);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            //this._sliderValueText = this._sliderValue.ToString();

        }

        private void ExecuteDone()
        {
            CampaignInteraction._inMenu = false;

            StartingOrderData orders = new StartingOrderData()
            {
                //InfantryOrder = GetOrderType(this._formationIOrder),
                InfantryOrder = GetOrderType(FormationI.SelectedIndex),
                ArcherOrder = GetOrderType(FormationII.SelectedIndex),
                CavalryOrder = GetOrderType(FormationIII.SelectedIndex),
                HorseArcherOrder = GetOrderType(FormationIV.SelectedIndex),
                SkirmisherOrder = GetOrderType(FormationV.SelectedIndex),
                HeavyInfantryOrder = GetOrderType(FormationVI.SelectedIndex),
                LightCavalryOrder = GetOrderType(FormationVII.SelectedIndex),
                HeavyCavalryOrder = GetOrderType(FormationVIII.SelectedIndex)
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


        private void ExecuteCancel()
        {
            CampaignInteraction._inMenu = false;
            ScreenManager.PopScreen();
        }

        private void GoToPlanDefinition()
        {
            ScreenManager.PushScreen(new CunningLordsPlanDefinitionScreen());
        }

        private void ExecuteMissionStart()
        {
            if (Game.Current != null)
            {
                InformationManager.DisplayMessage(new InformationMessage("Starting Test Mission!"));
                StartMission();
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("Can only start a Test Mission within campaign!"));
            }
        }

        public void StartMission()
        {
            Clan playerClan = Hero.MainHero.Clan;
            Clan clan = Clan.All.First();

            /*if (playerClan.Leader == clan.Leader)
            {
                clan = Clan.All.Skip(1).First();
            }*/

            Hero bestAvailableCommander = clan.Heroes.First();

            if (bestAvailableCommander == Hero.MainHero)
            {
                bestAvailableCommander = clan.Heroes.Skip(1).First();
            }
            /*CampaignInteraction.healthPreTest = bestAvailableCommander.HitPoints;
            CampaignInteraction.hadMission = true;
            CampaignInteraction.characterToHeal = bestAvailableCommander;*/

            bestAvailableCommander.HitPoints = 50;
            MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
            mobileParty.InitializeMobileParty(
                        GetEnemyParty(),
                        GetEnemyParty(),
                        mobileParty.Position2D,
                        0);
            PlayerEncounter.Start();
            PlayerEncounter.Current.SetupFields(PartyBase.MainParty, mobileParty.Party);
            PlayerEncounter.StartBattle();
            //CampaignMission.OpenBattleMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));
            BattleTestMissionManager.OpenBattleTestMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));
        }

        private TroopRoster GetEnemyParty()
        {
            TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);

            CharacterObject characterObject = CharacterObject.Find("imperial_veteran_infantryman");
            /*if (characterObject != null)
            {
                troopRoster.AddToCounts(characterObject, 2, false, 0, 0, true, -1);

            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("CustomTroopRoster id not found."));
            }*/

            return troopRoster;
        }

        [DataSourceProperty]
        public string CancelText
        {
            get
            {
                return this._cancelText;
            }
        }

        [DataSourceProperty]
        public string DoneText
        {
            get
            {
                return this._doneText;
            }
        }

        [DataSourceProperty]
        public string PressButton1
        {
            get
            {
                return this._pressButton1;
            }
        }

        [DataSourceProperty]
        public string PlanTabText
        {
            get
            {
                return this._planTabText;
            }
        }


        [DataSourceProperty]
        public bool BooleanValue
        {
            get
            {
                return this._booleanValue;
            }
            set
            {
                this._booleanValue = !BooleanValue;
                //InformationManager.DisplayMessage(new InformationMessage("Bool is currently " + this._booleanValue.ToString() + "!"));
            }
        }

        [DataSourceProperty]
        public bool CunningLordsAIActive
        {
            get
            {
                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                string finalPath = Path.Combine(path, "ModuleData", "configData.json");

                CunningLordsConfigData data;
                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer deserializer = new JsonSerializer();
                    data = (CunningLordsConfigData)deserializer.Deserialize(file, typeof(CunningLordsConfigData));
                }

                MissionAI.missionAiActive = data.AIActive;

                return data.AIActive;
            }
            set
            {
                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                string finalPath = Path.Combine(path, "ModuleData", "configData.json");

                CunningLordsConfigData data;
                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer deserializer = new JsonSerializer();
                    data = (CunningLordsConfigData)deserializer.Deserialize(file, typeof(CunningLordsConfigData));
                }

                data.AIActive = !data.AIActive;
                MissionAI.missionAiActive = data.AIActive;

                var serializer = new JsonSerializer();
                using (var sw = new StreamWriter(finalPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, data);
                }
                //InformationManager.DisplayMessage(new InformationMessage("AI is " + data.AIActive.ToString() + "!"));
            }
        }

  

        private string _doneText;
        private string _cancelText;
        private string _pressButton1;
        private string _tab1Text;
        private string _tab2Text;
        private string _sliderText;
        private float _sliderValue;
        private string _sliderValueText;
        private bool _booleanValue;
        private string _planTabText;


        public OrderType GetOrderType(int value)
        {
            if(value == 0)
            {
                return OrderType.Charge;
            }
            else if (value == 1)
            {
                return OrderType.FollowMe;
            }
            else if (value == 2)
            {
                return OrderType.StandYourGround;
            }
            else if (value == 3)
            {
                return OrderType.Advance;
            }
            else if (value == 4)
            {
                return OrderType.FallBack;
            }
            return OrderType.None;
        }

        public int GetIndex(OrderType order)
        {
            switch (order)
            {
                case OrderType.Charge:
                    return 0;
                case OrderType.FollowMe:
                    return 1;
                case OrderType.StandYourGround:
                    return 2;
                case OrderType.Advance:
                    return 3;
                case OrderType.FallBack:
                    return 4;
                default:
                    return 0;
            }
        }

    }
}
