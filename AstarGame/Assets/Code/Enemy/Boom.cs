using GMS.Code.Core.Events;
using UnityEngine;

public class Boom : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameEventChannelSO PlayerDestroyedEventSO;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            DeadEvent dead = new DeadEvent();
            PlayerDestroyedEventSO.RaiseEvent(dead);
        }
    }
}
