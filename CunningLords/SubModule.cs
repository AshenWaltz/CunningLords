﻿using TaleWorlds.MountAndBlade;
using HarmonyLib;

namespace CunningLords
{
    public class SubModule : MBSubModuleBase
    {
		protected override void OnSubModuleLoad()
		{
			Harmony harmony = new Harmony("mod.ashenwaltz.bannerlord.cunninglords");
			harmony.PatchAll();
		}
	}
}