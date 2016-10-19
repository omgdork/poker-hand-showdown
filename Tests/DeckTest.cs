using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using PokerHandShowdown;

namespace Tests
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void DeckHasFiftyTwoCards()
        {
            var deck = new Deck();
            Assert.AreEqual(52, deck.Cards.Count);
        }

        [TestMethod]
        public void DeckHasFiftyTwoUniqueCards()
        {
            var deck = new Deck();
            Assert.IsFalse(deck.Cards
                .GroupBy(card => card)
                .Any(group => group.Count() > 1));
        }

        [TestMethod]
        public void ShufflingDoesNotDuplicateCards()
        {
            // arrange
            var deck = new Deck();

            // act
            deck.Shuffle();

            // assert
            Assert.IsFalse(deck.Cards
                .GroupBy(card => card)
                .Any(group => group.Count() > 1));
        }
    }
}
