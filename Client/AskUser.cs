using System;
using System.Collections.Generic;
using Coinche.Common.Core.Contract;
using Coinche.Common.Core.Game;
using Coinche.Common.PacketType;

namespace Coinche.Client
{
    /// <summary>
    /// Ask user.
    /// </summary>
    public static class AskUser
    {
        /// <summary>
        /// Asks a promise.
        /// </summary>
        /// <returns>The promise.</returns>
        /// <param name="contract">Contract.</param>
        public static Promise AskPromise(ContractRequest contract)
        {
            Promise promise = Promise.Points150;
            Console.WriteLine("Choose between the following promises:");
            List<int> promiseList = new List<int>();
            foreach (Promise e in Enum.GetValues(typeof(Promise)))
            {
                if (contract.MinimumValue == 0 && e != Promise.Coinche
                    && e != Promise.ReCoinche)
                {
                    Console.WriteLine("(" + (int)e + ")" + " " + e.ToString());
                    promiseList.Add((int)e);
                }
                else if (contract.MinimumValue != 0 && !Program.clientInfos.CanRecoinche
                         && e != Promise.ReCoinche)
                {
                    Console.WriteLine("(" + (int)e + ")" + " " + e.ToString());
                    promiseList.Add((int)e);
                }
                else if (Program.clientInfos.CanRecoinche)
                {
                    Console.WriteLine("(" + (int)e + ")" + " " + e.ToString());
                    promiseList.Add((int)e);
                }
            }

            Console.Write("> ");
            // clearing the input buffer before asking something to user
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
            bool success = false;
            do
            {
                success = Reader.TryReadLine(out string userInput, 100);
                if (success)
                {
                    try
                    {
                        if (!Enum.IsDefined(typeof(Promise), Int32.Parse(userInput)))
                        {
                            throw new IndexOutOfRangeException("Invalid promise");
                        }
                        promise = ((Promise)Int32.Parse(userInput));
                        if (!promiseList.Contains((int)promise))
                        {
                            throw new IndexOutOfRangeException("Invalid promise");
                        }
                    }
                    catch (Exception)
                    {
                        Console.Write("Wrong choice\n> ");
                        success = false;                        
                    }
                }
            } while (!success && Lobby.IsGameStarted);
            return promise;
        }

        /// <summary>
        /// Asks the game mode.
        /// </summary>
        /// <returns>The game mode.</returns>
        public static GameMode AskGameMode()
        {
            GameMode gameMode = GameMode.ClassicClover;
            Console.WriteLine("Choose between the following game mode:");
            int menuCount = 0;
            foreach (GameMode e in Enum.GetValues(typeof(GameMode)))
            {
                Console.WriteLine(menuCount + ") " + e.ToString());
                menuCount++;
            }

            Console.Write("> ");
            bool success = false;
            do
            {
                success = Reader.TryReadLine(out string userInput, 100);
                if (success)
                {
                    try
                    {
                        if (!Enum.IsDefined(typeof(GameMode), Int32.Parse(userInput)))
                        {
                            throw new IndexOutOfRangeException("Invalid GameMode");
                        }
                        gameMode = ((GameMode)int.Parse(userInput));
                    }
                    catch (Exception)
                    {
                        Console.Write("Wrong choice\n> ");
                        success = false;
                    }
                }
            } while (!success && Lobby.IsGameStarted);

            return gameMode;
        }

        /// <summary>
        /// Asks the card.
        /// </summary>
        /// <returns>The card.</returns>
        /// <param name="cards">Cards.</param>
        public static int AskCard(List<Card> cards)
        {
            int choice = 0;

            Console.WriteLine("Number of cards: " + cards.Count);
            for (var i = 0; i < cards.Count; ++i)
            {
                Console.WriteLine(i + ") " + cards[i].Value.ToString() + " - " + cards[i].Color.ToString());
            }

            Console.Write(">");
            bool success = false;
            do
            {
                success = Reader.TryReadLine(out string userInput, 100);
                if (success)
                {
                    try
                    {
                        choice = Int32.Parse(userInput);
                        if (choice < 0 || choice >= cards.Count)
                        {
                            throw new IndexOutOfRangeException("Invalid Card");
                        }
                    }
                    catch (Exception)
                    {
                        success = false;
                        Console.Write("Wrong choice\n> ");
                    }
                }
            } while (!success && Lobby.IsGameStarted);
            return choice;
        }
    }
}
