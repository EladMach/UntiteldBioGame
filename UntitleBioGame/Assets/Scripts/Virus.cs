using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Virus : MonoBehaviour
{

  private BodySystem[] targets;
  private BodySystem randomTarget;

  private GameObject[] waypoints;
  public int _currentWaypointIndex = 0;
  public float _speed = 2f;

//   private SpawnManager _spawnManager;

  private void Start()
  {
    targets = GameObject.FindObjectsOfType<BodySystem>();
    randomTarget = targets[ Random.Range(0, targets.Length)];

    // waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    // _spawnManager = FindObjectOfType<SpawnManager>();
    // StartCoroutine(RandomWaypointIndex());
  }

  void Update()
  {
    transform.position = Vector3.MoveTowards(transform.position, randomTarget.transform.position, _speed * Time.deltaTime);
    
    // Transform wp = waypoints[_currentWaypointIndex].transform;

    // if (Vector3.Distance(transform.position, wp.position) < 1f)
    // {
    //   _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
    // }
    // else
    // {
    //   transform.position = Vector3.MoveTowards(transform.position, wp.position, _speed * Time.deltaTime);
    // }

  }


  private IEnumerator RandomWaypointIndex()
  {
    yield return new WaitForSeconds(5f);
    _currentWaypointIndex = Random.Range(0, waypoints.Length);
    yield return new WaitForSeconds(5f);
  }
}
