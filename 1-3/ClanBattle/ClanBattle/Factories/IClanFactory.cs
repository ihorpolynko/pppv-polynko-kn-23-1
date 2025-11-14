using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClanBattle.Models;

namespace ClanBattle.Factories
{
    public interface IClanFactory
    {
        IUnit CreateWarrior();
        IUnit CreateElf();
        IUnit CreateDwarf();
    }
}
