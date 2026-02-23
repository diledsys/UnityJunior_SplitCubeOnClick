using UnityEngine;

public sealed class ExplosionApplier : MonoBehaviour
{
    [Header("Explosion Settings")] [SerializeField]
    private float _explosionForce = 6f;

    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _upwardsModifier = 0.5f;

    public void Apply(Cube[] cubes, Vector3 origin)
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            Rigidbody rigidbody = cubes[i].GetComponent<Rigidbody>();
            if (rigidbody == null)
                continue;

            rigidbody.AddExplosionForce(
                _explosionForce,
                origin,
                _explosionRadius,
                _upwardsModifier,
                ForceMode.Impulse);
        }
    }
}