using ClanBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClanBattle.Mediator
{
    public class DwarfHandler : UnitHandler
    {
        private Random _rnd = new Random();

        public DwarfHandler(List<IUnit> units) : base(units) { }

        public override void HandleRandomized()
        {
            string[] actions = { "вперед", "назад", "битися" };

            foreach (var unit in Units.Where(u => UnwrapUnitBase(u) is Dwarf).ToList())
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
                        pos.ReceiveHit(18);
                        Console.WriteLine($"{unit.Name} поранений! Статус: {pos.Status}");
                        if (pos.Status == UnitStatus.OutOfBattle)
                            Console.WriteLine($"{unit.Name} вийшов з бою.");
                    }
                    break;
                case "назад":
                    if (pos.Status == UnitStatus.Wounded)
                    {
                        pos.Recover(18);
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

                    var target = Opponents.Where(e => e.Health > 0)
                                          .OrderBy(_ => _rnd.Next())
                                          .First();

                    double damageMod = unit.DamageModifier();
                    double defenseMod = target.DefenseModifier();
                    double dodgeMod = target.DodgeModifier();

                    if (_rnd.NextDouble() < 0.10 * dodgeMod)
                    {
                        Console.WriteLine($"{target.Name} ухилився від атаки!");
                        break;
                    }

                    int baseDamage = _rnd.Next(12, 36);

                    double weaponMultiplier = unit.Weapon switch
                    {
                        "Sword" => 1.0,
                        "Bow" => 0.9,
                        "Axe" => 1.4,
                        _ => 1.0
                    };

                    double rawDamage = baseDamage * weaponMultiplier * damageMod / defenseMod;
                    int totalDamage = Math.Max(1, (int)rawDamage);

                    target.Health -= totalDamage;
                    if (target.Health < 0) target.Health = 0;

                    if (_rnd.NextDouble() < 0.40)
                    {
                        pos.ReceiveHit(22);
                        Console.WriteLine($"{unit.Name} поранений під час бою! Статус: {pos.Status}");
                        if (pos.Status == UnitStatus.OutOfBattle)
                            Console.WriteLine($"{unit.Name} вийшов з бою.");
                    }

                    var tpos = UnwrapUnitBase(target);
                    Console.WriteLine($"{unit.Name} атакує {target.Name} ({totalDamage} dmg) | " +
                                      $"Здоров'я {target.Name}: {target.Health} | " +
                                      $"Позиція: ({pos.X},{pos.Y}) -> ({tpos?.X ?? 0},{tpos?.Y ?? 0})");
                    break;
            }
        }
    }
}