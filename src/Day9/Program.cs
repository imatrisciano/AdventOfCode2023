using System.Text.RegularExpressions;

class OasisReport
{
	public List<OasisReportEntry> Entries;

	public OasisReport(StreamReader reader)
	{
		Entries = new List<OasisReportEntry>();
		while(!reader.EndOfStream)
		{
			string? line = reader.ReadLine();
			if (line is null || String.IsNullOrEmpty(line))
				continue;
			
			Entries.Add(new OasisReportEntry(line));
		}
	}

	public void ReverseEntries()
	{
		foreach(var entry in Entries)
		{
			entry.ValueHistory.Reverse();
		}
	}

	public int SumAllPredictions()
	{
		return Entries.Sum(x=>x.Predict());
	}
}

class OasisReportEntry
{
	public List<int> ValueHistory {get; set;}

	public OasisReportEntry(string line)
	{
		var numbers = line.Split(' ');

		ValueHistory = new List<int>(numbers.Length);
		foreach(string number in numbers)
			ValueHistory.Add(int.Parse(number));
	}

	public int Predict()
	{
		List<int> LastNumberInEachStep = new List<int>();
		List<int> CurrentStep = new List<int>(ValueHistory);
		LastNumberInEachStep.Add(CurrentStep.Last());

		while(CurrentStep.Any(x => x != 0))
		{
			List<int> newStep = new List<int>(CurrentStep.Count - 1);
			for(int i = 1; i < CurrentStep.Count; i++)
				newStep.Add(CurrentStep[i] - CurrentStep[i-1]);
			CurrentStep = newStep;
			LastNumberInEachStep.Add(CurrentStep.Last());
		}

		return LastNumberInEachStep.Sum();
	}
}

class Program
{
	public static void Main(string[] args)
	{
		StreamReader reader = new StreamReader("input.txt");
		OasisReport report = new OasisReport(reader);
		reader.Close();

		Console.WriteLine($"Part 1 answer: {report.SumAllPredictions()}");
		report.ReverseEntries();
		Console.WriteLine($"Part 2 answer: {report.SumAllPredictions()}");
	}
}

