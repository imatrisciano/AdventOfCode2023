using System.Text.RegularExpressions;

ProblemPart1();
ProblemPart2();




List<int> ReadNumbersInLine(string line)
{
	Regex numberRegex = new Regex(@"\d+");
	List<int> list = new List<int>();

	foreach(Match numberString in numberRegex.Matches(line))
		list.Add(int.Parse(numberString.Value));

	return list;
}

UInt64 GetNumberOfSolutions(UInt64 Time, UInt64 Distance)
{
	/*
	(T-t)*t > D
	(T-t)*t - D > 0
	-t^2 + Tt - D > 0
	t^2 -Tt + D > 0
	solve for t:
		delta = T^2 - 4D
		t1 = (1/2) * (T - sqrt(delta))
		t2 = (1/2) * (T + sqrt(delta))
	# of solutions = floor(t2) - ceiling(t1) + 1
	*/

	double sqrtDelta = Math.Sqrt(Time*Time - 4*Distance);
	double t1 = (Time - sqrtDelta)/2d;
	double t2 = (Time + sqrtDelta)/2d;

	UInt64 numberOfSolutions = (UInt64)Math.Floor(t2) - (UInt64)Math.Ceiling(t1) + 1;
	return numberOfSolutions;
}

void ProblemPart1()
{
	StreamReader reader = new StreamReader("input.txt");
	List<int> times = ReadNumbersInLine(reader.ReadLine());
	List<int> distances= ReadNumbersInLine(reader.ReadLine());
	reader.Close();

	UInt64 problemSolution = 1;
	for (int i = 0; i < times.Count; i++)
		problemSolution *= GetNumberOfSolutions((UInt64)times[i], (UInt64)distances[i]);

	Console.WriteLine($"Part 1 result: {problemSolution}");
}

void ProblemPart2()
{
	StreamReader reader = new StreamReader("input.txt");
	string timeString = reader.ReadLine().Split(':')[1].Replace(" ", "");
	string distanceString = reader.ReadLine().Split(':')[1].Replace(" ", "");
	reader.Close();

	UInt64 problemSolutions = GetNumberOfSolutions(UInt64.Parse(timeString), UInt64.Parse(distanceString));

	Console.WriteLine($"Part 2 result: {problemSolutions}");
}