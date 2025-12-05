using System.Collections.Generic;
using System.Linq;
using ClanBattle.Clans;
using ClanBattle.Models;

namespace ClanBattle.Memento
{
    public class CheckpointManager
    {
        private readonly List<GameMemento> _snapshots = new();

        public int Save(List<Clan> clans)
        {
            var clones = new List<Clan>();
            foreach (var clan in clans)
            {
                var clonedUnits = clan.Units.Select(u => u.Clone()).ToList();
                var clonedLeader = clan.Leader?.Clone();
                var cloneClan = new Clan(clan.Name, null);
                cloneClan.RestoreFromSnapshot(clonedUnits, clonedLeader);
                clones.Add(cloneClan);
            }

            _snapshots.Add(new GameMemento(clones));
            return _snapshots.Count - 1;
        }

        public List<Clan> Restore(int id, bool resetForBattle = false)
        {
            if (id < 0 || id >= _snapshots.Count) return null;

            var snapshot = _snapshots[id];
            var restoredClans = new List<Clan>();

            foreach (var clan in snapshot.ClansSnapshot)
            {
                var clonedUnits = clan.Units.Select(u => resetForBattle && u is UnitBase ub
                                                            ? ub.CloneForBattle()
                                                            : u.Clone()).ToList();
                var clonedLeader = resetForBattle && clan.Leader is UnitBase l
                                    ? l.CloneForBattle()
                                    : clan.Leader?.Clone();

                var newClan = new Clan(clan.Name, null);
                newClan.RestoreFromSnapshot(clonedUnits, clonedLeader);

                restoredClans.Add(newClan);
            }

            return restoredClans;
        }

        public int SnapshotsCount => _snapshots.Count;
    }
}