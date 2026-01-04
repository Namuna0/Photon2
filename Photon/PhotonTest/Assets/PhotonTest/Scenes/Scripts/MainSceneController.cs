using Photon.Pun;
using PhotonTest;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private ActorView _actorView;
    [SerializeField] private TapEffectView tapEffectView;
    [SerializeField] private bool _debugEnabled = true;

    private void Start()
    {
        var peer = PhotonNetwork.NetworkingClient.LoadBalancingPeer;
        var sim = peer.NetworkSimulationSettings;

        sim.IncomingLag = 150;
        sim.OutgoingLag = 150;
        sim.IncomingJitter = 50;
        sim.OutgoingJitter = 50;
        sim.IncomingLossPercentage = 10;
        sim.OutgoingLossPercentage = 10;

        peer.IsSimulationEnabled = false;

        PhotonNetwork.NetworkStatisticsEnabled = true;

        Application.targetFrameRate = 60;

    }

    void Update()
    {
        var peer = PhotonNetwork.NetworkingClient.LoadBalancingPeer;
        peer.IsSimulationEnabled = _debugEnabled;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int jumpLayer = LayerMask.NameToLayer("IgnoreRaycast");
            int mask = ~(1 << jumpLayer);

            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                _actorView.SetMovingPosition(hit.point);
                tapEffectView.PlayEffect(hit.point);
            }
        }
    }
}
