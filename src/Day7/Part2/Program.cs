using System.Runtime.CompilerServices;

class Program
{
	public static void Main(string[] args)
	{
		List<Play> plays = new List<Play>();

		//StreamReader reader = new StreamReader("test.txt");
		StreamReader reader = new StreamReader("input.txt");
		while(!reader.EndOfStream)
		{
			string line = reader.ReadLine();
			if (line is null || line.Length == 0)
				continue;
			string[] lineContent = line.Split(' ');
			plays.Add(new Play(lineContent[0], int.Parse(lineContent[1])));
		}
		reader.Close();

		plays.Sort(new Play.PlayComparer());
		int totalWinnings = 0;
		for(int i = 0; i < plays.Count(); i++)
		{
			int playRank = i+1;
			int playBid = plays[i].Bid;
			totalWinnings += playRank * playBid;
		}
		Console.WriteLine($"Total winnings: {totalWinnings}");
		
	}
}

class Card
{
	private static readonly List<char> CardOrdering = ['J','2','3','4','5','6','7','8','9','T','Q','K','A'];

	public char CardSymbol {get; private set;}
	

	public Card(char symbol)
	{
		this.CardSymbol = symbol;
	}
	public int GetCardValue()
	{
		return CardOrdering.IndexOf(CardSymbol);
	}
}

class Hand
{
	public enum HandTypes
	{
		HighCard = 0,
		OnePair = 1,
		TwoPair = 2,
		ThreeOfAKind = 3,
		FullHouse = 4,
		FourOfAKind = 5,
		FiveOfAKind = 6
	}

	public HandTypes HandType {get; private set;} 
	public List<Card> Cards {get; private set;}
	public string CardString {get;private set;}

	public Hand(string cardString)
	{
		this.CardString = cardString;
		Cards = new List<Card>(5);
		foreach(char cardChar in cardString)
			Cards.Add(new Card(cardChar));
		
		HandType = GetHandType();
	}

	private HandTypes GetHandType()
	{
		var CardsWithoutJokers = Cards.Where(x=>x.CardSymbol != 'J');
		int numberOfJokers = Cards.Where(x=>x.CardSymbol == 'J').Count();
		var CardGroups = CardsWithoutJokers.GroupBy(x=>x.CardSymbol).OrderByDescending(x=>x.Count()).ToList(); // sort the card groups by # of occurences (descending)
		int numberOfDifferentCards = CardGroups.Count();
		if (numberOfDifferentCards == 0) // they are all jokers
			return HandTypes.FiveOfAKind;
		else if (numberOfDifferentCards == 1)
		{
			//all the cards are equal
			return HandTypes.FiveOfAKind;
		}
		else if (numberOfDifferentCards == 2)
		{
			// This can be: FourOfAKind, FullHouse or ThreeOfAKind
			
			int maxOccurencesOfAnyCard = CardGroups[0].Count();
			if (maxOccurencesOfAnyCard + numberOfJokers == 4)
				return HandTypes.FourOfAKind;
			else if (maxOccurencesOfAnyCard + numberOfJokers == 3)
			{
				// FullHouse or ThreeOfAKind
				// got 3 equal cars, now we check if the remaining cards are equal
				int instancesOfTheSecondMostCommonCard = CardGroups[1].Count();
				if (instancesOfTheSecondMostCommonCard == 2)
					return HandTypes.FullHouse;
				else
					return HandTypes.ThreeOfAKind;
			}
		}
		else if (numberOfDifferentCards == 3)
		{
			//this can be ThreeOfAKind or TwoPair, such as 112234 or 11123
			int maxOccurencesOfAnyCard = CardGroups[0].Count();
			if (maxOccurencesOfAnyCard + numberOfJokers == 3)
				return HandTypes.ThreeOfAKind;
			else
				return HandTypes.TwoPair;
		}
		else if (numberOfDifferentCards == 4)
		{
			//two cards are the same and the rest are all different from eachother
			return HandTypes.OnePair;
		}
		else
		{
			//got nothing, all cards are different
			return HandTypes.HighCard;
		}

		//We shouldn't get here, if we do that means we fucked up
		throw new Exception("Hand processing failed");
	}

}

class Play
{
	public Hand Hand {get;set;}
	public int Bid {get;set;}

	public Play(string hand, int bid)
	{
		this.Hand = new Hand(hand);
		this.Bid = bid;
	}
    public override string ToString()
    {
        return $"{Hand.CardString}  {Hand.HandType}  {Bid}";
    }

    public class PlayComparer : IComparer<Play>
	{
        int IComparer<Play>.Compare(Play? x, Play? y)
        {
            int xTypeValue = (int)x.Hand.HandType;
			int yTypeValye = (int)y.Hand.HandType;

			if (xTypeValue != yTypeValye)
				return xTypeValue - yTypeValye;
			else
			{
				//the two hands got the same hand type, we must compare each card until we find ona that's different
				for(int i = 0; i < x.Hand.Cards.Count; i++)
				{
					int xCardValue = x.Hand.Cards[i].GetCardValue();
					int yCardValue = y.Hand.Cards[i].GetCardValue();
					if (xCardValue != yCardValue)
						return xCardValue - yCardValue;
				}
			}

			return 0; // the two hands have the same value
        }
    }
}