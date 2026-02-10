using UnityEngine;

public class CubeInteractionController : MonoBehaviour
{
    [SerializeField] private ClickInput input;
    [SerializeField] private ClickRaycaster raycaster;
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private ExplosionService exploder;

    [Header("Split settings")]
    [SerializeField] private int minChildren = 2;
    [SerializeField] private int maxChildren = 6;

    [Header("Child explosion")]
    [SerializeField] private float childExplosionForce = 6f;
    [SerializeField] private float childExplosionRadius = 2f;
    [SerializeField] private float childExplosionUpwards = 0.4f;

    private const float Half = 0.5f;

    private void OnEnable()
    {
        input.Activated += OnActivated;
        raycaster.CubeClicked += OnCubeClicked;
    }

    private void OnDisable()
    {
        input.Activated -= OnActivated;
        raycaster.CubeClicked -= OnCubeClicked;
    }

    private void OnActivated()
    {
        raycaster.RaycastFromPointer(Input.mousePosition);
    }

    private void OnCubeClicked(CubeView cube)
    {
        float splitChance = Mathf.Pow(Half, cube.Generation);

        if (Random.value > splitChance)
        {
            exploder.ExplodeAt(cube);
            spawner.Despawn(cube);
            return;
        }

        int count = Random.Range(minChildren, maxChildren + 1);
        var children = spawner.SpawnChildren(cube, count);

        // Взрыв только новых детей (как в предыдущей задаче)
        var childBodies = new Rigidbody[children.Length];
        for (int i = 0; i < children.Length; i++)
            childBodies[i] = children[i].Rigidbody;

        exploder.ExplodeChildren(cube, childBodies, childExplosionForce, childExplosionRadius, childExplosionUpwards);

        spawner.Despawn(cube);
    }
}
