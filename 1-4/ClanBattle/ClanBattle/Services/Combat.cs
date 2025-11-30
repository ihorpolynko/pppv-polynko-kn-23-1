using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using ClanBattle.Clans;
using ClanBattle.Models;

namespace ClanBattle.Services
{
    public class CombatService
    {
        private Random _rnd = new Random();

        public void Battle(Clan clan1, Clan clan2)
        {
            Console.WriteLine($"\n\nБій між кланами {clan1.Name} та {clan2.Name} починається!");

            int round = 1;

            while (clan1.Units.Any(u => u.Health > 0) && clan2.Units.Any(u => u.Health > 0))
            {
                Console.WriteLine($"\n--- Раунд {round} ---");

                var attacker1 = clan1.Units.Where(u => u.Health > 0).OrderBy(u => _rnd.Next()).First();
                var defender2 = clan2.Units.Where(u => u.Health > 0).OrderBy(u => _rnd.Next()).First();

                Attack(attacker1, defender2);

                if (clan2.Units.Any(u => u.Health > 0))
                {
                    var attacker2 = clan2.Units.Where(u => u.Health > 0).OrderBy(u => _rnd.Next()).First();
                    var defender1 = clan1.Units.Where(u => u.Health > 0).OrderBy(u => _rnd.Next()).First();

                    Attack(attacker2, defender1);
                }

                round++;
            }

            var winner = clan1.Units.Any(u => u.Health > 0) ? clan1.Name : clan2.Name;
            Console.WriteLine($"\nБій закінчено! Переміг клан: {winner}");
        }

        private UnitBase? UnwrapUnitBase(IUnit unit)
        {
            if (unit is UnitBase ub) return ub;

            var visited = new HashSet<object?>();
            object? current = unit;

            while (current != null && !visited.Contains(current))
            {
                visited.Add(current);
                var t = current.GetType();

                var prop = t.GetProperty("Inner", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        ?? t.GetProperty("inner", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        ?? t.GetProperty("InnerUnit", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (prop != null && typeof(IUnit).IsAssignableFrom(prop.PropertyType))
                {
                    var val = prop.GetValue(current) as IUnit;
                    if (val is UnitBase ubb) return ubb;
                    current = val;
                    continue;
                }

                var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                bool moved = false;
                foreach (var fi in fields)
                {
                    if (typeof(IUnit).IsAssignableFrom(fi.FieldType))
                    {
                        var val = fi.GetValue(current) as IUnit;
                        if (val is UnitBase ubb) return ubb;
                        current = val;
                        moved = true;
                        break;
                    }
                }

                if (moved) continue;

                break;
            }

            return null;
        }

        private void Attack(IUnit attacker, IUnit defender)
        {
            int baseDamage = _rnd.Next(10, 31);

            double weaponMultiplier = attacker.Weapon switch
            {
                "Sword" => 1.2,
                "Bow" => 1.0,
                "Axe" => 1.5,
                _ => 1.0
            };

            double baseDodgeChance = defender.MoveType switch
            {
                "Run" => 0.2,
                "Walk" => 0.1,
                "Jump" => 0.25,
                _ => 0.0
            };

            var a = UnwrapUnitBase(attacker);
            var d = UnwrapUnitBase(defender);

            int attackerX = a?.X ?? 0;
            int attackerY = a?.Y ?? 0;
            int defenderX = d?.X ?? 0;
            int defenderY = d?.Y ?? 0;

            double positionAttackBonus = 1.0 + (5 - attackerY) * 0.05 + (Math.Abs(defenderX - attackerX) < 3 ? 0.05 : 0.0);

            double positionDodgeMultiplier = 1.0 - (5 - defenderY) * 0.03 - (defenderX * 0.01);
            positionDodgeMultiplier = Math.Max(0.0, positionDodgeMultiplier);

            double dodgeChance = baseDodgeChance * positionDodgeMultiplier;

            if (_rnd.NextDouble() < dodgeChance)
            {
                Console.WriteLine($"{defender.Name} ухилився від атаки!");
                return;
            }

            int totalDamage = (int)(baseDamage * weaponMultiplier * positionAttackBonus);
            if (d != null)
            {
                d.Health -= totalDamage;
                if (d.Health < 0) d.Health = 0;
            }
            else
            {
                defender.Health -= totalDamage;
                if (defender.Health < 0) defender.Health = 0;
            }

            Console.WriteLine($"{attacker.Name} атакує {defender.Name} ({totalDamage} dmg) | Здоров'я {defender.Name}: {(d?.Health ?? defender.Health)} | Позиція: ({attackerX},{attackerY} -> {defenderX},{defenderY})");
        }
    }
}
