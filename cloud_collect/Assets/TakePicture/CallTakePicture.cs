using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PictureBehaviour))]
public class CallTakePicture : MonoBehaviour
{
    PictureBehaviour pictureBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        pictureBehaviour = GetComponent<PictureBehaviour>();
    }
    
    public void Onclick()
    {
        string filePath = pictureBehaviour.TakePicture();
        Debug.Log(filePath);
    }
}
