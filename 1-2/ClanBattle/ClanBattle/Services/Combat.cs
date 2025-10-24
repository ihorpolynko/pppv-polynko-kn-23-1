using System;
using System.Linq;
using ClanBattle.Clans;
using ClanBattle.Models;

namespace ClanBattle.Services
{
    public class CombatService
    {
        private Random _rnd = new Random();

        public void Battle(Clan clan1, Clan clan2)
        {
            Console.WriteLine($"Бій між кланами {clan1.Name} та {clan2.Name} починається!");

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

            double dodgeChance = defender.MoveType switch
            {
                "Run" => 0.2,
                "Walk" => 0.1,
                "Jump" => 0.25,
                _ => 0.0
            };

            int attackerY = (attacker as UnitBase)?.Y ?? 0;
            int defenderY = (defender as UnitBase)?.Y ?? 0;

            double positionAttackBonus = 1.0 + (5 - attackerY) * 0.05;

            double positionDefenseBonus = 1.0 - defenderY * 0.03;

            if (_rnd.NextDouble() < dodgeChance)
            {
                Console.WriteLine($"{defender.Name} ухилився від атаки!");
                return;
            }

            int totalDamage = (int)(baseDamage * weaponMultiplier * positionAttackBonus * positionDefenseBonus);
            defender.Health -= totalDamage;
            if (defender.Health < 0) defender.Health = 0;

            Console.WriteLine($"{attacker.Name} атакує {defender.Name} ({totalDamage} dmg) | Здоров'я {defender.Name}: {defender.Health} | Позиція: ({attackerY}->{defenderY})");
        }
    }
}