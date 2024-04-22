using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FootballWindow : AnimationWindow
{
    public Sprite[] goalKeeperFrma_L;
    public Sprite[] goalKeeperFrma_R;
    public Sprite[] kickFrams;
    public Sprite goalkeeperIdl;
    public Sprite playerIdl;
    public GameObject football;
    public GameObject footballPlayer;
    public GameObject goalKeeper;
    public override GameIds id => GameIds.football;
    [SerializeField] Transform gameGrid;
         
    private void Start()
    {
        startingPos = football.transform.position;
    }
    public override void Play(int winNo)
    {
        base.Play(winNo);
        StartCoroutine(PlayKickAnimation(winNo));
    }

    IEnumerator PlayKickAnimation(int winIndex)
    {
        int i = 0;
        bool flag = false;
        foreach (var fram in kickFrams)
        {
            footballPlayer.GetComponent<Image>().sprite = fram;
            i++;
            if (i > kickFrams.Length - 7&& !flag)
            {
                StartCoroutine(PlayFootballAnimation(winIndex));
                flag = true;
            }

            yield return new WaitForSecondsRealtime(.05f);
        }
        if (winIndex < 50)
            StartCoroutine(PlayGoalKeeperAnimation(goalKeeperFrma_R));
        else
            StartCoroutine(PlayGoalKeeperAnimation(goalKeeperFrma_L));
    }
   public float animTime=5;
    private Vector3 startingPos;
    [SerializeField] iTween.EaseType easeType;
    IEnumerator PlayFootballAnimation(int winIndex)
    {
        var target = gameGrid.GetChild(winIndex);
        print($"target name{target.name}");
       // iTween.MoveBy(football, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
        iTween.MoveTo(football, iTween.Hash("position", target.position, "time", animTime, "easetype", easeType));

        //iTween.MoveBy(football, iTween.Hash("position", target.position, "time", animTime, "easetype", easeType));
        yield return new WaitForSeconds(animTime);
        gameGrid.GetChild(winIndex).GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(3f);
        football.transform.position = startingPos;
        goalKeeper.GetComponent<Image>().sprite = goalkeeperIdl;
        footballPlayer.GetComponent<Image>().sprite = playerIdl;
        yield return new WaitForSeconds(5f);
        gameGrid.GetChild(winIndex).GetComponent<Image>().color = Color.white;

      
    }
    IEnumerator PlayGoalKeeperAnimation(Sprite[] frams)
    {
        foreach (var fram in frams)
        {
            goalKeeper.GetComponent<Image>().sprite = fram;
            yield return new WaitForSecondsRealtime(.05f);
        }
    }
}
