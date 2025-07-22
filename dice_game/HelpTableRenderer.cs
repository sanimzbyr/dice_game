using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceGame
{
    public class HelpTableRenderer
    {
        private readonly ProbabilityCalculator _probCalc;

        public HelpTableRenderer(ProbabilityCalculator probCalc)
        {
            _probCalc = probCalc;
        }

        public void Render(List<Dice> diceList)
        {
            Console.WriteLine("This table shows the probability that the user wins for each pair of dice.\n");

            var headers = new List<string> { "User dice v" };
            headers.AddRange(diceList.Select(d => d.Description));

            int colWidth = headers.Max(h => h.Length);
            foreach (var d in diceList)
                colWidth = Math.Max(colWidth, d.Description.Length);
            colWidth = Math.Max(colWidth, "𝟹𝟹𝟹𝟹".Length);
            colWidth = Math.Max(colWidth, "0.0000".Length);

            int columns = headers.Count;
            int tableWidth = columns * (colWidth + 2) + (columns + 1);

            Console.WriteLine("+" + string.Join("", Enumerable.Repeat(new string('-', colWidth + 2) + "+", columns)));

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("|");
            foreach (var h in headers)
            {
                Console.Write(" " + h.PadRight(colWidth) + " |");
            }
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("+" + string.Join("", Enumerable.Repeat(new string('-', colWidth + 2) + "+", columns)));

            for (int r = 0; r < diceList.Count; r++)
            {
                Console.Write("|");
                Console.Write(" " + diceList[r].Description.PadRight(colWidth) + " |");
                for (int j = 0; j < diceList.Count; j++)
                {
                    string cell;
                    if (r == j)
                        cell = "𝟹𝟹𝟹𝟹";
                    else
                        cell = _probCalc.ProbabilityWin(diceList[r], diceList[j]).ToString("0.0000");

                    Console.Write(" " + cell.PadRight(colWidth) + " |");
                }
                Console.WriteLine();

                Console.WriteLine("+" + string.Join("", Enumerable.Repeat(new string('-', colWidth + 2) + "+", columns)));
            }

            Console.WriteLine();
        }
    }
}