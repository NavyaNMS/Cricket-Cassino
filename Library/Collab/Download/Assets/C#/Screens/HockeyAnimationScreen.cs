
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class HockeyAnimationScreen : Screen
{
    public override ScreenName ScreenID => ScreenName.HockeyScreen;

    public Sprite[] animation1;
    public Sprite[] animation2;
    public Button testBtn;
    Vector3 ball_initialPosion;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize(ScreenController screenController)
    {
        ball_initialPosion = football.transform.position;
        testBtn.onClick.AddListener(()=>PlayAnimation());
        base.Initialize(screenController);
    }

    public override void Show(object data = null)
    {
        int[] winNos = (int[])data;
        base.Show(data);
        PlayAnimation(winNos);
    }

    public void PlayAnimation(int[] no=null)
    {
        int[] n = new int[] { 11, 55, 22, 11, 22, 65, 54, 16, 85, 10 };
        StartCoroutine(KickAnimation(no));
    }
    public Image animationWindow;
    public int shootNo = 13;//this is the index when the football gets removed from the fram
    //it is constant in every frams
    public float delay = 1;
    IEnumerator KickAnimation(int[] winNos)
    {
        var diffrentAnimations = new Sprite[][] { animation1, animation2 };

        for (int i = 0; i < winNos.Length; i++)
        {
            int randomIndex = Random.Range(0, diffrentAnimations.Length);
            ChangeSerise(i);
            var frams = diffrentAnimations[randomIndex];
            int index = 0;
            foreach (var item in frams)
            {
                animationWindow.sprite = item;
                if (index == shootNo)
                {
                    int winNO = winNos[i];
                    StartCoroutine(MoveFootball(winNO));
                }
                yield return new WaitForSeconds(delay);
                index++;
            }

            yield return new WaitForSeconds(1f);
            animationWindow.sprite = frams[0];
            football.transform.position = ball_initialPosion;
            yield return new WaitForSeconds(2f);

        }
        yield return new WaitForSeconds(1f);
        screenController.ShowBettingScreen((int)GameIds.hockey);
    }
    char[] bettingSerise = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

    private void ChangeSerise(int changeTo)
    {
        var letter = bettingSerise[changeTo];

        for (int j = 0; j < resultGrid.childCount; j++)
        {
            if (j < 10)
            {
                resultGrid.GetChild(j).GetChild(0).GetComponent<TMP_Text>().text = $"{letter}\n0{j}";
            }
            else
            {
                resultGrid.GetChild(j).GetChild(0).GetComponent<TMP_Text>().text = $"{letter}\n{j}";
            }
            resultGrid.GetChild(j).GetComponent<Image>().color = Color.white;
        }

    }
    public Transform resultGrid;
    public GameObject football;
    public float footballSpeed = 1;//lower the faster
    public iTween.EaseType easeType;
    IEnumerator MoveFootball(int winNO)
    {
        var target = resultGrid.GetChild(winNO);
        iTween.MoveTo(football, iTween.Hash("position", target.position, "time", footballSpeed, "easetype", easeType));
        yield return new WaitForSeconds(footballSpeed);
        resultGrid.GetChild(winNO).GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(5f);
        resultGrid.GetChild(winNO).GetComponent<Image>().color = Color.white;
    }
}
