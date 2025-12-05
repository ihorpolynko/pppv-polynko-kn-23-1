using System;
using System.Collections.Generic;
using ClanBattle.Services;
using ClanBattle.Factories;
using ClanBattle.Mediator;
using ClanBattle.Memento;
using ClanBattle.Clans;

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
            var checkpoint = new CheckpointManager();

            List<Clan> clans = null;

            while (true)
            {
                if (clans == null) // новая игра
                {
                    var clan1 = generator.GenerateClan(generator.GenerateRandomClanName());
                    var clan2 = generator.GenerateClan(generator.GenerateRandomClanName());
                    clans = new List<Clan> { clan1, clan2 };
                }

                ClanMediator mediator = CreateAndRegisterMediator(clans);

                foreach (var clan in clans)
                    clan.Display();

                const string mainMenu = "\nМеню: (S)ave чекпоінт, (L)ist чекпоінтів, (R)estore чекпоінт, (C)ommands (лідери), (B)attle, (Q)uit";

                var exitApp = false;
                while (true)
                {
                    Console.WriteLine(mainMenu);
                    Console.Write("\nОберіть дію: ");
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.S)
                    {
                        int id = checkpoint.Save(clans);
                        Console.WriteLine($"Чекпоінт збережений: id = {id}");
                    }
                    else if (key == ConsoleKey.L)
                    {
                        Console.WriteLine($"Чекпоінтів: {Math.Max(0, checkpoint.SnapshotsCount)} (останній id = {Math.Max(0, checkpoint.SnapshotsCount - 1)})");
                    }
                    else if (key == ConsoleKey.R)
                    {
                        if (checkpoint.SnapshotsCount == 0)
                        {
                            Console.WriteLine("Нема збережених чекпоінтів.");
                            continue;
                        }

                        Console.Write("\nВведіть id для відновлення: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            var restored = checkpoint.Restore(id, resetForBattle: true);
                            if (restored != null)
                            {
                                clans = restored;
                                mediator = CreateAndRegisterMediator(clans);
                                Console.WriteLine($"Відновлення стану з чекпоінта {id}.\n");
                                foreach (var c in clans) c.Display();
                            }
                            else
                            {
                                Console.WriteLine("Неправильний id.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неправильнe введення.");
                        }
                    }
                    else if (key == ConsoleKey.C)
                    {
                        Console.WriteLine($"\nКоманда від лідера першого клану ({clans[0].Leader.Name}):");
                        mediator.SendRandomCommand(clans[0].Name);

                        Console.WriteLine($"\nКоманда від лідера другого клану ({clans[1].Leader.Name}):");
                        mediator.SendRandomCommand(clans[1].Name);
                    }
                    else if (key == ConsoleKey.B)
                    {
                        Console.WriteLine("\nЗапуск бою...");
                        combat.Battle(clans[0], clans[1]);

                        var continueGame = PostBattleMenu(clans, checkpoint, ref mediator);
                        if (!continueGame)
                        {
                            exitApp = true;
                            break;
                        }
                        else
                        {
                            clans = null; // нова гра --> обнуляємо клани
                            break;
                        }
                    }
                    else if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("Вихід.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Невідома команда.");
                    }
                }

                if (exitApp) break;
            }

            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }

        private static bool PostBattleMenu(List<Clan> clans, CheckpointManager checkpoint, ref ClanMediator mediator)
        {
            const string postMenu = "\n=== Пост-бій меню ===\n(N)ew game, (Q)uit";

            while (true)
            {
                Console.WriteLine(postMenu);
                Console.Write("\nОберіть дію: ");
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.N)
                {
                    return true;
                }
                else if (key == ConsoleKey.Q)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Невідома команда.");
                }
            }
        }

        private static ClanMediator CreateAndRegisterMediator(List<Clan> clans)
        {
            var mediator = new ClanMediator();
            foreach (var clan in clans)
                mediator.RegisterClan(clan);
            return mediator;
        }
    }
}