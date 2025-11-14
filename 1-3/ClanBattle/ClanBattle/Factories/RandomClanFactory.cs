using System;
using ClanBattle.Models;
using ClanBattle.Decorators;

namespace ClanBattle.Factories
{
    public class RandomClanFactory : IClanFactory
    {
        private Random _rnd = new Random();
        private int _counter = 0;

        private readonly IUnit _warriorPrototype = new Warrior();
        private readonly IUnit _elfPrototype = new Elf();
        private readonly IUnit _dwarfPrototype = new Dwarf();

        public IUnit CreateWarrior()
        {
            var unit = _warriorPrototype.Clone();
            Customize(unit);
            unit = ApplyRandomDecorators(unit);
            return unit;
        }

        public IUnit CreateElf()
        {
            var unit = _elfPrototype.Clone();
            Customize(unit);
            unit = ApplyRandomDecorators(unit);
            return unit;
        }

        public IUnit CreateDwarf()
        {
            var unit = _dwarfPrototype.Clone();
            Customize(unit);
            unit = ApplyRandomDecorators(unit);
            return unit;
        }

        private void Customize(IUnit unit)
        {
            _counter++;
            unit.Name += $" #{_counter}";

            unit.Health = Math.Max(1, unit.Health + _rnd.Next(-10, 11));

            if (unit is UnitBase baseUnit)
            {
                baseUnit.X = 0;
                baseUnit.Y = 0;
            }
        }

        private IUnit ApplyRandomDecorators(IUnit unit)
        {
            string[] colors = { "Red", "Blue", "Green" };
            string[] heights = { "Short", "Medium", "Tall" };
            string[] clothes = { "HeavyArmor", "LightArmor", "Robe" };

            unit = new ColorDecorator(unit, colors[_rnd.Next(colors.Length)]);
            unit = new HeightDecorator(unit, heights[_rnd.Next(heights.Length)]);
            unit = new ClothesDecorator(unit, clothes[_rnd.Next(clothes.Length)]);

            return unit;
        }
    }
}
