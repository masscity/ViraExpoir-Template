using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadHelper : MonoBehaviour
{
    public void GetCsvData(string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(CoroutineGetText(url, onError, onSuccess));
    }

    IEnumerator CoroutineGetText(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError 
                || unityWebRequest.result == UnityWebRequest.Result.DataProcessingError
                || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess(unityWebRequest.downloadHandler.text);
            }

            unityWebRequest.Dispose();
        }
    }

    public void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        StartCoroutine(CoroutineGetTexture(url, onError, onSuccess));
    }

    IEnumerator CoroutineGetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError
                || unityWebRequest.result == UnityWebRequest.Result.DataProcessingError
                || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                DownloadHandlerTexture downloadHandlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                onSuccess(downloadHandlerTexture.texture);
            }

            unityWebRequest.Dispose();
        }
    }
}
