using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattle.Models
{
    public class Warrior : UnitBase
    {
        public Warrior()
        {
            Name = "Warrior";
            Weapon = "Sword";
            MoveType = "Run";
            Health = 100;
        }

        public override IUnit Clone()
        {
            return (IUnit)this.MemberwiseClone();
        }
    }
}