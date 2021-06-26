using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CunningLords.DecisionTreeLogic
{
    class DecisionTreeJson
    {
        public List<String> NodeList = new List<string>();

        public DecisionTreeJsonNode RootInfantry = null;
        public DecisionTreeJsonNode RootArchers = null;
        public DecisionTreeJsonNode RootCavalry = null;
        public DecisionTreeJsonNode RootHorseArchers = null;
        public DecisionTreeJsonNode RootSkirmishers = null;
        public DecisionTreeJsonNode RootHeavyInfantry = null;
        public DecisionTreeJsonNode RootLightCavalry = null;
        public DecisionTreeJsonNode RootHeavyCavalry = null;
    }
}
