using System.Collections.Generic;
using ClanBattle.Models;

namespace ClanBattle.Memento
{
    public class GameMemento
    {
        public IDictionary<string, List<IUnit>> ClanUnits { get; }
        public IDictionary<string, IUnit?> Leaders { get; }

        public GameMemento(IDictionary<string, List<IUnit>> clanUnits, IDictionary<string, IUnit?> leaders)
        {
            ClanUnits = clanUnits;
            Leaders = leaders;
        }
    }
}