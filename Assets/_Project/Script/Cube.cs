
using UnityEngine;

namespace Di
{

    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Cube : MonoBehaviour
    {
        public float SplitChance { get; private set; } = 1f;
        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void SetSplitChance(float value)
        {
            SplitChance = Mathf.Clamp01(value);
        }
    }
}