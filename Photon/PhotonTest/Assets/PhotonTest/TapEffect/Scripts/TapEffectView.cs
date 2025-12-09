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
            var effect = Instantiate(_originalEffect, position, Quaternion.identity, transform);
        }
    }
}
