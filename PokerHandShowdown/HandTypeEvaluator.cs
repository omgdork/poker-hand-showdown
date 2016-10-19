using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public abstract class HandTypeEvaluator
    {
        public abstract HandType HandType { get; }

        public HandTypeEvaluator NextHandTypeEvaluator { get; set; }

        /// <summary>
        /// Calls the actual evaluation method DoEvaluate to determine the hand type of the player.
        /// If initial evaluation returns false, checks the evaluator for the next hand type rank.
        /// If still false until no next hand type rank evaluator is set, return high card hand type.
        /// </summary>
        /// <param name="cards">The player's cards.</param>
        /// <returns>The hand type of the player.</returns>
        public HandType Evaluate(IEnumerable<Card> cards)
        {
            if (DoEvaluate(cards))
            {
                return HandType;
            }

            if (NextHandTypeEvaluator == null)
            {
                return HandType.HighCard;
            }

            return NextHandTypeEvaluator.Evaluate(cards);
        }

        public abstract Boolean DoEvaluate(IEnumerable<Card> cards);

        public abstract List<Player> GetWinners(List<Player> players);
    }

    /// <summary>
    /// Evaluates hand for flush.
    /// </summary>
    public class FlushEvaluator : HandTypeEvaluator
    {
        public override HandType HandType
        {
            get { return HandType.Flush; }
        }

        /// <summary>
        /// Check if the player has a flush.
        /// </summary>
        /// <param name="cards">The player's cards.</param>
        /// <returns>True if player has a flush, else false.</returns>
        public override Boolean DoEvaluate(IEnumerable<Card> cards)
        {
            return cards
                .GroupBy(card => card.Suit)
                .Any(group => group.Count() == 5);
        }
        
        /// <summary>
        /// Compares flushes between players with the same hand type.
        /// </summary>
        /// <param name="players">List of players with flushes.</param>
        /// <returns>List of players; multiple players if there's a draw.</returns>
        public override List<Player> GetWinners(List<Player> players)
        {
            if (players.First().HandType == HandType)
            {
                var winners = new List<Player>();
                for (var i = 0; i < players.First().Cards.Count; i++)
                {
                    var highestCurrentCardValue = players.Max(player => player.Cards[i].CardValue);
                    winners = players.Where(player => player.Cards[i].CardValue == highestCurrentCardValue).ToList();

                    if (winners.Count() == 1)
                    {
                        break;
                    }
                }

                return winners;
            }

            return NextHandTypeEvaluator.GetWinners(players);
        }
    }
    
    /// <summary>
    /// Evaluates hand for three of a kind.
    /// </summary>
    public class ThreeOfAKindEvaluator : HandTypeEvaluator
    {
        public override HandType HandType
        {
            get { return HandType.ThreeOfAKind; }
        }

        /// <summary>
        /// Check if the player has three of a kind.
        /// </summary>
        /// <param name="cards">The player's cards.</param>
        /// <returns>True if player has three of a kind, else false.</returns>
        public override bool DoEvaluate(IEnumerable<Card> cards)
        {
            return cards
                .GroupBy(card => card.CardValue)
                .Any(group => group.Count() == 3);
        }

        /// <summary>
        /// Compares three of a kind value between players with the
        /// same hand type.
        /// </summary>
        /// <param name="players">List of players with three of a kind.</param>
        /// <returns>List containing a single player.</returns>
        public override List<Player> GetWinners(List<Player> players)
        {
            if (players.First().HandType == HandType)
            {
                var highestThreeOfAKindValue = players.Max(player => player.Cards.GroupBy(card => card.CardValue)
                    .Where(group => group.Count() == 3)
                    .Select(group => group.Key)
                    .OrderByDescending(group => group)
                    .FirstOrDefault());

                return players.Where(player => player.Cards.Where(card => card.CardValue == highestThreeOfAKindValue)
                    .Select(cards => cards).Count() == 3).ToList();
            }

            return NextHandTypeEvaluator.GetWinners(players);
        }
    }

    /// <summary>
    /// Evaluates hand for one pair. 
    /// </summary>
    public class OnePairEvaluator : HandTypeEvaluator
    {
        public override HandType HandType
        {
            get { return HandType.OnePair; }
        }

        /// <summary>
        /// Check if the player has a pair.
        /// </summary>
        /// <param name="cards">The player's cards.</param>
        /// <returns>True if player has a pair, else false.</returns>
        public override bool DoEvaluate(IEnumerable<Card> cards)
        {
            return cards
                .GroupBy(card => card.CardValue)
                .Any(group => group.Count() == 2);
        }

        /// <summary>
        /// Compares the remaining three cards when there is more than
        /// one, i.e. two, players with the same pair value.
        /// </summary>
        /// <param name="players">List of players either all with pairs or all with high cards.</param>
        /// <returns>List of players; multiple players if there's a draw.</returns>
        public override List<Player> GetWinners(List<Player> players)
        {
            var winners = new List<Player>();

            if (players.First().HandType == HandType)
            {
                var highestPairValue = players.Max(player => player.Cards.GroupBy(card => card.CardValue)
                    .Where(group => group.Count() == 2)
                    .Select(pair => pair.Key)
                    .OrderByDescending(pairValue => pairValue)
                    .FirstOrDefault());

                winners = players.Where(player => player.Cards.Where(card => card.CardValue == highestPairValue)
                    .Select(cards => cards).Count() == 2).ToList();

                if (winners.Count > 1)
                {
                    // clone list so cards aren't removed from the actual winners list.
                    var tempWinnersList = winners.Select(player => new Player(player.Name)
                    {
                        Cards = player.Cards.Select(card => new Card(card.Suit, card.CardValue)).ToList(),
                        HandType = player.HandType
                    });

                    // take out the pair from the list of cards.
                    foreach (var player in tempWinnersList)
                    {
                        var pairValue = player.Cards.Where(card => card.CardValue == highestPairValue).Take(2).ToList();
                        pairValue.ForEach(card => player.Cards.Remove(card));
                    }

                    // compare the remaining three cards.
                    for (var i = 0; i < tempWinnersList.First().Cards.Count; i++)
                    {
                        var highestCurrentCardValue = tempWinnersList.Max(player => player.Cards[i].CardValue);
                        winners = winners.Where(player => player.Cards[i].CardValue == highestCurrentCardValue).ToList();

                        if (winners.Count() == 1)
                        {
                            break;
                        }
                    }
                }
            }
            else // if hand type isn't one pair, evaluate for high card.
            {
                for (var i = 0; i < players.First().Cards.Count; i++)
                {
                    var highestCurrentCardValue = players.Max(player => player.Cards[i].CardValue);
                    winners = players.Where(player => player.Cards[i].CardValue == highestCurrentCardValue).ToList();

                    if (winners.Count() == 1)
                    {
                        break;
                    }
                }
            }

            return winners;
        }
    }
}
