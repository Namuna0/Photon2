using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class ActorView : MonoBehaviourPunCallbacks
{
    [SerializeField] private CinemachineCamera _camera;

    public static ActorView Instance { get; private set; }

    private Actor _mineActor;
    private List<Actor> _actors = new List<Actor>();

    public bool IsJamping
    {
        get
        {
            foreach (var actor in _actors)
            {
                if (actor.IsJumping) return true;
            }

            return false;
        }
    }

    private void Start()
    {
        Instance = this;
    }

    public override void OnJoinedRoom()
    {
        var actor = PhotonNetwork.Instantiate("Actor", Vector3.zero, Quaternion.identity).GetComponent<Actor>();

        if (actor.photonView.IsMine)
        {
            actor.transform.SetParent(transform);

            _camera.Follow = actor.transform;
            _camera.LookAt = actor.transform;

            _mineActor = actor;
            _actors.Add(actor);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        foreach (var actor in _actors)
        {
            actor.photonView.RPC(nameof(Actor.RPC_ReceivelPositions),
                newPlayer,
                actor.transform.position,
                actor.transform.rotation);
        }
    }

    public void RegisterActor(Actor actor)
    {
        actor.transform.SetParent(transform);
        _actors.Add(actor);
    }

    public void SetMovingPosition(Vector3 position)
    {
        _mineActor.SetMovingPosition(position);
    }
}
