using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;

Problem2TestString("123sevenine");
Problem2();



//DAY 1, problem 1

//ogni linea ha una stringa che contiene due numeri
//concatenando questi due numeri si forma un numero a due cifre
//somma tutti i numeri a due cifre
void Problem1()
{
    using StreamReader reader = new StreamReader("input.txt");
    int sum = 0;
    while (!reader.EndOfStream)
    {
        string? line = reader.ReadLine();
        if (line is null)
            throw new Exception("Invalid input file");

        var numeriInStringa = line.ToList<char>().Where(x => Char.IsNumber(x));
        char primoNumero = numeriInStringa.First();
        char ultimoNumero = numeriInStringa.Last();
        
        string numeriConcatenati = primoNumero.ToString() + ultimoNumero;
        int numero = int.Parse(numeriConcatenati);
        sum += numero;
    }

    Console.WriteLine("Somma: " + sum);
}


//stessa cosa di prima, ma i numeri possono anche essere scritti a lettere
//uso un regex per matchare tutto
void Problem2()
{
    //Regex regex = new Regex("one|two|three|four|five|six|seven|eight|nine|\\d", );
    Regex regex = new Regex("(?=(one|two|three|four|five|six|seven|eight|nine))|\\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    using StreamReader reader = new StreamReader("input.txt");
    int sum = 0;
    while (!reader.EndOfStream)
    {
        string? line = reader.ReadLine();
        if (line is null)
            throw new Exception("Invalid input file");
        
        try
        {
            List<System.Text.RegularExpressions.Group> AllMatches = new();
            foreach(Match match in regex.Matches(line))
                AllMatches.AddRange(match.Groups.Values.Where(x => x.Length != 0));

            char primoNumero = GetCharFromString(AllMatches.First().ToString());
            char ultimoNumero = GetCharFromString(AllMatches.Last().ToString());
            
            string numeriConcatenati = primoNumero.ToString() + ultimoNumero;
            int numero = int.Parse(numeriConcatenati);
            sum += numero;
        }
        catch (Exception exc)
        {
            throw new Exception("Error processing line " + line, exc);
        }
    }
    Console.WriteLine("Somma: " + sum);
}

void Problem2TestString(string str)
{
    Regex regex = new Regex("(?=(one|two|three|four|five|six|seven|eight|nine))|\\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    
    var matches = regex.Matches(str);
    foreach(Match match in matches)
    {
        foreach(var v in match.Groups.Values.Where(x => x.Length != 0))
        {
            Console.WriteLine(v);
        }
    }
}

char GetCharFromRegexMatch(Match match)
{
    //we now know that match is one of "one", "two", ..., "nine" or a digit
    string numberString = match.Value;
    return GetCharFromString(numberString);
}

char GetCharFromString(string numberString)
{
    if (numberString.Length == 1)
    {
        //we know it's a digit
        return (char)numberString[0];
    }
    else
    {
        return numberString switch{
            "one" => '1',
            "two" => '2',
            "three" => '3',
            "four" => '4',
            "five" => '5',
            "six" => '6',
            "seven" => '7',
            "eight" => '8',
            "nine" => '9',
            _ => throw new ArgumentException($"Unexpected input: '{numberString}'")
        };
    }
}