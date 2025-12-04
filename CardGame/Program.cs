namespace CardGame;

class Program
{
    static void Main(string[] args)
    {
        var pokerGame = new PokerGame(new Hand("Anders"), new Hand("Julia"), new Hand("Fredrik"), new Hand("Kerstin"));

        pokerGame.StartGame();
    }
}
