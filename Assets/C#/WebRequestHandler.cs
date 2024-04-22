using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class WebRequestHandler : MonoBehaviour
{
    public static WebRequestHandler instance;
    int attempts = 2;
    private void Awake()
    {
        instance = this;
    }
    public void Get(string url, Action<string, bool> OnRequestProcessed)
    {
        StartCoroutine(GetRequest(url, OnRequestProcessed));
    }

    private IEnumerator GetRequest(string url, Action<string, bool> OnRequestProcessed)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            print("check internet connection");
            yield break;
        }
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log($"web {url} request error in Get method with responce code : " + request.responseCode);
            OnRequestProcessed(request.error, false);
        }
        else
        {
            Debug.Log("sending get web request  : " + url + "got response:" + request.downloadHandler.text);
            OnRequestProcessed(request.downloadHandler.text, true);
        }
        request.Dispose();
    }




    public void Post(string url, string json, Action<string, bool> OnRequestProcessed)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            print("check internet connection");
            return;
        }
        Debug.Log(url + " json request: " + json);
        StartCoroutine(PostRequest(url, json, OnRequestProcessed));
    }

    private IEnumerator PostRequest(string url, string json, Action<string, bool> OnRequestProcessed,int attempt=2)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result ==UnityWebRequest.Result.ConnectionError)
        {

            Debug.LogError("something went wrong");
            if (attempt == 0)
            {
                OnRequestProcessed(request.error, false);
            }
            else
            {
                StartCoroutine(PostRequest(url, json, OnRequestProcessed, attempt--));
                Debug.Log("trying again");
            } 

        }
        else
        {
            Debug.Log(url + " json response: " + request.downloadHandler.text);
            OnRequestProcessed(request.downloadHandler.text, true);
        }
        request.Dispose();
    }




    public void DownloadSprite(string url, Action<Sprite> OnDownloadComplete)
    {
        StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
    }

    IEnumerator LoadFromWeb(string url, Action<Sprite> OnDownloadComplete)
    {
        UnityWebRequest webRequest = new UnityWebRequest(url);
        DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = textureDownloader;
        yield return webRequest.SendWebRequest();
        if (!(webRequest.isNetworkError || webRequest.isHttpError))
        {
            Texture2D texture = textureDownloader.texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
            OnDownloadComplete(sprite);
        }
        else
        {
            Debug.LogError("failed to download image");
        }
    }




    public void DownloadTexture(string url, Action<Texture> OnDownloadComplete)
    {
        StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
    }

    IEnumerator LoadFromWeb(string url, Action<Texture> OnDownloadComplete)
    {
        UnityWebRequest webRequest = new UnityWebRequest(url);
        DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = textureDownloader;
        yield return webRequest.SendWebRequest();
        if (!(webRequest.isNetworkError || webRequest.isHttpError))
        {
            OnDownloadComplete(textureDownloader.texture);
        }
        else
        {
            Debug.LogError("failed to download image");
        }
    }
}