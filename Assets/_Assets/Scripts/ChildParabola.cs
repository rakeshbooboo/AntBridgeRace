using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildParabola : MonoBehaviour
{
    public Transform targetTra;
    public Vector3 targetPos;
    public bool isOutDrop = false;
    Vector3 startPos;
    float timeNo = 0f;
    public Player player;
    public bool isRun;
    public Animator animator;
    public int randomNo = 0;
    internal int antNo;
    public Collider collider;
    public void Start()
    {
        if (isOutDrop == true)
        {
            startPos = new Vector3(transform.position.x, player.plateForm.centerTra.position.y, transform.position.z);
            //player.plateForm.StopReCreate();
        }
        else
        {
            if (isRun == false)
            {
                randomNo = Random.Range(1, 6);
                animator.SetBool("IsRun" + randomNo.ToString(), true);
                isRun = true;
                this.enabled = false;
            }
            else
            {
                transform.parent = targetTra;
                startPos = transform.position;
                animator.SetBool("IsRun1", false);
                animator.SetBool("IsRun2", false);
                animator.SetBool("IsRun3", false);
                animator.SetBool("IsRun4", false);
                animator.SetBool("IsRun5", false);
                if (antNo % 2 == 0)
                {
                    animator.SetBool("IsRun", true);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (isRun == true)
        {
            if (timeNo >= 1f)
            {
                if (isOutDrop == false)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(true);
                    player.plateForm.ReCreate(gameObject, startPos);
                    LeanTween.moveLocal(gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeOutExpo);
                    LeanTween.rotateLocal(gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeOutExpo);
                    timeNo = 0;
                    this.enabled = false;
                }
                else
                {
                    collider.enabled = true;
                    timeNo = 0;
                    animator.SetBool("IsRun" + randomNo.ToString(), true);
                    isRun = true;
                    isOutDrop = false;
                    this.enabled = false;
                }
            }
            else
            {
                timeNo += Time.deltaTime * 2f;
                if (isOutDrop == true)
                {
                    transform.position = MathParabola.Parabola(startPos, targetPos, 5f, timeNo);
                }
                else if(targetTra != null)
                {
                    transform.position = MathParabola.Parabola(startPos, targetTra.position, 5f, timeNo);
                }
            }
        }
    }
}
