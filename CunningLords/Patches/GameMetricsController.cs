using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using CunningLords.PlanDefinition;

namespace CunningLords.Patches
{
    class GameMetricsController
    {
        public void WriteToJson(GameMetricEnum metric)
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "GameMetrics.json");

            GameMetrics data;
            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer deserializer = new JsonSerializer();
                data = (GameMetrics)deserializer.Deserialize(file, typeof(GameMetrics));
            }

            switch (metric)
            {
                case GameMetricEnum.TotalBattles:
                    data.TotalBattles++;
                    break;
                case GameMetricEnum.BattlesUsingNative:
                    data.BattlesUsingNative++;
                    break;
                case GameMetricEnum.BattlesUsingAI:
                    data.BattlesUsingAI++;
                    break;
                case GameMetricEnum.NumberOfLoadoutSaves:
                    data.NumberOfLoadoutSaves++;
                    break;
                case GameMetricEnum.NumberOfLoadoutLoads:
                    data.NumberOfLoadoutLoads++;
                    break;
                case GameMetricEnum.NumberOfTestBattles:
                    data.NumberOfTestBattles++;
                    break;
                case GameMetricEnum.NumberOfPlansActivated:
                    data.NumberOfPlansActivated++;
                    break;
                case GameMetricEnum.NumberOfFieldBattleOrders:
                    data.NumberOfFieldBattleOrders++;
                    break;
            }

            if (data.TakeMetrics)
            {
                var serializer = new JsonSerializer();
                using (var sw = new StreamWriter(finalPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, data);
                }
            }
        }

        public void ChangeUserId(string user)
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "GameMetrics.json");

            GameMetrics data;
            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer deserializer = new JsonSerializer();
                data = (GameMetrics)deserializer.Deserialize(file, typeof(GameMetrics));
            }

            data.UserId = user;

            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(finalPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
