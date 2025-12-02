using System.Collections.Generic;
using System.Linq;
using ClanBattle.Clans;
using ClanBattle.Models;

namespace ClanBattle.Memento
{
    public class CheckpointManager
    {
        private readonly List<GameMemento> _snapshots = new();

        public int Save(IEnumerable<Clan> clans)
        {
            var unitsMap = new Dictionary<string, List<IUnit>>();
            var leadersMap = new Dictionary<string, IUnit?>();

            foreach (var clan in clans)
            {
                var clones = clan.Units.Select(u => u.Clone()).ToList();
                unitsMap[clan.Name] = clones;

                leadersMap[clan.Name] = clan.Leader?.Clone();
            }

            var m = new GameMemento(unitsMap, leadersMap);
            _snapshots.Add(m);
            return _snapshots.Count - 1;
        }

        public bool Restore(IEnumerable<Clan> clans, int id)
        {
            if (id < 0 || id >= _snapshots.Count) return false;
            var m = _snapshots[id];

            foreach (var clan in clans)
            {
                if (m.ClanUnits.TryGetValue(clan.Name, out var savedUnits))
                {
                    var unitsForRestore = savedUnits.Select(u => u.Clone()).ToList();
                    m.Leaders.TryGetValue(clan.Name, out var savedLeader);
                    var leaderForRestore = savedLeader?.Clone();

                    clan.RestoreFromSnapshot(unitsForRestore, leaderForRestore);
                }
            }

            return true;
        }

        public int SnapshotsCount => _snapshots.Count;
    }
}