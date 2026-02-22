using UnityEngine;

namespace Di
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Cube : MonoBehaviour
    {
        public float SplitChance { get; private set; } = 1f;
        public Rigidbody Rigidbody { get; private set; }

        private Renderer _renderer;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _renderer = GetComponent<Renderer>();
        }

        public void Initialize(
            float splitChance,
            Vector3 scale,
            float mass,
            Color color,
            int layer,
            bool useGravity = true)
        {
            SplitChance = Mathf.Clamp01(splitChance);

            transform.localScale = scale;
            gameObject.layer = layer;

            if (Rigidbody != null)
            {
                Rigidbody.useGravity = useGravity;
                Rigidbody.mass = mass;
            }

            if (_renderer != null)
                _renderer.material.color = color;
        }
    }
}