using System.ComponentModel;
using System.Text.RegularExpressions;

public class Program
{
	public static void Main(string[] args)
	{
		List<ScratchCard> scratchCards = new List<ScratchCard>();

		//StreamReader reader = new StreamReader("test.txt");
		StreamReader reader = new StreamReader("input.txt");
		while(!reader.EndOfStream)
		{
			string? line = reader.ReadLine();
			if (line is null || line.Length == 0)
				continue;
				
			scratchCards.Add(new ScratchCard(line));
		}


		int sumOfScartchCardPoints = scratchCards.Sum( x => x.GetScratchCardPoints() );
		Console.WriteLine($"Sum of scratch card points: {sumOfScartchCardPoints}");

		AnalyzePartTwoWinningCards(scratchCards);
		int numberOfScratchCards = scratchCards.Sum(x=>x.NumberOfCopies);
		Console.WriteLine($"Number of scratch cards: {numberOfScratchCards}");
	}

	static void AnalyzePartTwoWinningCards(List<ScratchCard> cards)
	{
		for(int i = 0; i < cards.Count; i++)
		{
			ScratchCard card = cards[i];
			int numberOfMatchingNumbers = card.GetCountOfMatchingNumbers();

			for (int j = 0; j < numberOfMatchingNumbers; j++)
				cards[i+j+1].NumberOfCopies += card.NumberOfCopies;
		}
	}

}



