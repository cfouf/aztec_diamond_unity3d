using System.Drawing;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Entities
{
    public class Square
    {
        public Square(Point[][] square)
        {
           selfObject = GameManager.Get().InstantiateSquare(square);
        }

        public readonly Object selfObject;
    }
}

