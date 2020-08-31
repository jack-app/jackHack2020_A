using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(PictureBehaviour))]
public class CallTakePicture : MonoBehaviour
{
    readonly string ClientID = "1cbf20db576d3f1";
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
        StartCoroutine(TweetWithScreenShot(filePath));
        images.ForEach(e => e.enabled = true);
        texts.ForEach(e => e.enabled = true);
    }

    private IEnumerator TweetWithScreenShot(string filepath)
    {
        yield return new WaitForEndOfFrame();

        // imgurへアップロード
        string UploadedURL = "";

        UnityWebRequest www;

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("image", Convert.ToBase64String(File.ReadAllBytes(filepath)));
        wwwForm.AddField("type", "base64");

        www = UnityWebRequest.Post("https://api.imgur.com/3/image.xml", wwwForm);

        www.SetRequestHeader("AUTHORIZATION", "Client-ID " + ClientID);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            XDocument xDoc = XDocument.Parse(www.downloadHandler.text);
            //Twitterカードように拡張子を外す
            string url = xDoc.Element("data").Element("link").Value;
            url = url.Remove(url.Length - 4, 4);
            UploadedURL = url;
        }

        string text = Uri.EscapeUriString("こんな雲ができました");
        string uri_str = "&url=" + UploadedURL;
        string hashtags = "&hashtags=" + Uri.EscapeUriString("くもあつめ");

        // ツイッター投稿用URL
        string TweetURL = "http://twitter.com/intent/tweet?text=" + text + uri_str + hashtags;

#if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", TweetURL));
#elif UNITY_EDITOR
        System.Diagnostics.Process.Start(TweetURL);
#else
            Application.OpenURL(TweetURL);
#endif
    }
}
