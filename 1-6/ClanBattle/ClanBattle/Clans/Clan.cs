using System;
using System.Collections.Generic;
using ClanBattle.Models;
using ClanBattle.Factories;

namespace ClanBattle.Clans
{
    public class Clan
    {
        public string Name { get; set; }
        public List<IUnit> Units { get; private set; } = new List<IUnit>();
        public IUnit Leader { get; private set; }
        private IClanFactory _factory;
        private Random _rnd = new Random();

        public Clan(string name, IClanFactory factory)
        {
            Name = name;
            _factory = factory;
        }

        public void GenerateRandomUnits()
        {
            int warriorCount = _rnd.Next(1, 5);
            int elfCount = _rnd.Next(1, 5);
            int dwarfCount = _rnd.Next(1, 5);

            void SetRandomPosition(IUnit unit)
            {
                if (unit is UnitBase baseUnit)
                {
                    baseUnit.X = _rnd.Next(0, 10); // 0..9
                    baseUnit.Y = _rnd.Next(0, 6);  // 0..5
                }
                else
                {
                    var inner = GetInnerUnitBase(unit);
                    if (inner != null)
                    {
                        inner.X = _rnd.Next(0, 10);
                        inner.Y = _rnd.Next(0, 6);
                    }
                }
            }

            for (int i = 0; i < warriorCount; i++)
            {
                var w = _factory.CreateWarrior();
                SetRandomPosition(w);
                w = _factory.FinalizeUnit(w);
                Units.Add(w);
            }

            for (int i = 0; i < elfCount; i++)
            {
                var e = _factory.CreateElf();
                SetRandomPosition(e);
                e = _factory.FinalizeUnit(e);
                Units.Add(e);
            }

            for (int i = 0; i < dwarfCount; i++)
            {
                var d = _factory.CreateDwarf();
                SetRandomPosition(d);
                d = _factory.FinalizeUnit(d);
                Units.Add(d);
            }

            int leaderIndex = _rnd.Next(Units.Count);
            var originalLeader = Units[leaderIndex];

            var leaderClone = ClanLeaderManager.Instance.SetLeader(Name, originalLeader);
            Units[leaderIndex] = leaderClone;
            Leader = leaderClone;
        }

        private UnitBase? GetInnerUnitBase(IUnit unit)
        {
            var t = unit.GetType();
            var fi = t.GetField("_inner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fi != null)
            {
                var inner = fi.GetValue(unit) as IUnit;
                if (inner is UnitBase ub) return ub;
                if (inner != null) return GetInnerUnitBase(inner);
            }
            return null;
        }

        // memento
        public void RestoreFromSnapshot(List<IUnit> unitsSnapshot, IUnit? leaderSnapshot)
        {
            Units = new List<IUnit>(unitsSnapshot);

            Leader = leaderSnapshot;
            ClanLeaderManager.Instance.SetLeader(Name, Leader);
        }

        public void Display()
        {
            Console.WriteLine($"=== Клан {Name} ===");
            foreach (var unit in Units)
                unit.Display();

            Console.WriteLine("\nЛідер клану:");
            Leader.Display();
            Console.WriteLine();
        }
    }
}
