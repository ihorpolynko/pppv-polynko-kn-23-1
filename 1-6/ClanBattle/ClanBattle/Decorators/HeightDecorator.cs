using ClanBattle.Decorators;
using ClanBattle.Models;
using System.Xml.Linq;

public class HeightDecorator : UnitDecoratorBase
{
    private readonly string _height;

    public HeightDecorator(IUnit unit, string height) : base(unit)
    {
        _height = height;
        Name += $" [{height}]";
    }

    public override double DamageModifier()
    {
        return _height == "Tall" ? 1.10 : 1.0;
    }

    public override double DodgeModifier()
    {
        return _height == "Short" ? 1.10 : 1.0;
    }
}