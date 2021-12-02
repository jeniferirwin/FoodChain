using System.Collections.Generic;
using UnityEngine;

namespace FoodChain
{
    public class GridOperations : MonoBehaviour
    {
        [SerializeField] private Vector3Int uLeftBounds;
        [SerializeField] private Vector3Int lRightBounds;
        [SerializeField] private GameObject grassPrefab;
        
        private Grid grid;
        private List<GameObject> grasses = new List<GameObject>();
        private Transform grassParent;
        
        private void Awake()
        {
            grid = GetComponent<Grid>();
            grassParent = new GameObject("GrassParent").transform;
            grassParent.position = Vector3.zero;
        }
        
        private void TestGrid()
        {
            for (int i = uLeftBounds.x; i < lRightBounds.x; i++)
            {
                for (int n = uLeftBounds.y; n > lRightBounds.y; n--)
                {
                    var _cellPos = new Vector3Int(i, n, 0);
                    var _worldPos = grid.CellToWorld(_cellPos);
                    var _newGrass = GameObject.Instantiate(grassPrefab,_worldPos,Quaternion.identity,grassParent);
                    grasses.Add(_newGrass);
                }
            }
            Debug.Log($"Number of grasses: {grasses.Count}");
        }
    }
}
