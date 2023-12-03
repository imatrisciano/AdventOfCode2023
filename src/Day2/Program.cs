using System.IO;

public class Run
{
    public int Red {get; set;} = 0;
    public int Green {get; set;} = 0;
    public int Blue {get; set;} = 0;
}

public class Game
{
    public int Id {get; set;}
    public List<Run> Runs = new List<Run>();

    public override string ToString()
    {
        return Id.ToString();
    }

    public bool IsPossible()
    {
        return Runs.All( x=> x.Red <= 12 && x.Green <= 13 && x.Blue <= 14);
    }
    public int GetGamePower()
    {
        int MinimumRequiredReds = Runs.Max(x => x.Red);
        int MinimumRequiredGreens = Runs.Max(x => x.Green);
        int MinimumRequiredBlues = Runs.Max(x => x.Blue);

        int power = MinimumRequiredReds * MinimumRequiredGreens * MinimumRequiredBlues;
        return power;
    }
}


class Program
{
    public static void Main(string[] args)
    {
        StreamReader file = new StreamReader("input.txt");
        List<Game> games = new List<Game>();
        while(!file.EndOfStream)
        {
            string? line = file.ReadLine();
            if (line is null || line.Length == 0)
                continue; // skip
            
            //line structure:
            //Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green ...
            
            string[] LineSplittedByColon = line.Split(':');
            string gameIdString = LineSplittedByColon[0]; //"Game 1"
            string gameInformation = LineSplittedByColon[1]; //" 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"

            string gameId = gameIdString.Split(' ')[1]; // "1"
            string[] runs = gameInformation.Split(';'); // " 3 blue, 4 red", " 1 red, 2 green, 6 blue", " 2 green"


            Game game = new Game();
            games.Add(game);
            game.Id = int.Parse(gameId);

            foreach(string runString in runs)
            {
                Run run = new Run();
                foreach(string runNumberAndType in runString.Split(',')) // " 3 blue", " 4 red"
                {
                    string[] SeparatedRunNumberAndType = runNumberAndType.Trim().Split(' '); // delete whitespaces at start and end of string
                    int number = int.Parse(SeparatedRunNumberAndType[0]);
                    switch(SeparatedRunNumberAndType[1])
                    {
                        case "red":
                            run.Red += number;
                            break;
                        case "blue":
                            run.Blue += number;
                            break;
                        case "green":
                            run.Green += number;
                            break;
                        default:
                            throw new Exception("Invalid colour: " + SeparatedRunNumberAndType[1]);
                    }
                }
                game.Runs.Add(run);
            }
        }
        file.Close();

        IEnumerable<Game> PossibleGames = games.Where(x => x.IsPossible());
        int SumOfPossibileGamesIds = PossibleGames.Sum(x=>x.Id);
        Console.WriteLine($"Sum of possible games ids: {SumOfPossibileGamesIds}");
        

        int SumOfGamePowers = games.Sum(x=>x.GetGamePower());
        Console.WriteLine($"Sum of game powers: {SumOfGamePowers}");

    }
}





