using System.Collections.Generic;
using UnityEngine;

namespace FoodChain
{
    public class GridOperations : MonoBehaviour
    {
        [SerializeField] private float testSpawnRate;
        [SerializeField] private Vector3Int uLeftBounds;
        [SerializeField] private Vector3Int lRightBounds;
        [SerializeField] private GameObject grassPrefab;

        private Grid grid;
        private Dictionary<Vector3Int, GameObject> grasses = new Dictionary<Vector3Int, GameObject>();
        private List<Vector3Int> emptyCells = new List<Vector3Int>();
        private Transform grassParent;
        
        private float _testSpawnRateTicker;
        
        private void Awake()
        {
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
            if (_testSpawnRateTicker > 0)
            {
                _testSpawnRateTicker -= Time.deltaTime;
                return;
            }
            _testSpawnRateTicker = testSpawnRate;
            if (emptyCells.Count > 0) FillCell(GetRandomEmptyCell());
        }

        public Vector3Int GetRandomEmptyCell()
        {
            var idx = Random.Range(0, emptyCells.Count);
            return emptyCells[idx];
        }

        public bool IsCellOccupied(Vector3Int cell)
        {
            return grasses.ContainsKey(cell);
        }

        public Vector3Int CellFromWorldPos(Vector3 worldPos)
        {
            return grid.WorldToCell(worldPos);
        }

        public bool ClearCell(Vector3Int cell)
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
            // TODO: Add Grass component here
            return GameObject.Instantiate(grassPrefab, worldPosition, Quaternion.identity, grassParent);
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
