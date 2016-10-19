using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class Card
    {
        public Suit Suit { get; private set; }
        public CardValue CardValue { get; private set; }

        public Card(Suit suit, CardValue cardValue)
        {
            Suit = suit;
            CardValue = cardValue;
        }
    }

    public enum Suit
    {
        Club,
        Diamond,
        Heart,
        Spade
    }

    public enum CardValue
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}
