using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviourPun
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private Animator _animator;

    private Vector3 _movingPosition;

    private void Update()
    {
        if (_meshAgent.velocity.sqrMagnitude < 0.05f)
        {
            _animator.SetBool("Running", false);
        }
        else
        {
            _animator.SetBool("Running", true);
        }
    }


    public void SetMovingPosition(Vector3 position)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_SetTargetPosition", RpcTarget.Others, position);
        }

        _movingPosition = position;
        _meshAgent.SetDestination(_movingPosition);
    }

    [PunRPC]
    void RPC_SetTargetPosition(Vector3 position)
    {
        _movingPosition = position;
        _meshAgent.SetDestination(_movingPosition);
    }

    public void OnCallChangeFace()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpTrigger")
        {
            _animator.SetTrigger("Jump");
        }
    }
}
