using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace CunningLords.Interaction
{
    public class CunningLordsTroopSupplier : IMissionTroopSupplier
    {
        public CunningLordsTroopSupplier(PartyBase cunningLordsBattleCombatant, bool isPlayerSide)
        {
            this._cunningLordsBattleCombatant = cunningLordsBattleCombatant;
            this._isPlayerSide = isPlayerSide;
            this.ArrangePriorities();
        }

		private void ArrangePriorities()
		{
			this._characters = new PriorityQueue<float, BasicCharacterObject>(new GenericComparer<float>());
			int[] array = new int[8];
			int i;
			int j;
			for (i = 0; i < 8; i = j + 1)
			{
				int count = 0;
                foreach (TroopRosterElement tre in this._cunningLordsBattleCombatant.MemberRoster.GetTroopRoster())
                {
                    if (tre.Character.DefaultFormationClass == (FormationClass)i)
                    {
						count++;
                    }
                }
				array[i] = count;
				j = i;
			}
			int num = 1000;
			foreach (TroopRosterElement tre in this._cunningLordsBattleCombatant.MemberRoster.GetTroopRoster())
			{
				BasicCharacterObject basicCharacterObject = tre.Character;
				FormationClass formationClass = basicCharacterObject.GetFormationClass(this._cunningLordsBattleCombatant);
				int num2;
				if (!basicCharacterObject.IsHero)
				{
					num2 = array[(int)formationClass] / array.Sum();
				}
				else
				{
					num = (num2 = num) - 1;
				}
				int num3 = num2;
				this._characters.Enqueue((float)num3, basicCharacterObject);
				array[(int)formationClass]--;
			}
		}

		public IEnumerable<IAgentOriginBase> SupplyTroops(int numberToAllocate)
		{
			List<BasicCharacterObject> list = this.AllocateTroops(numberToAllocate);
			CunningLordsAgentOrigin[] array = new CunningLordsAgentOrigin[list.Count];
			this._numAllocated += list.Count;
			for (int i = 0; i < array.Length; i++)
			{
				UniqueTroopDescriptor uniqueNo = new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed);
				array[i] = new CunningLordsAgentOrigin(this._cunningLordsBattleCombatant, list[i], this, this._isPlayerSide, i, uniqueNo);
			}
			if (array.Length < numberToAllocate)
			{
				this._anyTroopRemainsToBeSupplied = false;
			}
			return array;
		}

		private List<BasicCharacterObject> AllocateTroops(int numberToAllocate)
		{
			if (numberToAllocate > this._characters.Count)
			{
				numberToAllocate = this._characters.Count;
			}
			List<BasicCharacterObject> list = new List<BasicCharacterObject>();
			for (int i = 0; i < numberToAllocate; i++)
			{
				list.Add(this._characters.DequeueValue());
			}
			return list;
		}

		public void OnTroopWounded()
		{
			this._numWounded++;
		}

		public void OnTroopKilled()
		{
			this._numKilled++;
		}

		public void OnTroopRouted()
		{
			this._numRouted++;
		}

		public int NumActiveTroops
		{
			get
			{
				return this._numAllocated - (this._numWounded + this._numKilled + this._numRouted);
			}
		}

		public int NumRemovedTroops
		{
			get
			{
				return this._numWounded + this._numKilled + this._numRouted;
			}
		}

		public int NumTroopsNotSupplied
		{
			get
			{
				return this._characters.Count - this._numAllocated;
			}
		}

		public bool AnyTroopRemainsToBeSupplied
		{
			get
			{
				return this._anyTroopRemainsToBeSupplied;
			}
		}


        private PriorityQueue<float, BasicCharacterObject> _characters;
        private PartyBase _cunningLordsBattleCombatant;
        private bool _isPlayerSide;
        private int _numAllocated;
        private int _numKilled;
        private int _numRouted;
        private int _numWounded;
		private bool _anyTroopRemainsToBeSupplied = true;

    }
}
