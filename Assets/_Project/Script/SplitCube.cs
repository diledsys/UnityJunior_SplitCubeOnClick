using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class SplitCube : MonoBehaviour, IClickable
{
    [Range(0f, 2f)]
    [SerializeField] private float splitChance = 2f;

    [SerializeField] private int minChildren = 2;
    [SerializeField] private int maxChildren = 6;

    [Header("Explosion")]
    [SerializeField] private float explosionForce = 6f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionUpwards = 0.4f;

    [SerializeField] private float spawnJitter = 0.12f;

    private const float Half = 0.5f;
    private const float MinMass = 0.5f;

    private void Awake()
    {
        GetComponent<Rigidbody>().useGravity = true;
    }

    public void OnClick()
    {
        TrySplit();
    }

    private void TrySplit()
    {
        if (splitChance < 0.01f)
        {
            Destroy(gameObject);
            return;
        }

        bool success = Random.value < splitChance;

        if (!success)
        {
            Destroy(gameObject);
            return;
        }

        int count = Random.Range(minChildren, maxChildren + 1);
        Vector3 parentPos = transform.position;
        Vector3 childScale = transform.localScale * Half;

        float nextChance = splitChance * Half;

        Rigidbody parentRb = GetComponent<Rigidbody>();
        float massFactor = Half * Half * Half;

        Rigidbody[] spawned = new Rigidbody[count];

        for (int i = 0; i < count; i++)
        {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);
            child.transform.localScale = childScale;
            child.transform.position = parentPos + Random.insideUnitSphere * spawnJitter;
            child.layer = gameObject.layer;
            var rb = child.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = Mathf.Max(MinMass, parentRb.mass * massFactor);

            var split = child.AddComponent<SplitCube>();
            split.CopySettingsFrom(this, nextChance);

            ApplyRandomColor(child);

            spawned[i] = rb;
        }

        foreach (var rb in spawned)
        {
            rb.AddExplosionForce(
                explosionForce,
                parentPos,
                explosionRadius,
                explosionUpwards,
                ForceMode.Impulse
            );
        }

        Destroy(gameObject);
    }

    private void CopySettingsFrom(SplitCube source, float chance)
    {
        splitChance = chance;
        minChildren = source.minChildren;
        maxChildren = source.maxChildren;
        explosionForce = source.explosionForce;
        explosionRadius = source.explosionRadius;
        explosionUpwards = source.explosionUpwards;
        spawnJitter = source.spawnJitter;
    }

    private void ApplyRandomColor(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        renderer.material.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.6f, 1f);
    }
}
