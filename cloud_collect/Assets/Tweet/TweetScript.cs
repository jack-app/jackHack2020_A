using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Text;
using Twity.Helpers;

public class TweetScript : MonoBehaviour
{
    string ac = env.ACCESS_TOKEN;
    string acs = env.ACCESS_TOKEN_SECRET;
    string ap = env.API_KEY;
    string aps = env.API_KEY_SECRET;
    


    public Image image;
    public Texture2D mytexture;
    byte[] bytes;
    static string REQUEST_URL = "https://upload.twitter.com/1.1/media/upload.json";

    env env;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //IEnumerator Upload()
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddBinaryData("media", Convert.FromBase64String("Assets/Tweet/test.png"));

    //    Debug.Log(1);
    //    //using (UnityWebRequest www = UnityWebRequest.Post("https://upload.twitter.com/1.1/media/upload.json", form))
    //    //{
    //    //    yield return www.SendWebRequest();

    //    //    if (www.isNetworkError || www.isHttpError)
    //    //    {
    //    //        Debug.Log(www.error);
    //    //    }
    //    //    else
    //    //    {
    //    //        Debug.Log("Form upload complete!");
    //    //    }
    //    //}

    //}

    private static string GenerateNonce()
    {
        return Guid.NewGuid().ToString("N");
    }

    string GenerateSignature(string ac, string ap, string tm, string no)
    {
        return "";
    }

    public static string GenerateHeaderWithAccessToken(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)
    {
        string signature = GenerateSignature(parameters, requestMethod, requestURL);

        StringBuilder requestParamsString = new StringBuilder();
        foreach (KeyValuePair<string, string> param in parameters)
        {
            requestParamsString.Append(String.Format("{0}=\"{1}\",", Helper.UrlEncode(param.Key), Helper.UrlEncode(param.Value)));
        }

        string authHeader = "OAuth realm=\"Twitter API\",";
        string requestSignature = String.Format("oauth_signature=\"{0}\"", Helper.UrlEncode(signature));
        authHeader += requestParamsString.ToString() + requestSignature;
        return authHeader;
    }

    private static void AddDefaultOauthParams(SortedDictionary<string, string> parameters, string ap)
    {
        parameters.Add("oauth_consumer_key", ap);
        parameters.Add("oauth_signature_method", "HMAC-SHA1");
        parameters.Add("oauth_timestamp", GenerateTimeStamp());
        parameters.Add("oauth_nonce", GenerateNonce());
        parameters.Add("oauth_version", "1.0");
    }

    private static string GenerateSignature(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)
    {
        string ac = env.ACCESS_TOKEN;
        string acs = env.ACCESS_TOKEN_SECRET;
        

        AddDefaultOauthParams(parameters, env.API_KEY);
        parameters.Add("oauth_token", env.ACCESS_TOKEN);

        StringBuilder paramString = new StringBuilder();
        foreach (KeyValuePair<string, string> param in parameters)
        {
            paramString.Append(Helper.UrlEncode(param.Key) + "=" + Helper.UrlEncode(param.Value) + "&");
        }
        paramString.Length -= 1; // Remove "&" at the last of string
        string requestHeader = Helper.UrlEncode(requestMethod) + "&" + Helper.UrlEncode(requestURL);
        string signatureData = requestHeader + "&" + Helper.UrlEncode(paramString.ToString());

        string signatureKey = Helper.UrlEncode(env.API_KEY_SECRET) + "&" + Helper.UrlEncode(env.ACCESS_TOKEN_SECRET);
        HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(signatureKey));
        byte[] signatureBytes = hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
        return Convert.ToBase64String(signatureBytes);
    }

    private static string GenerateTimeStamp()
    {
        DateTimeOffset baseDt = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        return ((DateTimeOffset.Now - baseDt).Ticks / 10000000).ToString();
    }


    //public void OnClick()
    //{
    //    WWWForm form = new WWWForm();

    //    form.AddField("statuses", "API");

    //    UnityWebRequest webRequest = UnityWebRequest.Post("https://api.twitter.com/1.1/statuses/update.json",form);


    //    string tm = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
    //    string no = GenerateNonce();

    //    SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();



    //    webRequest.SetRequestHeader("oauth_token", ac);
    //    webRequest.SetRequestHeader("oauth_consumer_key", ap);
    //    webRequest.SetRequestHeader("oauth_timestamp", tm);
    //    webRequest.SetRequestHeader("oauth_signature_method", "HMAC-SHA1");
    //    webRequest.SetRequestHeader("oauth_version", "1.0");
    //    webRequest.SetRequestHeader("oauth_nonce", no);

    //    //string sg = signiture(ac, ap, tm, no);

    //    yield return webRequest.SendWebRequest();



    //    //WWWForm form = new WWWForm();

    //    //bytes = readPngFile("Assets/Tweet/test.png");
    //    //form.AddBinaryData("media", bytes, "", "");

    //    //Debug.Log(Convert.ToBase64String(bytes));

    //    //SortedDictionary<string, string> p = new SortedDictionary<string, string>();
    //    ////

    //    //UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL, form);
    //    ////yield return SendRequest(request, parameters, "POST", REQUEST_URL, form);
    //    //Debug.Log(1);
    //    //if (request.isNetworkError)
    //    //{
    //    //    //callback(false);
    //    //    Debug.Log(-1);
    //    //}
    //    //else
    //    //{
    //    //    if (request.responseCode == 200 || request.responseCode == 201)
    //    //    {
    //    //        string[] arr = request.downloadHandler.text.Split("&"[0]);
    //    //        Dictionary<string, string> d = new Dictionary<string, string>();
    //    //        foreach (string s in arr)
    //    //        {
    //    //            string k = s.Split("="[0])[0];
    //    //            string v = s.Split("="[0])[1];
    //    //            d[k] = v;
    //    //        }
    //    //        Debug.Log(2);
    //    //        //Oauth.accessToken = d["oauth_token"];
    //    //        //Oauth.accessTokenSecret = d["oauth_token_secret"];
    //    //        //screenName = d["screen_name"];
    //    //        //callback(true);
    //    //    }
    //    //    else
    //    //    {
    //    //        //callback(false);
    //    //        Debug.Log(3);
    //    //    }
    //    //}

    //}

    byte[] readPngFile(string path)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            BinaryReader bin = new BinaryReader(fileStream);
            byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);
            bin.Close();
            return values;
        }

    }
}
