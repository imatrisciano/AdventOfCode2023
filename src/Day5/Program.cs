public class Program
{
	public static void Main(string[] args)
	{
		//StreamReader reader = new StreamReader("test.txt");
		StreamReader reader = new StreamReader("input.txt");

		Almanac almanac = new Almanac(reader);
		var Locations = almanac.PropagateInitialSeedsToLocations();

		var closestLocation = Locations.Min();
		Console.WriteLine($"Closest location: {closestLocation}");

		/*
		Yeah, I know, brute force is not the optimal solution here
		but we gotta be practical, this thing took 16 seconds to run
		on my Ryzen 3700X when published in release
		That's way lower than the time it'd take me to find an optimal
		solution. Since this thing doesn't have to run on a daily basis
		any further optimization is a waste of time
		(This is my way to cope with the fact that I'm not able to find
		a better solution)
		*/
		var optimalSeed = almanac.BruteSearch();
		Console.WriteLine($"Optimal seed: {optimalSeed}");
	}


}
