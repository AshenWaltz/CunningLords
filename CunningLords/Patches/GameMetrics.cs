using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CunningLords.Patches
{
    class GameMetrics
    {
        public string UserId = "";

        public int TotalBattles = 0;

        public int BattlesUsingNative = 0;

        public int BattlesUsingAI = 0;

        public int NumberOfLoadoutSaves = 0;

        public int NumberOfLoadoutLoads = 0;

        public int NumberOfTestBattles = 0;

        public int NumberOfPlansActivated = 0;

        public int NumberOfFieldBattleOrders = 0;

        public bool TakeMetrics = true;
    }
}
