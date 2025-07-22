using System;
using System.Security.Cryptography;
using System.Text;
using SHA3.Net;

namespace DiceGame
{
    public class RandomService
    {
        public byte[] GenerateSecretKey(int bytes = 32)
        {
            var key = new byte[bytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        public int GenerateRandomInt(int max)
        {
            if (max <= 0) throw new ArgumentException("max must be positive");
            uint limit = (uint.MaxValue / (uint)max) * (uint)max;
            uint val;
            using (var rng = RandomNumberGenerator.Create())
            {
                do
                {
                    var bytes = new byte[4];
                    rng.GetBytes(bytes);
                    val = BitConverter.ToUInt32(bytes, 0);
                } while (val >= limit);
            }
            return (int)(val % (uint)max);
        }

        public string ComputeHMAC(byte[] key, int message)
        {
            const int blockSize = 136;
            byte[] keyBlock = new byte[blockSize];

            if (key.Length > blockSize)
            {
                var hashed = Sha3.Sha3256().ComputeHash(key);
                Array.Copy(hashed, keyBlock, hashed.Length);
            }
            else
            {
                Array.Copy(key, keyBlock, key.Length);
            }

            byte[] ipad = new byte[blockSize];
            byte[] opad = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                ipad[i] = 0x36;
                opad[i] = 0x5c;
            }

            for (int i = 0; i < blockSize; i++)
            {
                ipad[i] ^= keyBlock[i];
                opad[i] ^= keyBlock[i];
            }

            var msgBytes = BitConverter.GetBytes(message);

            var inner = new byte[ipad.Length + msgBytes.Length];
            Buffer.BlockCopy(ipad, 0, inner, 0, ipad.Length);
            Buffer.BlockCopy(msgBytes, 0, inner, ipad.Length, msgBytes.Length);
            var innerHash = Sha3.Sha3256().ComputeHash(inner);

            var outer = new byte[opad.Length + innerHash.Length];
            Buffer.BlockCopy(opad, 0, outer, 0, opad.Length);
            Buffer.BlockCopy(innerHash, 0, outer, opad.Length, innerHash.Length);
            var hmac = Sha3.Sha3256().ComputeHash(outer);

            return BitConverter.ToString(hmac).Replace("-", "");
        }
    }
}