using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviourPun
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private Animator _animator;

    private Vector3 _movingPosition;
    private bool _isJumping;

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
        if (_isJumping) return;

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

            StartCoroutine(JumoBreak());

            _isJumping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "JumpTrigger")
        {
            _isJumping = false;
        }
    }

    private IEnumerator JumoBreak()
    {
        yield return new WaitForSeconds(0.9f);

        _meshAgent.speed = 0.66f;

        yield return new WaitForSeconds(0.2f);

        _meshAgent.speed = 3.5f;
    }
}
