using UnityEngine;

namespace PhotonTest
{
    public class TapEffectView : MonoBehaviour
    {
        [SerializeField] private GameObject _originalEffect;
        [SerializeField] private Camera _screenCamera;
        [SerializeField] private Camera _worldCamera;

        public void PlayEffect(Vector3 position)
        {
            var screenPos = _screenCamera.WorldToScreenPoint(position);
            var worldPos = _worldCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;

            Instantiate(_originalEffect, worldPos, Quaternion.identity, transform);
        }
    }
}
