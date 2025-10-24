using ClanBattle.Services;
using ClanBattle.Factories;

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

            combat.Battle(clan1, clan2);

            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}