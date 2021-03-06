﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CallTakePicture : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenToBlankWindow(string _url);

    readonly string ClientID = "1cbf20db576d3f1";
    PictureBehaviour pictureBehaviour;
    public Camera camera;
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
        StartCoroutine(TweetWithScreenShot());
    }

    private IEnumerator TweetWithScreenShot()
    {
        camera.targetTexture = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, 16);
        yield return null;
        yield return new WaitForEndOfFrame();

        var renderTexture = camera.targetTexture;
        var renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        var rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);

        yield return null;

        var png = renderResult.EncodeToPNG();

        yield return null;

        // imgurへアップロード
        string UploadedURL = "";

        UnityWebRequest www;

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("image", Convert.ToBase64String(png));

        yield return null;

        RenderTexture.ReleaseTemporary(renderTexture);
        camera.targetTexture = null;

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
        string TweetURL = "https://twitter.com/intent/tweet?text=" + text + uri_str + hashtags;

#if UNITY_WEBGL && !UNITY_EDITOR
            OpenToBlankWindow(TweetURL);
#elif UNITY_EDITOR
        System.Diagnostics.Process.Start(TweetURL);
#else
            Application.OpenURL(TweetURL);
#endif
        images.ForEach(e => e.enabled = true);
        texts.ForEach(e => e.enabled = true);
        SceneManager.LoadScene("Finish");
    }
}
