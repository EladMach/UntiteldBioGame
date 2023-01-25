using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{

  [SerializeField]
  ResourceType resourceType;

  SpriteRenderer icon;
  BoxCollider2D collider;


  TextMeshPro currentValueText;
  TextMeshPro maxValueText;

  private void Awake()
  {
    currentValueText = transform.Find("Value/CurrentValue").GetComponent<TextMeshPro>();
    maxValueText = transform.Find("Value/MaxValue").GetComponent<TextMeshPro>();
    icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
    collider = transform.GetComponent<BoxCollider2D>();
  }

  void Start()
  {
    icon.color = GameManager.Instance.GetResourceColor(resourceType);
  }


  void Update()
  {
  }

  public void SetCurrentValue(float value)
  {
    currentValueText.text = value.ToString();
  }

  public void SetMaxValue(float value)
  {
    maxValueText.text = value.ToString();
  }

  public void UpdateValueColor(bool isLow)
  {
    if (isLow)
    {
      currentValueText.color = Color.red;
    }
    else
    {
      currentValueText.color = Color.white;
    }
  }
}
