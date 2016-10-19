using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerHandShowdown;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new string[3] { "Joe", "Jen", "Bob" };
            Play(players);
        }

        static void Play(string[] players)
        {
            var game = new Game(players);

            // show player cards.
            game.Deal();
            foreach (var player in game.Players)
            {
                Console.WriteLine(player.Name + "'s cards: " + player.HandType);

                foreach (var card in player.Cards)
                {
                    Console.WriteLine(card.CardValue + " " + card.Suit);
                }

                Console.WriteLine();
            }

            Console.WriteLine("Press any key to determine the winners.");
            Console.ReadKey();

            // show winners.
            var winners = game.GetWinners();
            var winnerString = new StringBuilder("Winners:");
            foreach (var winner in winners)
            {
                winnerString.AppendLine(winner.Name + " " + winner.HandType);
            }
            Console.WriteLine(winnerString);

            Console.WriteLine("Play again? (Y/n)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.WriteLine();
                Play(players);
            }
        }
    }
}
