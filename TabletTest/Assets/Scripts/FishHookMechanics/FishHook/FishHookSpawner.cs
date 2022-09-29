using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishHookSpawner : MonoBehaviour
{
    Transform myTransform;
    GameObject myGameOjbect;
    bool _isSpawning;

    [Header("Components")]
    [SerializeField] private FishHook[] fishHookToSpawn;

    [Header("Spawn Timer")]
    [SerializeField] private float initialDelay;
    [SerializeField] private float spawnDelay;

    [Header("SpawnDirection")]
    [SerializeField] private float spawnDirection;
    [Range(0, 360)]
    [SerializeField] private float spawnAngle;
    [Header("SpawnRange")]
    [SerializeField] private float spawnRangeMin;
    [SerializeField] private float spawnRangeMax;

    private void Awake()
    {
        myTransform = transform;
        myGameOjbect = gameObject;
    }

    private void Start()
    {
        InvokeRepeating(nameof(InitiateEnemy), initialDelay, spawnDelay);
    }

    private void InitiateEnemy()
    {
        var enemy = Instantiate(fishHookToSpawn, GetRandomSpawnPoint(), Quaternion.identity);
        enemy.transform.LookAt(myTransform);
    }

    Vector3 GetRandomSpawnPoint()
    {
        float randAngle = Random.Range((-spawnAngle / 2) + spawnDirection, (spawnAngle / 2) + spawnDirection);
        var randRad = randAngle * Mathf.PI / 180;
        var randDist = Random.Range(spawnRangeMin, spawnRangeMax);

        return myTransform.position + new Vector3(Mathf.Sin(randRad), 0, Mathf.Cos(randRad)) * randDist;
    }

    void OnDrawGizmosSelected()
    {
        float halfFOV = spawnAngle / 2.0f;

        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + spawnDirection, Vector3.up);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + spawnDirection, Vector3.up);

        Vector3 spawnIndicatorLeft = upRayRotation * transform.forward * spawnRangeMax;
        Vector3 spawnIndicatorRight = downRayRotation * transform.forward * spawnRangeMax;

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, spawnIndicatorLeft);
        Gizmos.DrawRay(transform.position, spawnIndicatorRight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRangeMax);

        Vector3 safeIndicatorLeft = upRayRotation * transform.forward * spawnRangeMin;
        Vector3 safeIndicatorRight = downRayRotation * transform.forward * spawnRangeMin;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, safeIndicatorLeft);
        Gizmos.DrawRay(transform.position, safeIndicatorRight);
        Gizmos.DrawWireSphere(transform.position, spawnRangeMin);
    }
}
