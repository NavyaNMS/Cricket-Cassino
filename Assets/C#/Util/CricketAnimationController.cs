using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class CricketAnimationController : MonoBehaviour
{
    [SerializeField] Sprite[] idelSprits;
    [SerializeField] Sprite[] shots0;
    [SerializeField] Sprite[] shots1;
    [SerializeField] Sprite[] shots2;
    [SerializeField] Sprite[] shots3;
    [SerializeField] Sprite[] shots4;
    [SerializeField] Sprite[] shots5;
    [SerializeField] Sprite[] shots6;
    [SerializeField] Sprite[] shots7;
    [SerializeField] Sprite[] shots8;
    [SerializeField] Sprite[] shots9;
    [SerializeField] List<Sprite[]> shotsFrames=new List<Sprite[]>();
    [SerializeField] Image animatonArea;
    [SerializeField] Image batsmanImg;
    private void Start()
    {

        shotsFrames.Add(shots0);
        shotsFrames.Add(shots1);
        shotsFrames.Add(shots2);
        shotsFrames.Add(shots3);
        shotsFrames.Add(shots4);
        shotsFrames.Add(shots5);
        shotsFrames.Add(shots6);
        shotsFrames.Add(shots7);
        shotsFrames.Add(shots8);
        shotsFrames.Add(shots9);

    }
    public void Play(int num)
    {
        StartCoroutine(UpdateFrams(shotsFrames[num]));
    }
    IEnumerator UpdateFrams(Sprite[] frams)
    {
        foreach (var item in idelSprits) 
        {
            batsmanImg.sprite = item;
            yield return new WaitForEndOfFrame();
        }
        foreach (var item in frams) 
        {
            batsmanImg.sprite = item;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2f);
        batsmanImg.sprite = idelSprits[0];

    }
}
