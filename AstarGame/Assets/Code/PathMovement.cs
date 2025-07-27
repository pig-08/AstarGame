using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Astar
{
    public class PathMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private int maxPathCount = 100;
        [SerializeField] private Tilemap baseTilemap;

        [SerializeField] private Vector3[] _pathArr;

        private int _totalPathCount;

        private void Awake()
        {
            _pathArr = new Vector3[maxPathCount];
        }

        public void SetDestination(Vector3 destination)
        {
            Vector3Int startCell = baseTilemap.WorldToCell(transform.position);
            Vector3Int endCell = baseTilemap.WorldToCell(destination);
        }

        private void OnDrawGizmos()
        {
            if (_totalPathCount <= 0) return;

            for (int i = 0; i < _totalPathCount - 1; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_pathArr[i], 0.25f);
                Gizmos.DrawLine(_pathArr[i], _pathArr[i + 1]);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_pathArr[_totalPathCount - 1], 0.25f);
        }
    }
}