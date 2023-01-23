using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceStock
{
  public float value;
  public float maxValue;

  public ResourceStock(float maxValue, float value)
  {
    this.maxValue = maxValue;
    this.value = value;
  }

}
