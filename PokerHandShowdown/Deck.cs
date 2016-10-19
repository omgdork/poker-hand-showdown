using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class Deck
    {
        public List<Card> Cards { get; private set; }

        /// <summary>
        /// Set each card value with each suit and assign to cards.
        /// </summary>
        public Deck()
        {
            var suits = Enum.GetValues(typeof (Suit)).Cast<Suit>();
            var cardValues = Enum.GetValues(typeof(CardValue)).Cast<CardValue>();
            var cards = from suit in suits
                        from cardValue in cardValues
                        select new Card(suit, cardValue);

            Cards = cards.ToList();
        }

        /// <summary>
        /// Reorder the list of cards.
        /// </summary>
        public void Shuffle()
        {
            var random = new Random();

            for (var i = Cards.Count() - 1; i >= 1; i--)
            {
                var j = random.Next(0, i + 1);
                var tempCard = Cards[j];
                Cards[j] = Cards[i];
                Cards[i] = tempCard;
            }
        }
    }
}
