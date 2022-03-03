using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement3 : MonoBehaviour
{
    [Header("Control")]
    public CharacterController controller;
    public Transform cam;
    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    private bool isGrounded;
    public Transform groundCheck;                     
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    [Header("Combat")]
    public GameObject throwablePog;
    public Transform pogSpawnLoc1;
    public Transform pogSpawnLocButBasedOnCameraLol;
    public Transform pogSpawnLoc2;
    private bool isShooting;
    public GameObject thirdPersonCameraThingy;
    public GameObject aimCameraThingy;
    public GameObject theCursor;

    private void Start()
    {
        thirdPersonCameraThingy.SetActive(true);
        aimCameraThingy.SetActive(false);
        theCursor.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //****************************JUMPING & GRAVITY************************
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            velocity.y = jumpHeight;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //*******************MOVEMENT************************
        if ((!Input.GetButton("Fire2")) /* || Mathf.Round(Input.GetAxisRaw("Fire2")) < 0)*/)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }
        //************************FIRING*************************
        pogSpawnLocButBasedOnCameraLol.rotation = cam.rotation;
        // bla bla bla something about health and number of pug in an array or something
        if (Input.GetButton("Fire2") ) /*|| Mathf.Round(Input.GetAxisRaw("Fire2")) < 0)*/
        {
            thirdPersonCameraThingy.SetActive(false);
            aimCameraThingy.SetActive(true);
            theCursor.SetActive(true);
            if (((Input.GetButtonDown("Fire1") || Mathf.Round(Input.GetAxisRaw("Fire1")) > 0)) && isShooting == false)
            {
                isShooting = true;
                Instantiate(throwablePog, pogSpawnLoc2.transform.position, pogSpawnLoc2.transform.rotation);
                StartCoroutine(shootingcooldown()); //I had to start a cooldown because it would spawn thousands of bullet when pressing the gamepad trigger lol
            }
        } else
        {
            thirdPersonCameraThingy.SetActive(true);
            aimCameraThingy.SetActive(false);
            theCursor.SetActive(false);
        }
        if (((Input.GetButtonDown("Fire1") || Mathf.Round(Input.GetAxisRaw("Fire1")) > 0)) && isShooting == false)
        {
          isShooting = true;
          Instantiate(throwablePog, pogSpawnLoc1.transform.position, pogSpawnLoc1.transform.rotation);
            StartCoroutine(shootingcooldown()); //I had to start a cooldown because it would spawn thousands of bullet when pressing the gamepad trigger lol
        }
    }


    IEnumerator shootingcooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isShooting = false;
    }

}
