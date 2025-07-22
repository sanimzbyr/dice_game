using System.Collections.Generic;

namespace DiceGame
{
    public class ProbabilityCalculator
    {
        public double ProbabilityWin(Dice diceA, Dice diceB)
        {
            int win = 0, total = 0;
            foreach (var a in diceA.Faces)
                foreach (var b in diceB.Faces)
                {
                    if (a > b) win++;
                    total++;
                }
            return total > 0 ? (double)win / total : 0;
        }

        public double ProbabilityDraw(Dice diceA, Dice diceB)
        {
            int draw = 0, total = 0;
            foreach (var a in diceA.Faces)
                foreach (var b in diceB.Faces)
                {
                    if (a == b) draw++;
                    total++;
                }
            return total > 0 ? (double)draw / total : 0;
        }
    }
}