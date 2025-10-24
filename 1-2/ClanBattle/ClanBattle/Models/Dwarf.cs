using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattle.Models
{
    public class Dwarf : UnitBase
    {
        public Dwarf()
        {
            Name = "Dwarf";
            Weapon = "Axe";
            MoveType = "Walk";
            Health = 120;
        }

        public override IUnit Clone()
        {
            return (IUnit)this.MemberwiseClone();
        }
    }
}