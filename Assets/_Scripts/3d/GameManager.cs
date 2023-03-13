using System;
using System.Collections.Generic;
using _Scripts.Entities;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts._3d
{
    public class GameManager : MonoBehaviourService<GameManager>
    {
        public int size;
        public int iterationsPerFrame;
        [SerializeField] private ClickableObject clickArea;
        private readonly Dictionary<Vector2, Stack<Cube>> cubes = new();
        public bool isIterating;


        protected override void OnCreateService()
        {
            clickArea.OnClick += Iterate;
        }

        public void StopStart() => isIterating = !isIterating;

        public void StartScene()
        {
            GenerateEdge();
            isIterating = true;
        }


        private void Update()
        {
            if (!isIterating) return;
            IterateNTimes(iterationsPerFrame);
        }


        private void IterateNTimes(int n)
        {
            for (var i = 0; i < n; i++)
                Iterate();
        }

        private bool IsCubeLower(float cubeHeight, int x, int z)
        {
            var v1 = new Vector2(x - 1, z - 1);
            var v2 = new Vector2(x - 1, z);
            var v3 = new Vector2(x, z - 1);
            if (x - 1 != 0)
                if (cubes[v2].Peek().Position.y < cubeHeight)
                    return true;

            if (z - 1 != 0)
                if (cubes[v3].Peek().Position.y < cubeHeight)
                    return true;

            if (x - 1 == 0 || z - 1 == 0) return false;
            return cubes[v1].Peek().Position.y < cubeHeight;
        }

        private bool IsCubeHigher(float cubeHeight, int x, int z)
        {
            var v1 = new Vector2(x + 1, z + 1);
            var v2 = new Vector2(x + 1, z);
            var v3 = new Vector2(x, z + 1);
            if (x + 1 != size - 1)
                if (cubes[v2].Peek().Position.y > cubeHeight)
                    return true;

            if (z + 1 != size - 1)
                if (cubes[v3].Peek().Position.y > cubeHeight)
                    return true;

            if (x + 1 == size - 1 || z + 1 == size - 1) return false;
            return cubes[v1].Peek().Position.y > cubeHeight;
        }

        private void Iterate(PointerEventData data = null)
        {
            while (true)
            {
                var (x, z) = GetRandomXZ();
                var upOrDown = UnityEngine.Random.Range(0, 2);
                var curV = new Vector2(x, z);
                if (upOrDown == 0)
                {
                    var curCubeY = cubes[curV].Peek().Position.y + 1;
                    if (curCubeY == size - 1)
                        continue;

                    if (IsCubeLower(curCubeY, x, z)) continue;
                    cubes[new Vector2(x, z)].Push(new Cube(new Vector3(x, curCubeY, z)));
                }
                else
                {
                    var curCubeY = cubes[curV].Peek().Position.y - 1;
                    if (curCubeY == -1)
                        continue;

                    if (IsCubeHigher(curCubeY, x, z)) continue;

                    Destroy(cubes[curV].Pop().SelfObject);
                }

                break;
            }
        }

        private void GenerateEdge()
        {
            var borderCube = Resources.Load<GameObject>($"CubeR");
            for (var x = 0; x < size; x++)
            for (var y = 0; y < size; y++)
            for (var z = 0; z < size; z++)
            {
                if (IsBorderCube(x, y, z))
                    Instantiate(borderCube, new Vector3(x, y, z), Quaternion.identity);

                if (IsCornerCube(x, y, z))
                    continue;

                AddCubeToStack(x, y, z);
            }

            GenerateCubes();
        }

        private bool IsBorderCube(int x, int y, int z)
        {
            return x == 0 && y == 0 || x == 0 && z == 0 || y == 0 && z == 0 || x == 0 && y == size - 1 ||
                   x == 0 && z == size - 1 || y == 0 && z == size - 1 || x == size - 1 && y == 0 ||
                   x == size - 1 && z == 0 || y == size - 1 && z == 0;
        }

        private bool IsCornerCube(int x, int y, int z)
        {
            return (x != 0 || y == 0 || z == 0 || y == size - 1 || z == size - 1) &&
                   (y != 0 || x == 0 || z == 0 || x == size - 1 || z == size - 1) &&
                   (z != 0 || x == 0 || y == 0 || x == size - 1 || y == size - 1);
        }

        private void AddCubeToStack(int x, int y, int z)
        {
            if (!cubes.ContainsKey(new Vector2(x, z)))
                cubes[new Vector2(x, z)] = new Stack<Cube>();

            cubes[new Vector2(x, z)].Push(new Cube(new Vector3(x, y, z)));
        }


        private void GenerateCubes()
        {
            for (var x = 1; x < size; x++)
            for (var y = 1; y < size; y++)
            for (var z = 1; z < size; z++)
            {
                if (x + z + y >= size + 1) continue;
                var cube = new Cube(new Vector3(x, y, z));
                if (!cubes.ContainsKey(new Vector2(x, z)))
                    cubes.Add(new Vector2(x, z), new Stack<Cube>());
                cubes[new Vector2(x, z)].Push(cube);
            }
        }

        private Tuple<int, int> GetRandomXZ()
        {
            var x = UnityEngine.Random.Range(1, size - 1);
            var z = UnityEngine.Random.Range(1, size - 1);
            return new Tuple<int, int>(x, z);
        }

        public GameObject InstantiateCube(Cube cube) =>
            Instantiate(Resources.Load<GameObject>($"CubeY"), cube.Position, Quaternion.identity);


        protected override void OnDestroyService()
        {
        }
    }
}