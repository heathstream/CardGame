class Deck
{
    private Stack<Card> _cards = new();
    public int Count => _cards.Count;

    public Deck()
    {
        for (var suit = CardSuit.Clubs; suit < CardSuit.Spades; suit++)
            for (var rank = CardRank.Two; rank < CardRank.Ace; rank++)
                _cards.Push(new Card(suit, rank));
    }

    public Card Deal() => _cards.Pop();

    public void Shuffle() => _cards = new Stack<Card>(_cards.Shuffle());

    public void Restore()
    {
        for (var suit = CardSuit.Clubs; suit < CardSuit.Spades; suit++)
            for (var rank = CardRank.Two; rank < CardRank.Ace; rank++)
                if (!_cards.Any(c => c.Suit == suit && c.Rank == rank))
                    _cards.Push(new Card(suit, rank));
        this.Shuffle();
    }
}