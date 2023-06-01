using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
   public float speed;
    public VariableJoystick variableJoystick;
    //public Rigidbody rb;

   // Quaternion StartPos;

    public bool smoothMotion = true;


    public float smooth = 0f;
    //private Quaternion targetRotation;

    public static JoystickPlayerExample Instance;


    //This is how quickly we MoveTowards the input axis.
    public float smoothSpeed = 10f;

    //The maximum we want our input axis to reach. Setting this lower will rotate the platform less, and higher will be more.
    public float multiplier = 0.1f;

    //Variables, don't edit these.
    private float hSmooth = 0f;
    private float vSmooth = 0f;
    private float h;
    private float v;

    public void Start()
    {
       

        Instance = this;
    }

    private void OnEnable()
    {
        variableJoystick = (VariableJoystick) GameObject.FindObjectOfType<VariableJoystick>();
        //Variable Joystick
    }

    public void Update()
    {








        //if (StartGameFlow.Instance.Is_JoyStick == true && StartGameFlow.Instance.IsTimerStart == true)
        {

            //Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
           // rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);


            h = variableJoystick.Vertical * multiplier;
            v = variableJoystick.Horizontal * multiplier;

            //  if (StartGameFlow.Instance.isJoyStickMove == true)
            {

            }

            // transform.eulerAngles = new Vector3(variableJoystick.Vertical * speed , 0, variableJoystick.Horizontal * speed );



            //transform.Rotate(new Vector3(variableJoystick.Vertical * speed * Time.deltaTime , 0.0f, variableJoystick.Horizontal * speed * Time.deltaTime ));

            // transform.eulerAngles = new Vector3(Mathf.Atan2(variableJoystick.Vertical, variableJoystick.Horizontal) * 180 / Mathf.PI,0 , Mathf.Atan2(variableJoystick.Vertical, variableJoystick.Horizontal) * 180 / Mathf.PI);


            //float MoveHorizontal = variableJoystick.Horizontal;
            // float MoveVertical = variableJoystick.Vertical;



            // Vector3 movement = new Vector3(MoveHorizontal * 2,0,MoveVertical * -2);
            // rb.AddForce(movement);


            //Vector3 charAngle = transform.eulerAngles;
            //charAngle.x = (charAngle.x > 180) ? charAngle.x - 360 : charAngle.x;
            //charAngle.x = Mathf.Clamp(charAngle.x, -10, 10);    


            //charAngle.z = (charAngle.z > 180) ? charAngle.z - 360 : charAngle.z;
            //charAngle.z = Mathf.Clamp(charAngle.z, -10, 10);

            //charAngle.y = 0;
            //transform.rotation = Quaternion.Euler(charAngle);



            //if (StartGameFlow.Instance.isJoyStickMove == false)
            {
                // transform.rotation = Quaternion.Lerp();


            }

        }
        }
    void FixedUpdate()
    {
        //if (StartGameFlow.Instance.Is_JoyStick == true)// && StartGameFlow.Instance.IsTimerStart == true)
        {

            hSmooth = Mathf.MoveTowards(hSmooth, -h, smoothSpeed * Time.fixedDeltaTime);
            vSmooth = Mathf.MoveTowards(vSmooth, v, smoothSpeed * Time.fixedDeltaTime);


            //Rotate to match the new axis using EulerAngles.
            if (!smoothMotion)
            {
                transform.rotation = Quaternion.EulerAngles(new Vector3(h, 0f, v));

            }
            else
            {
                transform.rotation = Quaternion.EulerAngles(new Vector3(hSmooth, 0f, vSmooth));
            }

        }
    }
       


}