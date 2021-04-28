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
            this._formationIOrderString = getOrderType(this._formationIOrder).ToString();

            this._formationIIOrder = GetFloatValue(data.ArcherOrder);
            this._formationIIOrderString = getOrderType(this._formationIIOrder).ToString();

            this._formationIIIOrder = GetFloatValue(data.CavalryOrder);
            this._formationIIIOrderString = getOrderType(this._formationIIIOrder).ToString();

            this._formationIVOrder = GetFloatValue(data.HorseArcherOrder);
            this._formationIVOrderString = getOrderType(this._formationIVOrder).ToString();

            this._formationVOrder = GetFloatValue(data.SkirmisherOrder);
            this._formationVOrderString = getOrderType(this._formationVOrder).ToString();

            this._formationVIOrder = GetFloatValue(data.HeavyInfantryOrder);
            this._formationVIOrderString = getOrderType(this._formationVIOrder).ToString();

            this._formationVIIOrder = GetFloatValue(data.LightCavalryOrder);
            this._formationVIIOrderString = getOrderType(this._formationVIIOrder).ToString();

            this._formationVIIIOrder = GetFloatValue(data.HeavyCavalryOrder);
            this._formationVIIIOrderString = getOrderType(this._formationVIIIOrder).ToString();

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
                ArcherOrder = OrderType.Advance,
                CavalryOrder = OrderType.Advance,
                HorseArcherOrder = OrderType.Advance,
                SkirmisherOrder = OrderType.Advance,
                HeavyInfantryOrder = OrderType.Advance,
                LightCavalryOrder = OrderType.Advance,
                HeavyCavalryOrder = OrderType.Advance
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
                /*string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                string finalPath = Path.Combine(path, "ModuleData", "startdata.json");

                StartingOrderData data;

                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    data = (StartingOrderData)serializer.Deserialize(file, typeof(StartingOrderData));
                }
                return GetFloatValue(data.InfantryOrder);*/
                return this._formationIOrder;
            }
            set
            {
                /*string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                string finalPath = Path.Combine(path, "ModuleData", "startdata.json");

                StartingOrderData data;

                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer deserializer = new JsonSerializer();
                    data = (StartingOrderData)deserializer.Deserialize(file, typeof(StartingOrderData));
                }

                //this.FormationIOrder = value;

                data.InfantryOrder = getOrderType(this.FormationIOrder);

                var serializer = new JsonSerializer();
                using (var sw = new StreamWriter(finalPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, data);
                }*/

                bool flag = value != this._formationIOrder;
                if (flag)
                {
                    this._formationIOrder = value;
                    this._formationIOrderString = getOrderType(this.FormationIOrder).ToString();
                    base.OnPropertyChanged("BuyThresholdValue");
                }

                InformationManager.DisplayMessage(new InformationMessage("Current infantry order is " + this.FormationIOrderString + "!"));
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
                    base.OnPropertyChanged("BuyThresholdValueAsString");
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

    }
}
