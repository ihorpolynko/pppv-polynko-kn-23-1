using ClanBattle.Decorators;
using ClanBattle.Models;
using System.Xml.Linq;

public class ClothesDecorator : UnitDecoratorBase
{
    private readonly string _clothes;

    public ClothesDecorator(IUnit unit, string clothes) : base(unit)
    {
        _clothes = clothes;
        Name += $" <{clothes}>";
    }

    public override double DefenseModifier()
    {
        return _clothes switch
        {
            "HeavyArmor" => 0.85,
            _ => 1.0
        };
    }

    public override double DodgeModifier()
    {
        return _clothes switch
        {
            "LightArmor" => 1.10,
            _ => 1.0
        };
    }
}