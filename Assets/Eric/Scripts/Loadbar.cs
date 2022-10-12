using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadbar : MonoBehaviour
{
    public GameObject MainPage;

    private void OnEnable()
    {
        StartCoroutine(StartFilling());
        Debug.Log("Start filling");
    }

    IEnumerator StartFilling()
    {
        float time = 0;
        float duration = 2;
        Vector2 targetLength = this.GetComponent<RectTransform>().sizeDelta;
        while (time < duration)
        {
            this.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0,12),targetLength, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        this.GetComponent<RectTransform>().sizeDelta = targetLength;
        MainPage.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
    }
}
