using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Toast : MonoBehaviour
{
    public static Toast instance;
    [SerializeField] TMP_Text msgTxt;
    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
        gameObject.transform.GetChild(0).transform.gameObject.SetActive(true);
    }
    public void Show(string msg, float delay = .25f)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(DisplayMsg(msg, delay));
    }

    private IEnumerator DisplayMsg(string msg, float delay)
    {
        msgTxt.text = msg;
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}
