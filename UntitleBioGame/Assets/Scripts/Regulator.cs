using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Regulator : MonoBehaviour
{
  private int HP;

  private Collider2D plusButton;
  private Collider2D minusButton;
  private TextMeshPro levelText;
  private BodySystem bodySystem;

  // Start is called before the first frame update
  void Start()
  {
    minusButton = transform.Find("MinusButton").GetComponent<Collider2D>();
    plusButton = transform.Find("PlusButton").GetComponent<Collider2D>();
    bodySystem = transform.parent.GetComponent<BodySystem>();
    levelText = transform.Find("Level").GetComponent<TextMeshPro>();
    levelText.text = bodySystem.level.ToString();
  }

  void Update()
  {
    RegulateConnectorLevel();
  }

  void RegulateConnectorLevel()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

      RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

      if (hit.collider == plusButton)
      {
        bodySystem.IncreaseLevel();
        levelText.text = bodySystem.level.ToString();
      }
      else if (hit.collider == minusButton)
      {
        bodySystem.DecreaseLevel();
        levelText.text = bodySystem.level.ToString();
      }
    }
  }
}