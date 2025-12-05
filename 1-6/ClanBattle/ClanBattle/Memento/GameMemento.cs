using System.Collections.Generic;
using ClanBattle.Clans;

namespace ClanBattle.Memento
{
    public class GameMemento
    {
        public List<Clan> ClansSnapshot { get; }

        public GameMemento(List<Clan> clans)
        {
            ClansSnapshot = clans;
        }
    }
}