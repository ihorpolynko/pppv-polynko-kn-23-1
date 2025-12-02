using ClanBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClanBattle.Mediator
{
    public class ElfHandler : UnitHandler
    {
        private Random _rnd = new Random();

        public ElfHandler(List<IUnit> units) : base(units) { }

        public override void HandleCommand(string command)
        {
            foreach (var unit in Units.Where(u => UnwrapUnitBase(u) is Elf).ToList())
            {
                if (IsSameLogicalUnit(unit, Leader))
                    continue;

                ProcessUnitAction(unit, command);
            }

            Next?.HandleCommand(command);
        }

        public override void HandleRandomized()
        {
            string[] actions = { "вперед", "назад", "битися" };

            foreach (var unit in Units.Where(u => UnwrapUnitBase(u) is Elf).ToList())
            {
                if (IsSameLogicalUnit(unit, Leader))
                    continue;

                var action = actions[_rnd.Next(actions.Length)];
                ProcessUnitAction(unit, action);
            }

            Next?.HandleRandomized();
        }

        private void ProcessUnitAction(IUnit unit, string action)
        {
            var pos = UnwrapUnitBase(unit);
            if (pos == null) return;
            if (pos.Status == UnitStatus.OutOfBattle) return;

            switch (action)
            {
                case "вперед":
                    pos.X = Math.Min(9, pos.X + 1);
                    Console.WriteLine($"{unit.Name} йде вперед! Нова позиція: ({pos.X},{pos.Y})");
                    if (_rnd.NextDouble() < 0.25)
                    {
                        pos.ReceiveHit(12);
                        Console.WriteLine($"{unit.Name} поранений! Статус: {pos.Status}");
                        if (pos.Status == UnitStatus.OutOfBattle)
                            Console.WriteLine($"{unit.Name} вийшов з бою.");
                    }
                    break;
                case "назад":
                    if (pos.Status == UnitStatus.Wounded)
                    {
                        pos.Recover(15);
                        Console.WriteLine($"{unit.Name} відновився після відступу. Здоров'я: {pos.Health}");
                    }
                    else
                    {
                        pos.X = Math.Max(0, pos.X - 1);
                        Console.WriteLine($"{unit.Name} відступає! Нова позиція: ({pos.X},{pos.Y})");
                    }
                    break;
                case "битися":
                    if (Opponents == null || !Opponents.Any(e => e.Health > 0))
                    {
                        Console.WriteLine($"{unit.Name} хоче атакувати, але немає живих ворогів.");
                        break;
                    }

                    var target = Opponents.Where(e => e.Health > 0).OrderBy(_ => _rnd.Next()).First();

                    int baseDamage = _rnd.Next(8, 26);
                    double weaponMultiplier = unit.Weapon switch
                    {
                        "Sword" => 1.0,
                        "Bow" => 1.3,
                        "Axe" => 1.2,
                        _ => 1.0
                    };
                    int totalDamage = (int)(baseDamage * weaponMultiplier);

                    target.Health -= totalDamage;
                    if (target.Health < 0) target.Health = 0;

                    if (_rnd.NextDouble() < 0.35)
                    {
                        pos.ReceiveHit(18);
                        Console.WriteLine($"{unit.Name} поранений під час бою! Статус: {pos.Status}");
                        if (pos.Status == UnitStatus.OutOfBattle)
                            Console.WriteLine($"{unit.Name} вийшов з бою.");
                    }

                    var tpos = UnwrapUnitBase(target);
                    Console.WriteLine($"{unit.Name} атакує {target.Name} ({totalDamage} dmg) | Здоров'я {target.Name}: {target.Health} | Позиція: ({pos.X},{pos.Y}) -> ({tpos?.X ?? 0},{tpos?.Y ?? 0})");
                    break;
            }
        }
    }
}