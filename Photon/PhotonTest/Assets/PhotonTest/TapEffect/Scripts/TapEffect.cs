using UnityEngine;

namespace PhotonTest
{
    public class TapEffect : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float _factor;
        [SerializeField] private Material _material;
        [SerializeField] private Transform _effect;
        [SerializeField] private float _scale;

        private Material _runtimeMaterial;
        private float _time = 0.0f;

        private void Start()
        {
            _runtimeMaterial = new Material(_material);

            meshRenderer.material = _runtimeMaterial;

            float dist = Vector3.Distance(Camera.main.transform.position, _effect.position);
            _effect.localScale = Vector3.one * dist * _scale;
        }

        private void OnDestroy()
        {
            Destroy(_runtimeMaterial);
        }

        void Update()
        {
            _runtimeMaterial.SetFloat("_Factor", _factor);

            _time += Time.deltaTime;

            if (_time > 0.5f) Destroy(gameObject);
        }
    }
}
