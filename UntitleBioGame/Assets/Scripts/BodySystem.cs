using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class BodySystem : MonoBehaviour
{
  [SerializeField]
  string systemName;
  [SerializeField]
  ResourceType mainResourceType;

  [SerializeField]
  public int level;

    [SerializeField]
    public int health;
    
  [SerializeField]
  int generateIntervalTimeInSec;
  [SerializeField]
  Resource[] costs;

  [SerializeField]
  Resource[] output;
  [SerializeField]
  SystemCondition[] conditions;

  private GameObject warning;
  private TextMeshPro tooltipText;
  private Transform cooldownOverlay;

  private float lastGenerateTime;

  void Start()
  {
        health = 10;

    lastGenerateTime = Time.time;

    cooldownOverlay = transform.Find("System/CooldownOverlay");
    cooldownOverlay.localScale = new Vector3(1, 0, 1);

    warning = transform.Find("Warning").gameObject;
    warning.SetActive(false);
    tooltipText = transform.Find("Tooltip").GetComponent<TextMeshPro>();
    tooltipText.gameObject.SetActive(false);

    Color systemColor;
    ColorUtility.TryParseHtmlString(GameManager.Instance.GetResourceColor(mainResourceType), out systemColor);
    transform.Find("System").GetComponent<SpriteRenderer>().color = systemColor;
  }

  void Update()
  {
    GenerateResources();
    UpdateCooldown();
  }


  void GenerateResources()
  {

    float modifiedTimeInterval = ModifyTimeValue(generateIntervalTimeInSec);

    if (lastGenerateTime + 2 < Time.time)
    {
      tooltipText.gameObject.SetActive(false);
    }

    if (level > 0)
    {
      if (lastGenerateTime + modifiedTimeInterval < Time.time)
      {
        var missingResources = costs.Where(resource => !GameManager.Instance.HasStock(resource.type, ModifyGenerateValue(resource.value)));

        if (missingResources.Count() > 0)
        {
          warning.SetActive(true);
        }
        else
        {
          lastGenerateTime = Time.time;
          warning.SetActive(false);
          string generateText = "";

          // Generate output
          foreach (var resource in output)
          {
            int modifiedGeneratedValue = ModifyGenerateValue(resource.value);
            string color = GameManager.Instance.GetResourceColor(resource.type);
            GameManager.Instance.UpdateResource(resource.type, modifiedGeneratedValue);
            generateText += $"<color={color}>+{modifiedGeneratedValue} {resource.type.ToString()}</color> \n";
          }

          generateText += "\n";

          // Pay costs
          foreach (var resource in costs)
          {
            int modifiedGeneratedValue = ModifyGenerateValue(resource.value);
            string color = GameManager.Instance.GetResourceColor(resource.type);
            GameManager.Instance.UpdateResource(resource.type, -modifiedGeneratedValue);
            generateText += $"<color={color}>-{modifiedGeneratedValue} {resource.type.ToString()}</color> \n";
          }

          tooltipText.gameObject.SetActive(true);
          tooltipText.text = generateText;

        }
      }
    }
    else
    {
      warning.SetActive(false);
    }
  }

  void UpdateCooldown()
  {
    float modifiedTimeInterval = ModifyTimeValue(generateIntervalTimeInSec);
    float cooldownScale = 1 - Mathf.Clamp(((Time.time - lastGenerateTime) / modifiedTimeInterval), 0, 1);
    cooldownOverlay.localScale = new Vector3(1, cooldownScale, 1);
  }

  private int ModifyGenerateValue(int value)
  {
    return value * level;
  }

  private float ModifyTimeValue(float value)
  {
    float modifiedValue = value;

    foreach (SystemCondition condition in conditions)
    {
      ResourceStock resourceStock = GameManager.Instance.GetResourceStock(condition.resourceType);
      float valuePercentage = resourceStock.value / resourceStock.maxValue;
      if (condition.maxThreshold != 0 && valuePercentage > condition.maxThreshold)
      {
        modifiedValue *= condition.timeMultiplier;
      }
      else if (condition.minThreshold != 0 && valuePercentage < condition.minThreshold)
      {
        modifiedValue *= condition.timeMultiplier;
      }
    }

    return modifiedValue;
  }

  public void IncreaseLevel()
  {
    if (level < 3)
    {
      level++;
    }
  }
  public void DecreaseLevel()
  {
    if (level > 0)
    {
      level--;
    }
  }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Virus"))
        {
            Debug.Log("Virus");
            health--;
            Destroy(other.gameObject);
        }
    }
}


