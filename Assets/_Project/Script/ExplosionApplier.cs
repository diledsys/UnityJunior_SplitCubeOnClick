using System.Collections.Generic;
using UnityEngine;

namespace Di
{
    public class ExplosionApplier : MonoBehaviour
    {
        private const float MinAllowedSize = 0.001f;

        [Header("Explosion")]
        [SerializeField] private float baseExplosionForce = 2f;
        [SerializeField] private float baseExplosionRadius = 3f;
        [SerializeField] private float upwardsModifier = 0.25f;

        public void Apply(IReadOnlyList<Rigidbody> bodies, Vector3 center, Vector3 referenceScale)
        {
            if (bodies == null || bodies.Count == 0)
                return;

            float size = ( referenceScale.x + referenceScale.y + referenceScale.z ) / 3f;
            float safeSize = Mathf.Max(MinAllowedSize, size);

            float force = baseExplosionForce / safeSize;
            float radius = baseExplosionRadius / safeSize;

            for (int i = 0; i < bodies.Count; i++)
            {
                Rigidbody rb = bodies[i];
                if (rb == null)
                    continue;

                rb.AddExplosionForce(force, center, radius, upwardsModifier, ForceMode.Impulse);
            }
        }
    }
}