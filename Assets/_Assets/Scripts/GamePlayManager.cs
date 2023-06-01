using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[Serializable]
public struct LevelManager
{
    public List<GameObject> plateformObjList;
}

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager In;

    public GameObject pfObj, pfEndObj;
    public float pfDist;
    public List<LevelManager> levelManagers;
    public List<PlateForm> plateFormsList;
    public int tmpLvlNo;
    Vector3 tmpPos;

    public bool isWin = false;

    public GameObject cPAntObj, cAiAnt1Obj, cAiAnt2Obj, cAiAnt3Obj, gamePlayScreen, levelcompleteScreen, levelFailedScreen;
    public Text lvlTxt, lcTxt, lfTxt;
    public GameObject lastPlateformObj;
    public List<Transform> tmpAntList;

    public List<Player> allPlayerList;

    private void Awake()
    {
        In = this;
    }
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("TmpLvlNo"))
        {
            PlayerPrefs.SetInt("TmpLvlNo", 1);
        }
        if (!PlayerPrefs.HasKey("LvlNo"))
        {
            PlayerPrefs.SetInt("LvlNo", 1);
        }
        lvlTxt.text = "Level " + PlayerPrefs.GetInt("TmpLvlNo");
        lcTxt.text = "Level " + PlayerPrefs.GetInt("TmpLvlNo");
        lfTxt.text = "Level " + PlayerPrefs.GetInt("TmpLvlNo");
        tmpLvlNo = PlayerPrefs.GetInt("LvlNo")-1;

        for (int i=0; i < levelManagers[tmpLvlNo].plateformObjList.Count; i++)
        {
            tmpPos = new Vector3(0f, 0f, i * pfDist);
            GameObject tmppfStartObj = Instantiate(levelManagers[tmpLvlNo].plateformObjList[i], tmpPos, Quaternion.identity) as GameObject;
            tmppfStartObj.name = "PlateForm" + (i + 1).ToString();
            plateFormsList.Add(tmppfStartObj.GetComponent<PlateForm>());
            lastPlateformObj = tmppfStartObj;
        }
    }

    public void CheckWinLooseFunc()
    {
        for (int i=0; i< allPlayerList.Count; i++)
        {
            if (allPlayerList[i].isWin == false)
            {
                allPlayerList[i].LooseFunc();
            }
        }
    }

    public void SetFinalGate(GameObject tmpObj, Player player)
    {
        PlateForm plateForm = plateFormsList[player.plateFormInd].GetComponent<PlateForm>();
        int tmpNo = plateForm.plateformTraList.Count;
        int sameInd = 0;
        for (int i=0; i< tmpNo; i++)
        {
            if (plateForm.plateformTraList[i].gameObject.name == tmpObj.name)
            {
                sameInd = i;
            }
        }
        if (plateForm.plateformTraList[sameInd].gameObject.name == tmpObj.name)
        {
            plateForm.plateformTraList.RemoveAt(sameInd);
        }
    }

    public void LevelCompleted()
    {
        PlayerPrefs.SetInt("LvlNo", PlayerPrefs.GetInt("LvlNo") + 1);
        PlayerPrefs.SetInt("TmpLvlNo", PlayerPrefs.GetInt("TmpLvlNo") + 1);
        if (PlayerPrefs.GetInt("LvlNo") > levelManagers.Count)
        {
            PlayerPrefs.SetInt("LvlNo", 1);
        }
        SceneManager.LoadScene(0);
    }
    public void LevelFailed()
    {
        SceneManager.LoadScene(0);
    }
}