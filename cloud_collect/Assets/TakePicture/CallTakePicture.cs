using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PictureBehaviour))]
public class CallTakePicture : MonoBehaviour
{
    PictureBehaviour pictureBehaviour;
    public List<Image> images;
    public List<Text> texts;

    // Start is called before the first frame update
    void Start()
    {
        pictureBehaviour = GetComponent<PictureBehaviour>();
    }
    
    public async void Onclick()
    {
        images.ForEach(e => e.enabled = false);
        texts.ForEach(e => e.enabled = false);
        string filePath = pictureBehaviour.TakePicture();
        await Task.Delay(100);
        images.ForEach(e => e.enabled = true);
        texts.ForEach(e => e.enabled = true);
        Debug.Log(filePath);
    }
}
