using GMS.Code.Core.Events;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO PlayerClearEventSO;
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            ClearEvent clear = new ClearEvent();
            PlayerClearEventSO.RaiseEvent(clear);
        }
    }
}
