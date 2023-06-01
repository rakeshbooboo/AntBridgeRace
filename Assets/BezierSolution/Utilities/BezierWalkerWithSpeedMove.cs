using UnityEngine;
using UnityEngine.Events;

namespace BezierSolution
{
	public class BezierWalkerWithSpeedMove : MonoBehaviour, IBezierWalker
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

        public string playerName;
        public Gates gates;

        private void Awake()
		{
			cachedTransform = transform;
		}

        public void ChangeProgressValue()
        {
            //targetProgress = Random.Range(0.1f, 0.9f);
            if (speed > 0f)
            {
                speed = Random.Range(-5f, -2.5f);
            }
            else
            {
                speed = Random.Range(2.5f, 5f);
            }
        }

        private void Start()
        {
            transform.GetChild(0).Rotate(0f, 0f, Random.Range(-50f, 50f));
            transform.GetChild(0).localPosition += transform.GetChild(0).up / 5f;
            speed = Random.Range(2.5f, 5f);
            if (playerName != "")
            {
                if (playerName == "Player")
                {
                    gates.playerMaxProgress = targetProgress;
                }
                else if (playerName == "Ai1")
                {
                    gates.ai1MaxProgress = targetProgress;
                }
                else if (playerName == "Ai2")
                {
                    gates.ai2MaxProgress = targetProgress;
                }
                else if (playerName == "Ai3")
                {
                    gates.ai3MaxProgress = targetProgress;
                }
            }
        }
        private void Update()
		{
            if (spline)
            {
                if (playerName != "")
                {
                    if (playerName == "Player")
                    {
                        minProgress = gates.playerMinProgress;
                    }
                    else if (playerName == "Ai1")
                    {
                        minProgress = gates.ai1MinProgress;
                    }
                    else if (playerName == "Ai2")
                    {
                        minProgress = gates.ai2MinProgress;
                    }
                    else if (playerName == "Ai3")
                    {
                        minProgress = gates.ai3MinProgress;
                    }
                }
                if (isRotate)
                {
                    transform.GetChild(0).Rotate(0f, 0f, Random.Range(-50f, 50f));
                }
                float targetSpeed = (isGoingForward) ? speed : -speed;

                Vector3 targetPos = spline.MoveAlongSpline(ref progress, targetSpeed * Time.deltaTime);
                cachedTransform.position = targetPos;
                //cachedTransform.position = Vector3.Lerp( cachedTransform.position, targetPos, movementLerpModifier * Time.deltaTime );

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
                            progress = minProgress;
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