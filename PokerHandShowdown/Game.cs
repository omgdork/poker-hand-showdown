using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class Game
    {
        public Deck Deck { get; set; }
        public List<Player> Players { get; private set; }
        private int CardsPerPlayer { get; set; }
        private HandTypeEvaluator Evaluator { get; set; }

        public Game(string[] players, int cardsPerPlayer = 5)
        {
            Deck = new Deck();
            Evaluator = Evaluate();
            CardsPerPlayer = cardsPerPlayer;
            Players = new List<Player>();

            foreach (string player in players)
            {
                Players.Add(new Player(player));
            }
        }

        /// <summary>
        /// Distributes the cards to the players one card per round.
        /// Determines the players' hand types.
        /// </summary>
        public void Deal()
        {
            // shuffle deck.
            Deck.Shuffle();

            // clear player cards.
            foreach (var player in Players)
            {
                player.ClearCards();
            }

            // deal cards.
            var cardIndex = 0;
            for (var i = 0; i < CardsPerPlayer; i++)
            {
                foreach (var player in Players)
                {
                    player.Cards.Add(Deck.Cards.ElementAt(cardIndex));
                    cardIndex++;
                }

                cardIndex++;
            }

            // determine player hand type.
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(Players[i].Cards);
            }
        }

        /// <summary>
        /// Gets the winners.
        /// </summary>
        /// <returns>List of players with the winning hands.</returns>
        public List<Player> GetWinners()
        {
            // get players with best hand types.
            var playersWithBestHandType = Players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            return Evaluator.GetWinners(playersWithBestHandType.ToList());
        }

        /// <summary>
        /// Set up the evaluators by rank by assigning another evaluator to the NextHandTypeEvaluator.
        /// </summary>
        /// <returns>Initial HandTypeEvaluator.</returns>
        private HandTypeEvaluator Evaluate()
        {
            var flushEvaluator = new FlushEvaluator();
            var threeOfAKindEvaluator = new ThreeOfAKindEvaluator();
            var onePairEvaluator = new OnePairEvaluator();

            flushEvaluator.NextHandTypeEvaluator = threeOfAKindEvaluator;
            threeOfAKindEvaluator.NextHandTypeEvaluator = onePairEvaluator;

            return flushEvaluator;
        }
    }
}
