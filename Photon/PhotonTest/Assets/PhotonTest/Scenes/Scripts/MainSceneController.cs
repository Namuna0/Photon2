using PhotonTest;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private ActorView _actorView;
    [SerializeField] private TapEffectView tapEffectView;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int jumpLayer = LayerMask.NameToLayer("IgnoreRaycast");
            int mask = ~(1 << jumpLayer);

            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                if (hit.collider)

                    _actorView.SetMovingPosition(hit.point);
                tapEffectView.PlayEffect(hit.point);
            }
        }
    }
}
