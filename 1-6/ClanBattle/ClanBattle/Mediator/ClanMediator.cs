using System.Collections.Generic;
using System.Linq;
using ClanBattle.Clans;
using ClanBattle.Models;

namespace ClanBattle.Mediator
{
    public enum SquadType { Warriors, Elves, Dwarves, All }

    public class ClanMediator
    {
        private record RegisteredClan(string Name, WarriorHandler Warrior, ElfHandler Elf, DwarfHandler Dwarf, List<IUnit> Units);

        private readonly List<RegisteredClan> _registered = new();

        public void RegisterClan(Clan clan)
        {
            var warrior = new WarriorHandler(clan.Units);
            var elf = new ElfHandler(clan.Units);
            var dwarf = new DwarfHandler(clan.Units);

            warrior.SetNext(elf);
            elf.SetNext(dwarf);

            warrior.SetLeader(clan.Leader);
            elf.SetLeader(clan.Leader);
            dwarf.SetLeader(clan.Leader);

            _registered.Add(new RegisteredClan(clan.Name, warrior, elf, dwarf, clan.Units));

            RecomputeOpponents();
        }

        private void RecomputeOpponents()
        {
            foreach (var entry in _registered)
            {
                var opponents = _registered
                    .Where(e => !ReferenceEquals(e, entry))
                    .SelectMany(e => e.Units)
                    .ToList();

                entry.Warrior.SetOpponents(opponents);
                entry.Elf.SetOpponents(opponents);
                entry.Dwarf.SetOpponents(opponents);
            }
        }

        public void SendRandomCommand(string clanName)
        {
            var entry = _registered.FirstOrDefault(e => e.Name == clanName);
            if (entry != null)
                entry.Warrior.HandleRandomized();
        }
    }
}