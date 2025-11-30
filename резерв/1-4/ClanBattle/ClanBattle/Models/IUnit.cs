using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattle.Models
{
    public interface IUnit
    {
        string Name { get; set; }
        string Weapon { get; set; }
        string MoveType { get; set; }
        int Health { get; set; }
        IUnit Clone(); // для Prototype
        void Display();
    }
}