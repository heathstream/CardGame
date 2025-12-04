using System.Runtime.CompilerServices;
using System.Text;

class Hand : IComparable<Hand>
{
    public const int HAND_SIZE = 5;
    public List<Card> Cards = new(HAND_SIZE);
    public HandRank Rank { get; private set; }
    public Card this[int i] => Cards[i];
    public int Count => Cards.Count;
    public string Name { get; set; }

    public Hand(string name = "Unnamed") => Name = name;

    public void AddCard(Card card)
    {
        if (Count != HAND_SIZE)
            Cards.Add(card);
        if (Count == HAND_SIZE)
            Rank = CalculateRank();
        Cards.Order();
    }

    private HandRank CalculateRank()
    {
        if (Cards.Count != HAND_SIZE)
            return HandRank.None;

        var rankCounts = Cards.GroupBy(c => c.Rank).Select(g => g.Count()).Order().Reverse();

        if (rankCounts.SequenceEqual(new[] { 2, 1, 1, 1 }))
            return HandRank.Pair;
        if (rankCounts.SequenceEqual(new[] { 2, 2, 1 }))
            return HandRank.Two_Pairs;
        if (rankCounts.SequenceEqual(new[] { 3, 1, 1 }))
            return HandRank.Three_Of_A_Kind;
        if (rankCounts.SequenceEqual(new[] { 3, 2 }))
            return HandRank.Full_House;
        if (rankCounts.SequenceEqual(new[] { 4, 1 }))
            return HandRank.Four_Of_A_Kind;

        bool hasAce = Cards.Max()?.Rank == CardRank.Ace;
        bool isFlush = Cards.All(c => c.Suit == Cards.First().Suit);
        bool isStraight = IsStraight();

        if (hasAce && isStraight && isFlush)
            return HandRank.Royal_Flush;
        if (isStraight && isFlush)
            return HandRank.Straight_Flush;
        if (isStraight)
            return HandRank.Straight;
        if (isFlush)
            return HandRank.Flush;

        return HandRank.High_Card;
    }

    private bool IsStraight()
    {
        var ranks = Cards.Select(c => c.Rank).Order().ToList();

        if (ranks.SequenceEqual(new[] { CardRank.Two, CardRank.Three, CardRank.Four, CardRank.Five, CardRank.Ace }))
            return true;

        for (int i = 1; i < HAND_SIZE; i++)
        {
            if (ranks[i] != ranks[i - 1] + 1)
                return false;
        }

        return true;
    }

    public int CompareTo(Hand? other)
    {
        if (other?.Rank != Rank)
            return Rank.CompareTo(other?.Rank);
        else
            return Cards.Max()!.CompareTo(other?.Cards.Max());
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(Name + ":");
        foreach (var card in Cards)
            sb.AppendLine(card.ToString());
        return sb.ToString();
    }
}

enum HandRank
{
    None, High_Card, Pair, Two_Pairs, Three_Of_A_Kind, Straight, Flush,
    Full_House, Four_Of_A_Kind, Straight_Flush, Royal_Flush
}