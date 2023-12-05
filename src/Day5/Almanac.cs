using NumberType = System.UInt64;

class Almanac
{
	public record SeedRange(NumberType RangeStart, NumberType RangeLength);
	
	public List<NumberType> Seeds = new List<NumberType>();
	public List<SeedRange> SeedRanges = new List<SeedRange>();
	public MyMap SeedToSoilMap = new MyMap();
	public MyMap SoilToFertilzierMap = new MyMap();
	public MyMap FertilizerToWaterMap = new MyMap();
	public MyMap WaterToLightMap = new MyMap();
	public MyMap LightToTemperatureMap = new MyMap();
	public MyMap TemperatureToHumidityMap = new MyMap();
	public MyMap HumidityToLocationMap = new MyMap();

	public Almanac()
	{

	}
	public Almanac(StreamReader stream)
	{
		ReadSeeds(stream);
		ReadAllMaps(stream);
		stream.Close();
	}


	private void ReadSeeds(StreamReader stream)
	{
		string seedsLine = stream.ReadLine() ?? throw new Exception("Invalid input format");
		string valuesString = seedsLine.Substring("seeds: ".Length);
		string[] values = valuesString.Split(' ');
		foreach(string value in values)
			Seeds.Add(NumberType.Parse(value));

		for(int i = 0; i < Seeds.Count; i+=2)
			SeedRanges.Add(new SeedRange(Seeds[i], Seeds[i+1]));

		stream.ReadLine(); // read the empty line after the seeds line
    }

    private void ReadAllMaps(StreamReader stream)
	{
		ReadMap(SeedToSoilMap, stream);
		ReadMap(SoilToFertilzierMap, stream);
		ReadMap(FertilizerToWaterMap, stream);
		ReadMap(WaterToLightMap, stream);
		ReadMap(LightToTemperatureMap, stream);
		ReadMap(TemperatureToHumidityMap, stream);
		ReadMap(HumidityToLocationMap, stream);
	}

	private void ReadMap(MyMap map, StreamReader stream)
	{
		string? line = stream.ReadLine() ?? throw new Exception("Invalid input format"); // skip the header line
		while ((line = stream.ReadLine()) is not null && line.Length > 0)
		{
			string[] numbers = line.Split(' ');

			NumberType DestinationRangeStart = NumberType.Parse(numbers[0]);
			NumberType SourceRangeStart = NumberType.Parse(numbers[1]);
			NumberType RangeLength = NumberType.Parse(numbers[2]);

			map.AddMapRange(new MapRange(DestinationRangeStart, SourceRangeStart, RangeLength));
		}
	}

	public IEnumerable<NumberType> PropagateInitialSeedsToLocations()
	{
		List<NumberType> locations = new List<NumberType>();
		foreach(NumberType seed in Seeds)
			locations.Add(PropagateSeed(seed));

		return locations;
	}
	private NumberType PropagateSeed(NumberType seed)
	{
		NumberType soil = SeedToSoilMap.GetDestination(seed);
		NumberType fertilizer = SoilToFertilzierMap.GetDestination(soil);
		NumberType water = FertilizerToWaterMap.GetDestination(fertilizer);
		NumberType light = WaterToLightMap.GetDestination(water);
		NumberType temperature = LightToTemperatureMap.GetDestination(light);
		NumberType humidity = TemperatureToHumidityMap.GetDestination(temperature);
		NumberType location = HumidityToLocationMap.GetDestination(humidity);
		return location;
	}

	public NumberType BruteSearch()
	{
		NumberType minLocationSoFar = NumberType.MaxValue;

		for(int rangeIndex = 0; rangeIndex < SeedRanges.Count; rangeIndex++)
		{
			SeedRange range = SeedRanges[rangeIndex];
			Parallel.For((long)0, (long)range.RangeLength, i => 
			{
				NumberType location = PropagateSeed(range.RangeStart + (ulong)i);
				if (location < minLocationSoFar)
					minLocationSoFar = location;

				if (i % 3000000 == 0)
					Console.WriteLine($"Progress: {rangeIndex+1}/{SeedRanges.Count} {100*i / (long)range.RangeLength}%");
			});
		}

		return minLocationSoFar;
	}
}