using System;
using System.Drawing;
using _Scripts.Managers;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Entities
{
    public class Brick
    {
        public Brick(Tuple<Point, Point> position, Direction direction, bool isFlat)
        {
            this.position = position;
            this.direction = direction;
            this.isFlat = isFlat;
            if (isFlat)
            {
                rotation = direction switch
                {
                    Direction.Down => Quaternion.Euler(-45, 180, 0),
                    Direction.Left => Quaternion.Euler(45, -90, 0),
                    Direction.Right => Quaternion.Euler(45, 90, 0),
                    Direction.Up => Quaternion.Euler(-45, 0, 0),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
            }
            else
            {
                rotation = direction switch
                {
                    Direction.Down => Quaternion.Euler(0, 180, 0),
                    Direction.Left => Quaternion.Euler(0, -90, 0),
                    Direction.Right => Quaternion.Euler(0, 90, 0),
                    Direction.Up => Quaternion.Euler(0, 0, 0),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
            }

            selfObject = GameManager.Get().InstantiateBrick(position, direction, rotation);
        }

        public Tuple<Tuple<Point, Point>, Tuple<Point, Point>> Iterate()
        {
            var oldPosition = position;
            position = direction switch
            {
                Direction.Down => new Tuple<Point, Point>(new Point(position.Item1.X, position.Item1.Y - 1),
                    new Point(position.Item2.X, position.Item2.Y - 1)),
                Direction.Up => new Tuple<Point, Point>(new Point(position.Item1.X, position.Item1.Y + 1),
                    new Point(position.Item2.X, position.Item2.Y + 1)),
                Direction.Left => new Tuple<Point, Point>(new Point(position.Item1.X - 1, position.Item1.Y),
                    new Point(position.Item2.X - 1, position.Item2.Y)),
                Direction.Right => new Tuple<Point, Point>(new Point(position.Item1.X + 1, position.Item1.Y),
                    new Point(position.Item2.X + 1, position.Item2.Y)),
                _ => position
            };

            var newPosition = Logic.GetWorldPosition(position);
            if (isFlat)
                if (direction is Direction.Down or Direction.Up)
                    newPosition.y = selfObject.transform.position.y + 1f;
                else
                    newPosition.y = selfObject.transform.position.y - 1f;


            selfObject.transform.position = newPosition;
            return new Tuple<Tuple<Point, Point>, Tuple<Point, Point>>(oldPosition, position);
        }

        private Tuple<Point, Point> position;
        private readonly Direction direction;

        private readonly Quaternion rotation;
        private readonly bool isFlat;

        public readonly GameObject selfObject;
    }
}