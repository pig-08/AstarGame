using DG.Tweening;
using GMS.Code.Core.Events;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO PlayerDestroyedEventSO;
    private void Awake()
    {
        PlayerDestroyedEventSO.RemoveAll();
        PlayerDestroyedEventSO.AddListener<DeadEvent>(GameOver);
    }



    private void OnDestroy()
    {
        PlayerDestroyedEventSO.RemoveListener<DeadEvent>(GameOver);
    }

    private void GameOver(DeadEvent evt)
    {
        transform.DOLocalMove(Vector2.zero,0.3f);
        transform.DOScale(Vector3.one,0.3f);
    }
}
