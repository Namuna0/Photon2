using Photon.Pun;
using TMPro;
using UnityEngine;

public class DebugTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ping;
    [SerializeField] private TextMeshProUGUI _statistics;

    private void Update()
    {
        _ping.text = $"ping:{PhotonNetwork.GetPing()}";
        _statistics.text = $"statistics:{PhotonNetwork.NetworkStatisticsToString()}";
    }
}
