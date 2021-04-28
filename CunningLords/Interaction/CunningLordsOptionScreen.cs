using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using CunningLords.Interaction;

namespace CunningLords.Interaction
{
    class CunningLordsOptionScreen : ScreenBase
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            this._viewModel = new CunningLordsMenuViewModel();
            this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
            this._gauntletLayer.LoadMovie("SimpleInterface", this._viewModel);
            this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            base.AddLayer(this._gauntletLayer);
        }

        private GauntletLayer _gauntletLayer;

        private CunningLordsMenuViewModel _viewModel;
    }
}
