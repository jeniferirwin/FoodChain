using System.Collections.Generic;
using UnityEngine;

namespace FoodChain.Core
{
    public class GridOperations : MonoBehaviour
    {
        [SerializeField] private float testSpawnRate;
        [SerializeField] private Vector3Int uLeftBounds;
        [SerializeField] private Vector3Int lRightBounds;
        [SerializeField] private GameObject grassPrefab;

        private static Grid grid;
        private static Dictionary<Vector3Int, GameObject> grasses = new Dictionary<Vector3Int, GameObject>();
        private static List<Vector3Int> emptyCells = new List<Vector3Int>();
        private static Transform grassParent;
        
        private static float _testSpawnRateTicker;
        
        private static GridOperations _instance;
        public GridOperations Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance == null)
                {
                    _instance = this;
                }
            }
        }
        
        private void Awake()
        {
            Instance = this;
            grid = GetComponent<Grid>();
            grassParent = new GameObject("GrassParent").transform;
            grassParent.position = Vector3.zero;
            _testSpawnRateTicker = testSpawnRate;
            PopulateEmptyCells();
        }
        
        private void PopulateEmptyCells()
        {
            for (int i = uLeftBounds.x; i < lRightBounds.x; i++)
            {
                for (int n = uLeftBounds.y; n > lRightBounds.y; n--)
                {
                    emptyCells.Add(new Vector3Int(i, n, 0));
                }
            }
        }
        
        private void Update()
        {
            // ABSTRACTION
            if (!RespawnOffCooldown()) return;
            if (emptyCells.Count > 0) FillCell(GetRandomEmptyCell());
        }
        
        private bool RespawnOffCooldown()
        {
            if (_testSpawnRateTicker > 0)
            {
                _testSpawnRateTicker -= Time.deltaTime;
                return false;
            }
            _testSpawnRateTicker = testSpawnRate;
            return true;
        }

        public static Vector3Int GetRandomEmptyCell()
        {
            var idx = Random.Range(0, emptyCells.Count);
            return emptyCells[idx];
        }

        public static bool IsCellOccupied(Vector3Int cell)
        {
            return grasses.ContainsKey(cell);
        }

        public static Vector3Int CellFromWorldPos(Vector3 worldPos)
        {
            return grid.WorldToCell(worldPos);
        }

        public static bool ClearCell(Vector3Int cell)
        {
            if (!IsCellOccupied(cell)) return false;
            else
            {
                if (grasses[cell] != null)
                {
                    Destroy(grasses[cell]);
                }
                grasses.Remove(cell);
                emptyCells.Add(cell);
                return true;
            }
        }

        public bool FillCell(Vector3Int cell)
        {
            if (IsCellOccupied(cell)) return false;
            var _newGrass = MakeNewGrass(cell);
            grasses.Add(cell, _newGrass);
            emptyCells.Remove(cell);
            return true;
        }

        private GameObject MakeNewGrass(Vector3Int cell)
        {
            var worldPosition = grid.CellToWorld(cell);
            var newGrass = GameObject.Instantiate(grassPrefab, worldPosition, Quaternion.identity, grassParent);
            return newGrass;
        }

        private void TestGrid()
        {
            for (int i = uLeftBounds.x; i < lRightBounds.x; i++)
            {
                for (int n = uLeftBounds.y; n > lRightBounds.y; n--)
                {
                    var _cellPos = new Vector3Int(i, n, 0);
                    FillCell(_cellPos);
                }
            }
            Debug.Log($"Number of grasses: {grasses.Count}");
        }
    }
}
