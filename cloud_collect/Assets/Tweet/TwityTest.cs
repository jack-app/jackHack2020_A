using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twity.DataModels.Core;
using System.IO;


public class TwityTest : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{


		Twity.Oauth.consumerKey = env.API_KEY;
		Twity.Oauth.consumerSecret = env.API_KEY_SECRET;
		Twity.Oauth.accessToken = env.ACCESS_TOKEN;
		Twity.Oauth.accessTokenSecret = env.ACCESS_TOKEN_SECRET;

		byte[] imgBinary = File.ReadAllBytes("Assets/Tweet/Test.png");
		string imgbase64 = System.Convert.ToBase64String(imgBinary);

		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters["media_data"] = imgbase64;
		//parameters["additional_owners"] = "additional owner if you have";
		StartCoroutine(Twity.Client.Post("media/upload", parameters, MediaUploadCallback));

		//Dictionary<string, string> parameters = new Dictionary<string, string>();
		//parameters["status"] = "Tweet from Unity";
		//StartCoroutine(Twity.Client.Post("statuses/update", parameters, Callback));
	}

	void Callback(bool success, string response)
	{
		if (success)
		{
			Tweet tweet = JsonUtility.FromJson<Tweet>(response);
		}
		else
		{
			Debug.Log(response);
		}
	}



	void MediaUploadCallback(bool success, string response)
	{
		if (success)
		{
			UploadMedia media = JsonUtility.FromJson<UploadMedia>(response);

			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters["media_ids"] = media.media_id.ToString();
			parameters["status"] = "Tweet text with image";
			StartCoroutine(Twity.Client.Post("statuses/update", parameters, StatusesUpdateCallback));
		}
		else
		{
			Debug.Log(response);
		}
	}

	void StatusesUpdateCallback(bool success, string response)
	{
		if (success)
		{
			Tweet tweet = JsonUtility.FromJson<Tweet>(response);
		}
		else
		{
			Debug.Log(response);
		}
	}

}
