using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlateForm : MonoBehaviour
{
    public List<Transform> plateformTraList, plateformTraList2;

    public Transform allChildAntTra;
    public int bridgeAntNo;
    public List<GameObject> allChildAntList;
    public List<Vector3> allChildPosList;
    GameObject randomObj;
    public int rowVal, columVal, disVal;
    public int playerTVal, aiTval1, aiTval2, aiTval3;
    int tandNo = 1;
    GamePlayManager gamePlayManager;
    public Transform centerTra;
    public List<IEnumerator> tmpReCreateIEnumList = new List<IEnumerator> ();
    bool isFirst = false;
    void Start()
    {
        gamePlayManager = GamePlayManager.In;
        //FirstFunc();
    }

    public void FirstFunc()
    {
        if (isFirst == false)
        {
            isFirst = true;
            StartCoroutine(FirstIEnum());
        }
    }
    public IEnumerator FirstIEnum()
    {
        playerTVal = (rowVal * columVal) / 4;
        aiTval1 = playerTVal;
        aiTval2 = playerTVal;
        aiTval3 = playerTVal;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < playerTVal; j++)
            {
                if (i == 0)
                {
                    allChildAntList.Add(gamePlayManager.cPAntObj);
                }
                else if (i == 1)
                {
                    allChildAntList.Add(gamePlayManager.cAiAnt1Obj);
                }
                else if (i == 2)
                {
                    allChildAntList.Add(gamePlayManager.cAiAnt2Obj);
                }
                else if (i == 3)
                {
                    allChildAntList.Add(gamePlayManager.cAiAnt3Obj);
                }
            }
        }
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < rowVal; i++)
        {
            for (int j = 0; j < columVal; j++)
            {
                allChildPosList.Add(new Vector3(i * disVal, 0, j * disVal));
            }
        }
        yield return new WaitForEndOfFrame();
        System.Random _rand = new System.Random();
        var shuffledList = allChildPosList.OrderBy(_ => _rand.Next()).ToList();
        allChildPosList = shuffledList;
        yield return new WaitForEndOfFrame();
        int tmpInd = allChildAntList.Count;
        for (int i = 0; i < tmpInd; i++)
        {
            if (allChildAntList[i] != null)
            {
                GameObject tmpCObj = Instantiate(allChildAntList[i], Vector3.zero, Quaternion.identity);
                tmpCObj.name = allChildAntList[i].name;
                tmpCObj.transform.parent = allChildAntTra;
                tmpCObj.transform.localPosition = allChildPosList[i];
                tmpCObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {
                if (i % 2 == 0)
                {
                    GameObject tmpCObj = Instantiate(gamePlayManager.cPAntObj, Vector3.zero, Quaternion.identity);
                    tmpCObj.name = gamePlayManager.cPAntObj.name;
                    tmpCObj.transform.parent = allChildAntTra;
                    tmpCObj.transform.localPosition = allChildPosList[i];
                    tmpCObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                }
                else if (i % 3 == 0)
                {
                    GameObject tmpCObj = Instantiate(gamePlayManager.cAiAnt1Obj, Vector3.zero, Quaternion.identity);
                    tmpCObj.name = gamePlayManager.cAiAnt1Obj.name;
                    tmpCObj.transform.parent = allChildAntTra;
                    tmpCObj.transform.localPosition = allChildPosList[i];
                    tmpCObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                }else 
                {
                    GameObject tmpCObj = Instantiate(gamePlayManager.cAiAnt2Obj, Vector3.zero, Quaternion.identity);
                    tmpCObj.name = gamePlayManager.cAiAnt2Obj.name;
                    tmpCObj.transform.parent = allChildAntTra;
                    tmpCObj.transform.localPosition = allChildPosList[i];
                    tmpCObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                }
            }
        }
    }

    public void StopReCreate()
    {
        for (int i = 0; i < tmpReCreateIEnumList.Count; i++)
        {
            if (tmpReCreateIEnumList[i] != null)
            {
                StopCoroutine(tmpReCreateIEnumList[i]);
            }
            if (i >= tmpReCreateIEnumList.Count-1)
            {
                tmpReCreateIEnumList.Clear();
                tmpReCreateIEnumList = new List<IEnumerator>();
            }
        }
    }

    public void ReCreate(GameObject tmpObj, Vector3 tmpPos)
    {
        if (tmpObj.name == gamePlayManager.cPAntObj.name)
        {
            tmpReCreateIEnumList.Add(ReCreateIEnum(gamePlayManager.cPAntObj, tmpPos));
            StartCoroutine(tmpReCreateIEnumList[tmpReCreateIEnumList.Count-1]);
        }
        else if (tmpObj.name == gamePlayManager.cAiAnt1Obj.name)
        {
            tmpReCreateIEnumList.Add(ReCreateIEnum(gamePlayManager.cAiAnt1Obj, tmpPos));
            StartCoroutine(tmpReCreateIEnumList[tmpReCreateIEnumList.Count - 1]);
        }
        else if (tmpObj.name == gamePlayManager.cAiAnt2Obj.name)
        {
            tmpReCreateIEnumList.Add(ReCreateIEnum(gamePlayManager.cAiAnt2Obj, tmpPos));
            StartCoroutine(tmpReCreateIEnumList[tmpReCreateIEnumList.Count - 1]);
        }
        else if (tmpObj.name == gamePlayManager.cAiAnt3Obj.name)
        {
            tmpReCreateIEnumList.Add(ReCreateIEnum(gamePlayManager.cAiAnt3Obj, tmpPos));
            StartCoroutine(tmpReCreateIEnumList[tmpReCreateIEnumList.Count - 1]);
        }
    }

    public IEnumerator ReCreateIEnum(GameObject tmpObj, Vector3 tmpPos)
    {
        yield return new WaitForSeconds(Random.Range(7f, 10f));
        GameObject tmpCObj = Instantiate(tmpObj, tmpPos, Quaternion.identity);
        tmpCObj.name = tmpObj.name;
        tmpCObj.transform.parent = allChildAntTra;
        tmpCObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
    }
}
