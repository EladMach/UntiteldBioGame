using System;

[Serializable]
public struct SystemCondition
{
  public ResourceType resourceType;
  public float maxThreshold;
  public float minThreshold;

  public float timeMultiplier;
  public float generationMultiplier;
}
