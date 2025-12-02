using System;

namespace ClanBattle.Models
{
    public enum UnitStatus
    {
        Active,
        Wounded,
        OutOfBattle
    }

    public abstract class UnitBase : IUnit
    {
        public string Name { get; set; }
        public string Weapon { get; set; }
        public string MoveType { get; set; }
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int HitsTaken { get; private set; } = 0;
        public UnitStatus Status { get; private set; } = UnitStatus.Active;

        public abstract IUnit Clone();

        public void ReceiveHit(int damage = 20)
        {
            if (Status == UnitStatus.OutOfBattle) return;

            HitsTaken++;
            Health = Math.Max(0, Health - damage);

            if (HitsTaken >= 2)
            {
                Status = UnitStatus.OutOfBattle;
                Health = 0;
            }
            else
            {
                Status = UnitStatus.Wounded;
                if (Health <= 0) Health = 1;
            }
        }

        public void Recover(int healAmount = 20)
        {
            if (Status == UnitStatus.Wounded)
            {
                HitsTaken = 0;
                Status = UnitStatus.Active;
                Health += healAmount;
            }
        }

        public virtual void Display()
        {
            string statusTag = Status switch
            {
                UnitStatus.Active => "",
                UnitStatus.Wounded => " [WOUNDED]",
                UnitStatus.OutOfBattle => " [OUT]",
                _ => ""
            };

            Console.WriteLine($"{Name}{statusTag} | Weapon: {Weapon} | Move: {MoveType} | Health: {Health} | Pos: ({X},{Y})");
        }
    }
}