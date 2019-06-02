using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Cards
{
    public class PlayingCard : Card
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Suit property
        /// </summary>
        private PlayingCardSuit _Suit;

        /// <summary>
        /// The suit of this playing card
        /// </summary>
        public PlayingCardSuit Suit
        {
            get { return _Suit; }
        }

        /// <summary>
        /// Private backing member variable for the Rank property
        /// </summary>
        private PlayingCardRank _Rank;

        /// <summary>
        /// The rank of this playing card
        /// </summary>
        public PlayingCardRank Rank
        {
            get { return _Rank; }

        }

        #endregion

        #region Constructors

        public PlayingCard(PlayingCardRank rank, PlayingCardSuit suit)
        {
            _Rank = rank;
            _Suit = suit;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Rank.ToString() + " of " + Suit.ToString();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Build a standard deck of 52 playing cards
        /// </summary>
        /// <returns></returns>
        public static IList<PlayingCard> BuildDeck()
        {
            var deck = new List<PlayingCard>(52);
            for (PlayingCardSuit suit = PlayingCardSuit.Spades;
                suit <= PlayingCardSuit.Clubs; suit++)
            {
                for (PlayingCardRank rank = PlayingCardRank.Ace;
                    rank <= PlayingCardRank.King; rank++)
                {
                    deck.Add(new PlayingCard(rank, suit));
                }
            }
            return deck;
        }

        #endregion
    }
}
