using ClanBattle.Models;

namespace ClanBattle.Factories
{
    public interface IClanFactory
    {
        IUnit CreateWarrior();
        IUnit CreateElf();
        IUnit CreateDwarf();
        IUnit FinalizeUnit(IUnit unit);
    }
}
