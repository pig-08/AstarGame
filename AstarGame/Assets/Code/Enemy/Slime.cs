using Code.Astar;
using GMS.Code.Core.Events;
using UnityEngine;

public class Slime : PathAgent
{
    [SerializeField] private Vector2[] movePoints;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameEventChannelSO PlayerDestroyedEventSO;

    private PathMovement _movement;

    private int _pointIndex = 0;

    public override void Awake()
    {
        base.Awake();
        _movement = GetCompo<PathMovement>();
    }

    private void Update()
    {
        if(_movement != null)
        {
            MoveSlime();
        }
    }

    private void MoveSlime()
    {
        if (_movement.IsMove) return;
        
        _movement.SetDestination(movePoints[_pointIndex]);

        _pointIndex += 1;
        
        if (_pointIndex >= movePoints.Length)
            _pointIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            print("аж╠щ");
            DeadEvent dead = new DeadEvent();
            PlayerDestroyedEventSO.RaiseEvent(dead);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}

