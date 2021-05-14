using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;


namespace CunningLords.Interaction
{
    public class CunningLordsAgentOrigin : IAgentOriginBase
    {
        public PartyBase CunningLordsCombatant { get; private set; }

		IBattleCombatant IAgentOriginBase.BattleCombatant
		{
			get
			{
				return this.CunningLordsCombatant;
			}
		}

		public BasicCharacterObject Troop { get; private set; }

		public int Rank { get; private set; }

		public Banner Banner
		{
			get
			{
				return this.CunningLordsCombatant.Banner;
			}
		}

		public bool IsUnderPlayersCommand
		{
			get
			{
				return this._isPlayerSide;
			}
		}

		public uint FactionColor
		{
			get
			{
				return this.CunningLordsCombatant.BasicCulture.Color;
			}
		}

		public uint FactionColor2
		{
			get
			{
				return this.CunningLordsCombatant.BasicCulture.Color2;
			}
		}

		public int Seed
		{
			get
			{
				return this.Troop.GetDefaultFaceSeed(this.Rank);
			}
		}

		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		public CunningLordsAgentOrigin(PartyBase cunningLordsCombatant, BasicCharacterObject characterObject, CunningLordsTroopSupplier troopSupplier, bool isPlayerSide, int rank = -1, UniqueTroopDescriptor uniqueNo = default(UniqueTroopDescriptor))
		{
			this.CunningLordsCombatant = cunningLordsCombatant;
			this.Troop = characterObject;
			this._descriptor = ((!uniqueNo.IsValid) ? new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed) : uniqueNo);
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._troopSupplier = troopSupplier;
			this._isPlayerSide = isPlayerSide;
		}

		public void SetWounded()
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopWounded();
				this._isRemoved = true;
			}
		}

		public void SetKilled()
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopKilled();
				this._isRemoved = true;
			}
		}

		public void SetRouted()
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopRouted();
				this._isRemoved = true;
			}
		}

		public void OnAgentRemoved(float agentHealth)
		{
		}

		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		public void SetBanner(Banner banner)
		{
			throw new NotImplementedException();
		}

		private readonly UniqueTroopDescriptor _descriptor;

		private readonly bool _isPlayerSide;

		private CunningLordsTroopSupplier _troopSupplier;

		private bool _isRemoved;
	}
}
