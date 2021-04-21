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
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
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

        public void SaveOffsets(Mission mission, int index)
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "data.json");

            List<PositionData> deserialized = Deserialize(finalPath);

            PositionData newData = GetCurrentPositionData(mission);
            deserialized[index] = newData;
            Serialize(deserialized, finalPath);

            InformationManager.DisplayMessage(new InformationMessage("Saved to index " + index.ToString()));
        }

        public PositionData GetCurrentPositionData(Mission mission)
        {
            PositionData result = new PositionData();

            Formation mainFormation = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);

            foreach(Formation f in mission.MainAgent.Team.Formations)
            {
                if(f.FormationIndex == FormationClass.Ranged)
                {
                    result.ArchersXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.ArchersYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;
                }
                else if (f.FormationIndex == FormationClass.Cavalry)
                {
                    result.CavalryXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.CavalryYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;
                }
                else if (f.FormationIndex == FormationClass.HorseArcher)
                {
                    result.HorseArchersXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.HorseArchersYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;
                }
            }

            return result;
        }

        public void ApplyPosition(Mission mission, int index)
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "data.json");

            List<PositionData> deserialized = Deserialize(finalPath);

            Formation mainFormation = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);

            foreach (Formation f in mission.MainAgent.Team.Formations)
            {
                if (f.FormationIndex == FormationClass.Ranged)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].ArchersXOffset, deserialized[index].ArchersYOffset);
                    f.MovementOrder = MovementOrder.MovementOrderMove(position);
                }
                else if (f.FormationIndex == FormationClass.Cavalry)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].CavalryXOffset, deserialized[index].CavalryYOffset);
                    f.MovementOrder = MovementOrder.MovementOrderMove(position);
                }
                else if (f.FormationIndex == FormationClass.HorseArcher)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].HorseArchersXOffset, deserialized[index].HorseArchersYOffset);
                    f.MovementOrder = MovementOrder.MovementOrderMove(position);
                }
            }

            /*PositionData data = new PositionData();
            PositionData data2 = new PositionData
            {
                ArchersXOffset = 0f,
                ArchersYOffset = 0f,
                CavalryXOffset = 0f,
                CavalryYOffset = 0f,
                HorseArchersXOffset = 0f,
                HorseArchersYOffset = 0f
            };
            PositionData data3 = new PositionData
            {
                ArchersXOffset = 0f,
                ArchersYOffset = 0f,
                CavalryXOffset = 0f,
                CavalryYOffset = 0f,
                HorseArchersXOffset = 0f,
                HorseArchersYOffset = 0f
            };

            List<PositionData> _data = new List<PositionData>
            {
                data,
                data2,
                data3
            };

            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "data.json");

            Serialize(_data, finalPath);

            var deserialized = Deserialize(finalPath);

            InformationManager.DisplayMessage(new InformationMessage(deserialized.ToString()));*/
        }

        public void Serialize(List<PositionData> data, string finalPath)
        {
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(finalPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data);
            }
        }

        public List<PositionData> Deserialize(string path)
        {
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<PositionData> data = (List<PositionData>)serializer.Deserialize(file, typeof(List<PositionData>));
                return data;
            }
        }

        private static WorldPosition CalculateWorldpositionsBasedOnOffset(Formation formation, Mission __instance, float XOffset, float YOffset)
        {
            float num = formation.Direction.x * YOffset;
            float num2 = formation.Direction.y * YOffset;
            float num3 = formation.Direction.y * XOffset;
            float num4 = -formation.Direction.x * XOffset;
            Vec2 currentPosition = formation.CurrentPosition;
            Vec3 position = new Vec3(currentPosition.X + num + num3, currentPosition.Y + num2 + num4, 0f, -1f);
            WorldPosition result = new WorldPosition(__instance.Scene, position);
            return result;
        }

        private static Vec2 CalculateXYOffsetsBasedOnWorldPosition(Formation mainFormation, Formation formation)
        {
            double num = (double)formation.CurrentPosition.X;
            double num2 = (double)formation.CurrentPosition.Y;
            double num3 = (double)mainFormation.CurrentPosition.X;
            double num4 = (double)mainFormation.CurrentPosition.Y;
            double num5 = Math.Atan2((double)mainFormation.Direction.Y, (double)mainFormation.Direction.X);
            num -= num3;
            num2 -= num4;
            double num6 = num * Math.Sin(num5) - num2 * Math.Cos(num5);
            double num7 = num * Math.Cos(num5) + num2 * Math.Sin(num5);
            return new Vec2((float)num6, (float)num7);
        }

        public void SetInitialFormationorders(Mission mission)
        {
            //ToDo: 
            //Create a function which receives a formation and a Ordertype and applies that ordertype -> OrderController.SetOrder()
            //Create a Json which can contain the order
            //Apply it on first Tick
        }
    }
}
