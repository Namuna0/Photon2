using UnityEngine;

namespace PhotonTest
{
    public class TapEffectView : MonoBehaviour
    {
        [SerializeField] private GameObject _originalEffect;

        [SerializeField] private Transform _cameraPos;

        public void PlayEffect(Vector3 position)
        {
            var effect = Instantiate(_originalEffect, position, Quaternion.identity, transform);
            effect.transform.rotation = Quaternion.LookRotation(_cameraPos.position - position);
        }
    }
}
