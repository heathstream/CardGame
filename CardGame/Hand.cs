using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

class Hand : IComparable<Hand>
{
    public const int HAND_SIZE = 5;
    public List<Card> Cards = new(HAND_SIZE);
    public int Count => Cards.Count;
    public string Name { get; set; }

    public HandRank Rank { get; private set; }
    public List<Card> WinningCards { get; private set; } = new();
    public List<Card> Kickers { get; private set; } = new();

    public Hand(string name = "Unnamed") => Name = name;

    public void AddCard(Card card)
    {
        if (Count != HAND_SIZE)
            Cards.Add(card);
        if (Count == HAND_SIZE)
        {
            CalculateRank();
            WinningCards.Sort();
            Kickers.Sort();
        }
    }

    private void CalculateRank()
    {
        Cards.Sort();
        WinningCards.Clear();
        Kickers.Clear();

        if (Cards.Count != HAND_SIZE)
            Rank = HandRank.None;

        var groups = Cards
        .GroupBy(c => c.Rank)
        .OrderByDescending(c => c.Count())
        .ThenByDescending(g => g.Key)
        .ToList();
        var counts = groups.Select(g => g.Count()).ToList();

        if (counts.SequenceEqual(new[] { 2, 1, 1, 1 }))
        {
            Rank = HandRank.Pair;
            WinningCards.AddRange(groups[0]);
            var kickers = groups.SelectMany(g => g).Skip(1);
            Kickers.AddRange(kickers);
            return;
        }
        if (counts.SequenceEqual(new[] { 2, 2, 1 }))
        {
            Rank = HandRank.Two_Pairs;
            WinningCards.AddRange(groups[0]);
            WinningCards.AddRange(groups[1]);
            Kickers.AddRange(groups[2]);
            return;
        }
        if (counts.SequenceEqual(new[] { 3, 1, 1 }))
        {
            Rank = HandRank.Three_Of_A_Kind;
            WinningCards.AddRange(groups[0]);
            Kickers.AddRange(groups[1]);
            Kickers.AddRange(groups[2]);
            return;
        }
        if (counts.SequenceEqual(new[] { 3, 2 }))
        {
            Rank = HandRank.Full_House;
            WinningCards.AddRange(groups[0]);
            WinningCards.AddRange(groups[1]);
            return;
        }
        if (counts.SequenceEqual(new[] { 4, 1 }))
        {
            Rank = HandRank.Four_Of_A_Kind;
            WinningCards.AddRange(groups[0]);
            Kickers.AddRange(groups[1]);
            return;
        }

        var cardRanks = Cards.Select(c => c.Rank);
        bool isRoyal = Cards.Max()!.Rank == CardRank.Ace && Cards.Min()!.Rank == CardRank.Ten;
        bool isFlush = Cards.All(c => c.Suit == Cards.First().Suit);
        bool isStraight = IsStraight();

        if (isRoyal && isStraight && isFlush)
        {
            Rank = HandRank.Royal_Flush;
            WinningCards.AddRange(groups.SelectMany(g => g));
            Kickers.AddRange(WinningCards);
            return;
        }
        if (isStraight && isFlush)
        {
            Rank = HandRank.Straight_Flush;
            WinningCards.AddRange(groups.SelectMany(g => g));
            Kickers.AddRange(WinningCards);
            return;
        }
        if (isStraight)
        {
            Rank = HandRank.Straight;
            WinningCards.AddRange(groups.SelectMany(g => g));
            Kickers.AddRange(WinningCards);
            return;
        }
        if (isFlush)
        {
            Rank = HandRank.Flush;
            WinningCards.AddRange(groups.SelectMany(g => g));
            Kickers.AddRange(WinningCards);
            return;
        }

        Rank = HandRank.High_Card;
        WinningCards.Add(Cards.Max()!);
        Kickers.AddRange(groups.SelectMany(g => g).Skip(1));
    }

    private bool IsStraight()
    {
        var ranks = Cards.Select(c => c.Rank).ToList();

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
        if (other == null) return 0;

        if (this.Rank != other.Rank)
            return this.Rank.CompareTo(other.Rank);

        // If ranks are the same, go through each winning card and compare their ranks:
        for (int i = 0; i < WinningCards.Count; i++)
        {
            if (this.WinningCards[i].Rank != other.WinningCards[i].Rank)
                return this.WinningCards[i].Rank.CompareTo(other.WinningCards[i].Rank);
        }

        // If all else fails, go through each of the kickers and compare their ranks:
        for (int i = 0; i < Kickers.Count; i++)
        {
            if (this.Kickers[i].Rank != other.Kickers[i].Rank)
                return this.Kickers[i].Rank.CompareTo(other.Kickers[i].Rank);
        }

        return 0;
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