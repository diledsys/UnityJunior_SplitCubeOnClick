using System.Collections.Generic;
using UnityEngine;

public class ExplosionApplier : MonoBehaviour
{
    [SerializeField] private float _baseForce = 8f;
    [SerializeField] private float _baseRadius = 3f;
    [SerializeField] private float _upwardsModifier = 0.25f;

    private const float MinSize = 0.01f;

    public void Explode(Vector3 center, IReadOnlyList<Rigidbody> bodies, Vector3 childScale)
    {
        float size = (childScale.x + childScale.y + childScale.z) / 3f;
        float safeSize = Mathf.Max(MinSize, size);

        float force = _baseForce / safeSize;
        float radius = _baseRadius / safeSize;

        for (int i = 0; i < bodies.Count; i++)
        {
            var rigidbody = bodies[i];
            if (rigidbody == null)
                continue;

            rigidbody.AddExplosionForce(force, center, radius, _upwardsModifier, ForceMode.Impulse);
        }
    }
}