using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BezierSolution;
using DG.Tweening;
public class Player : MonoBehaviour
{
    public LayerMask borderLayer;
    public DynamicBone dynamicBone;
    public bool isPlayer, isMove, isFindTarget;
    public Transform rootTra, allPointTra, playerChildTra;
    public Transform[] allPointTraList;
    public int pointInd = 0;

    public VariableJoystick variableJoystick;
    public Animator animator;
    public float moveSpeed;
    public float rayDis;
    public LayerMask layerMask;
    public Collider[] hitColliders = new Collider[5000];
    public Transform targetTra, gateTra;
    public Gates gates;
    public List<Transform> collectedAntList, bridgeChildAntList;

    public PlateForm plateForm;
    public int plateFormInd, collectAntNo, aiAntStackNo = 0;

    public GamePlayManager gamePlayManager;

    public BezierWalkerWithSpeed bezierWalkerWithSpeed;
    public bool isBridgeForward = true;
    CameraFollow cameraFollow;
    public Transform nameTra, bridgeChildTra;

    Vector3 randomPos;

    public Collider collider;
    public ChildCollider childCollider;

    public bool isWin = false;
    public int fixedBridgeNo = -1;

    void Start()
    {
        cameraFollow = CameraFollow.Instance;
        allPointTraList = allPointTra.GetComponentsInChildren<Transform>();
        if (isPlayer == false)
        {
            animator.SetBool("IsRun", true);
            aiAntStackNo = Random.Range(10, 5);
        }
        StartCoroutine(StartIEnum());
    }
    void Update()
    {
        if (nameTra != null)
        {
            //if (pointInd > 0 && pointInd < allPointTraList.Length)
            //{
            //    nameTra.position = allPointTraList[pointInd - 1].transform.position + (Vector3.up * 2f);
            //}
            //else if (pointInd >= allPointTraList.Length - 1)
            //{
            //    nameTra.position = allPointTraList[allPointTraList.Length - 1].transform.position + (Vector3.up * 2f);
            //}
            nameTra.LookAt(cameraFollow.childTra.position);
        }
        if (isMove == true)
        {
            Vector3 joySticPos = new Vector3(variableJoystick.Horizontal, 0f, variableJoystick.Vertical);
            if (isPlayer == true)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position + (transform.up * 2f), transform.TransformDirection(Vector3.forward), out hit, 2.5f, borderLayer))
                {
                    transform.position += transform.forward * joySticPos.magnitude * moveSpeed * Time.deltaTime;
                }
                if (joySticPos != Vector3.zero)
                {
                    Quaternion wantedRotation = Quaternion.LookRotation(joySticPos, transform.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * 7f);
                    if (animator.GetBool("IsRun") == false)
                    {
                        animator.SetBool("IsRun", true);
                    }
                    animator.speed = joySticPos.magnitude;
                }
                else if (animator.GetBool("IsRun") == true)
                {
                    animator.speed = 1f;
                    animator.SetBool("IsRun", false);
                }
            }
            else
            {
                if (isFindTarget == false)
                {
                    if (targetTra == null)
                    {
                        //if (gateTra != null)
                        //{
                        //    targetTra = gateTra;
                        //}
                        //else
                        //{
                        if (fixedBridgeNo == -1)
                        {
                            if (plateForm.plateformTraList.Count > 0)
                            {
                                fixedBridgeNo = Random.Range(0, plateForm.plateformTraList.Count);
                                targetTra = plateForm.plateformTraList[fixedBridgeNo];
                                plateForm.plateformTraList.RemoveAt(fixedBridgeNo);
                            }
                            else
                            {
                                fixedBridgeNo = Random.Range(0, plateForm.plateformTraList2.Count);
                                targetTra = plateForm.plateformTraList2[fixedBridgeNo];
                            }
                            gateTra = targetTra;
                            gates = targetTra.GetComponent<Gates>();
                        }
                        else
                        {
                            targetTra = gateTra;
                        }
                        //}
                    }
                }
                else
                {
                    hitColliders = new Collider[5000];
                    Physics.OverlapSphereNonAlloc(transform.position, rayDis, hitColliders, layerMask);

                    if (targetTra == null && isFindTarget == true)
                    {
                        targetTra = GetNearestEnemy();
                    }
                }

                if (targetTra != null)
                {
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    Vector3 tmpPos = targetTra.position - transform.position;
                    if (plateForm != null)
                    {
                        tmpPos = new Vector3(targetTra.position.x, plateForm.centerTra.position.y, targetTra.position.z) - transform.position;
                    }
                    Quaternion wantedRotation = Quaternion.LookRotation(tmpPos, transform.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * 7f);
                }
                else
                {
                    if (Vector3.Distance(transform.position, randomPos) <= 5f)
                    {
                        SetRandomPos();
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, randomPos, moveSpeed * Time.deltaTime);
                        Quaternion wantedRotation = Quaternion.LookRotation(randomPos - transform.position, transform.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * 7f);
                    }
                }
            }
        }
    }

    public void HitNUpAnim()
    {
        LeanTween.rotateLocal(playerChildTra.gameObject, new Vector3 (-180f, 0f, 0f), 0.5f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveLocal(playerChildTra.gameObject, new Vector3 (0f, 0.714f, -0.4400003f), 1f).setEase(LeanTweenType.easeOutExpo);
    }

    public void HitNNormalAnim()
    {
        LeanTween.rotateLocal(playerChildTra.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveLocal(playerChildTra.gameObject, new Vector3(0f, 0f, -0.4400003f), 1f).setEase(LeanTweenType.easeOutExpo);
    }

    public void WinFunc()
    {
        nameTra.gameObject.SetActive(false);
        LeanTween.rotate(gameObject, new Vector3(0f, 180f, 0f), 1f).setEase(LeanTweenType.easeOutSine);
        isWin = true;
        animator.SetBool("IsWin", true);
        if (isPlayer)
        {
            gamePlayManager.lastPlateformObj.transform.GetChild(0).gameObject.SetActive(true);
            gamePlayManager.gamePlayScreen.SetActive(false);
            gamePlayManager.levelcompleteScreen.SetActive(true);
            FinishCameraMovement();
        }
        gamePlayManager.CheckWinLooseFunc();
        Destroy(this);
    }

    public void LooseFunc()
    {
        isMove = false;
        animator.SetBool("IsLoose", true);
        Destroy(bezierWalkerWithSpeed);
        Destroy(this);
    }

    public void SetRandomPos()
    {
        randomPos = new Vector3(Random.Range(-25f, 25f), transform.position.y, Random.Range(plateForm.centerTra.position.z - 25f, plateForm.centerTra.position.z + 25f));
    }

    public IEnumerator StartIEnum()
    {
        yield return new WaitForSeconds(0.25f);
        plateForm = gamePlayManager.plateFormsList[plateFormInd].GetComponent<PlateForm>();
        plateForm.FirstFunc();
        fixedBridgeNo = -1;
        transform.position = new Vector3(transform.position.x, plateForm.centerTra.position.y, transform.position.z);
        SetRandomPos();
    }

    public void FinishFunc()
    {
        if (bezierWalkerWithSpeed.progress >= 0.99f)
        {
            bridgeACDist = 0;
            //collectAntNo = 0;
            plateFormInd += 1;

            bezierWalkerWithSpeed.enabled = false;
            if (plateFormInd < gamePlayManager.levelManagers[gamePlayManager.tmpLvlNo].plateformObjList.Count - 1)
            {
                if (isPlayer)
                {
                    cameraFollow.ChangeXDistance(2f, 0);
                    cameraFollow.ChangeZDistance(bridgeDist * gates.playerAntNo, -45f);
                    cameraFollow.ChangeHeight(bridgeDist * gates.playerAntNo, 45f);
                }

                Vector3 tmpPos = transform.position + (Vector3.forward * 10f);
                tmpPos.y = plateForm.centerTra.position.y;
                LeanTween.rotate(gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeOutExpo);
                transform.DOJump(tmpPos, 3.5f, 1, 1f).OnComplete(() =>
                {
                    if (isPlayer == true)
                    {
                        animator.SetBool("IsRun", false);
                    }
                    else
                    {
                        aiAntStackNo = Random.Range(10, 15);
                    }
                    fixedBridgeNo = -1;
                    gateTra = null;
                    gates = null;
                    plateForm = gamePlayManager.plateFormsList[plateFormInd].GetComponent<PlateForm>();
                    plateForm.FirstFunc();
                    transform.position = new Vector3(transform.position.x, plateForm.centerTra.position.y, transform.position.z);
                    SetRandomPos();
                    isFindTarget = true;
                    targetTra = null;
                    isMove = true;
                    isCollider = false;
                });
            }
            else
            {
                Vector3 tmpPos = transform.position + (Vector3.forward * 10f);
                tmpPos.y = plateForm.centerTra.position.y;
                LeanTween.rotate(gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeOutExpo);
                transform.DOJump(tmpPos, 3.5f, 1, 1f).OnComplete(() =>
                {
                    if (isPlayer == true)
                    {
                        animator.SetBool("IsRun", false);
                    }
                    else
                    {
                        aiAntStackNo = Random.Range(10, 15);
                    }
                    plateForm = gamePlayManager.plateFormsList[plateFormInd].GetComponent<PlateForm>();
                    plateForm.FirstFunc();
                    fixedBridgeNo = -1;
                    transform.position = new Vector3(transform.position.x, plateForm.centerTra.position.y, transform.position.z);
                    SetRandomPos();
                    isFindTarget = true;
                    targetTra = null;
                    //gateTra = null;
                    //gates = null;
                    isMove = true;
                    isCollider = false;
                });
            }
        }
        else
        {
            if (isBridgeForward == true)
            {
                bridgeACDist = bezierWalkerWithSpeed.targetProgress;
                isBridgeForward = false;
                bezierWalkerWithSpeed.speed = -10f;
            }
            else
            {
                if (isPlayer)
                {
                    cameraFollow.ChangeXDistance(2f, 0);
                    cameraFollow.ChangeZDistance(bridgeDist * gates.playerAntNo, -45f);
                    cameraFollow.ChangeHeight(bridgeDist * gates.playerAntNo, 45f);
                }
                Vector3 tmpRotation = new Vector3(0f, 180f, 0f);
                LeanTween.rotate(gameObject, tmpRotation, 1f).setEase(LeanTweenType.easeOutExpo);
                Vector3 tmpPos = transform.position - (Vector3.forward * 10f);
                tmpPos.y = plateForm.centerTra.position.y;
                bezierWalkerWithSpeed.enabled = false;
                bezierWalkerWithSpeed.speed = 10f;
                transform.DOJump(tmpPos, 3.5f, 1, 1f).OnComplete(() =>
                {
                    isBridgeForward = true;
                    isMove = true;
                    isFindTarget = true;
                    targetTra = null;
                    isCollider = false;
                    if (isPlayer == false)
                    {
                        aiAntStackNo = Random.Range(10, 15);
                    }
                });
            }
        }
    }

    public void FinishCameraMovement()
    {
        if (isPlayer == true)
        {
            cameraFollow.ChangeChildPos(1f, new Vector3(0f, 2f, 0f));
            cameraFollow.ChangeXDistance(1f, 30f);
            cameraFollow.ChangeZDistance(1f, -35f);
            cameraFollow.ChangeHeight(1f, 25f);
            cameraFollow.ChangeCameraFieldOfView(1f, 35f);
        }
    }

    float bridgeDist = 0f;
    float bridgeACDist = 0;
    int moveAntNo = 5;
    int deleteAntNo = 0;
    BezierWalkerWithSpeedMove bezierWalkerWithSpeed3;
    public IEnumerator BridgeIEnum(BezierSpline bezierSpline, float tmpbridgeDist)
    {
        if (collectedAntList.Count > 0 && collectedAntList != null)
        {
            int lastInd = collectedAntList.Count;
            yield return new WaitForSeconds(tmpbridgeDist);
            if (lastInd > 0 && gates != null)
            {
                if (gates.childAntTraList[deleteAntNo] != null)
                {
                    if (collectedAntList[lastInd-1].gameObject.name != gates.childAntTraList[deleteAntNo].gameObject.name)
                    {
                        gates.UpdateVal(gameObject);
                        Destroy(gates.childAntTraList[deleteAntNo].gameObject);
                        gates.childAntTraList[deleteAntNo] = null;
                    }
                }
                if (gates.childAntTraList[deleteAntNo] == null)
                {
                    collectedAntList[lastInd-1].transform.parent = bridgeChildTra;
                    gates.childAntTraList[deleteAntNo] = collectedAntList[lastInd-1];
                    if ((lastInd) % 3 == 0)
                    {
                        collectedAntList[lastInd - 1].GetChild(0).GetComponent<Animator>().SetBool("IsRun", true);
                        collectedAntList[lastInd - 1].gameObject.AddComponent<BezierWalkerWithSpeedMove>();
                        BezierWalkerWithSpeedMove bezierWalkerWithSpeed3 = collectedAntList[lastInd - 1].GetComponent<BezierWalkerWithSpeedMove>();
                        collectedAntList[lastInd - 1].GetChild(0).GetComponent<Animator>().enabled = true;
                        bezierWalkerWithSpeed3.gates = gates;
                        bezierWalkerWithSpeed3.playerName = gameObject.name;
                        bezierWalkerWithSpeed3.speed = 5f;
                        bezierWalkerWithSpeed3.spline = bezierSpline;
                        bezierWalkerWithSpeed3.progress = bezierWalkerWithSpeed.progress;
                        bezierWalkerWithSpeed3.targetProgress = (bridgeDist * ((float)deleteAntNo + 1f));
                        bezierWalkerWithSpeed3.onPathCompleted.AddListener(() => bezierWalkerWithSpeed3.ChangeProgressValue());
                    }
                    else
                    {
                        collectedAntList[lastInd - 1].GetChild(0).gameObject.SetActive(true);
                        collectedAntList[lastInd - 1].GetChild(1).gameObject.SetActive(false);
                        collectedAntList[lastInd - 1].gameObject.AddComponent<BezierWalkerWithSpeed>();
                        BezierWalkerWithSpeed bezierWalkerWithSpeed2 = collectedAntList[lastInd - 1].GetComponent<BezierWalkerWithSpeed>();
                        bezierWalkerWithSpeed2.speed = 10f;
                        bezierWalkerWithSpeed2.isRotate = true;
                        bezierWalkerWithSpeed2.spline = bezierSpline;
                        bezierWalkerWithSpeed2.progress = bezierWalkerWithSpeed.progress;

                        bezierWalkerWithSpeed2.targetProgress = (bridgeDist * ((float)deleteAntNo + 1f));
                    }
                    yield return new WaitForEndOfFrame();
                    collectedAntList.RemoveAt(lastInd-1);
                    //yield return new WaitForEndOfFrame();
                    collectAntNo -= 1;
                    pointInd = collectedAntList.Count - 1;
                    if (pointInd <= 0)
                    {
                        pointInd = 0;
                    }
                    //yield return new WaitForEndOfFrame();
                    deleteAntNo += 1;
                    if (collectedAntList.Count > 0)
                    {
                        StartCoroutine(BridgeIEnum(bezierSpline, bridgeDist));
                    }
                }
                else
                {
                    deleteAntNo += 1;
                    StartCoroutine(BridgeIEnum(bezierSpline, 0));
                }
                if (deleteAntNo >= gates.childAntTraList.Count-1)
                {
                    deleteAntNo = gates.childAntTraList.Count - 1;
                }
                if (lastInd <= 1)
                {
                    float tmpDist = (bridgeDist * ((float)deleteAntNo + 1f));
                    if (gameObject.name == "Player")
                    {
                        gates.playerMinProgress = 0f;
                        gates.ai1MinProgress = gates.ai1MaxProgress - tmpDist;
                        gates.ai2MinProgress = gates.ai2MaxProgress - tmpDist;
                        gates.ai3MinProgress = gates.ai3MaxProgress - tmpDist;
                    }
                    else if (gameObject.name == "Ai1")
                    {
                        gates.playerMinProgress = gates.playerMaxProgress - tmpDist;
                        gates.ai1MinProgress = 0;
                        gates.ai2MinProgress = gates.ai2MaxProgress - tmpDist;
                        gates.ai3MinProgress = gates.ai3MaxProgress - tmpDist;
                    }
                    else if (gameObject.name == "Ai2")
                    {
                        gates.playerMinProgress = gates.playerMaxProgress - tmpDist;
                        gates.ai1MinProgress = gates.ai1MaxProgress - tmpDist;
                        gates.ai2MinProgress = 0;
                        gates.ai3MinProgress = gates.ai3MaxProgress - tmpDist;
                    }
                    else if (gameObject.name == "Ai3")
                    {
                        gates.playerMinProgress = gates.playerMaxProgress - tmpDist;
                        gates.ai1MinProgress = gates.ai1MaxProgress - tmpDist;
                        gates.ai2MinProgress = gates.ai2MaxProgress - tmpDist;
                        gates.ai3MinProgress = 0f;
                    }
                    FinishFunc();
                }
            }
        }
    }
    IEnumerator RemoveAnimator(Animator tmpAnimator)
    {
        yield return new WaitForSeconds(2f);
        tmpAnimator.enabled = false;

    }
    bool isCollider = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isMove == true)
        {
            if (isCollider == false && other.gameObject.tag == "Collider" && collectedAntList.Count > 0)
            {
                isCollider = true;
                isMove = false;

                if (isPlayer == true)
                {
                    gateTra = other.transform;
                    gates = other.transform.GetComponent<Gates>();
                    if (transform.position.x >= -1.5f)
                    {
                        cameraFollow.ChangeXDistance(2f, 30);
                    }
                    else
                    {
                        cameraFollow.ChangeXDistance(2f, -30);
                    }
                    cameraFollow.ChangeZDistance(2, -30);
                    cameraFollow.ChangeHeight(2, 30);
                }
                LeanTween.rotate(gameObject, new Vector3(-45f, 0f, 0f), 1f).setEase(LeanTweenType.easeOutExpo);
                transform.DOJump(other.transform.position, 1f, 1, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    StartCoroutine(TriggerIEnum(other.transform));
                });
            }
            else if (other.gameObject.tag == "ChildAnt" && gameObject.layer == other.gameObject.layer)
            {
                if (isPlayer == false && isFindTarget == true && collectAntNo >= aiAntStackNo)
                {
                    isFindTarget = false;
                    targetTra = null;
                }
                else if (allPointTraList[allPointTraList.Length - 1].childCount == 0 && pointInd < allPointTraList.Length)
                {
                    //StartCoroutine(RemoveAnimator(other.transform.GetChild(0).GetComponent<Animator>()));
                    collectAntNo += 1;
                    ChildParabola childParabola = other.GetComponent<ChildParabola>();
                    childParabola.player = this;
                    collectedAntList.Add(other.transform);
                    childParabola.targetTra = allPointTraList[pointInd];
                    childParabola.antNo = collectedAntList.Count;
                    childParabola.Start();
                    childParabola.enabled = true;
                    pointInd += 1;
                    targetTra = null;
                    other.enabled = false;
                    //Destroy(other);
                }
            }
        }
    }

    public IEnumerator TriggerIEnum(Transform tmpTra)
    {
        LeanTween.cancel(gameObject);
        bezierWalkerWithSpeed.spline = tmpTra.GetChild(0).GetComponent<BezierSpline>();
        bezierWalkerWithSpeed.progress = 0f;

        deleteAntNo = 0;

        bridgeDist = 1f / (float)plateForm.bridgeAntNo;
        bridgeChildTra = gateTra.transform.GetChild(1);

        yield return new WaitForEndOfFrame();

        //yield return new WaitForEndOfFrame();
        //int tmpChildAntNo = gates.childAntTraList.Count;
        //if (tmpChildAntNo > 0)
        //{
        //    gamePlayManager.tmpAntList.Clear();
        //    int tmpAntNo = tmpChildAntNo - collectedAntList.Count;
        //    if (tmpAntNo <= 0)
        //    {
        //        tmpAntNo = 0;
        //    }
        //    else
        //    {
        //        for (int i = tmpAntNo; i < tmpChildAntNo; i++)
        //        {
        //            if (gates.childAntTraList[i] != null)
        //            {
        //                gamePlayManager.tmpAntList.Add(gates.childAntTraList[i].transform);
        //                gates.childAntTraList[i].transform.parent = null;
        //            }
        //        }
        //    }
        //}
        //yield return new WaitForEndOfFrame();

        //tmpChildAntNo = gates.childAntTraList.Count;

        //gates.childAntTraList.Clear();
        //gates.childAntTraList = new List<Transform>();

        //for (int i = 0; i < bridgeChildTra.childCount; i++)
        //{
        //    if (i >= tmpChildAntNo)
        //    {
        //        gates.childAntTraList.Add(bridgeChildTra.GetChild(i));
        //    }
        //}

        //yield return new WaitForEndOfFrame();
        //for (int j = 0; j < gamePlayManager.tmpAntList.Count; j++)
        //{
        //    gates.childAntTraList.Add(gamePlayManager.tmpAntList[j]);
        //    gamePlayManager.tmpAntList[j].transform.parent = bridgeChildTra;
        //}

        //yield return new WaitForEndOfFrame();

        bezierWalkerWithSpeed.targetProgress = 1f;
        bezierWalkerWithSpeed.enabled = true;
        StartCoroutine(BridgeIEnum(tmpTra.GetChild(0).GetComponent<BezierSpline>(), bridgeDist));
    }

    public IEnumerator ColliderToWinIEnum()
    {
        animator.speed = 3f;
        isMove = false;
        animator.SetBool("IsRun", false);
        animator.SetBool("IsWin", true);
        yield return new WaitForSeconds(1);
        animator.speed = 1f;
        isMove = true;
        animator.SetBool("IsRun", true);
        animator.SetBool("IsWin", false);
    }

    public IEnumerator ThrowAntOutSideIEnum()
    {
        allPointTra.parent = transform;
        dynamicBone.enabled = false;
        HitNUpAnim();
        isFindTarget = true;
        targetTra = null;
        //gateTra = null;
        //gates = null;
        animator.speed = 3f;
        //animator.SetBool("IsRun", false);
        //animator.SetBool("IsLoose", true);
        isMove = false;
        collider.enabled = false;
        childCollider.collider.enabled = false;
        pointInd = 0;
        targetTra = null;
        yield return new WaitForEndOfFrame();
        int maxNo = collectedAntList.Count - 1;
        for (int i = maxNo; i >= 0; i--)
        {
            collectedAntList[i].transform.GetChild(0).gameObject.SetActive(true);
            collectedAntList[i].transform.GetChild(1).gameObject.SetActive(false);

            collectedAntList[i].transform.parent = null;
            collectedAntList[i].eulerAngles = Vector3.zero;
            ChildParabola childParabola = collectedAntList[i].GetComponent<ChildParabola>();
            //player.plateForm.StopReCreate();
            childParabola.isOutDrop = true;
            childParabola.targetTra = null;
            childParabola.targetPos = new Vector3(Random.Range(transform.position.x - 10f, transform.position.x + 10f), transform.position.y, Random.Range(transform.position.z - 10f, transform.position.z + 10f));
            childParabola.Start();
            childParabola.enabled = true;
            //yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForEndOfFrame();
        collectedAntList = null;
        collectedAntList = new List<Transform>();

        yield return new WaitForSeconds(1.5f);
        animator.speed = 1f;
        isMove = true;

        //animator.SetBool("IsRun", true);
        //animator.SetBool("IsLoose", false);
        collectAntNo = 0;
        HitNNormalAnim();
        dynamicBone.enabled = true;
        allPointTra.parent = rootTra;
        allPointTra.localPosition = new Vector3(0f, 7.00000019e-05f, 0.000113999959f);
        allPointTra.localEulerAngles = new Vector3(85.6455307f, 185.306763f, 185.254547f);
        yield return new WaitForSeconds(2f);

        collider.enabled = true;
        childCollider.collider.enabled = true;
    }

    private Transform GetNearestEnemy()
    {
        float minDist = 2f;
        float maxDist = 50f;
        int nearestIndex = -1;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && hitColliders[i].tag != "Collider")
            {
                var dist = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (dist > minDist && dist < maxDist)
                {
                    nearestIndex = i;
                    minDist = dist;
                }
            }
        }
        return nearestIndex == -1 ? null : hitColliders[nearestIndex].transform;
    }
}