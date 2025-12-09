using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ActorView : MonoBehaviourPunCallbacks
{
    private List<Actor> _actorList = new List<Actor>();

    public override void OnJoinedRoom()
    {
        var actor = PhotonNetwork.Instantiate("Actor", Vector3.zero, Quaternion.identity).GetComponent<Actor>();

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
