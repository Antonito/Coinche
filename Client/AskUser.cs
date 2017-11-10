using System;
using Coinche.Common.Core.Contract;
using Coinche.Common.Core.Game;
using Coinche.Common.PacketType;

namespace Coinche.Client
{
    public static class AskUser
    {
        public static Promise AskPromise(ContractRequest contract)
        {
            Promise promise = Promise.Points150;
            Console.WriteLine("Choose between the following promise:");
            foreach (Promise e in Enum.GetValues(typeof(Promise)))
            {
                if (e == 0 || e > contract.MinimumValue)
                {
                    Console.WriteLine("(" + (int)e + ")" + " " + e.ToString());
                }
            }

            Console.Write(">");
            // clearing the input buffer before asking something to user
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
            bool success = false;
            string userInput;
            do
            {
                success = Reader.TryReadLine(out userInput, 100);
                if (success)
                {
                    if (Enum.IsDefined(typeof(Promise), Int32.Parse(userInput)))
                    {
                        promise = ((Promise)Int32.Parse(userInput));
                    }
                    else
                    {
                        Console.WriteLine("Wrong choice\n>");
                        success = false;
                    }
                }
            } while (!success && Lobby.IsGameStarted) ;
            return promise;
        }

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

            Console.Write(">");
            bool success = false;
            string userInput;
            do
            {
                success = Reader.TryReadLine(out userInput, 100);
                if (success)
                {
                    if (Enum.IsDefined(typeof(GameMode), Int32.Parse(userInput)))
                    {
                        gameMode = ((GameMode)Int32.Parse(userInput));
                    }
                    else
                    {
                        Console.WriteLine("Wrong choice\n>");
                        success = false;
                    }
                }
            } while (!success && Lobby.IsGameStarted);

            return gameMode;
        }
    }
}
