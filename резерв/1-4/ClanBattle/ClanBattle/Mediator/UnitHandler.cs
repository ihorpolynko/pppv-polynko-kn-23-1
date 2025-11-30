using ClanBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClanBattle.Mediator
{
    public abstract class UnitHandler
    {
        protected List<IUnit> Units;
        protected List<IUnit>? Opponents;
        protected UnitHandler? Next;
        protected IUnit? Leader;

        public UnitHandler(List<IUnit> units)
        {
            Units = units;
        }

        public void SetNext(UnitHandler next)
        {
            Next = next;
        }

        public void SetOpponents(List<IUnit> opponents)
        {
            Opponents = opponents;
            if (Next != null)
                Next.SetOpponents(opponents);
        }

        public void SetLeader(IUnit? leader)
        {
            Leader = leader;
            if (Next != null)
                Next.SetLeader(leader);
        }

        protected UnitBase? UnwrapUnitBase(IUnit unit)
        {
            if (unit is UnitBase ub) return ub;

            var visited = new HashSet<object?>();
            object? current = unit;

            while (current != null && !visited.Contains(current))
            {
                visited.Add(current);
                var t = current.GetType();

                var prop = t.GetProperty("Inner", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        ?? t.GetProperty("inner", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        ?? t.GetProperty("InnerUnit", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (prop != null && typeof(IUnit).IsAssignableFrom(prop.PropertyType))
                {
                    var val = prop.GetValue(current) as IUnit;
                    if (val is UnitBase innerUb) return innerUb;
                    current = val;
                    continue;
                }

                var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                bool moved = false;
                foreach (var fi in fields)
                {
                    if (typeof(IUnit).IsAssignableFrom(fi.FieldType))
                    {
                        var val = fi.GetValue(current) as IUnit;
                        if (val is UnitBase innerUb) return innerUb;
                        current = val;
                        moved = true;
                        break;
                    }
                }

                if (moved) continue;
                break;
            }

            return null;
        }

        protected bool IsSameLogicalUnit(IUnit? a, IUnit? b)
        {
            if (a == null || b == null) return false;
            if (ReferenceEquals(a, b)) return true;
            var ua = UnwrapUnitBase(a);
            var ub = UnwrapUnitBase(b);
            return ua != null && ReferenceEquals(ua, ub);
        }

        public abstract void HandleCommand(string command);

        public abstract void HandleRandomized();
    }
}
