using Code.Astar;
using UnityEngine;

public class Player : PathAgent
{
    [SerializeField] private PlayerInputSO inputSO;

    public override void Awake()
    {
        base.Awake();
        GetCompo<PathAnimator>().SetParam(Animator.StringToHash("IDLE"), true);
        inputSO.OnClickPressedEvent += GetCompo<PathMovement>().SetDestination;
    }
    private void OnDestroy()
    {
        inputSO.OnClickPressedEvent -= GetCompo<PathMovement>().SetDestination;
    }

}
