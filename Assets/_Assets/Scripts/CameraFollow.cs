using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    public Camera camera;
    public float cameraFieldOfView = 60f;
    public Transform target, childTra;
    public Vector3 childPos;
    public float xDistance, zDistance = 3.0f;
    public float height = 3.0f;
    public bool smoothRotation, lookAtBool = true;
    public float damping = 5.0f;
    public float rotationDamping = 10.0f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
    }

    private void Update()
    {
        camera.fieldOfView = cameraFieldOfView;
        if (target != null)
        {
            if (smoothRotation)
            {
                if (lookAtBool == true)
                {
                    Vector3 wantedPosition = target.position + (target.forward * zDistance);
                    wantedPosition.y = target.position.y + height;
                    wantedPosition.x = target.position.x + xDistance;
                    transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
                    Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                }
                else
                {
                    Vector3 wantedPosition = target.position + (Vector3.forward * zDistance);
                    wantedPosition.y = target.position.y + height;
                    wantedPosition.x = target.position.x + xDistance;
                    transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
                    Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                }
            }
            else
            {
                Vector3 wantedPosition = new Vector3(target.position.x + xDistance, target.position.y + height, target.position.z + zDistance);
                transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

                if (lookAtBool)
                {
                    transform.LookAt(target.position);
                }
            }
            childTra.localPosition = Vector3.Lerp(childTra.localPosition, childPos, Time.deltaTime * damping);
        }
    }

    public void ChangeChildPos(float tmpTime, Vector3 tmpChildPos)
    {
        LeanTween.value(gameObject, childPos, tmpChildPos, tmpTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((Vector3 val) => {
            childPos = val;
        });
    }
    public void ChangeXDistance(float tmpTime, float tmpXDistance)
    {
        LeanTween.value(gameObject, xDistance, tmpXDistance, tmpTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float val) => {
            xDistance = val;
        });
    }
    public void ChangeZDistance(float tmpTime, float tmpZDistance)
    {
        LeanTween.value(gameObject, zDistance, tmpZDistance, tmpTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float val) => {
            zDistance = val;
        });
    }
    public void ChangeHeight(float tmpTime, float tmpHeight)
    {
        LeanTween.value(gameObject, height, tmpHeight, tmpTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float val) => {
            height = val;
        });
    }
    public void ChangeCameraFieldOfView(float tmpTime, float tmpCameraFieldOfView)
    {
        LeanTween.value(gameObject, cameraFieldOfView, tmpCameraFieldOfView, tmpTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float val) => {
            cameraFieldOfView = val;
        });
    }
    public void ChangeDamping(float tmpTime, float tmpDamping)
    {
        LeanTween.value(gameObject, damping, tmpDamping, tmpTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float val) => {
            damping = val;
        });
    }
    public void ChangeRotationDamping(float tmpTime, float tmpRotationDamping)
    {
        LeanTween.value(gameObject, rotationDamping, tmpRotationDamping, tmpTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float val) => {
            rotationDamping = val;
        });
    }
}