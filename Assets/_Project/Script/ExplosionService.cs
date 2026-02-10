using UnityEngine;

public class ExplosionService : MonoBehaviour
{
    private const float MinExplosionDistance = 0.001f;

    [SerializeField] private float baseForce = 8f;
    [SerializeField] private float baseRadius = 3f;
    [SerializeField] private float minAttenuation = 0.15f;

    public void ExplodeAt(CubeView source)
    {
        Vector3 center = source.transform.position;
        float size = Mathf.Max(0.001f, source.transform.localScale.x);

        float sizeMultiplier = 1f / size;
        float radius = baseRadius * sizeMultiplier;
        float force = baseForce * sizeMultiplier;

        var cols = Physics.OverlapSphere(center, radius, ~0, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < cols.Length; i++)
        {
            if (!cols[i].TryGetComponent<Rigidbody>(out var rb))
                continue;
            if (rb == source.Rigidbody)
                continue;

            Vector3 to = rb.worldCenterOfMass - center;
            float dist = to.magnitude;
            if (dist < MinExplosionDistance)
                continue;

            float t = Mathf.Clamp01(dist / radius);
            float attenuation = Mathf.Max(minAttenuation, 1f - t);

            rb.AddForce(( to / dist ) * ( force * attenuation ), ForceMode.Impulse);
        }
    }

    public void ExplodeChildren(CubeView parent, Rigidbody[] children, float force, float radius, float upwards)
    {
        Vector3 center = parent.transform.position;
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] == null)
                continue;
            children[i].AddExplosionForce(force, center, radius, upwards, ForceMode.Impulse);
        }
    }
}
