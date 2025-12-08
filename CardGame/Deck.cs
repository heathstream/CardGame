class Deck
{
    private Stack<Card> _cards = new();
    private List<Card> _dealtCards = new();
    public int Count => _cards.Count;

    public Deck()
    {
        for (var suit = CardSuit.Clubs; suit < CardSuit.Spades; suit++)
            for (var rank = CardRank.Two; rank < CardRank.Ace; rank++)
                _cards.Push(new Card(suit, rank));
    }

    public Card Deal()
    {
        Card card = _cards.Pop();
        _dealtCards.Add(card);
        return card;
    }

    public void Shuffle() => _cards = new Stack<Card>(_cards.Shuffle());

    public void Restore()
    {
        foreach (var card in _dealtCards)
        {
            _cards.Push(card);
        }
        _dealtCards.Clear();
        Shuffle();
    }
}