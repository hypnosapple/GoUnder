using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Mail", menuName = "Mail")]
public class MailObject : ScriptableObject
{
    public string PersonName, EmailTitle, EmailAddress;
    [TextArea(15, 20)]
    public string EmailContent;
    public Color IconColor;
    public Texture IconSprite;
}
