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

namespace CunningLords.Interaction
{
    internal class CunningLordsMenuViewModel : ViewModel
    {
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

            this._formationIOrder = GetFloatValue(data.InfantryOrder);
            this._formationIOrderString = getString(this._formationIOrder);

            this._formationIIOrder = GetFloatValue(data.ArcherOrder);
            this._formationIIOrderString = getString(this._formationIIOrder);

            this._formationIIIOrder = GetFloatValue(data.CavalryOrder);
            this._formationIIIOrderString = getString(this._formationIIIOrder);

            this._formationIVOrder = GetFloatValue(data.HorseArcherOrder);
            this._formationIVOrderString = getString(this._formationIVOrder);

            this._formationVOrder = GetFloatValue(data.SkirmisherOrder);
            this._formationVOrderString = getString(this._formationVOrder);

            this._formationVIOrder = GetFloatValue(data.HeavyInfantryOrder);
            this._formationVIOrderString = getString(this._formationVIOrder);

            this._formationVIIOrder = GetFloatValue(data.LightCavalryOrder);
            this._formationVIIOrderString = getString(this._formationVIIOrder);

            this._formationVIIIOrder = GetFloatValue(data.HeavyCavalryOrder);
            this._formationVIIIOrderString = getString(this._formationVIIIOrder);

        }
        public override void RefreshValues()
        {
            base.RefreshValues();
            this.SliderValueText = this.SliderValue.ToString();
            //this._sliderValueText = this._sliderValue.ToString();

        }

