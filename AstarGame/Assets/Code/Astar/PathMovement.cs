using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Astar
{
    public class PathMovement : MonoBehaviour, IAgentCompo
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private int maxPathCount = 100;
        [SerializeField] private Tilemap baseTilemap;
        
        [SerializeField] private Vector3[] _pathArr;

        public bool IsMove { get; private set; }

        private PathAgent _agent;
        private PathAnimator _animator;

        private int _movePointIndex = 1;
        private int _totalPathCount;

        private Rigidbody2D _playerRigid;

        private Vector3 _moveDir;
        private Vector3 _movePoint;

        public void Init(PathAgent agent)
        {
            _agent = agent;

            _animator = _agent.GetCompo<PathAnimator>();

            _pathArr = new Vector3[maxPathCount];

            _playerRigid = agent.GetComponent<Rigidbody2D>();
            _movePoint = transform.position;
        }

        public void SetDestination(Vector3 destination)
        {
            Vector3Int startCell = baseTilemap.WorldToCell(transform.position);
            Vector3Int endCell = baseTilemap.WorldToCell(destination);

            for (int i = 0; i < maxPathCount; ++i)
                _pathArr[i] = Vector2.zero;

            IsMove = true;
            _movePointIndex = 1;
            _totalPathCount = _agent.GetPath(startCell, endCell, _pathArr);
        }

        private void FixedUpdate()
        {
            SetMoveDir();
        }
        public void SetMoveDir()
        {
            if (Vector3.Distance(_agent.transform.position, _movePoint) <= 0.2f)
            {
                if (_movePointIndex >= _totalPathCount)
                    StopMove();
                else
                {
                    _movePoint = _pathArr[_movePointIndex++];
                    _moveDir = (_movePoint - _agent.transform.position).normalized;
                    _animator.SetParam(Animator.StringToHash("MOVE"), true);
                    _animator.SetParam(Animator.StringToHash("XValue"), _moveDir.x);
                    _animator.SetParam(Animator.StringToHash("YValue"), _moveDir.y);
                }
            }
            else
                _playerRigid.linearVelocity = _moveDir * moveSpeed;

        }

        public void StopMove()
        {
            IsMove = false;
            _animator.SetParam(Animator.StringToHash("IDLE"), true);
            _playerRigid.linearVelocity = Vector2.zero;
        }

        public void AllStopMove()
        {
            _movePointIndex = maxPathCount;
            StopMove();
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