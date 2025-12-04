class PokerGame
{
    private const int MAX_PLAYERS = 4;
    public List<Hand> Hands { get; private set; } = new(MAX_PLAYERS);
    public Deck Deck { get; init; } = new();

    public PokerGame() { }
    public PokerGame(params List<Hand> hands)
    {
        Hands.AddRange(hands);
    }

    public void StartGame()
    {
        bool running = true;
        while (running)
        {
            Deck.Shuffle();
            DealCards();
            PrintHands();
            Hand winner = PickWinner()!;
            Console.WriteLine($"The winner is {winner.Name} with {winner.Rank.ToString().Replace('_', ' ')}!");

            Console.Write("Do you want to play again? (y/n): ");
            if (Console.ReadLine() != "y")
                running = false;
            else
            {
                Console.Clear();
                ReturnAllCards();
                Deck.Restore();
            }
        }
    }

    public void DealCards(int number = Hand.HAND_SIZE)
    {
        for (int i = 0; i < number; i++)
            foreach (var hand in Hands)
                hand.AddCard(Deck.Deal());
    }

    public void ReturnAllCards()
    {
        foreach (var player in Hands)
            player.Cards.Clear();
        Deck.Restore();
    }

    public Hand? PickWinner()
    {
        if (Hands.Any())
            return Hands.Max();
        else return null;
    }

    public void PrintHands()
    {
        foreach (var hand in Hands)
        {
            Console.WriteLine(hand);
            Console.WriteLine();
        }
    }
}