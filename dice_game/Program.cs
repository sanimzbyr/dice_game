using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var parser = new DiceParser();
                List<Dice> diceList = parser.ParseDiceArgs(args);

                var game = new Game(diceList);
                game.Run();
            }
            catch (DiceParsingException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine();
                Console.WriteLine("Example of correct usage:");
                Console.WriteLine("  dotnet run 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}