using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCell : MonoBehaviour
{
  [Header("Variables")]
  public float _speed = 0.8f;

  [Header("Unity Attributes")]
  private Transform target;
  private GameManager _gameManager;
  private Vector3 initialPosition;

  private void Start()
  {
    initialPosition = transform.position;
    _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  private void Update()
  {
    // if (target == null)
    // {
    //   Destroy(gameObject);
    //   return;
    // }

    bool hasReturnedHome = target == null && transform.position.Equals(initialPosition);
    if (hasReturnedHome) {
      Destroy(gameObject);
      GameManager.Instance.UpdateResource(ResourceType.WhiteCell, 1);
    }

    MoveToPosition(target == null ? initialPosition : target.position);
  }

  void MoveToPosition(Vector3 position)
  {
    float speed = target == null ? _speed / 2 : _speed;
    Vector3 dir = position - transform.position;
    float distanceThisFrame = speed * Time.deltaTime;

    if (dir.magnitude <= distanceThisFrame)
    {
      // HitTarget();
      return;
    }

    transform.Translate(dir.normalized * distanceThisFrame, Space.World);
  }

  public void Seek(Transform _target)
  {
    target = _target;
  }

  public bool HasTarget() {
    return target != null;
  }

  public void HitTarget()
  {
    if (target.gameObject.tag == "Virus")
    {
      Destroy(target.gameObject);
    }
    Debug.Log(gameObject.name);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Virus"))
    {
      Debug.Log("WhiteCell killed a virus");
      Destroy(other.gameObject);
      Destroy(gameObject);
    }
  }
}
