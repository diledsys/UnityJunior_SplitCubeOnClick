using Di;
using System.Collections.Generic;
using UnityEngine;

namespace Di
{

    public class CubeSplitterSpawner : MonoBehaviour
    {
        [Header("Spawn")] [SerializeField] private int minChildren = 2;
        [SerializeField] private int maxChildren = 6;
        [SerializeField] private float childScaleFactor = 0.5f;

        [Header("Explosion applied ONLY to spawned children")] 
        [SerializeField] private float baseExplosionForce = 2f;

        [SerializeField] private float baseExplosionRadius = 3f;
        [SerializeField] private float upwardsModifier = 0.25f;

        [Header("Physics")] [SerializeField] private float minMass = 0.01f;

        public void SplitOrDisappear(Cube parent)
        {
            if (Random.value > parent.SplitChance)
            {
                Destroy(parent.gameObject);
                return;
            }

            int count = Random.Range(minChildren, maxChildren + 1);

            Vector3 center = parent.transform.position;
            Vector3 childScale = parent.transform.localScale * childScaleFactor;
            float childChance = parent.SplitChance * 0.5f;

            var spawnedBodies = new List<Rigidbody>(count);

            for (int i = 0; i < count; i++)
            {
                var child = SpawnChild(center, childScale, parent.gameObject.layer, parent.Rigidbody.mass);
                child.SetSplitChance(childChance);
                spawnedBodies.Add(child.Rigidbody);
            }

            ExplodeChildren(spawnedBodies, center, childScale);

            Destroy(parent.gameObject);
        }

        private Cube SpawnChild(Vector3 position, Vector3 scale, int layer, float parentMass)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            gameObject.transform.position = position;
            gameObject.transform.localScale = scale;
            gameObject.layer = layer;

            var rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;

            float massFactor = childScaleFactor * childScaleFactor * childScaleFactor;
            rb.mass = Mathf.Max(minMass, parentMass * massFactor);

            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.color = Random.ColorHSV();

            var cube = gameObject.AddComponent<Cube>();
            return cube;
        }

        private void ExplodeChildren(List<Rigidbody> bodies, Vector3 center, Vector3 childScale)
        {
            float size = (childScale.x + childScale.y + childScale.z) / 3f;

            float safeSize = Mathf.Max(0.01f, size);

            float force = baseExplosionForce / safeSize;
            float radius = baseExplosionRadius / safeSize;

            foreach (var rb in bodies)
            {
                if (rb == null)
                    continue;
                rb.AddExplosionForce(force, center, radius, upwardsModifier, ForceMode.Impulse);
            }
        }
    }
}