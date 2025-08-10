using Code.Astar;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PathAnimator : MonoBehaviour, IAgentCompo
{
    private PathAgent _agent;
    private Animator _animator;

    private int _currHash;

    public void Init(PathAgent agent)
    {
        _agent = agent;
        _animator = GetComponent<Animator>();
    }

    public void SetParam(int hash, float value)
    {
        _animator.SetFloat(hash, value);
    }
    public void SetParam(int hash, int value)
    {
        _animator.SetInteger(hash, value);
    }
    public void SetParam(int hash, bool value)
    {
        if(_currHash != 0)
            _animator.SetBool(_currHash, false);

        _currHash = hash;
        _animator.SetBool(hash, value);
    }
    public void SetParam(int hash)
    {
        _animator.SetTrigger(hash);
    }
}
