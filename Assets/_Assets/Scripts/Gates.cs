using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates : MonoBehaviour
{
    public int bridgeAntNo, playerAntNo, aiPlayerAntNo1, aiPlayerAntNo2, aiPlayerAntNo3;
    public List<Transform> childAntTraList;
    public float playerMinProgress, ai1MinProgress, ai2MinProgress, ai3MinProgress, playerMaxProgress, ai1MaxProgress, ai2MaxProgress, ai3MaxProgress;
    private void Start()
    {
    }
    public void UpdateVal(GameObject tmpObj)
    {
        if (tmpObj.name == "Player")
        {
            aiPlayerAntNo1 -= 1;
            aiPlayerAntNo2 -= 1;
            aiPlayerAntNo3 -= 1;
        }
        else if (tmpObj.name == "Ai1")
        {
            playerAntNo -= 1;
            aiPlayerAntNo2 -= 1;
            aiPlayerAntNo3 -= 1;
        }
        else if (tmpObj.name == "Ai2")
        {
            aiPlayerAntNo1 -= 1;
            playerAntNo -= 1;
            aiPlayerAntNo3 -= 1;
        }
        else if (tmpObj.name == "Ai3")
        {
            aiPlayerAntNo1 -= 1;
            aiPlayerAntNo2 -= 1;
            playerAntNo -= 1;
        }

        if (playerAntNo <= 0)
        {
            playerAntNo = 0;
        }
        if (aiPlayerAntNo1 <= 0)
        {
            aiPlayerAntNo1 = 0;
        }
        if (aiPlayerAntNo2 <= 0)
        {
            aiPlayerAntNo2 = 0;
        }
        if (aiPlayerAntNo3 <= 0)
        {
            aiPlayerAntNo3 = 0;
        }
    }
}
