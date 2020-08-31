using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PictureBehaviour : MonoBehaviour
{
    private string _filename = "screenShot";
    [DllImport("__Internal")]
    private static extern string TakeScreenShot();
    /// <summary>
    /// 画像を撮影して指定のファイルパスに保存、ファイルパスを戻り値として渡す。
    /// </summary>
    public string TakePicture()
    {
        string filepath = Application.dataPath + "/" + _filename + ".png";
        StartCoroutine(Screenshot(filepath));
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken cancellationToken = source.Token;

        _ = Task.Run(() => Timeout(source));
        var task = Task.Run(() => WaitFileFinder(filepath), cancellationToken);

        Task.WaitAll(task);


        return filepath;
    }

    private IEnumerator Screenshot(string path)
    {
        yield return new WaitForEndOfFrame();
#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log(ScreenCapture.CaptureScreenshotAsTexture().EncodeToPNG());
        File.WriteAllBytes(path, ScreenCapture.CaptureScreenshotAsTexture().EncodeToPNG());
#else
        ScreenCapture.CaptureScreenshot(path);
#endif
    }

    async void Timeout(CancellationTokenSource source) {
        await Task.Delay(5000);
        source.Cancel();
    }


    async void WaitFileFinder(string path)
    {
        while (!File.Exists(path))
        {
            await Task.Delay(100);
        }
        Debug.Log("CaptureComplete");
    }

}
