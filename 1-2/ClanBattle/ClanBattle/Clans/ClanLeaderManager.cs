using System;
using System.Collections.Generic;
using ClanBattle.Models;

namespace ClanBattle.Clans
{
    public class ClanLeaderManager
    {
        private static ClanLeaderManager? _instance;
        private readonly Dictionary<string, IUnit> _leaders = new();

        private ClanLeaderManager() { }

        public static ClanLeaderManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClanLeaderManager();
                return _instance;
            }
        }

        public IUnit SetLeader(string clanName, IUnit leader)
        {
            var leaderCopy = leader.Clone();
            if (leaderCopy is UnitBase baseUnit)
            {
                baseUnit.Health = (int)(baseUnit.Health * 1.5);
                baseUnit.Name = "[LEADER] " + baseUnit.Name;
            }
            _leaders[clanName] = leaderCopy;
            return leaderCopy;
        }

        public IUnit? GetLeader(string clanName)
        {
            return _leaders.TryGetValue(clanName, out var l) ? l : null;
        }
    }
}