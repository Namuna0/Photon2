using UnityEngine;

namespace PhotonTest
{
    public class TapEffect : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float _factor;
        [SerializeField] private Material _material;

        private Material _runtimeMaterial;
        private float _time = 0.0f;

        private void Start()
        {
            _runtimeMaterial = new Material(_material);

            meshRenderer.material = _runtimeMaterial;
        }

        private void OnDestroy()
        {
            Destroy(_runtimeMaterial);
        }

        void Update()
        {
            _runtimeMaterial.SetFloat("_Factor", _factor);

            _time += Time.deltaTime;

            if (_time > 10.5f) Destroy(gameObject);
        }
    }
}
