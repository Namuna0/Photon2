using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviourPun, IPunObservable
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 自分の値を送信
            stream.SendNext(transform.position);
        }
        else
        {
            // 他プレイヤーから受信
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }

    public void SetMovingPosition(Vector3 position)
    {
        if (!photonView.IsMine) return;

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
