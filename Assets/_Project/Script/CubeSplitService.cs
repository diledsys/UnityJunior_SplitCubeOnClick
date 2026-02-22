using System.Collections.Generic;
using UnityEngine;

namespace Di
{
    public class CubeSplitService : MonoBehaviour
    {
        [SerializeField] private CubeSpawner spawner;
        [SerializeField] private ExplosionApplier explosion;

        public void SplitOrDisappear(Cube parent)
        {
            if (parent == null || spawner == null)
                return;

            if (Random.value > parent.SplitChance)
            {
                Destroy(parent.gameObject);
                return;
            }

            Vector3 center = parent.transform.position;
            float childChance = parent.SplitChance * 0.5f;

            List<Rigidbody> bodies = spawner.SpawnChildren(parent, center, childChance);

            if (explosion != null && bodies.Count > 0)
            {
                Vector3 childScale = spawner.GetChildScale(parent.transform.localScale);
                explosion.Apply(bodies, center, childScale);
            }

            Destroy(parent.gameObject);
        }
    }
}