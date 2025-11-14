using ClanBattle.Models;

namespace ClanBattle.Decorators
{
    public class ColorDecorator : UnitDecoratorBase
    {
        private readonly string _color;

        public ColorDecorator(IUnit unit, string color) : base(unit)
        {
            _color = color;
            Name += $" ({color})";
        }

        public override double DamageModifier()
        {
            return _color switch
            {
                "Red" => 1.05,
                _ => 1.0
            };
        }

        public override double DodgeModifier()
        {
            return _color switch
            {
                "Blue" => 1.05,
                _ => 1.0
            };
        }

        public override double DefenseModifier()
        {
            return _color switch
            {
                "Green" => 1.10,
                _ => 1.0
            };
        }
    }
}