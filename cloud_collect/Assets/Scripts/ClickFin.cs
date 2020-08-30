using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFin : MonoBehaviour
{
    public CloudManager manger;
    public GameObject rect;

    public void OnClick()
    {
        manger.Stop();
        rect.SetActive(true);
        gameObject.SetActive(false);
    }
}
