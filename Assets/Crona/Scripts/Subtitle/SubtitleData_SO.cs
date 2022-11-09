using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Subtitle", menuName = "Subtitle Data")]
public class SubtitleData_SO : ScriptableObject
{
    [TextArea(5, 200)]
    public List<string> content;

    public List<float> visibleTime;
}