        private void ExecuteDone()
        {
            CampaignInteraction._inMenu = false;

            StartingOrderData orders = new StartingOrderData()
            {
                InfantryOrder = getOrderType(this._formationIOrder),
                ArcherOrder = getOrderType(this._formationIIOrder),
                CavalryOrder = getOrderType(this._formationIIIOrder),
                HorseArcherOrder = getOrderType(this._formationIVOrder),
                SkirmisherOrder = getOrderType(this._formationVOrder),
                HeavyInfantryOrder = getOrderType(this._formationVIOrder),
                LightCavalryOrder = getOrderType(this._formationVIIOrder),
                HeavyCavalryOrder = getOrderType(this._formationVIIIOrder)
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

        private void ExecutePressButton1()
        {

        }

        private void ExecuteTab1()
        {

        }

        private void ExecuteTab2()
        {
        }

        public void SetActiveState(bool isActive)
        {
        }

        [DataSourceProperty]
        public float SliderValue
        {
            get
            {
                return this._sliderValue;
            }
            set
            {
                if (SliderValue != this._sliderValue)
                {
                    //this._sliderValueText = this._sliderValue.ToString();
                    this._sliderValue = SliderValue;
                    this.SliderValueText = SliderValue.ToString();
                    base.OnPropertyChanged("SliderValue");
                }

            }
        }

        [DataSourceProperty]
        public string SliderText
        {
            get
            {
                return this._sliderText;
            }
        }

        [DataSourceProperty]
        public string SliderValueText
        {
            get
            {
                return this._sliderValueText;
            }
            set
            {
                if (SliderValueText != this._sliderValueText)
                {
                    this._sliderValueText = SliderValueText;
                    InformationManager.DisplayMessage(new InformationMessage("Text Should Change!"));
                    base.OnPropertyChanged("SliderValueText");
                }
            }
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
        public string Tab1Text
        {
            get
            {
                return this._tab1Text;
            }
        }

        [DataSourceProperty]
        public string Tab2Text
        {
            get
            {
                return this._tab2Text;
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

        [DataSourceProperty]
        public float FormationIOrder
        {
            get
            {
                return this._formationIOrder;
            }
            set
            {
                bool flag = value != this._formationIOrder;
                if (flag)
                {
                    this._formationIOrder = value;
                    this.FormationIOrderString = getString(this.FormationIOrder);
                    base.OnPropertyChanged("FormationIOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current infantry order is " + this.FormationIOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationIOrderString
        {
            get
            {
                return this._formationIOrderString;
            }
            set
            {
                bool flag = value != this._formationIOrderString;
                if (flag)
                {
                    this._formationIOrderString = value;
                    base.OnPropertyChanged("FormationIOrderString");
                }
            }
        }

        [DataSourceProperty]
        public float FormationIIOrder
        {
            get
            {
                return this._formationIIOrder;
            }
            set
            {
                bool flag = value != this._formationIIOrder;
                if (flag)
                {
                    this._formationIIOrder = value;
                    this.FormationIIOrderString = getString(this.FormationIIOrder);
                    base.OnPropertyChanged("FormationIIOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current archer order is " + this.FormationIIOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationIIOrderString
        {
            get
            {
                return this._formationIIOrderString;
            }
            set
            {
                bool flag = value != this._formationIIOrderString;
                if (flag)
                {
                    this._formationIIOrderString = value;
                    base.OnPropertyChanged("FormationIIOrderString");
                }
            }
        }

        [DataSourceProperty]
        public float FormationIIIOrder
        {
            get
            {
                return this._formationIIIOrder;
            }
            set
            {
                bool flag = value != this._formationIIIOrder;
                if (flag)
                {
                    this._formationIIIOrder = value;
                    this.FormationIIIOrderString = getString(this.FormationIIIOrder);
                    base.OnPropertyChanged("FormationIIIOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current cavalry order is " + this.FormationIIIOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationIIIOrderString
        {
            get
            {
                return this._formationIIIOrderString;
            }
            set
            {
                bool flag = value != this._formationIIIOrderString;
                if (flag)
                {
                    this._formationIIIOrderString = value;
                    base.OnPropertyChanged("FormationIIIOrderString");
                }
            }
        }

        [DataSourceProperty]
        public float FormationIVOrder
        {
            get
            {
                return this._formationIVOrder;
            }
            set
            {
                bool flag = value != this._formationIVOrder;
                if (flag)
                {
                    this._formationIVOrder = value;
                    this.FormationIVOrderString = getString(this.FormationIVOrder);
                    base.OnPropertyChanged("FormationIVOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current cavalry order is " + this.FormationIVOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationIVOrderString
        {
            get
            {
                return this._formationIVOrderString;
            }
            set
            {
                bool flag = value != this._formationIVOrderString;
                if (flag)
                {
                    this._formationIVOrderString = value;
                    base.OnPropertyChanged("FormationIVOrderString");
                }
            }
        }

        [DataSourceProperty]
        public float FormationVOrder
        {
            get
            {
                return this._formationVOrder;
            }
            set
            {
                bool flag = value != this._formationVOrder;
                if (flag)
                {
                    this._formationVOrder = value;
                    this.FormationVOrderString = getString(this.FormationVOrder);
                    base.OnPropertyChanged("FormationVOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current cavalry order is " + this.FormationVOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationVOrderString
        {
            get
            {
                return this._formationVOrderString;
            }
            set
            {
                bool flag = value != this._formationVOrderString;
                if (flag)
                {
                    this._formationVOrderString = value;
                    base.OnPropertyChanged("FormationVOrderString");
                }
            }
        }

        [DataSourceProperty]
        public float FormationVIOrder
        {
            get
            {
                return this._formationVIOrder;
            }
            set
            {
                bool flag = value != this._formationVIOrder;
                if (flag)
                {
                    this._formationVIOrder = value;
                    this.FormationVIOrderString = getString(this.FormationVIOrder);
                    base.OnPropertyChanged("FormationVIOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current cavalry order is " + this.FormationVIOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationVIOrderString
        {
            get
            {
                return this._formationVIOrderString;
            }
            set
            {
                bool flag = value != this._formationVIOrderString;
                if (flag)
                {
                    this._formationVIOrderString = value;
                    base.OnPropertyChanged("FormationVIOrderString");
                }
            }
        }

        [DataSourceProperty]
        public float FormationVIIOrder
        {
            get
            {
                return this._formationVIIOrder;
            }
            set
            {
                bool flag = value != this._formationVIIOrder;
                if (flag)
                {
                    this._formationVIIOrder = value;
                    this.FormationVIIOrderString = getString(this.FormationVIIOrder);
                    base.OnPropertyChanged("FormationVIIOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current cavalry order is " + this.FormationVIIOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationVIIOrderString
        {
            get
            {
                return this._formationVIIOrderString;
            }
            set
            {
                bool flag = value != this._formationVIIOrderString;
                if (flag)
                {
                    this._formationVIIOrderString = value;
                    base.OnPropertyChanged("FormationVIIOrderString");
                }
            }
        }

        [DataSourceProperty]
        public float FormationVIIIOrder
        {
            get
            {
                return this._formationVIIIOrder;
            }
            set
            {
                bool flag = value != this._formationVIIIOrder;
                if (flag)
                {
                    this._formationVIIIOrder = value;
                    this.FormationVIIIOrderString = getString(this.FormationVIIIOrder);
                    base.OnPropertyChanged("FormationVIIIOrder");
                }

                //InformationManager.DisplayMessage(new InformationMessage("Current cavalry order is " + this.FormationVIIIOrderString + "!"));
            }
        }

        [DataSourceProperty]
        public string FormationVIIIOrderString
        {
            get
            {
                return this._formationVIIIOrderString;
            }
            set
            {
                bool flag = value != this._formationVIIIOrderString;
                if (flag)
                {
                    this._formationVIIIOrderString = value;
                    base.OnPropertyChanged("FormationVIIIOrderString");
                }
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

        private float _formationIOrder;
        private string _formationIOrderString;
        private float _formationIIOrder;
        private string _formationIIOrderString;
        private float _formationIIIOrder;
        private string _formationIIIOrderString;
        private float _formationIVOrder;
        private string _formationIVOrderString;
        private float _formationVOrder;
        private string _formationVOrderString;
        private float _formationVIOrder;
        private string _formationVIOrderString;
        private float _formationVIIOrder;
        private string _formationVIIOrderString;
        private float _formationVIIIOrder;
        private string _formationVIIIOrderString;

        public float GetFloatValue(OrderType order)
        {
            switch (order)
            {
                case OrderType.Charge:
                    return 10.0f;
                case OrderType.FollowMe:
                    return 40.0f;
                case OrderType.StandYourGround:
                    return 70.0f;
                case OrderType.AdvanceTenPaces:
                    return 100.0f;
                case OrderType.FallBackTenPaces:
                    return 130.0f;
                case OrderType.Advance:
                    return 160.0f;
                case OrderType.FallBack:
                    return 190.0f;
                default:
                    return 0.0f;
            }
        }

        public OrderType getOrderType(float value)
        {
            if(value >= 0.0f && value < 30.0f)
            {
                return OrderType.Charge;
            }
            else if (value >= 30.0f && value < 60.0f)
            {
                return OrderType.FollowMe;
            }
            else if (value >= 60.0f && value < 90.0f)
            {
                return OrderType.StandYourGround;
            }
            else if (value >= 90.0f && value < 120.0f)
            {
                return OrderType.AdvanceTenPaces;
            }
            else if (value >= 120.0f && value < 150.0f)
            {
                return OrderType.FallBackTenPaces;
            }
            else if (value >= 150.0f && value < 180.0f)
            {
                return OrderType.Advance;
            }
            else if (value >= 180.0f && value <= 200.0f)
            {
                return OrderType.FallBack;
            }
            return OrderType.None;
        }

        public string getString(float value)
        {
            if (value >= 0.0f && value < 30.0f)
            {
                return "Charge";
            }
            else if (value >= 30.0f && value < 60.0f)
            {
                return "Follow Me";
            }
            else if (value >= 60.0f && value < 90.0f)
            {
                return "Hold";
            }
            else if (value >= 90.0f && value < 120.0f)
            {
                return "Flank";
            }
            else if (value >= 120.0f && value < 150.0f)
            {
                return "Skirmish";
            }
            else if (value >= 150.0f && value < 180.0f)
            {
                return "Advance";
            }
            else if (value >= 180.0f && value <= 200.0f)
            {
                return "Fallback";
            }
            return "None";
        }

    }
}
