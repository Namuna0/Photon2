using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ActorLock : MonoBehaviourPun
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _shadow;

    public bool IsJumping { get; private set; }

    private void Start()
    {
        _animator.SetBool("Running", false);
    }

    private void Update()
    {
        var def = (_shadow.position - transform.position);

        if (def.sqrMagnitude > 0.02f)
        {
            transform.position += def.sqrMagnitude < 1 ? def.normalized * Time.deltaTime * 3 : def * Time.deltaTime * 3;

            def.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, _shadow.rotation, 0.1f);

            _animator.SetBool("Running", true);
        }
        else
        {
            _animator.SetBool("Running", false);
        }
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

            //_meshAgent.avoidancePriority = 0;
            //_meshAgent.radius = 0.01f;

            IsJumping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "JumpTrigger")
        {
            //_meshAgent.avoidancePriority = 10;
            //_meshAgent.radius = 0.1f;

            IsJumping = false;
        }
    }

    private IEnumerator JumpBreak()
    {
        yield return new WaitForSeconds(0.9f);

        //_meshAgent.speed = 0.66f;

        yield return new WaitForSeconds(0.2f);

        //_meshAgent.speed = 3.5f;
    }
}
