using UnityEngine;
using UnityEngine.AI;

public class ActorView : MonoBehaviour
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
