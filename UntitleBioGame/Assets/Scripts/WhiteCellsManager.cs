using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Linq;

public class WhiteCellsManager : MonoBehaviour
{

  [Header("Variables")]
  public float range = 15f;
  public float fireRate = 1f;
  private float fireCountdown = 0f;
  public string virusTag;


  [Header("Unity Attributes")]

  public GameObject whiteCellPrefab;
  private Transform target;

  GameObject[] enemies;
  WhiteCell[] whiteCells;


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
    enemies = GameObject.FindGameObjectsWithTag(virusTag);
    whiteCells = GameObject.FindObjectsOfType<WhiteCell>();

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

    // ReTarget white cells
    if (target != null)
    {
      foreach (WhiteCell whiteCell in whiteCells)
      {
        if (!whiteCell.HasTarget())
        {
          whiteCell.Seek(target);
          break;
        }
      }
    }
  }

  void Shoot()
  {
    if (whiteCells.Length < enemies.Length)
    {
      ResourceStock whiteCellsStock = GameManager.Instance.GetResourceStock(ResourceType.WhiteCell);

      if (whiteCellsStock.value > 0)
      {
        GameObject whiteCellGO = (GameObject)Instantiate(whiteCellPrefab, transform.position, Quaternion.identity);
        WhiteCell whiteCell = whiteCellGO.GetComponent<WhiteCell>();

        if (whiteCell != null)
        {
          GameManager.Instance.UpdateResource(ResourceType.WhiteCell, -1);
          whiteCell.Seek(target);
        }
      }
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, range);
  }

}
