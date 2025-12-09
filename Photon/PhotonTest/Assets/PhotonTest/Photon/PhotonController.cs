using Photon.Pun;
using UnityEngine;

public class PhotonController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photonに接続しました！");
        PhotonNetwork.JoinLobby(); // ロビーに接続
    }

    public override void OnJoinedLobby()
    {
        // ランダムな部屋へ入る（無ければエラーになる）
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 部屋が無ければ作る
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("部屋に入りました！");
    }
}
