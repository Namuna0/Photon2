using Photon.Pun;
using UnityEngine;

public class ActorShadow : MonoBehaviourPun
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        StopMove();
    }

    public void OnCallChangeFace()
    {
    }

    /// <summary>ˆÚ“®ŠJn</summary>
    public void StartMove()
    {
        _animator.SetBool("Running", true);
    }

    /// <summary>ˆÚ“®I—¹</summary>
    public void StopMove()
    {
        _animator.SetBool("Running", false);
    }
}
