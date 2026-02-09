using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class SplitCube : MonoBehaviour, IClickable
{
    [Header("Split settings")]
    [SerializeField] private int minChildren = 2;
    [SerializeField] private int maxChildren = 6;

    [Tooltip("Поколение куба. 0 = стартовый (100%)")]
    [SerializeField] private int generation = 0;

    [Header("Physics")]
    [SerializeField] private float spawnJitter = 0.15f;
    [SerializeField] private float explosionForce = 6f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionUpwards = 0.4f;

    private const float Half = 0.5f;
    private const float MinMass = 0.01f;

    // --- цвет без создания новых материалов ---
    private static readonly int ColorId = Shader.PropertyToID("_BaseColor");
    private static MaterialPropertyBlock _mpb;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            _rb = gameObject.AddComponent<Rigidbody>();

        _rb.useGravity = true;
    }

    public void OnClick()
    {
        TrySplit();
    }

    private void TrySplit()
    {
        // 1. Шанс строго по условию задачи:
        // 0 → 100%, 1 → 50%, 2 → 25% ...
        float splitChance = Mathf.Pow(Half, generation);

        if (Random.value > splitChance)
        {
            Destroy(gameObject);
            return;
        }

        int count = Random.Range(minChildren, maxChildren + 1);

        Transform t = transform;
        Vector3 parentPos = t.position;
        Vector3 childScale = t.localScale * Half;

        float massFactor = Half * Half * Half; // объём (0.125)

        for (int i = 0; i < count; i++)
        {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // transform
            child.transform.position = parentPos + Random.insideUnitSphere * spawnJitter;
            child.transform.localScale = childScale;

            // слой — чтобы raycast продолжал работать
            child.layer = gameObject.layer;

            // физика
            Rigidbody rb = child.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = Mathf.Max(MinMass, _rb.mass * massFactor);

            // логика деления
            SplitCube split = child.AddComponent<SplitCube>();
            split.generation = generation + 1;
            split.minChildren = minChildren;
            split.maxChildren = maxChildren;
            split.spawnJitter = spawnJitter;
            split.explosionForce = explosionForce;
            split.explosionRadius = explosionRadius;
            split.explosionUpwards = explosionUpwards;

            // цвет
            ApplyRandomColor(child.GetComponent<Renderer>());
        }

        Destroy(gameObject);
    }

    private void ApplyRandomColor(Renderer r)
    {
        _mpb ??= new MaterialPropertyBlock();
        _mpb.SetColor(ColorId, Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.6f, 1f));
        r.SetPropertyBlock(_mpb);
    }
}
