class Card : IEquatable<Card>, IComparable<Card>
{
    public CardSuit Suit { get; set; }
    public CardRank Rank { get; set; }
    public override string ToString() => $"{SuitSymbol} {Rank} of {Suit}";

    public Card(CardSuit suit, CardRank rank) => (Suit, Rank) = (suit, rank);

    public string SuitSymbol => Suit switch
    {
        CardSuit.Clubs => "♧",
        CardSuit.Diamonds => "♢",
        CardSuit.Hearts => "♡",
        CardSuit.Spades => "♤",
        _ => ""
    };

    #region IEquatable
    public bool Equals(Card? other) => (Suit, Rank) == (other?.Suit, other?.Rank);
    public override bool Equals(object? obj) => Equals(obj as Card);
    public override int GetHashCode() => (Suit, Rank).GetHashCode();
    #endregion

    #region IComparable
    public int CompareTo(Card? other)
    {
        if (Rank != other?.Rank)
            return Rank.CompareTo(other?.Rank);
        else return 0;
    }
    #endregion
}

enum CardSuit { Clubs, Diamonds, Hearts, Spades }
enum CardRank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }