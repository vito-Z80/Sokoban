using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using JetBrains.Annotations;
using Objects;
using Objects.Boxes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ClassicLevels
{
    public class ClassicLevelBuilder
    {
        CameraManager m_cameraManager;
        Assembler m_character;
        Transform m_pointsTransform;
        Transform m_boxesTransform;
        Transform m_levelTransform;


        const string LevelIdFormat = "000";

        int m_floorLayerMask;
        int m_wallsLayerMask;

        bool m_isLevelFinished;

        ClassicLevelsRecipient m_levelsRecipient;
        ClassicLevelModel m_levelModel;


        GameObject[] m_floorPrefabs;
        GameObject m_wallPrefab;
        GameObject m_pointPrefab;
        GameObject m_boxPrefab;

        [CanBeNull] public ContactorBoxContainer[] boxContainers;
        public Box[] boxes;

        public ClassicLevelBuilder(CameraManager cameraManager, Assembler character, Transform pointsTransform, Transform boxesTransform, Transform levelTransform)
        {
            m_floorLayerMask = LayerMask.NameToLayer("Floor");
            m_wallsLayerMask = LayerMask.NameToLayer("Wall");
            m_cameraManager = cameraManager;
            m_character = character;
            m_pointsTransform = pointsTransform;
            m_boxesTransform = boxesTransform;
            m_levelTransform = levelTransform;
        }


        
        
        async Task LoadPrefabs()
        {
            m_floorPrefabs ??= await LoadAssetsByLabel("ClassicFloor");
            m_wallPrefab ??= await LoadPrefab("StoneBlockFourth1x1");
            m_pointPrefab ??= await LoadPrefab("MetallicPoint");
            m_boxPrefab ??= await LoadPrefab("MetallicBox");
        }


        public async Task<bool> NextLevel(int levelIndex)
        {
            await LoadPrefabs();
            var levelName = $"ClassicLevel_{levelIndex.ToString(LevelIdFormat).Trim()}";
            m_levelsRecipient ??= new ClassicLevelsRecipient();
            m_levelModel = await m_levelsRecipient.GetModel(levelName);
            if (m_levelModel == null) return false;
            InstantiateFloor(m_levelModel.Walls, m_levelModel.Buttons);
            InstantiatePoints(m_levelModel.Buttons);
            InstantiateWalls(m_levelModel.Walls, m_levelModel.Buttons);
            InstantiateBoxes(m_levelModel.Boxes);
            InstantiateCharacter(m_levelModel.Player);

            m_cameraManager.SetFollow();
            m_cameraManager.SetCameraState(CameraManager.State.FollowCharacter);
            UndoController.CollectUndoableObjects(m_boxesTransform.gameObject, m_character);

            return true;
        }


        void InstantiateCharacter(Position characterPosition)
        {
            var position = new Vector3(characterPosition.X, -0.5f, characterPosition.Y);
            m_character.transform.position = position;
            m_character.TargetPosition = position;
            m_character.Freezed = false;
        }

        void InstantiatePoints(Position[] levelModelPoints)
        {
            var bc = new List<ContactorBoxContainer>();
            foreach (var point in levelModelPoints)
            {
                var position = new Vector3(point.X, -1.0f, point.Y);
                var b = Object.Instantiate(m_pointPrefab, m_pointsTransform);
                b.transform.position = position;
                if (b.TryGetComponent<ContactorBoxContainer>(out var p))
                {
                    bc.Add(p);
                    p.pointColor = BoxColor.Red;
                }
            }

            boxContainers = bc.ToArray();
        }

        void InstantiateBoxes(Position[] levelModelBoxes)
        {
            var b = new List<Box>();
            foreach (var boxPosition in levelModelBoxes)
            {
                var position = new Vector3(boxPosition.X, 0.0f, boxPosition.Y);
                var boxGameObject = Object.Instantiate(m_boxPrefab, m_boxesTransform);
                boxGameObject.transform.position = position;
                if (boxGameObject.TryGetComponent<Box>(out var box))
                {
                    box.TargetPosition = boxGameObject.transform.position;
                    box.canFall = false;
                    box.Freezed = false;
                    box.boxColor = BoxColor.Red;
                    b.Add(box);
                }
            }
            boxes = b.ToArray();
        }


        void InstantiateFloor(int[][] walls, Position[] points)
        {
            int[][] floorCells = new int[walls.Length][];
            for (var y = 0; y < walls.Length; y++)
            {
                floorCells[y] = new int[walls[y].Length];
                for (var x = 0; x < walls[y].Length; x++)
                {
                    if (walls[y][x] == 0 && points.FirstOrDefault(p => p.X == x && p.Y == y) == null)
                    {
                        floorCells[y][x] = 1;
                        var position = new Vector3(x, -1, y);
                        var floor = Object.Instantiate(GetRandomFloorPrefab(), m_levelTransform);
                        floor.AddComponent<BoxCollider>();
                        floor.layer = m_floorLayerMask;
                        floor.transform.position = position;
                        RandomRotate(floor, Vector3.up);
                    }
                }
            }
        }

        void InstantiateWalls(int[][] wallCells, Position[] points)
        {
            for (var y = 0; y < wallCells.Length; y++)
            {
                for (var x = 0; x < wallCells[y].Length; x++)
                {
                    if (wallCells[y][x] == 0)
                    {
                        SetWallsAroundFloor(wallCells, x, y, points);
                    }
                }
            }


            for (var y = 0; y < wallCells.Length; y++)
            {
                for (var x = 0; x < wallCells[y].Length; x++)
                {
                    if (wallCells[y][x] == 2)
                    {
                        var position = new Vector3(x, 0, y);
                        var wall = Object.Instantiate(m_wallPrefab, m_levelTransform);
                        wall.transform.position = position;
                        var boxCollider = wall.AddComponent<BoxCollider>();
                        boxCollider.size = Vector3.one * 0.9f;
                        wall.layer = m_wallsLayerMask;
                        RandomRotate(wall, Vector3.one);
                    }
                }
            }

            for (var y = 0; y < wallCells.Length; y++)
            {
                for (var x = 0; x < wallCells[y].Length; x++)
                {
                    if (wallCells[y][x] == 2)
                    {
                        var position = new Vector3(x, -1, y);
                        var floor = Object.Instantiate(GetRandomFloorPrefab(), m_levelTransform);
                        floor.AddComponent<BoxCollider>();
                        floor.layer = m_floorLayerMask;
                        floor.transform.position = position;
                        RandomRotate(floor, Vector3.up);
                    }
                }
            }
        }


        void SetWallsAroundFloor(int[][] walls, int x, int y, Position[] points)
        {
            if (walls[y][x - 1] == 1 && points.FirstOrDefault(p => p.X == x - 1 && p.Y == y) == null) walls[y][x - 1] = 2;
            if (walls[y - 1][x] == 1 && points.FirstOrDefault(p => p.X == x && p.Y == y - 1) == null) walls[y - 1][x] = 2;
            if (walls[y - 1][x - 1] == 1 && points.FirstOrDefault(p => p.X == x - 1 && p.Y == y - 1) == null) walls[y - 1][x - 1] = 2;
            if (walls[y][x + 1] == 1 && points.FirstOrDefault(p => p.X == x + 1 && p.Y == y) == null) walls[y][x + 1] = 2;
            if (walls[y + 1][x] == 1 && points.FirstOrDefault(p => p.X == x && p.Y == y + 1) == null) walls[y + 1][x] = 2;
            if (walls[y + 1][x + 1] == 1 && points.FirstOrDefault(p => p.X == x + 1 && p.Y == y + 1) == null) walls[y + 1][x + 1] = 2;
            if (walls[y + 1][x - 1] == 1 && points.FirstOrDefault(p => p.X == x - 1 && p.Y == y + 1) == null) walls[y + 1][x - 1] = 2;
            if (walls[y - 1][x + 1] == 1 && points.FirstOrDefault(p => p.X == x + 1 && p.Y == y - 1) == null) walls[y - 1][x + 1] = 2;
        }


        GameObject GetRandomFloorPrefab()
        {
            var index = Random.Range(0, m_floorPrefabs.Length);
            return m_floorPrefabs[index];
        }


        void RandomRotate(GameObject go, Vector3 axis)
        {
            if (axis.x != 0.0f)
            {
                axis.x = GetRandomAngle();
            }

            if (axis.y != 0.0f)
            {
                axis.y = GetRandomAngle();
            }

            if (axis.z != 0.0f)
            {
                axis.z = GetRandomAngle();
            }

            go.transform.rotation = Quaternion.Euler(axis);
        }

        float GetRandomAngle()
        {
            return Mathf.Round(Random.value * 360.0f / 90.0f) * 90.0f;
        }

        async Task<GameObject[]> LoadAssetsByLabel(string label)
        {
            var handle = Addressables.LoadAssetsAsync<GameObject>(label);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                return null;

            return new List<GameObject>(handle.Result).ToArray();
        }


        Task<GameObject> LoadPrefab(string prefabName)
        {
            return Addressables.LoadAssetAsync<GameObject>(prefabName).Task;
        }
    }
}