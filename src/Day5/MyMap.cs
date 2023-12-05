using NumberType = System.UInt64;
class MyMap
{
	public List<MapRange> MapRanges {get; private set;}
	public MyMap()
	{
		MapRanges = new List<MapRange>();
	}

	public void AddMapRange(MapRange range)
	{
		MapRanges.Add(range);
	}

	public NumberType GetDestination(NumberType source)
	{
		foreach(MapRange range in MapRanges)
		{
			if (source >= range.SourceStart && source < range.SourceStart + range.RangeLenght)
			{
				NumberType delta = source - range.SourceStart;
				return range.DestinationStart + delta;
			}
		}

		return source;
	}
	public NumberType GetSource(NumberType destination)
	{
		foreach(MapRange range in MapRanges)
		{
			if (destination >= range.DestinationStart && destination < range.SourceStart + range.RangeLenght)
			{
				NumberType delta = destination - range.SourceStart;
				return range.SourceStart + delta;
			}
		}

		return destination;
	}
}

record MapRange(NumberType DestinationStart, NumberType SourceStart, NumberType RangeLenght);