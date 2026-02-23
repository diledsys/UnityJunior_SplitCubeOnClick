using UnityEngine;

public sealed class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    [SerializeField] private int _minChildren = 2;
    [SerializeField] private int _maxChildren = 6;

    [SerializeField] private float _childScaleFactor = 0.5f;
    [SerializeField] private float _splitChanceDecay = 0.5f;

    private void Start()
    {
        RegisterExistingCubes();
    }

    private void RegisterExistingCubes()
    {
        Cube[] cubes = FindObjectsOfType<Cube>();

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].Clicked -= OnCubeClicked;
            cubes[i].Clicked += OnCubeClicked;
        }
    }

    public Cube SpawnCube(Vector3 position, Quaternion rotation, float splitChance, Vector3 scale)
    {
        Cube cube = Instantiate(_cubePrefab, position, rotation);

        cube.Initialize(
            splitChance,
            scale,
            mass: 1f,
            color: Random.ColorHSV(),
            layer: cube.gameObject.layer,
            useGravity: true);

        cube.Clicked += OnCubeClicked;

        return cube;
    }

    private void OnCubeClicked(Cube parent)
    {
        Debug.Log("OnCubeClicked");
        if (parent == null)
            return;

        parent.Clicked -= OnCubeClicked;

        Vector3 parentPosition = parent.transform.position;
        Vector3 parentScale = parent.transform.localScale;
        float parentChance = parent.SplitChance;

        Destroy(parent.gameObject);

        bool shouldSplit = Random.value <= parentChance;
        if (!shouldSplit)
            return;

        int childCount = Random.Range(_minChildren, _maxChildren + 1);

        Vector3 childScale = parentScale * _childScaleFactor;
        float childChance = parentChance * _splitChanceDecay;

        for (int i = 0; i < childCount; i++)
        {
            SpawnCube(parentPosition, Random.rotation, childChance, childScale);
        }
    }
}