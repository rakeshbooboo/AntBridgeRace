using UnityEngine;
using UnityEngine.Events;

namespace BezierSolution
{
	public class BezierWalkerWithSpeed : MonoBehaviour, IBezierWalker
	{
		public enum TravelMode { Once, Loop, PingPong };

		private Transform cachedTransform;

		public BezierSpline spline;
		public TravelMode travelMode;

		public float speed = 5f;
		public float progress, targetProgress, minProgress = 0f;

		public BezierSpline Spline { get { return spline; } }

		public float NormalizedT
		{
			get { return progress; }
			set { progress = value; }
		}

		//public float movementLerpModifier = 10f;
		public float rotationLerpModifier = 10f;

		public bool lookForward = true;

		private bool isGoingForward = true;
		public bool MovingForward { get { return ( speed > 0f ) == isGoingForward; } }

		public UnityEvent onPathCompleted = new UnityEvent();
		private bool onPathCompletedCalledAt1 = false;
		private bool onPathCompletedCalledAt0 = false;

        public bool isRotate;

        
		private void Awake()
		{
			cachedTransform = transform;
		}
        private void Start()
        {
            
        }
        private void Update()
		{
            
            if (spline)
            {
                if (isRotate)
                {
                    transform.GetChild(0).Rotate(0f, 0f, Random.Range(-150f, 150f));
                    transform.GetChild(1).Rotate(Random.Range(-10f, 10f), 0f, 0);
                    transform.GetChild(1).localEulerAngles = transform.GetChild(0).localEulerAngles;
                }
                float targetSpeed = (isGoingForward) ? speed : -speed;

                Vector3 targetPos = spline.MoveAlongSpline(ref progress, targetSpeed * Time.deltaTime);
                cachedTransform.position = targetPos;

                bool movingForward = MovingForward;

                if (lookForward)
                {
                    Quaternion targetRotation;
                    if (movingForward)
                        targetRotation = Quaternion.LookRotation(spline.GetTangent(progress));
                    else
                        targetRotation = Quaternion.LookRotation(-spline.GetTangent(progress));

                    cachedTransform.rotation = Quaternion.Lerp(cachedTransform.rotation, targetRotation, rotationLerpModifier * Time.deltaTime);
                }

                if (movingForward)
                {
                    if (progress >= targetProgress)
                    {
                        if (!onPathCompletedCalledAt1)
                        {
                            onPathCompleted.Invoke();
                            onPathCompletedCalledAt1 = true;
                        }

                        if (travelMode == TravelMode.Once)
                            progress = targetProgress;
                        else if (travelMode == TravelMode.Loop)
                            progress -= 1f;
                        else
                        {
                            progress = 2f - progress;
                            isGoingForward = !isGoingForward;
                        }
                        if (isRotate)
                        {
                            //Destroy(transform.GetChild(0).GetComponent<Animator>());
                            transform.GetChild(0).gameObject.SetActive(false);
                            transform.GetChild(1).gameObject.SetActive(true);
                            Destroy(this);
                        }
                    }
                    else
                    {
                        onPathCompletedCalledAt1 = false;
                    }
                }
                else
                {
                    if (progress <= minProgress)
                    {
                        if (!onPathCompletedCalledAt0)
                        {
                            onPathCompleted.Invoke();
                            onPathCompletedCalledAt0 = true;
                        }

                        if (travelMode == TravelMode.Once)
                            progress = 0f;
                        else if (travelMode == TravelMode.Loop)
                            progress += 1f;
                        else
                        {
                            progress = -targetProgress;
                            isGoingForward = !isGoingForward;
                        }
                    }
                    else
                    {
                        onPathCompletedCalledAt0 = false;
                    }
                }
            }
		}
	}
}