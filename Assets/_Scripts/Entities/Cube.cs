using _Scripts._3d;
using UnityEngine;

namespace _Scripts.Entities
{
    public class Cube
    {
        public Cube(Vector3 position)
        {
            Position = position;
            SelfObject = GameManager.Get().InstantiateCube(this);
        }

        public Vector3 Position { get; }
        public GameObject SelfObject { get; set; }
    }
}
