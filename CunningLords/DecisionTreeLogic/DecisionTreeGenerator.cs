using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.DecisionTreeLogic;
using CunningLords.Patches;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;

namespace CunningLords.DecisionTreeLogic
{
    class DecisionTreeGenerator
    {
        private DecisionTreeNode infantryTree = null;
        private DecisionTreeNode archersTree = null;
        private DecisionTreeNode leftCavalryTree = null;
        private DecisionTreeNode rightCavalryTree = null;
        private DecisionTreeNode HorseArchersTree = null;

        public DecisionTreeGenerator()
        {
            /*string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "DecisionTree.json");

            DecisionTreeJson data;

            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                data = (DecisionTreeJson)serializer.Deserialize(file, typeof(DecisionTreeJson));
            }

            Stack treeStack= new Stack();

            //DecisionTreeNode node = data.RootInfantry;*/
        }

        public bool isLeaf(DecisionTreeJsonNode node) 
        {
            if (node.trueNode == null && node.falseNode == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
