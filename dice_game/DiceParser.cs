using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceGame
{
    public class DiceParsingException : Exception
    {
        public DiceParsingException(string msg) : base(msg) { }
    }

    public class DiceParser
    {
        public List<Dice> ParseDiceArgs(string[] args)
        {
            if (args.Length < 3)
                throw new DiceParsingException("You must specify at least three dice as command line arguments.");

            var diceList = new List<Dice>();
            int minFaceCount = 2;
            int maxFaceCount = 20;

            for (int i = 0; i < args.Length; i++)
            {
                var tokens = args[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0)
                    throw new DiceParsingException($"Dice {i + 1} is empty.");

                var faces = new List<int>();
                foreach (var t in tokens)
                {
                    if (!int.TryParse(t.Trim(), out int val) || val <= 0)
                        throw new DiceParsingException("Dices must have natural numbers as values.");
                    faces.Add(val);
                }

                if (faces.Count < minFaceCount || faces.Count > maxFaceCount)
                    throw new DiceParsingException($"All dices must have at least {minFaceCount} faces and at most {maxFaceCount} faces.");

                diceList.Add(new Dice(faces));
            }

            int expectedFaceCount = diceList[0].Faces.Count;
            for (int i = 1; i < diceList.Count; i++)
            {
                if (diceList[i].Faces.Count != expectedFaceCount)
                    throw new DiceParsingException("All dices must have the same number of faces.");
            }

            return diceList;
        }
    }
}