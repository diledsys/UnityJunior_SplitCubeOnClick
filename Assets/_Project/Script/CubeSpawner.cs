using System.Collections.Generic;
using UnityEngine;

namespace Di
{
    public class CubeSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private Cube cubePrefab;

        [Header("Spawn Count")]
        [SerializeField] private int minChildren = 2;
        [SerializeField] private int maxChildren = 6;

        [Header("Child Params")]
        [SerializeField] private float childScaleFactor = 0.5f;
        [SerializeField] private float minMass = 0.01f;

        public Vector3 GetChildScale(Vector3 parentScale) => parentScale * childScaleFactor;

        public List<Rigidbody> SpawnChildren(Cube parent, Vector3 center, float childSplitChance)
        {
            if (parent == null || cubePrefab == null)
                return new List<Rigidbody>(0);

            int count = Random.Range(minChildren, maxChildren + 1);
            var bodies = new List<Rigidbody>(count);

            Vector3 childScale = GetChildScale(parent.transform.localScale);

            for (int i = 0; i < count; i++)
            {
                Cube child = SpawnOne(parent, center, childScale, childSplitChance);
                if (child != null && child.Rigidbody != null)
                    bodies.Add(child.Rigidbody);
            }

            return bodies;
        }

        private Cube SpawnOne(Cube parent, Vector3 position, Vector3 scale, float splitChance)
        {
            Cube child = Instantiate(cubePrefab, position, Quaternion.identity);

            float massFactor = childScaleFactor * childScaleFactor * childScaleFactor;
            float mass = Mathf.Max(minMass, parent.Rigidbody.mass * massFactor);

            child.Initialize(
                splitChance: splitChance,
                scale: scale,
                mass: mass,
                color: Random.ColorHSV(),
                layer: parent.gameObject.layer,
                useGravity: true);

            return child;
        }
    }
}