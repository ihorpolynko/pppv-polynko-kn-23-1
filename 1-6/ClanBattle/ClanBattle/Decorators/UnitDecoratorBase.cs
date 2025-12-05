using ClanBattle.Models;

namespace ClanBattle.Decorators
{
    public abstract class UnitDecoratorBase : IUnit
    {
        protected readonly IUnit _unit;

        protected UnitDecoratorBase(IUnit unit)
        {
            _unit = unit;
        }

        public Guid Id => _unit.Id;
        public string Name { get => _unit.Name; set => _unit.Name = value; }
        public string Weapon { get => _unit.Weapon; set => _unit.Weapon = value; }
        public string MoveType { get => _unit.MoveType; set => _unit.MoveType = value; }
        public int Health { get => _unit.Health; set => _unit.Health = value; }
        public int X { get => (_unit as UnitBase)?.X ?? 0; set { if (_unit is UnitBase b) b.X = value; } }
        public int Y { get => (_unit as UnitBase)?.Y ?? 0; set { if (_unit is UnitBase b) b.Y = value; } }

        public virtual IUnit Clone() => _unit.Clone();

        public virtual void Display() => _unit.Display();

        public virtual double DamageModifier() => 1.0;
        public virtual double DodgeModifier() => 1.0;
        public virtual double DefenseModifier() => 1.0;
    }
}