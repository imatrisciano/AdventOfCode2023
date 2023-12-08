class Map
{
	public enum Direction
	{
		Left, Right
	}
	record Intersection(string Left, string Right);


	class InstructionFollower
	{
		public List<Direction> InstructionList {get; set;}
		private int CurrentStep = 0;

		public InstructionFollower(string instructions)
		{
			InstructionList = new List<Direction>(instructions.Length);
			foreach(char c in instructions)
			{
				if (c == 'L')
					InstructionList.Add(Direction.Left);
				else 
					InstructionList.Add(Direction.Right);
			}
		}

		public Direction GetNextStep()
		{
			Direction direction = InstructionList[CurrentStep];
			CurrentStep = (CurrentStep + 1) % InstructionList.Count;
			return direction;
		}

		public void Reset()
		{
			CurrentStep = 0;
		}
	}



	Dictionary<string, Intersection> intersections;
	InstructionFollower instructionFollower;
	public Map(StreamReader reader)
	{
		intersections = new Dictionary<string, Intersection>();

		string line = reader.ReadLine();
		instructionFollower = new InstructionFollower(line);
		reader.ReadLine(); // throw away empty line

		while (!reader.EndOfStream)
		{
			line = reader.ReadLine();
			if (line is null || line.Length == 0)
				continue;

			string intersection = line.Substring(0, 3);
			string leftDestination = line.Substring(7, 3);
			string rightDestination = line.Substring(12, 3);

			intersections.Add(intersection, new Intersection(leftDestination, rightDestination));
		}
	}


	public int NavigatePart1()
	{
		int numberOfSteps = 0;
		string currentPosition = "AAA";

		while (currentPosition != "ZZZ")
		{
			Direction direction = instructionFollower.GetNextStep();

			if (direction == Direction.Left)
				currentPosition = intersections[currentPosition].Left;
			else
				currentPosition = intersections[currentPosition].Right;

			numberOfSteps ++;
		}

		return numberOfSteps;
	}


	//don't even try, this thing will never finish
	public int NavigatePart2_BruteForce()
	{
		int numberOfSteps = 0;

		//select all of the positions ending with an A
		List<string> currentPositions = intersections.Keys.Where(x=>x.EndsWith('A')).ToList();

		while (currentPositions.Any(x=>!x.EndsWith('Z'))) // repeat until all of the positions end with a Z
		{
			Direction direction = instructionFollower.GetNextStep();

			for(int i = 0; i < currentPositions.Count; i++)
			{
				if (direction == Direction.Left)
					currentPositions[i] = intersections[currentPositions[i]].Left;
				else
					currentPositions[i] = intersections[currentPositions[i]].Right;
			}
			
			numberOfSteps ++;
			if (numberOfSteps % 100000 == 0)
				Console.WriteLine($"Steps: {numberOfSteps}");
		}

		return numberOfSteps;
	}

	public UInt64 NavigatePart2()
	{
		//select all of the positions ending with an A
		List<string> currentPositions = intersections.Keys.Where(x=>x.EndsWith('A')).ToList();
		List<int> numberOfStepsForEachPosition = new List<int>(currentPositions.Count);

		for (int i = 0; i < currentPositions.Count; i++)
		{
			//solve this path
			int numberOfSteps = 0;
			while(!currentPositions[i].EndsWith('Z'))
			{
				Direction direction = instructionFollower.GetNextStep();
				if (direction == Direction.Left)
					currentPositions[i] = intersections[currentPositions[i]].Left;
				else
					currentPositions[i] = intersections[currentPositions[i]].Right;
				
				numberOfSteps++;
			}

			numberOfStepsForEachPosition.Add(numberOfSteps);
			numberOfSteps = 0;
			instructionFollower.Reset();
		}
		
		//now we know how many steps to finish each path
		//since this is periodic we must take the least common multiple

		UInt64 leastCommonMultiple = (UInt64)numberOfStepsForEachPosition[0];
		for (int i = 0; i < numberOfStepsForEachPosition.Count; i++)
			leastCommonMultiple = LeastCommonMultiple(leastCommonMultiple, (UInt64)numberOfStepsForEachPosition[i]);
		return leastCommonMultiple;
	}

	// https://stackoverflow.com/questions/13569810/least-common-multiple
	static UInt64 GreatestCommonFactor(UInt64 a, UInt64 b)
	{
		while (b != 0)
		{
			UInt64 temp = b;
			b = a % b;
			a = temp;
		}
		return a;
	}
	// https://stackoverflow.com/questions/13569810/least-common-multiple
	static UInt64 LeastCommonMultiple(UInt64 a, UInt64 b)
	{
		return (a / GreatestCommonFactor(a, b)) * b;
	}



}



class Program
{
	public static void Main(string[] args)
	{
		//StreamReader reader = new StreamReader("test2.txt");
		StreamReader reader = new StreamReader("input.txt");
		Map map = new Map(reader);
		reader.Close();

		Console.WriteLine($"Part 1 number of steps: {map.NavigatePart1()}");
		Console.WriteLine($"Part 2 number of steps: {map.NavigatePart2()}");
		
	}
}