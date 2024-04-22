using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class test : MonoBehaviour
{
    public GameObject toggleBtn;
    public Transform parent;
    private void Start()
    {
        InstantiateObjets();
    }

    void InstantiateObjets()
    {
        for (int i = 10; i < 24; i++)
        {
            var obj00 = Instantiate(toggleBtn,parent);
            obj00.gameObject.transform.GetChild(1).
                GetComponent<Text>().text = $"{i}:00";


            var obj15 = Instantiate(toggleBtn,parent);
            obj15.gameObject.transform.GetChild(1).
                GetComponent<Text>().text = $"{i}:15";

            var obj30 = Instantiate(toggleBtn,parent);
            obj30.gameObject.transform.GetChild(1).
                GetComponent<Text>().text = $"{i}:30";

            
            var obj45 = Instantiate(toggleBtn,parent);
            obj45.gameObject.transform.GetChild(1).
                GetComponent<Text>().text = $"{i}:45";


        }
    }
}
