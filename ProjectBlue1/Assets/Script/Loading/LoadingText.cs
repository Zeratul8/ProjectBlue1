using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI loadingText;

    private readonly string[] loadText = {"Loading.", "Loading..", "Loading..."};

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Coroutine_LoadingText());
    }

    IEnumerator Coroutine_LoadingText()
    {
        var sec = new WaitForSeconds(0.2f);
        int cnt = 0;
        while(true)
        {
            loadingText.text = loadText[cnt++];
            if (cnt > 2) cnt = 0;
            yield return sec;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
