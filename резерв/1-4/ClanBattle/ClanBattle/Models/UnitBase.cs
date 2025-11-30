using System;

namespace ClanBattle.Models
{
    public abstract class UnitBase : IUnit
    {
        public string Name { get; set; }
        public string Weapon { get; set; }
        public string MoveType { get; set; }
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public abstract IUnit Clone();

        public virtual void Display()
        {
            Console.WriteLine($"{Name} | Weapon: {Weapon} | Move: {MoveType} | Health: {Health} | Pos: ({X},{Y})");
        }
    }
}