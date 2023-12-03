using System.IO;
using System.Text.RegularExpressions;

class Schematic
{
	public List<SchematicCode> SchematicCodes = new List<SchematicCode>();
	public List<SchematicSymbol> SchematicSymbols = new List<SchematicSymbol>();


	public List<SchematicCode> GetPartNumbers()
	{
		//part numbers are the codes that have a symbol nearby (even in diagonal)
		List<SchematicCode> partNumbers = new List<SchematicCode>();
		foreach(SchematicCode code in SchematicCodes)
			if (SchematicSymbols.Any(symbol => AreSymbolAndCodeAdiacent(symbol, code)))
				partNumbers.Add(code);

		return partNumbers;
	}

	private IEnumerable<SchematicCode> GetCodesAdiacentToSymbol(SchematicSymbol symbol)
	{
		return SchematicCodes.Where( code => AreSymbolAndCodeAdiacent(symbol, code));
	}

	private bool AreSymbolAndCodeAdiacent(SchematicSymbol symbol, SchematicCode code)
	{
		bool AdiacentOnPreviousAndNextRow = (code.LineNumber == symbol.LineNumber - 1 || code.LineNumber == symbol.LineNumber + 1) && (symbol.Position >= code.StartPosition -1 && symbol.Position <= code.EndPosition +1);
		bool AdiacentOnSameRow = (code.LineNumber == symbol.LineNumber && (code.EndPosition == symbol.Position - 1 || code.StartPosition == symbol.Position + 1));

		return AdiacentOnPreviousAndNextRow || AdiacentOnSameRow;
	}

	public IEnumerable<Gear> GetGears()
	{
		List<Gear> gears = new List<Gear>();
		IEnumerable<SchematicSymbol> AsteriscsSymbols = SchematicSymbols.Where(x=>x.Symbol == "*");
		foreach(SchematicSymbol asterisc in AsteriscsSymbols)
		{
			var codesAdiacentToAsterisc = GetCodesAdiacentToSymbol(asterisc);
			if (codesAdiacentToAsterisc.Count() == 2)
			{
				//code is a gear
				gears.Add(new Gear(codesAdiacentToAsterisc.ElementAt(0), codesAdiacentToAsterisc.ElementAt(1)));
			}
		}
		return gears;
	}
}

record SchematicCode(int LineNumber, int StartPosition, int EndPosition, int Number);

record SchematicSymbol(int LineNumber, int Position, string Symbol);
record Gear(SchematicCode Code1, SchematicCode Code2)
{
	public int GetGearRatio()
	{
		return Code1.Number * Code2.Number;
	}
}


public class Program
{
	static Schematic schematic = new Schematic();
	static readonly Regex CodeExtractorRegex = new Regex(@"\d+", RegexOptions.Compiled); // any sequence of digits

	static readonly Regex SymbolsExtractorRegex = new Regex(@"[^\d|.|\s]", RegexOptions.Compiled); // anything that's not a digit, a dot or a whitespace character


	public static void Main(string[] args)
	{
		//StreamReader reader = new StreamReader("test.txt");
		StreamReader reader = new StreamReader("input.txt");
		int lineNumber = 0;
		while(!reader.EndOfStream)
		{
			string? line = reader.ReadLine();
			if (line is null || line.Length == 0)
				continue;
			ProcessFileLine(line, lineNumber);
			lineNumber++;
		}

		var partNumbers = schematic.GetPartNumbers();
		int sumOfPartNumbers = partNumbers.Sum( x => x.Number );

		Console.WriteLine($"Sum of part numbers: {sumOfPartNumbers}");


		var gears = schematic.GetGears();
		int sumOfGearRatios = gears.Sum( x => x.GetGearRatio() );
		Console.WriteLine($"Sum of gear ratios: {sumOfGearRatios}");
	}


	static void ProcessFileLine(string line, int lineNumber)
	{
		ExtractLineCodes(line, lineNumber);
		ExtractLineSymbols(line, lineNumber);
	}

	static void ExtractLineCodes(string line, int lineNumber)
	{
		foreach(Match match in CodeExtractorRegex.Matches(line))
		{
			SchematicCode code = new SchematicCode(lineNumber, match.Index,match.Index + match.Length - 1, int.Parse(match.Value));
			schematic.SchematicCodes.Add(code);
		}
	}
	static void ExtractLineSymbols(string line, int lineNumber)
	{
		foreach(Match match in SymbolsExtractorRegex.Matches(line))
		{
			SchematicSymbol symbol = new SchematicSymbol(lineNumber, match.Index, match.Value);
			schematic.SchematicSymbols.Add(symbol);
		}
	}

}



