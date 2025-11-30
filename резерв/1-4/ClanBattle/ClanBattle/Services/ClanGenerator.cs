using System;
using ClanBattle.Clans;
using ClanBattle.Factories;

namespace ClanBattle.Services
{
    public class ClanGenerator
    {
        private IClanFactory _factory;
        private Random _rnd = new Random();

        public ClanGenerator(IClanFactory factory)
        {
            _factory = factory;
        }

        public Clan GenerateClan(string name)
        {
            var clan = new Clan(name, _factory);
            clan.GenerateRandomUnits();
            return clan;
        }

        public string GenerateRandomClanName()
        {
            string[] names = { "Орки", "Ельфи", "Гноми", "Люди", "Варвари" };
            return names[_rnd.Next(names.Length)];
        }
    }
}