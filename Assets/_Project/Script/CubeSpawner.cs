using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _childScaleFactor = 0.5f;
    [SerializeField] private float _minMass = 0.01f;

    public List<Rigidbody> SpawnChildren(Cube parent, int count, float childChance)
    {
        var bodies = new List<Rigidbody>(count);

        Vector3 parentPos = parent.transform.position;
        Vector3 childScale = parent.transform.localScale * _childScaleFactor;
        int layer = parent.gameObject.layer;

        float parentMass = 1f;
        if (parent.TryGetComponent(out Rigidbody parentRb))
            parentMass = parentRb.mass;

        float massFactor = _childScaleFactor * _childScaleFactor * _childScaleFactor;
        float childMass = Mathf.Max(_minMass, parentMass * massFactor);

        for (int i = 0; i < count; i++)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            gameObject.transform.position = parentPos;
            gameObject.transform.localScale = childScale;
            gameObject.layer = layer;

            var rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = childMass;

            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.color = Random.ColorHSV();

            var cube = gameObject.AddComponent<Cube>();
            cube.SetSplitChance(childChance);

            bodies.Add(rb);
        }

        return bodies;
    }

    public void Remove(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    public float ChildScaleFactor => _childScaleFactor;
}