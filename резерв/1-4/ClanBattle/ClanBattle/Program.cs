using ClanBattle.Services;
using ClanBattle.Factories;
using ClanBattle.Mediator;

namespace ClanBattle
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.Default;

            var factory = new RandomClanFactory();
            var generator = new ClanGenerator(factory);
            var combat = new CombatService();

            var clan1 = generator.GenerateClan(generator.GenerateRandomClanName());
            var clan2 = generator.GenerateClan(generator.GenerateRandomClanName());

            clan1.Display();
            clan2.Display();

            var mediator = new ClanMediator();
            mediator.RegisterClan(clan1);
            mediator.RegisterClan(clan2);

            Console.WriteLine($"\nКоманда від лідера першого клану ({clan1.Leader.Name}):");
            mediator.SendRandomCommand(clan1.Name);

            Console.WriteLine($"\nКоманда від лідера другого клану ({clan2.Leader.Name}):");
            mediator.SendRandomCommand(clan2.Name);

            combat.Battle(clan1, clan2);

            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}