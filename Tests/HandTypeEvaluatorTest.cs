using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerHandShowdown;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class HandTypeEvaluatorTest
    {
        HandTypeEvaluator Evaluator;

        [TestInitialize]
        public void SetUp()
        {
            var flushEvaluator = new FlushEvaluator();
            var threeOfAKindEvaluator = new ThreeOfAKindEvaluator();
            var onePairEvaluator = new OnePairEvaluator();

            flushEvaluator.NextHandTypeEvaluator = threeOfAKindEvaluator;
            threeOfAKindEvaluator.NextHandTypeEvaluator = onePairEvaluator;

            Evaluator = flushEvaluator;
        }

        [TestMethod]
        public void FlushesWin()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Club, CardValue.King));
            john.Cards.Add(new Card(Suit.Club, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Club, CardValue.Eight));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Seven));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Six));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Five));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Four));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(1, winners.Count());
            Assert.AreEqual("John", winners.First().Name);
            Assert.AreEqual(HandType.Flush, winners.First().HandType);
        }

        [TestMethod]
        public void TwoPlayersWithFlushesOnePlayerWithHigherCardValue()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Club, CardValue.King));
            john.Cards.Add(new Card(Suit.Club, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Ace));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.King));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Queen));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Ten));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Eight));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(1, winners.Count());
            Assert.AreEqual("John", winners.FirstOrDefault().Name);
        }

        [TestMethod]
        public void TwoPlayersWithFlushesDraw()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Club, CardValue.King));
            john.Cards.Add(new Card(Suit.Club, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Ace));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.King));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Queen));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Ten));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Nine));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(2, winners.Count());
            Assert.AreEqual("John", winners[0].Name);
            Assert.AreEqual("Jane", winners[1].Name);
        }

        [TestMethod]
        public void ThreeOfAKindWins()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Heart, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Spade, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Club, CardValue.Eight));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Seven));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Six));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Five));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Four));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(1, winners.Count());
            Assert.AreEqual("John", winners.First().Name);
            Assert.AreEqual(HandType.ThreeOfAKind, winners.First().HandType);
        }

        [TestMethod]
        public void OnePairWins()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Heart, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Spade, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Club, CardValue.Eight));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Seven));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Six));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Five));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Four));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(1, winners.Count());
            Assert.AreEqual("John", winners.First().Name);
            Assert.AreEqual(HandType.OnePair, winners.First().HandType);
        }

        [TestMethod]
        public void TwoPlayersWithOnePairOneWinner()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Heart, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Spade, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Club, CardValue.King));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Seven));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Six));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Five));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.King));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(1, winners.Count());
            Assert.AreEqual("John", winners.First().Name);
            Assert.AreEqual(HandType.OnePair, winners.First().HandType);
        }

        [TestMethod]
        public void TwoPlayersWithSamePairOneWinner()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Diamond, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Heart, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Spade, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            jane.Cards.Add(new Card(Suit.Spade, CardValue.Ace));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Six));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Five));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.King));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(1, winners.Count());
            Assert.AreEqual("Jane", winners.First().Name);
            Assert.AreEqual(HandType.OnePair, winners.First().HandType);
            Assert.AreEqual(5, winners.First().Cards.Count);
        }

        [TestMethod]
        public void TwoPlayersWithSamePairDraw()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Diamond, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Heart, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Spade, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            jane.Cards.Add(new Card(Suit.Spade, CardValue.Ace));
            jane.Cards.Add(new Card(Suit.Diamond, CardValue.Queen));
            jane.Cards.Add(new Card(Suit.Diamond, CardValue.Ten));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Nine));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(2, winners.Count());
            Assert.AreEqual("John", winners[0].Name);
            Assert.AreEqual("Jane", winners[1].Name);
        }

        [TestMethod]
        public void HighCardWins()
        {
            // arrange
            var john = new Player("John");
            john.Cards.Add(new Card(Suit.Club, CardValue.Ace));
            john.Cards.Add(new Card(Suit.Heart, CardValue.King));
            john.Cards.Add(new Card(Suit.Spade, CardValue.Queen));
            john.Cards.Add(new Card(Suit.Club, CardValue.Ten));
            john.Cards.Add(new Card(Suit.Club, CardValue.Nine));

            var jane = new Player("Jane");
            jane.Cards.Add(new Card(Suit.Club, CardValue.Eight));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Seven));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Six));
            jane.Cards.Add(new Card(Suit.Club, CardValue.Five));
            jane.Cards.Add(new Card(Suit.Heart, CardValue.Four));

            var juan = new Player("Juan");
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Eight));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Seven));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Six));
            juan.Cards.Add(new Card(Suit.Spade, CardValue.Five));
            juan.Cards.Add(new Card(Suit.Club, CardValue.Three));

            var players = new List<Player>();
            players.Add(john);
            players.Add(jane);
            players.Add(juan);

            // act
            // determine player hand type.
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.SortCards();
                player.HandType = Evaluator.Evaluate(players[i].Cards);
            }

            // get players with best hand types.
            var playersWithBestHandType = players
                .GroupBy(player => player.HandType)
                .OrderBy(handType => handType.Key)
                .Last();

            var winners = Evaluator.GetWinners(playersWithBestHandType.ToList());

            // assert
            Assert.AreEqual(1, winners.Count());
            Assert.AreEqual("John", winners.First().Name);
            Assert.AreEqual(HandType.HighCard, winners.First().HandType);
            Assert.AreEqual(CardValue.Ace, winners.First().Cards.Max(card => card.CardValue));
        }
    }
}
