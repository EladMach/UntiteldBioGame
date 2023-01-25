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
  int generateIntervalTimeInSec;
  [SerializeField]
  Resource[] costs;

  [SerializeField]
  Resource[] output;

  [SerializeField]
  SystemCondition[] conditions;

  [SerializeField]
  Line[] inputLines;

  [SerializeField]
  Line[] outputLines;

  private int health = 10;

  private GameObject warning;
  private TextMeshPro outputText;
  private Vector3 outputTextInitialPosition;
  private TextMeshPro inputText;
  private Vector3 inputTextInitialPosition;
  private Transform cooldownOverlay;

  private float lastGenerateTime;

  void Start()
  {
    lastGenerateTime = Time.time;

    cooldownOverlay = transform.Find("System/CooldownOverlay");
    cooldownOverlay.localScale = new Vector3(1, 0, 1);

    warning = transform.Find("Warning").gameObject;
    warning.SetActive(false);
    outputText = transform.Find("OutputText").GetComponent<TextMeshPro>();
    outputTextInitialPosition = outputText.transform.localPosition;
    outputText.text = "";
    inputText = transform.Find("InputText").GetComponent<TextMeshPro>();
    inputTextInitialPosition = inputText.transform.localPosition;
    inputText.text = "";
    HideGenerateTooltip();

    transform.Find("System").GetComponent<SpriteRenderer>().color = GameManager.Instance.GetResourceColor(mainResourceType);
  }

  void Update()
  {
    GenerateResources();
    UpdateCooldown();
  }


  void GenerateResources()
  {

    float modifiedTimeInterval = ModifyTimeValue(generateIntervalTimeInSec);

    // if (lastGenerateTime + 2 < Time.time)
    // {
    HideGenerateTooltip();
    // }

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
          string outputMessage = "";
          string inputMessage = "";

          // Generate output
          foreach (var resource in output)
          {
            int modifiedGeneratedValue = ModifyGenerateValue(resource.value);
            string color = GameManager.Instance.GetResourceColorString(resource.type);
            GameManager.Instance.UpdateResource(resource.type, modifiedGeneratedValue);
            outputMessage += $"<color={color}>+{modifiedGeneratedValue} {resource.type.ToString()}</color> \n";
          }

          // Pay costs
          foreach (var resource in costs)
          {
            int modifiedGeneratedValue = ModifyGenerateValue(resource.value);
            string color = GameManager.Instance.GetResourceColorString(resource.type);
            GameManager.Instance.UpdateResource(resource.type, -modifiedGeneratedValue);
            inputMessage += $"<color={color}>-{modifiedGeneratedValue} {resource.type.ToString()}</color> \n";
          }

          ShowGenerateTooltip(outputMessage, true);
          ShowGenerateTooltip(inputMessage, false);

        }
      }
    }
    else
    {
      warning.SetActive(false);
    }
  }

  void ShowGenerateTooltip(string text, bool isOutput)
  {
    TextMeshPro textUI = isOutput ? outputText : inputText;
    Vector3 initialTextPosition = isOutput ? outputTextInitialPosition : inputTextInitialPosition; 
    Line[] connectingLines = isOutput ? outputLines : inputLines;

    textUI.text = text;

    Color color = textUI.color;
    color.a = 1;
    textUI.color = color;
    textUI.transform.localPosition = initialTextPosition;

    foreach (Line line in connectingLines)
    {
      line.SetLineColor();
    }

    // foreach (Line inputLine in inputLines)
    // {
    //   inputLine.SetLineColor();
    // }
  }

  void HideGenerateTooltip()
  {
    Vector3 outputTextPosition = outputText.transform.localPosition;
    Color outputTextColor = outputText.color;
    Vector3 inputTextPosition = inputText.transform.localPosition;
    Color inputTextColor = inputText.color;

    float interpolationValue = (Time.time - lastGenerateTime) / 4;

    outputTextColor.a = Mathf.Lerp(1, 0, interpolationValue);
    outputTextPosition.y = Mathf.Lerp(outputTextInitialPosition.y, outputTextInitialPosition.y + 0.1f, interpolationValue); ;
    inputTextColor.a = Mathf.Lerp(1, 0, interpolationValue);
    inputTextPosition.y = Mathf.Lerp(inputTextInitialPosition.y, inputTextInitialPosition.y + 0.1f, interpolationValue); ;

    outputText.color = outputTextColor;
    outputText.transform.localPosition = outputTextPosition;
    inputText.color = inputTextColor;
    inputText.transform.localPosition = inputTextPosition;

    foreach (Line outputLine in outputLines)
    {
      outputLine.HighlightLine(interpolationValue);
    }

    foreach (Line inputLine in inputLines)
    {
      inputLine.HighlightLine(interpolationValue);
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
      Debug.Log("Virus hit system!");
      health--;
      Destroy(other.gameObject);
    }
  }
}
