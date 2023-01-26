using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipPanel : MonoBehaviour
{


  TextMeshProUGUI tooltipTitle;
  TextMeshProUGUI tooltipText;

  // Start is called before the first frame update
  void Awake()
  {
    tooltipTitle = transform.Find("Title").GetComponent<TextMeshProUGUI>();
    tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void Show(TooltipObject tooltip)
  {
    try
    {
      if (tooltip != null)
      {
        gameObject.SetActive(true);
        tooltipTitle.text = tooltip.title;
        tooltipText.text = tooltip.text;
      }
    }
    catch (System.Exception)
    {
      Debug.Log("Failed showing tooltip" + tooltip);

      throw;
    }
  }
}
