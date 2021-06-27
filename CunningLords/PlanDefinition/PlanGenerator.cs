using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;

namespace CunningLords.PlanDefinition
{
    public class PlanGenerator
    {
        public Plan plan;
        public PlanGenerator()
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "DecisionTree.json");

            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer serializer2 = new JsonSerializer();
                this.plan = (Plan)serializer2.Deserialize(file, typeof(Plan));
            }
        }

        public void Run()
        {

        }

    }
}
