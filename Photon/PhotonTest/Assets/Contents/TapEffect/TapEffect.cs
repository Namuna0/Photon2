using UnityEngine;

namespace PhotonTest
{
    public class TapEffect : MonoBehaviour
    {
        [SerializeField] private float _factor;
        [SerializeField] private Material _material;

        private float _time = 0.0f;

        // Update is called once per frame
        void Update()
        {
            _material.SetFloat("_Factor", _factor);

            _time += Time.deltaTime;

            if (_time > 2.0f) Destroy(gameObject);
        }
    }
}
