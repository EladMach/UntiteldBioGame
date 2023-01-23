using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AntidoteManager : MonoBehaviour
{
 
    [Header("Variables")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public string virusTag;
    

    [Header("Unity Attributes")]
    
    public GameObject _antidotePrefab;
    private Transform target;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector2 dir = target.position - transform.position;
        Quaternion lookRoataion = Quaternion.LookRotation(dir);
 
        if (fireCountdown <= 0)
        {
            Shoot();

            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(virusTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject antidoteGO = (GameObject)Instantiate(_antidotePrefab, transform.position, Quaternion.identity);

        Antidote antidote = antidoteGO.GetComponent<Antidote>();

        if (antidote != null)
        {
            antidote.Seek(target);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
