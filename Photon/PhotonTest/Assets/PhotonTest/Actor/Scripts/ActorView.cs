using Photon.Pun;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class ActorView : MonoBehaviourPunCallbacks
{
    [SerializeField] private CinemachineCamera _camera;

    private List<Actor> _actorList = new List<Actor>();

    public override void OnJoinedRoom()
    {
        var actor = PhotonNetwork.Instantiate("Actor", Vector3.zero, Quaternion.identity).GetComponent<Actor>();

        if (actor.photonView.IsMine)
        {
            _camera.Follow = actor.transform;
            _camera.LookAt = actor.transform;
        }

        _actorList.Add(actor);
    }

    public void SetMovingPosition(Vector3 position)
    {
        foreach (var actor in _actorList)
        {
            actor.SetMovingPosition(position);
        }
    }
}
