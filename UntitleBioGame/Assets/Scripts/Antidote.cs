using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antidote : MonoBehaviour
{
    [Header("Variables")]
    public float _speed = 10f;

    [Header("Unity Attributes")]
    private Transform target;
    private GameManager _gameManager;
    


    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = _speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void HitTarget()
    {
        if (target.gameObject.tag == "Virus")
        {
            Destroy(target.gameObject);
        }
        Debug.Log(gameObject.name);
    }

}
