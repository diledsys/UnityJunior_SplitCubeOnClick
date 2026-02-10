using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private float spawnJitter = 0.15f;

    private const float Half = 0.5f;
    private const float MinMass = 0.01f;

    private static readonly int ColorId = Shader.PropertyToID("_BaseColor");
    private static MaterialPropertyBlock materialPropertyBlock;

    public CubeView[] SpawnChildren(CubeView parent, int count)
    {
        var children = new CubeView[count];

        Vector3 parentPos = parent.transform.position;
        Vector3 childScale = parent.transform.localScale * Half;

        float massFactor = Half * Half * Half;

        for (int i = 0; i < count; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = parentPos + Random.insideUnitSphere * spawnJitter;
            go.transform.localScale = childScale;

            go.layer = parent.gameObject.layer;

            var rb = go.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = Mathf.Max(MinMass, parent.Rigidbody.mass * massFactor);

            var view = go.AddComponent<CubeView>();
            view.SetGeneration(parent.Generation + 1);

            ApplyRandomColor(view.Renderer);
            children[i] = view;
        }

        return children;
    }

    private void ApplyRandomColor(Renderer renderer)
    {
        materialPropertyBlock ??= new MaterialPropertyBlock();
        materialPropertyBlock.SetColor(ColorId, Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.6f, 1f));
        renderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void Despawn(CubeView cube)
    {
        if (cube != null)
            Destroy(cube.gameObject);
    }
}
