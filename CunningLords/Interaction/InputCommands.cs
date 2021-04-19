using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.Patches;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Path = System.IO.Path;
using System.Reflection;

namespace CunningLords.Interaction
{
    class InputCommands
    {
        public void ApplyActiontoFormation(Mission mission)
        {

            InformationManager.DisplayMessage(new InformationMessage("Infantry will now advance"));

            Team playerTeam = mission.MainAgent.Team;

            IEnumerable<Formation> playerFormations = playerTeam.FormationsIncludingSpecial;

            foreach(Formation f in playerFormations)
            {
                if(f.FormationIndex == FormationClass.Infantry)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.Normalized() * 10f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.MovementOrder = MovementOrder.MovementOrderMove(position);
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
            }
        }

        public void ApplyOnStartPositions(Mission mission) 
        {
            InformationManager.DisplayMessage(new InformationMessage("Infantry will now advance"));

            Team playerTeam = mission.MainAgent.Team;

            IEnumerable<Formation> playerFormations = playerTeam.FormationsIncludingSpecial;

            foreach (Formation f in playerFormations)
            {
                if (f.FormationIndex == FormationClass.Cavalry)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.LeftVec().Normalized() * 20f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.MovementOrder = MovementOrder.MovementOrderMove(position);
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
                else if (f.FormationIndex == FormationClass.Ranged)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.Normalized() * -20f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.MovementOrder = MovementOrder.MovementOrderMove(position);
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
                else if (f.FormationIndex == FormationClass.HorseArcher)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.RightVec().Normalized() * 20f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.MovementOrder = MovementOrder.MovementOrderMove(position);
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
            }
        }

        public void ApplyPosition(Mission mission)
        {
            PositionData data = new PositionData();

            List<PositionData> _data = new List<PositionData>
            {
                data
            };

            string json = JsonSerializer.Serialize(_data);

            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "data.json");

            //string finalPath = "C:/Program Files (x86)/Steam/steamapps/common/Mount & Blade II Bannerlord/Modules/CunningLords/ModuleData/data.json";

            File.WriteAllText(finalPath, json);
        }
    }
}
