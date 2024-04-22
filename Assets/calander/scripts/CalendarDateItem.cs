using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour {

    public void OnDateItemClick()
    {
        GetComponentInParent<Calendar>().OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }
}
