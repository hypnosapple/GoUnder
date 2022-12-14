using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Robot1 : MonoBehaviour
{
    public EnableFunction B, A;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(clickAutomation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator clickAutomation()
    {
        yield return new WaitForSeconds(0.5f);
        B.MailAndInfoButton();

        yield return new WaitForSeconds(0.5f);
        B.MailAndInfoButton();

        yield return new WaitForSeconds(0.5f);
        B.MailAndInfoButton();

        yield return new WaitForSeconds(0.5f);
        A.MailAndInfoButton();
    }
}
