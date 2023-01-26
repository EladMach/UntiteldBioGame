using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class Line : MonoBehaviour
{
  [SerializeField]
  ResourceType resourceType;

  float LINE_WIDTH = 0.05f;

  private LineRenderer lineRenderer;
  private Connector[] connectors;

  private void Awake()
  {
    lineRenderer = GetComponent<LineRenderer>();
    connectors = GetComponentsInChildren<Connector>();
    lineRenderer.positionCount = connectors.Length;
    lineRenderer.startWidth = LINE_WIDTH;
    lineRenderer.endWidth = LINE_WIDTH;

    for (int i = 0; i < connectors.Length; i++)
    {
      lineRenderer.SetPosition(i, connectors[i].transform.position);
      connectors[i].gameObject.SetActive(false);
      // if (i > 0 && i < connectors.Length - 1) {
      //   connectors[i].gameObject.SetActive(false);
      // } else {
      //   connectors[i].GetComponent<SpriteRenderer>().color = GameManager.Instance.GetResourceColor(resourceType);
      // }
    }
  }
  void Start()
  {

    // if (!lineRenderer.material)
    // {
    //   lineRenderer.material = new Material(Shader.Find("Sprites/Bla"));
    // }

    SetLineColor();


    // int connectorIndex = 0;
    // foreach (Transform connector in transform)
    // {
    //   Debug.Log("Connector position" + connector.position);
    //   //   connectors.Add(connector);
    //   lineRenderer.SetPosition(connectorIndex, connector.localPosition);
    //   //   connectorIndex++;
    // }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void SetLineColor()
  {
    Color color = GameManager.Instance.GetResourceColor(resourceType);
    color.a = 0.3f;
    lineRenderer.startColor = color;
    lineRenderer.endColor = color;
  }

  public void HighlightLine(float interpolationValue)
  {
    Color color = GameManager.Instance.GetResourceColor(resourceType);
    color.a = Mathf.Lerp(1f, 0.3f, interpolationValue);
    lineRenderer.startColor = color;
    lineRenderer.endColor = color;
  }
}
