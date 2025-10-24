using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattle.Models
{
    public class Elf : UnitBase
    {
        public Elf()
        {
            Name = "Elf";
            Weapon = "Bow";
            MoveType = "Jump";
            Health = 80;
        }

        public override IUnit Clone()
        {
            return (IUnit)this.MemberwiseClone();
        }
    }
}