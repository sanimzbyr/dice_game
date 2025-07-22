using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceGame
{
    public class Game
    {
        private List<Dice> _diceList;
        private RandomService _randService = new RandomService();
        private ProbabilityCalculator _probCalc = new ProbabilityCalculator();
        private HelpTableRenderer _helpTable;

        public Game(List<Dice> diceList)
        {
            _diceList = diceList;
            _helpTable = new HelpTableRenderer(_probCalc);
        }

        public void Run()
        {
            Console.WriteLine("Let's determine who makes the first move.");
            int diceFaceCount = _diceList[0].FaceCount;

            var firstMoveProtocol = new FairRandomProtocol(_randService, 2);
            var fairResult = firstMoveProtocol.Run("I selected a random value in the range 0..1");
            if (fairResult.IsCanceled) { Console.WriteLine("Game canceled."); return; }

            bool computerMovesFirst = fairResult.FinalResult != 0;
            if (computerMovesFirst)
                Console.WriteLine("I make the first move and choose my dice.");
            else
                Console.WriteLine("You make the first move and choose your dice.");

            int computerDiceIdx = -1, userDiceIdx = -1;
            if (computerMovesFirst)
            {
                computerDiceIdx = ComputerSelectDice();
                userDiceIdx = UserSelectDice(computerDiceIdx);
                if (userDiceIdx == -1) return;
            }
            else
            {
                userDiceIdx = UserSelectDice();
                if (userDiceIdx == -1) return;
                computerDiceIdx = ComputerSelectDice(userDiceIdx);
            }

            var computerDice = _diceList[computerDiceIdx];
            var userDice = _diceList[userDiceIdx];
            Console.WriteLine($"Computer dice: [{computerDice.Description}]");
            Console.WriteLine($"Your dice: [{userDice.Description}]");

            Console.WriteLine("\nIt's time for my roll.");
            var compRollProtocol = new FairRandomProtocol(_randService, diceFaceCount);
            var compRollResult = compRollProtocol.Run($"I selected a random value in the range 0..{diceFaceCount - 1}");
            if (compRollResult.IsCanceled) return;
            int computerRollFace = computerDice.GetFace(compRollResult.FinalResult);
            Console.WriteLine($"My roll result is {computerRollFace}.");

            Console.WriteLine("\nIt's time for your roll.");
            var userRollProtocol = new FairRandomProtocol(_randService, diceFaceCount);
            var userRollResult = userRollProtocol.Run($"I selected a random value in the range 0..{diceFaceCount - 1}");
            if (userRollResult.IsCanceled) return;
            int userRollFace = userDice.GetFace(userRollResult.FinalResult);
            Console.WriteLine($"Your roll result is {userRollFace}.");

            if (userRollFace > computerRollFace)
                Console.WriteLine("You win ({0} > {1})!", userRollFace, computerRollFace);
            else if (userRollFace < computerRollFace)
                Console.WriteLine("I win ({0} > {1})!", computerRollFace, userRollFace);
            else
                Console.WriteLine("It's a draw ({0} = {1})!", userRollFace, computerRollFace);
        }

        private int UserSelectDice(int? excludeIdx = null)
        {
            while (true)
            {
                Console.WriteLine("Choose your dice:");
                for (int i = 0; i < _diceList.Count; i++)
                {
                    if (i == excludeIdx) continue;
                    Console.WriteLine($"{i} - {_diceList[i].Description}");
                }
                Console.WriteLine("X - exit");
                Console.WriteLine("? - help");
                Console.Write("Your selection: ");
                var line = Console.ReadLine()?.Trim().ToLower();
                if (line == "x")
                    return -1;
                if (line == "?")
                {
                    _helpTable.Render(_diceList);
                    continue;
                }
                if (int.TryParse(line, out int idx) &&
                    idx >= 0 && idx < _diceList.Count && idx != excludeIdx)
                    return idx;
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        private int ComputerSelectDice(int? excludeIdx = null)
        {
            double bestProb = double.NegativeInfinity;
            int bestIdx = -1;
            for (int i = 0; i < _diceList.Count; i++)
            {
                if (i == excludeIdx) continue;
                double probSum = 0;
                for (int j = 0; j < _diceList.Count; j++)
                {
                    if (j == i || j == excludeIdx) continue;
                    probSum += _probCalc.ProbabilityWin(_diceList[i], _diceList[j]);
                }
                if (probSum > bestProb)
                {
                    bestProb = probSum;
                    bestIdx = i;
                }
            }
            Console.WriteLine($"I choose the [{_diceList[bestIdx].Description}] dice.");
            return bestIdx;
        }
    }
}