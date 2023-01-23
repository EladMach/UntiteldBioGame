using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "TooltipObject", menuName = "UntitleBioGame/Tooltip", order = 0)]
public class TooltipObject : ScriptableObject
{
  [SerializeField]
  public string title;
  [SerializeField]
  [TextArea]
  public string text;
}
