using System.Collections.Generic;
using ClanBattle.Clans;

namespace ClanBattle.Memento
{
    public class GameMemento
    {
        // збережені стани кланів
        public List<Clan> ClansSnapshot { get; }

        public GameMemento(List<Clan> clans)
        {
            // приймання списку кланів для збереження їх стану
            ClansSnapshot = clans;
        }
    }
}