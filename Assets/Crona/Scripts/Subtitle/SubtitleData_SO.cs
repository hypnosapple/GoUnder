using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Subtitle", menuName = "Subtitle Data")]
public class SubtitleData_SO : ScriptableObject
{
    public string ClipName;

    [TextArea(5, 200)]
    public List<string> Contents;

    public List<float> VisibleTime;

    public AudioClip AudioFile;
}
