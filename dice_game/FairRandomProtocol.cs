using System;
using System.Text;

namespace DiceGame
{
    public class FairRandomProtocol
    {
        private readonly RandomService _randomService;
        private readonly int _rangeMax;

        public FairRandomProtocol(RandomService randomService, int rangeMax)
        {
            _randomService = randomService;
            _rangeMax = rangeMax;
        }

        public FairRandomResult Run(string prompt)
        {
            int computerValue = _randomService.GenerateRandomInt(_rangeMax);
            byte[] key = _randomService.GenerateSecretKey();

            string hmac = _randomService.ComputeHMAC(key, computerValue);

            Console.WriteLine($"{prompt} (HMAC={hmac})");

            int userValue = GetUserSelection();

            if (userValue == -1)
                return new FairRandomResult { IsCanceled = true };

            Console.WriteLine($"My number is {computerValue} (KEY={BitConverter.ToString(key).Replace("-", "")}).");
            int finalResult = (computerValue + userValue) % _rangeMax;
            Console.WriteLine($"The fair number generation result is {computerValue} + {userValue} = {finalResult} (mod {_rangeMax}).");
            return new FairRandomResult
            {
                ComputerValue = computerValue,
                UserValue = userValue,
                Key = key,
                Hmac = hmac,
                FinalResult = finalResult,
                IsCanceled = false
            };
        }

        public int GetUserSelection()
        {
            while (true)
            {
                Console.WriteLine($"Add your number modulo {_rangeMax}.");
                for (int i = 0; i < _rangeMax; i++)
                    Console.WriteLine($"{i} - {i}");
                Console.WriteLine("X - exit");
                Console.WriteLine("? - help");
                Console.Write("Your selection: ");
                var line = Console.ReadLine()?.Trim().ToLower();
                if (line == "x")
                    return -1;
                if (line == "?")
                {
                    Console.WriteLine("Enter an integer between 0 and " + (_rangeMax - 1) + ".");
                    continue;
                }
                if (int.TryParse(line, out int sel) && sel >= 0 && sel < _rangeMax)
                    return sel;
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }

    public class FairRandomResult
    {
        public int ComputerValue { get; set; }
        public int UserValue { get; set; } 
        public byte[] Key { get; set; } = Array.Empty<byte>();
        public string Hmac { get; set; } = "";
        public int FinalResult { get; set; }
        public bool IsCanceled { get; set; }
    }
}