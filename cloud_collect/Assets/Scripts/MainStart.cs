using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainStart : MonoBehaviour
{
    public Image image;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        for (int i = 0; i < 60; i++) {
            image.color = new Color(1, 1, 1, i / 60.0f);
            text.color = new Color(0, 0, 0, i / 60.0f);
            yield return new WaitForEndOfFrame();
        }
    }
}
