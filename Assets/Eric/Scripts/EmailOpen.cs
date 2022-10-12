using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailOpen : MonoBehaviour
{
    public GameObject MyEmailContent;
    public GameObject EmailContentParent;
    public void OpenMyContent()
    {
        for (int i = 0; i < EmailContentParent.transform.childCount; i++)
        {
            EmailContentParent.transform.GetChild(i).gameObject.SetActive(false);
        }
        MyEmailContent.SetActive(true);
    }
}
