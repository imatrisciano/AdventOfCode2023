using System.Text.RegularExpressions;

class ScratchCard
{
	public int CardNumber {get; set;}
	public int NumberOfCopies {get; set;}
	public List<int> WinningNumbers {get;set;}
	public List<int> MyNumbers {get;set;}

	private static readonly Regex numberRegex = new Regex(@"\d+");

	public ScratchCard(string scratchCardLine)
	{
		WinningNumbers = new List<int>();
		MyNumbers = new List<int>();
		NumberOfCopies = 1;

		// "scratchCardLineCard" has the following schema: "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"
		
		int indexOfColon = scratchCardLine.IndexOf(':');
		int indexOfPipe = scratchCardLine.IndexOf('|');

		foreach(Match match in numberRegex.Matches(scratchCardLine).AsEnumerable())
		{
			int indexOfNumber = match.Index;
			int number = int.Parse(match.Value);

			if (indexOfNumber < indexOfColon)
				CardNumber = number;
			else if (indexOfNumber > indexOfColon && indexOfNumber < indexOfPipe)
				WinningNumbers.Add(number);
			else
				MyNumbers.Add(number);
		}
	}

	public int GetCountOfMatchingNumbers()
	{
		return MyNumbers.Where(x=>WinningNumbers.Contains(x)).Count();
	}
	public int GetScratchCardPoints()
	{
		/*
			As far as the Elf has been able to figure out, you have to figure out which of the numbers you have appear in the list of winning numbers.
			The first match makes the card worth one point and each match after the first doubles the point value of that card.
		*/
		
		int numberOfMatchingNumbers = GetCountOfMatchingNumbers();

		if (numberOfMatchingNumbers > 0)
			return (int)Math.Pow(2, numberOfMatchingNumbers - 1); // aka 1 << (numberOfMatchingNumbers - 1)
		else
			return 0;

	}
}