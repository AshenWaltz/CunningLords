using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;
using CunningLords.Interaction;
using HarmonyLib;

namespace CunningLords
{
    public class SubModule : MBSubModuleBase
    {
		protected override void OnSubModuleLoad()
		{
			Harmony harmony = new Harmony("mod.ashenwaltz.bannerlord.cunninglords");
			harmony.PatchAll();

			base.OnSubModuleLoad();

            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("TestMainMenuOption",
                new TextObject("Cunning Lords Options"), 3,
                () =>
                {
                    ScreenManager.PushScreen(new CunningLordsOptionScreen());
                }, () => (false, new TextObject("Cunning Lords Options"))));

            /*Module.CurrentModule.AddInitialStateOption(new InitialStateOption("TestMainMenuPlanOption",
                new TextObject("Cunning Lords Plan Defenition"), 3,
                () =>
                {
                    ScreenManager.PushScreen(new CunningLordsPlanDefinitionScreen());
                }, () => (false, new TextObject("Cunning Lords Plan Defenition"))));*/
        }
	}
}