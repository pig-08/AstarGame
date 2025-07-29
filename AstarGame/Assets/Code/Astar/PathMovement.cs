using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Astar
{
    public class PathMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO inputSO;
        [SerializeField] private PathAgent agent;
        [SerializeField] private float moveSpeed;
        [SerializeField] private int maxPathCount = 100;
        [SerializeField] private Tilemap baseTilemap;
        
        [SerializeField] private Vector3[] _pathArr;

        private int _movePointIndex = 1;
        private int _totalPathCount;

        private Rigidbody2D _playerRigid;

        private Vector3 _moveDir;
        private Vector3 _movePoint;

        private void Awake()
        {
            _pathArr = new Vector3[maxPathCount];

            _playerRigid = agent.GetComponent<Rigidbody2D>();
            inputSO.OnClickPressedEvent += SetDestination;
        }

        private void OnDestroy()
        {
            inputSO.OnClickPressedEvent -= SetDestination;
        }

        public void SetDestination(Vector3 destination)
        {
            Vector3Int startCell = baseTilemap.WorldToCell(transform.position);
            Vector3Int endCell = baseTilemap.WorldToCell(destination);

            for (int i = 0; i < maxPathCount; ++i)
                _pathArr[i] = Vector2.zero;

            _movePointIndex = 1;
            _totalPathCount = agent.GetPath(startCell, endCell, _pathArr);
        }

        private void FixedUpdate()
        {
            SetMoveDir();
        }


        public void SetMoveDir()
        {
            if (Vector3.Distance(agent.transform.position, _movePoint) <= 0.2f)
            {
                if (_movePointIndex >= _totalPathCount)
                    _playerRigid.linearVelocity = Vector2.zero;
                else
                {
                    _movePoint = _pathArr[_movePointIndex++];
                    _moveDir = (_movePoint - agent.transform.position).normalized;
                }
            }
            else
                _playerRigid.linearVelocity = _moveDir * moveSpeed;

        }

        private void OnDrawGizmos()
        {
            if (_totalPathCount <= 0) return;

            for (int i = 0; i < _totalPathCount - 1; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_pathArr[i], 0.25f);
                Gizmos.DrawLine(_pathArr[i], _pathArr[i+1]); 
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_pathArr[_totalPathCount - 1 ], 0.25f);
        }
    }
}