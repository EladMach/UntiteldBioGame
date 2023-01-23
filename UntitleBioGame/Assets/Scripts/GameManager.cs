using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;

  // Game UI
  [SerializeField]
  Transform resourcePool;
  [SerializeField]
  Slider energyBar;
  [SerializeField]
  GameObject gameOverUI;

  [SerializeField]
  public TooltipPanel tooltipPanel;


  private Button restartButton;
  private Dictionary<ResourceType, ResourceUI> resourcePoolItems;


  // Resources
  Dictionary<ResourceType, ResourceStock> resourcesPool;
  Dictionary<ResourceType, string> resourceColors;


  [SerializeField]
  Resource[] intake;

  [SerializeField]
  int intakeIntervalTimeInSec;

  private float lastIntakeTime;


  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }

    // gameOverUI.SetActive(false);
    restartButton = gameOverUI.GetComponentInChildren<Button>();
    restartButton.onClick.AddListener(RestartGame);

    tooltipPanel.Hide();

    ResetResourcePool();
  }

  private void Update()
  {
    Intake();
    CheckGameOver();
    TooltipDetector();
  }

  void ResetResourcePool()
  {
    resourceColors = new Dictionary<ResourceType, string>();
    resourcesPool = new Dictionary<ResourceType, ResourceStock>();
    resourcePoolItems = new Dictionary<ResourceType, ResourceUI>();

    resourceColors.Add(ResourceType.Energy, "#06d6a0");
    resourceColors.Add(ResourceType.Blood, "#d90429");
    resourceColors.Add(ResourceType.Oxygen, "#ade8f4");
    resourceColors.Add(ResourceType.Nutrient, "#C86AAF");
    resourceColors.Add(ResourceType.WhiteCell, "#edede9");
    resourceColors.Add(ResourceType.Waste, "#C38783");

    resourcesPool.Add(ResourceType.Energy, new ResourceStock(60, 45));
    energyBar.minValue = 0;
    resourcesPool.Add(ResourceType.Blood, new ResourceStock(50, 40));
    resourcesPool.Add(ResourceType.Oxygen, new ResourceStock(30, 30));
    resourcesPool.Add(ResourceType.Nutrient, new ResourceStock(30, 0));
    resourcesPool.Add(ResourceType.WhiteCell, new ResourceStock(25, 5));
    resourcesPool.Add(ResourceType.Waste, new ResourceStock(35, 0));

    resourcePoolItems[ResourceType.Blood] = resourcePool.Find("Blood").GetComponentInChildren<ResourceUI>();
    resourcePoolItems[ResourceType.Oxygen] = resourcePool.Find("Oxygen").GetComponentInChildren<ResourceUI>();
    resourcePoolItems[ResourceType.Nutrient] = resourcePool.Find("Nutrient").GetComponentInChildren<ResourceUI>();
    resourcePoolItems[ResourceType.WhiteCell] = resourcePool.Find("WhiteCells").GetComponentInChildren<ResourceUI>();
    resourcePoolItems[ResourceType.Waste] = resourcePool.Find("Waste").GetComponentInChildren<ResourceUI>();
  }

  private void Start()
  {
    UpdateResourceStockUI(ResourceType.Energy);
    UpdateResourceStockUI(ResourceType.Blood);
    UpdateResourceStockUI(ResourceType.Oxygen);
    UpdateResourceStockUI(ResourceType.Nutrient);
    UpdateResourceStockUI(ResourceType.WhiteCell);
    UpdateResourceStockUI(ResourceType.Waste);
  }

  void UpdateResourceStockUI(ResourceType resourceType)
  {
    var resourceStock = resourcesPool[resourceType];
    if (resourceType == ResourceType.Energy)
    {
      energyBar.value = resourceStock.value;
      energyBar.maxValue = resourceStock.maxValue;
    }
    else
    {
      resourcePoolItems[resourceType].SetMaxValue(resourceStock.maxValue);
      resourcePoolItems[resourceType].SetCurrentValue(resourceStock.value);
      if (resourceType == ResourceType.Waste)
      {
        resourcePoolItems[resourceType].UpdateValueColor(resourceStock.value / resourceStock.maxValue > 0.7);
      }
      else
      {
        resourcePoolItems[resourceType].UpdateValueColor(resourceStock.value / resourceStock.maxValue < 0.3);
      }
    }
  }

  public void UpdateResource(ResourceType resourceType, int valueChange)
  {
    if (resourcesPool.ContainsKey(resourceType))
    {
      var resourceStock = resourcesPool[resourceType];
      resourceStock.value = Mathf.Clamp(resourceStock.value + valueChange, 0, resourceStock.maxValue);
      resourcesPool[resourceType] = resourceStock;
      UpdateResourceStockUI(resourceType);
    }
  }

  public bool HasStock(ResourceType resourceType, int requiredValue)
  {
    if (resourcesPool.ContainsKey(resourceType))
    {
      var resourceStock = resourcesPool[resourceType];
      return resourceStock.value >= requiredValue;
    }
    return false;
  }

  public ResourceStock GetResourceStock(ResourceType type)
  {
    return resourcesPool[type];
  }

  public string GetResourceColor(ResourceType type)
  {
    return resourceColors[type];
  }


  void Intake()
  {
    if (lastIntakeTime + intakeIntervalTimeInSec < Time.time)
    {
      foreach (var resource in intake)
      {
        UpdateResource(resource.type, resource.value);
      }
    }
  }

  void TooltipDetector()
  {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

    if (hit && hit.collider)
    {
      Tooltip tooltip = hit.transform.GetComponent<Tooltip>();
      if (tooltip != null)
      {
        tooltipPanel.Show(tooltip.tooltipObject);
      }
    }
    else
    {
      tooltipPanel.Hide();
    }
  }

  void CheckGameOver()
  {
    if (resourcesPool[ResourceType.Energy].value == 0)
    {
      gameOverUI.SetActive(true);
    }
  }

  void RestartGame()
  {
    SceneManager.LoadScene("MainScene");
  }
}
