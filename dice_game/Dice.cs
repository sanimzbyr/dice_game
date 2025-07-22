using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceGame
{
    public class Dice
    {
        public List<int> Faces { get; }
        public string Description => string.Join(",", Faces);

        public Dice(IEnumerable<int> faces)
        {
            Faces = faces.ToList();
            if (Faces.Count < 2 || Faces.Count > 20)
                throw new ArgumentException("Dice must have at least 2 faces and at most 20 faces.");
        }

        public int FaceCount => Faces.Count;
        public int GetFace(int index) => Faces[index];
    }
}