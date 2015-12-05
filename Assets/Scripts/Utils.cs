using UnityEngine;
using System.Collections;

public class Utils
{
    public static void DestroyWithExplosion(GameObject obj, GameObject explosion)
    {
        SpawnExplosion(explosion, obj.transform);
        GameObject.Destroy(obj);
    }

    public static void SpawnExplosion(GameObject explosionPrefab, Transform transform)
    {
        GameObject explosion = GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
        float destroyTime = 1f;
        if (explosion)
        {
            ParticleSystem particles = explosion.GetComponent<ParticleSystem>();
            if (particles)
            {
                destroyTime = particles.duration;
            }
        }
        GameObject.Destroy(explosion, destroyTime);
    }

}
