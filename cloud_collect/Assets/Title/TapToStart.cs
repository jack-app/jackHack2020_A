using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TapToStart : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

#if UNITY_IOS || UNITYANDROID
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Main");
        }
#else
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Main");
        }
#endif
    }
}
