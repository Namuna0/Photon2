using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviourPun
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private Animator _animator;

    private Vector3 _movingPosition;

    public bool IsJumping { get; private set; }

    private void Start()
    {
        ActorView.Instance?.RegisterActor(this);
    }

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

    [PunRPC]
    public void RPC_ReceivelPositions(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SetMovingPosition(Vector3 position)
    {
        if (IsJumping) return;

        if (photonView.IsMine)
        {
            photonView.RPC("RPC_SetTargetPosition", RpcTarget.Others, position);
        }

        _movingPosition = position;
        _meshAgent.SetDestination(_movingPosition);
    }

    [PunRPC]
    public void RPC_SetTargetPosition(Vector3 position)
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

            StartCoroutine(JumpBreak());

            _meshAgent.avoidancePriority = 0;
            _meshAgent.radius = 0.01f;

            IsJumping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "JumpTrigger")
        {
            _meshAgent.avoidancePriority = 10;
            _meshAgent.radius = 0.1f;

            IsJumping = false;
        }
    }

    private IEnumerator JumpBreak()
    {
        yield return new WaitForSeconds(0.9f);

        _meshAgent.speed = 0.66f;

        yield return new WaitForSeconds(0.2f);

        _meshAgent.speed = 3.5f;
    }
}
