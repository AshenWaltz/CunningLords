using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using System.IO;
using System.Xml.Serialization;

namespace CunningLords.Patches
{
    //[XmlRoot("CunningLordsCommands")]
    public class Commands
    {
        public static InputKey ShowRelevantData
        {
            get
            {
                return (InputKey)Commands.ShowRelevantData;
            }
        }
    }
}
