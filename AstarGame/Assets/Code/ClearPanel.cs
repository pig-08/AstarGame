using DG.Tweening;
using GMS.Code.Core.Events;
using UnityEngine;

public class ClearPanel : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO PlayerClearEventSO;
    private void Awake()
    {
        PlayerClearEventSO.RemoveAll();
        PlayerClearEventSO.AddListener<ClearEvent>(GameOver);
    }

    private void OnDestroy()
    {
        PlayerClearEventSO.RemoveListener<ClearEvent>(GameOver);
    }

    private void GameOver(ClearEvent evt)
    {
        transform.DOLocalMove(Vector2.zero, 0.3f);
        transform.DOScale(Vector3.one, 0.3f);
    }
}
