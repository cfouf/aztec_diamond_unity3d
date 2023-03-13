using System;
using System.Collections.Generic;
using System.Drawing;
using _Scripts.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Utilities
{
    public abstract class Logic
    {
        public static Direction GetRandomDirection() =>
            (Direction) Random.Range(0, 4);

        public static GameObject GetTilePrefab(Direction direction, string suffix = null) =>
            direction switch
            {
                Direction.Up => Resources.Load<GameObject>($"Blue" + suffix),
                Direction.Down => Resources.Load<GameObject>($"Green" + suffix),
                Direction.Left => Resources.Load<GameObject>($"Yellow" + suffix),
                Direction.Right => Resources.Load<GameObject>($"Red" + suffix),
                _ => null
            };

        public static Direction GetOppositeDirection(Direction direction) =>
            direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

        public static Direction GetPerpendicularDirection(Direction direction) =>
            direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

        public static Tuple<Brick, Brick> GeneratePairOfBricks(Point[][] square, bool isFlat)
        {
            if (square.Length != 2 || square[0].Length != 2 || square[1].Length != 2)
                throw new ArgumentException(nameof(square));

            var direction1 = GetRandomDirection();
            var direction2 = GetOppositeDirection(direction1);

            return direction1 switch
            {
                Direction.Up => new Tuple<Brick, Brick>(
                    new Brick(new Tuple<Point, Point>(square[0][1], square[1][1]), direction1, isFlat),
                    new Brick(new Tuple<Point, Point>(square[0][0], square[1][0]), direction2, isFlat)),
                Direction.Down => new Tuple<Brick, Brick>(
                    new Brick(new Tuple<Point, Point>(square[0][0], square[1][0]), direction1, isFlat),
                    new Brick(new Tuple<Point, Point>(square[0][1], square[1][1]), direction2, isFlat)),
                Direction.Left => new Tuple<Brick, Brick>(
                    new Brick(new Tuple<Point, Point>(square[0][0], square[0][1]), direction1, isFlat),
                    new Brick(new Tuple<Point, Point>(square[1][0], square[1][1]), direction2, isFlat)),
                Direction.Right => new Tuple<Brick, Brick>(
                    new Brick(new Tuple<Point, Point>(square[1][0], square[1][1]), direction1, isFlat),
                    new Brick(new Tuple<Point, Point>(square[0][0], square[0][1]), direction2, isFlat)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static Vector3 GetWorldPosition(Tuple<Point, Point> position)
        {
            var x = position.Item1.X + position.Item2.X;
            var y = position.Item1.Y + position.Item2.Y;
            return new Vector3(x, 0, y);
        }
        
        public static Vector3 GetWorldPosition(Point[][] position)
        {
            var x = position[0][0].X + position[0][1].X + 1;
            var y = position[0][0].Y + position[0][1].Y;
            return new Vector3(x, 0, y);
        }
        
        public static Vector3 GetWorldPosition(Point position)
        {
            var x = position.X * 2;
            var y = position.Y * 2;
            return new Vector3(x, 0, y);
        }

        public static IEnumerable<Point[][]> FindSquares(HashSet<Point> positions)
        {
            var squares = new List<Point[][]>();
            var usedPositions = new HashSet<Point>();
            foreach (var position in positions)
            {
                if (!positions.Contains(new Point(position.X + 1, position.Y)) ||
                    !positions.Contains(new Point(position.X, position.Y + 1)) ||
                    !positions.Contains(new Point(position.X + 1, position.Y + 1)))
                    continue;
                
                var newPoint2 = new Point(position.X + 1, position.Y);  
                var newPoint3 = new Point(position.X, position.Y + 1);
                var newPoint4 = new Point(position.X + 1, position.Y + 1);
                if (usedPositions.Contains(newPoint2) || usedPositions.Contains(newPoint3) ||
                    usedPositions.Contains(newPoint4) || usedPositions.Contains(position))
                    continue;
                var square = new[]
                {
                    new[] {position, newPoint3},
                    new[] {newPoint2, newPoint4}
                };
                
                usedPositions.Add(position);
                usedPositions.Add(newPoint2);
                usedPositions.Add(newPoint3);
                usedPositions.Add(newPoint4);
                squares.Add(square);
            }
            
            return squares;
        }

        
        public static HashSet<Point> GetAllPositions(int iterationCount)
        {
            var positions = new HashSet<Point>();
            for (var y = -iterationCount; y < iterationCount; y++)
            {
                for (var x = -iterationCount; x < iterationCount; x++)
                {
                    if (Math.Abs(x + 0.5) + Math.Abs(y + 0.5) > iterationCount)
                        continue;
                    positions.Add(new Point(x, y));
                }
            }
            return positions;
        }
    }
}