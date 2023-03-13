using System;
using System.Collections.Generic;
using System.Drawing;
using _Scripts.Entities;
using _Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Managers
{
    public class GameManager : MonoBehaviourService<GameManager>
    {
        [SerializeField] private ClickableObject clickArea;
        [SerializeField] private int numberOfIterationsPerSpeedIterate;
        [SerializeField] private TMP_Text sizeCount;
        [SerializeField] private bool areBricksFlat;
        private readonly List<Brick> bricks = new();
        private readonly List<Square> squares = new();
        private HashSet<Point> eligiblePositions = new();
        private int iterationCount;


        protected override void OnCreateService()
        {
            eligiblePositions = Logic.GetAllPositions(iterationCount);
            iterationCount++;
            Iterate();
            clickArea.OnClick += Iterate;
        }

        private void Iterate(PointerEventData obj = null)
        {
            iterationCount++;
            switch (iterationCount % 3)
            {
                case 0:
                    IterateStepOne();
                    break;
                case 1:
                    IterateStepTwo();
                    break;
                default:
                    IterateStepThree();
                    break;
            }
        }

        private void IterateStepOne()
        {
            foreach (var square in Logic.FindSquares(eligiblePositions))
                squares.Add(new Square(square));
        }

        private void IterateStepTwo()
        {
            foreach (var square in squares)
                Destroy(square.selfObject);
            squares.Clear();

            foreach (var square in Logic.FindSquares(eligiblePositions))
            {
                var newBricks = Logic.GeneratePairOfBricks(square, areBricksFlat);
                bricks.Add(newBricks.Item1);
                bricks.Add(newBricks.Item2);
            }
        }

        private void IterateStepThree()
        {
            CameraManager.Get().StartMoving();
            var oldBricks = new Dictionary<Tuple<Point, Point>, Brick>();
            var oldAndNewPositions = new Dictionary<Brick, Tuple<Tuple<Point, Point>, Tuple<Point, Point>>>();
            var newPositions = new List<Point>();
            var brickToRemove = new HashSet<Brick>();
            eligiblePositions = Logic.GetAllPositions((iterationCount + 1) / 3);
            sizeCount.text = ((iterationCount + 1) / 3).ToString();
            foreach (var brick in bricks)
            {
                if (brickToRemove.Contains(brick))
                    continue;
                var positions = brick.Iterate();
                if (oldBricks.TryGetValue(positions.Item2, out var oldBrick) &&
                    oldAndNewPositions.TryGetValue(oldBrick, out var oldAndNewPosition) &&
                    Equals(oldAndNewPosition.Item1, positions.Item2) &&
                    Equals(oldAndNewPosition.Item2, positions.Item1))
                {
                    Destroy(oldBrick.selfObject);
                    Destroy(brick.selfObject);
                    brickToRemove.Add(brick);
                    brickToRemove.Add(oldBrick);

                    newPositions.Remove(oldAndNewPosition.Item2.Item1);
                    newPositions.Remove(oldAndNewPosition.Item2.Item2);
                    continue;
                }

                oldBricks.Add(positions.Item1, brick);
                oldAndNewPositions.Add(brick,
                    new Tuple<Tuple<Point, Point>, Tuple<Point, Point>>(positions.Item1, positions.Item2));

                newPositions.Add(positions.Item2.Item1);
                newPositions.Add(positions.Item2.Item2);
            }

            bricks.RemoveAll(brickToRemove.Contains);
            eligiblePositions.ExceptWith(newPositions);
        }

        public GameObject InstantiateBrick(Tuple<Point, Point> position, Direction direction, Quaternion rotation)
        {
            var worldPosition = Logic.GetWorldPosition(position);
            return Instantiate(areBricksFlat ? Logic.GetTilePrefab(direction, "F") : Logic.GetTilePrefab(direction),
                worldPosition, rotation);
        }

        public GameObject InstantiateSquare(Point[][] square)
        {
            return Instantiate(Resources.Load<GameObject>($"Square"),
                Logic.GetWorldPosition(square),
                Quaternion.Euler(90, 0, 0));
        }

        public void IterateQuiteAFewTimes()
        {
            for (var i = 0; i < numberOfIterationsPerSpeedIterate || iterationCount % 3 != 1; i++)
                Iterate();
        }

        protected override void OnDestroyService()
        {
        }
    }
}