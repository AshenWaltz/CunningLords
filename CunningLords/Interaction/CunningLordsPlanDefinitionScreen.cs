using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using CunningLords.Interaction;

namespace CunningLords.Interaction
{
    class CunningLordsPlanDefinitionScreen : ScreenBase
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            this._viewModel = new CunningLordsPlanViewModel();
            this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
            this._gauntletLayer.LoadMovie("PlanInterface", this._viewModel);
            this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            base.AddLayer(this._gauntletLayer);
        }

        private GauntletLayer _gauntletLayer;

        private CunningLordsPlanViewModel _viewModel;
    }
}
