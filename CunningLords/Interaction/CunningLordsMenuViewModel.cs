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
                "Skirmish",
                "Flank",
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

        private void ExecuteMissionStart()
        {
            if (Game.Current != null)
            {
                InformationManager.DisplayMessage(new InformationMessage("Starting Test Mission!"));
                //StartMission();
                StartSandBoxMission();
                //StartOwnMission();
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("Can only start a Test Mission within campaign!"));
            }
        }

        private void StartMission()
        {
            string scene = "battle_terrain_001";

            if (Game.Current == null)
            {
                return;
            }

            //BasicCharacterObject character = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_1");
            BasicCharacterObject character = Game.Current.PlayerTroop;

            //Creating playerTeam
            BasicCultureObject culture = Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire");
            CustomBattleCombatant playerParty = new CustomBattleCombatant(culture.Name, culture, Banner.CreateRandomBanner());
            playerParty.Side = BattleSideEnum.Attacker;
            playerParty.AddCharacter(character, 1);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_veteran_infantryman"), 5);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_archer"), 5);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_heavy_horseman"), 5);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("bucellarii"), 5);

            //Creating EnemyTeam
            //BasicCharacterObject enemyCharacter = MBObjectManager.Instance.GetObject<BasicCharacterObject>("commander_6");
            CustomBattleCombatant enemyParty = new CustomBattleCombatant(culture.Name, culture, Banner.CreateRandomBanner());
            enemyParty.Side = BattleSideEnum.Defender;
            //enemyParty.AddCharacter(enemyCharacter, 1);
            enemyParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_veteran_infantryman"), 1);

            bool isPlayerGeneral = true;
            BasicCharacterObject playerSideGeneralCharacter = null;
            String sceneLevels = "";
            string seasonString = "summer";
            float timeOfDay = 6f;

            BannerlordMissions.OpenCustomBattleMission(scene, character, playerParty, enemyParty, isPlayerGeneral, playerSideGeneralCharacter,
                                                       sceneLevels, seasonString, timeOfDay);
        }

        private AtmosphereInfo GetAtmosphereInfo()
        {
            string atmosphereName = "field_battle";
            int season = 0;

            return new AtmosphereInfo
            {
                AtmosphereName = atmosphereName,
                TimeInfo = new TimeInformation
                {
                    Season = season
                }
            };
        }

        private Mission StartOwnMission()
        {
            string scene = "battle_terrain_001";

            if (Game.Current == null)
            {
                return null;
            }

            BasicCharacterObject character = Game.Current.PlayerTroop;

            //Creating playerTeam
            BasicCultureObject culture = Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire");
            CustomBattleCombatant playerParty = new CustomBattleCombatant(culture.Name, culture, Banner.CreateRandomBanner());
            playerParty.Side = BattleSideEnum.Attacker;
            playerParty.AddCharacter(character, 1);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_veteran_infantryman"), 5);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_archer"), 5);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_heavy_horseman"), 5);
            playerParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("bucellarii"), 5);

            //Creating EnemyTeam
            CustomBattleCombatant enemyParty = new CustomBattleCombatant(culture.Name, culture, Banner.CreateRandomBanner());
            enemyParty.Side = BattleSideEnum.Defender;
            enemyParty.AddCharacter(MBObjectManager.Instance.GetObject<BasicCharacterObject>("imperial_veteran_infantryman"), 1);

            bool isPlayerGeneral = true;
            BasicCharacterObject playerSideGeneralCharacter = null;
            String sceneLevels = "";
            string seasonString = "summer";
            float timeOfDay = 6f;

            BattleSideEnum playerSide = playerParty.Side;
            bool isPlayerAttacker = playerSide == BattleSideEnum.Attacker;
            IMissionTroopSupplier[] troopSuppliers = new IMissionTroopSupplier[2];
            CustomBattleTroopSupplier customBattleTroopSupplier = new CustomBattleTroopSupplier(playerParty, true);
            troopSuppliers[(int)playerParty.Side] = customBattleTroopSupplier;
            CustomBattleTroopSupplier customBattleTroopSupplier2 = new CustomBattleTroopSupplier(enemyParty, false);
            troopSuppliers[(int)enemyParty.Side] = customBattleTroopSupplier2;
            bool isPlayerSergeant = !isPlayerGeneral;

            return MissionState.OpenNew("MyBattle", new MissionInitializerRecord(scene)
            {
                DoNotUseLoadingScreen = false,
                PlayingInCampaignMode = false,
                AtmosphereOnCampaign = GetAtmosphereInfo(),
                SceneLevels = sceneLevels,
                TimeOfDay = timeOfDay
            }, delegate (Mission mission)
            {
                MissionBehaviour[] array = new MissionBehaviour[22];
                array[0] = new MissionOptionsComponent();
                array[1] = new BattleEndLogic();
                array[2] = new MissionCombatantsLogic(null, playerParty, (!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty, Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
                array[3] = new MissionDefaultCaptainAssignmentLogic();
                array[4] = new BattleObserverMissionLogic();
                array[5] = new CustomBattleAgentLogic();
                array[6] = new MissionAgentSpawnLogic(troopSuppliers, playerSide);
                array[7] = new CustomBattleMissionSpawnHandler((!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty);
                array[8] = new AgentBattleAILogic();
                array[9] = new AgentVictoryLogic();
                array[10] = new MissionAgentPanicHandler();
                array[11] = new MissionHardBorderPlacer();
                array[12] = new MissionBoundaryPlacer();
                array[13] = new MissionBoundaryCrossingHandler();
                array[14] = new BattleMissionAgentInteractionLogic();
                array[15] = new FieldBattleController();
                //array[16] = new AgentFadeOutLogic();
                array[17] = new AgentMoraleInteractionLogic();
                array[18] = new AssignPlayerRoleInTeamMissionController(isPlayerGeneral, isPlayerSergeant, false, isPlayerSergeant ? Enumerable.Repeat<string>(character.StringId, 1).ToList<string>() : new List<string>(), FormationClass.NumberOfRegularFormations);
                array[19] = new CreateBodyguardMissionBehavior((isPlayerAttacker & isPlayerGeneral) ? character.GetName().ToString() : ((isPlayerAttacker & isPlayerSergeant) ? playerSideGeneralCharacter.GetName().ToString() : null), (!isPlayerAttacker & isPlayerGeneral) ? character.GetName().ToString() : ((!isPlayerAttacker & isPlayerSergeant) ? playerSideGeneralCharacter.GetName().ToString() : null), null, null, true);
                array[20] = new HighlightsController();
                array[21] = new BattleHighlightsController();
                return array;
            }, true, true);
        }

        private Mission StartSandBoxMission()
        {
            /*string scene = "battle_terrain_001";
            Campaign camp = Campaign.Current;
            MobileParty mob= MobileParty.MainParty;
            InformationManager.DisplayMessage(new InformationMessage(camp.ToString()));
            
            SandBox.SandBoxMissions.OpenBattleMission(scene);*/

            MissionInitializerRecord rec = new MissionInitializerRecord(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));
            rec.TerrainType = (int)Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace);
            rec.DamageToPlayerMultiplier = Campaign.Current.Models.DifficultyModel.GetDamageToPlayerMultiplier();
            //rec.DamageToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetDamageToFriendsMultiplier();
            rec.NeedsRandomTerrain = false;
            rec.PlayingInCampaignMode = true;
            rec.RandomTerrainSeed = MBRandom.RandomInt(10000);
            rec.AtmosphereOnCampaign = Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(CampaignTime.Now, MobileParty.MainParty.GetLogicalPosition());
            rec.TimeOfDay = Campaign.CurrentTime % 24f;

            Campaign camp = Campaign.Current;
            MobileParty mob = MobileParty.MainParty;
            InformationManager.DisplayMessage(new InformationMessage(camp.ToString()));

            PartyBase party = new PartyBase(new MobileParty());

            CharacterObject recruit = MBObjectManager.Instance.GetObject<CharacterObject>("imperial_veteran_infantryman");
            party.AddMember(recruit, 1, 0);

            InformationManager.DisplayMessage(new InformationMessage("Blyat!"));


            PartyBase playerParty = PartyBase.MainParty;
            //playerParty.Side = BattleSideEnum.Attacker;
            BattleSideEnum playerSide = playerParty.Side; //might give trouble
            bool isPlayerAttacker = playerSide == BattleSideEnum.Attacker;
            BasicCharacterObject playerSideGeneralCharacter = null;
            BasicCharacterObject character = Game.Current.PlayerTroop;
            bool isPlayerGeneral = true;
            bool isPlayerSergeant = !isPlayerGeneral;

            PartyBase enemyParty = new PartyBase(new MobileParty());
            enemyParty.AddMember(MBObjectManager.Instance.GetObject<CharacterObject>("imperial_veteran_infantryman"), 10,0);

            IMissionTroopSupplier[] troopSuppliers = new IMissionTroopSupplier[2];
            CunningLordsTroopSupplier cunningLordsTroopSupplier = new CunningLordsTroopSupplier(playerParty, true);
            troopSuppliers[(int)BattleSideEnum.Attacker] = cunningLordsTroopSupplier;
            CunningLordsTroopSupplier cunningLordsTroopSupplier2 = new CunningLordsTroopSupplier(enemyParty, false);
            troopSuppliers[(int)BattleSideEnum.Defender] = cunningLordsTroopSupplier2;


            List<PartyBase> involvedParties = new List<PartyBase>()
            {
                playerParty,
                enemyParty
            };

            return MissionState.OpenNew("Battle", rec, delegate (Mission mission)
            {
                MissionBehaviour[] array = new MissionBehaviour[27];
                array[0] = new MissionOptionsComponent();
                array[1] = new CampaignMissionComponent();
                array[2] = new BattleEndLogic();
                array[3] = new MissionCombatantsLogic(involvedParties, playerParty, enemyParty, playerParty, Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
                array[4] = new MissionDefaultCaptainAssignmentLogic();
                array[5] = new BattleMissionStarterLogic();
                array[6] = new BattleSpawnLogic("battle_set");
                array[7] = new AgentBattleAILogic();
                array[8] = new MissionAgentSpawnLogic(troopSuppliers, playerSide);
                array[9] = new BaseMissionTroopSpawnHandler();
                //array[10] = new AgentFadeOutLogic();
                array[11] = new BattleObserverMissionLogic();
                array[12] = new BattleAgentLogic();
                array[13] = new MountAgentLogic();
                array[14] = new AgentVictoryLogic();
                array[15] = new MissionDebugHandler();
                array[16] = new MissionAgentPanicHandler();
                array[17] = new MissionHardBorderPlacer();
                array[18] = new MissionBoundaryPlacer();
                array[19] = new MissionBoundaryCrossingHandler();
                array[20] = new BattleMissionAgentInteractionLogic();
                array[21] = new FieldBattleController();
                array[22] = new AgentMoraleInteractionLogic();
                array[23] = new HighlightsController();
                array[24] = new BattleHighlightsController();
                array[25] = new AssignPlayerRoleInTeamMissionController(isPlayerGeneral, isPlayerSergeant, false, isPlayerSergeant ? Enumerable.Repeat<string>(character.StringId, 1).ToList<string>() : new List<string>(), FormationClass.NumberOfRegularFormations);
                array[26] = new CreateBodyguardMissionBehavior((isPlayerAttacker & isPlayerGeneral) ? character.GetName().ToString() : ((isPlayerAttacker & isPlayerSergeant) ? playerSideGeneralCharacter.GetName().ToString() : null), (!isPlayerAttacker & isPlayerGeneral) ? character.GetName().ToString() : ((!isPlayerAttacker & isPlayerSergeant) ? playerSideGeneralCharacter.GetName().ToString() : null), null, null, true);
                return array;
            }, true, true);
        }

        private static MissionAgentSpawnLogic CreateMissionAgentSpawnLogic()
        {
            return new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
            {
                new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null),
                new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null)
            }, PartyBase.MainParty.Side);
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
        public bool BooleanValue
        {
            get
            {
                return this._booleanValue;
            }
            set
            {
                this._booleanValue = !BooleanValue;
                InformationManager.DisplayMessage(new InformationMessage("Bool is currently " + this._booleanValue.ToString() + "!"));
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
                InformationManager.DisplayMessage(new InformationMessage("AI is " + data.AIActive.ToString() + "!"));
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
                return OrderType.AdvanceTenPaces;
            }
            else if (value == 4)
            {
                return OrderType.FallBackTenPaces;
            }
            else if (value == 5)
            {
                return OrderType.Advance;
            }
            else if (value == 6)
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
                case OrderType.AdvanceTenPaces:
                    return 3;
                case OrderType.FallBackTenPaces:
                    return 4;
                case OrderType.Advance:
                    return 5;
                case OrderType.FallBack:
                    return 6;
                default:
                    return 0;
            }
        }

    }
}
