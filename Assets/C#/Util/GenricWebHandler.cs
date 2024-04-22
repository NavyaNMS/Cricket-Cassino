using Newtonsoft.Json;
using System;
using UnityEngine;
class GenricWebHandler<T> where T : class
{
    private object obj;
    Action<T> onRequetcomplete;
    public GenricWebHandler(object responseType)
    {
        this.obj = responseType;
    }
    public GenricWebHandler()
    {

    }
    public void SendRequestToServer(string url, Action<T> OnRequestComplete)
    {
        WebRequestHandler.instance.Post(url, JsonConvert.SerializeObject(obj), OnServerResponse);
        onRequetcomplete = OnRequestComplete;
    }

    public void SendGetRequestToServer(string url, Action<T> OnRequestComplete)
    {
        WebRequestHandler.instance.Get(url, OnServerResponse);
        onRequetcomplete = OnRequestComplete;
    }

    private void OnServerResponse(string json, bool status)
    {

        try
        {
            //T currentRound = JsonUtility.FromJson<T>(json);
            if (!status)
            {
                MonoBehaviour.print("something went wrong");
                if (onRequetcomplete != null)
                    onRequetcomplete(null);
            }
            T currentRound = JsonConvert.DeserializeObject<T>(json);
            if (onRequetcomplete != null)
            {
                onRequetcomplete(currentRound);
            }
        }
        catch (Exception e)
        {
            T currentRound = JsonConvert.DeserializeObject<T>(json);
            MonoBehaviour.print("something went wrong");
            MonoBehaviour.print(e.StackTrace);
            onRequetcomplete(currentRound);
        }
    }

   
}
