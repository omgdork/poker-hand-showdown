using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class Player
    {
        public String Name { get; set; }
        public List<Card> Cards { get; set; }
        public HandType HandType { get; set; }

        public Player(string name)
        {
            Name = name;
            Cards = new List<Card>();
        }

        public void SortCards()
        {
            Cards = Cards.OrderByDescending(card => card.CardValue).ToList();
        }

        public void ClearCards()
        {
            Cards.Clear();
        }
    }
}
