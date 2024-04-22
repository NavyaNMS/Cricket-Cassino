using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnApplicationPause(bool pause)
    {
        print("application pause " + pause);
    }
}
