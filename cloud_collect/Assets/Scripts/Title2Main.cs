using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title2Main : MonoBehaviour
{
    public List<Text> texts;
    public List<SpriteRenderer> images;

    public void OnClick()
    {
        StartCoroutine(DelayNextScene());
    }

    IEnumerator DelayNextScene()
    {
        float h = 193;
        float s_end = 82;
        float v_end = 90;

        var statrt = Time.time;

        for (int i = 0; i < 60; i++) {
            var s = Mathf.Lerp(0, s_end, i / 60.0f);
            var v = Mathf.Lerp(100, v_end, i / 60.0f);
            texts.ForEach(e => e.color = Color.HSVToRGB(h / 360.0f, s / 100.0f, v / 100.0f));
            images.ForEach(e => e.color = Color.HSVToRGB(h / 360.0f, s / 100.0f, v / 100.0f));
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Main");
    }
}
