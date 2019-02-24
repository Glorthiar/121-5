using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Movement
    [Header("Move")]
    [SerializeField] private float SetMovementSpeed;
    [SerializeField] private string HorizontalInputName;
    [SerializeField] private string VerticalInputName;
    [SerializeField] private KeyCode WalkKey;
    [SerializeField] private KeyCode WalkHoldKey;
    [SerializeField] private KeyCode CrouchKey;
    [SerializeField] private KeyCode CrouchHoldKey;
    private float MovementSpeed;
    private bool Walking;
    private bool Crouching;
    //Jump
    [Header("Jump")]
    [SerializeField] private AnimationCurve JumpFallOff;
    [SerializeField] private float JumpMultiplier;
    [SerializeField] private KeyCode JumpKey;
    private bool IsJumping;

    //Shooting
    [Header("Shooting")]
    [SerializeField] private KeyCode FireGun;
    [SerializeField] public int Ammo;
    [SerializeField] GameObject ShootingPoint;
    [SerializeField] private Camera PlayerCamera;
    public bool IsShooting;
    private bool CanShoot;
    public bool HasGun;
    public float TimeBetweenShots;
    private float GunRefresh;
    public AudioClip GunShot;
    public AudioClip GunEmpty;
    private AudioSource MyAudio;
    [SerializeField] private float Recoil;
    [SerializeField] private float RecoilIntensity;


    [Header("Affects")]
    [SerializeField] private GameObject Dust;
    public Animator GunAnimator;
    [SerializeField] GameObject MuzzleFlareLight;
    public GameObject MuzzleFlare;


    private CharacterController CharController;

    // Start is called before the first frame update
    void Awake()
    {
        MuzzleFlareLight.SetActive(false);
        Recoil = 0;
        Walking = false;
        IsShooting = false;
        CanShoot = true;
        HasGun = true;
        Ammo = 24;
        TimeBetweenShots = 0.108f;
        GunRefresh = 0;
        MovementSpeed = SetMovementSpeed;
        CharController = GetComponent<CharacterController>();
        MyAudio = GetComponent<AudioSource>();

    }

    void Update()
    {
        Debug.DrawRay(ShootingPoint.transform.position, (PlayerCamera.transform.forward * 100), Color.red, 0);
        Debug.DrawRay(PlayerCamera.transform.position, (PlayerCamera.transform.forward * 100), Color.red, 0);
        Shoot();
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float VertInput = Input.GetAxis(VerticalInputName) * MovementSpeed;
        float HorizontalInput = Input.GetAxis(HorizontalInputName) * MovementSpeed;

        Vector3 ForwardMovement = transform.forward * VertInput;
        Vector3 RightMovment = transform.right * HorizontalInput;

        CharController.SimpleMove(ForwardMovement + RightMovment);

        JumpInput();
        WalkToggle();
        CrouchToggle();
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(JumpKey) && !IsJumping)
        {
            IsJumping = true;
            StartCoroutine(JumpEvent());

        }
    }

    private IEnumerator JumpEvent()
    {
        CharController.slopeLimit = 90.0f;
        float TimeInAir = 0.0f;

        do
        {
            float jumpForce = JumpFallOff.Evaluate(TimeInAir);
            CharController.Move(Vector3.up * jumpForce * JumpMultiplier * Time.deltaTime);
            TimeInAir += Time.deltaTime;
            yield return null;
        } while (!CharController.isGrounded && CharController.collisionFlags != CollisionFlags.Above);


        CharController.slopeLimit = 45.0f;
        IsJumping = false;

    }

    private void CrouchToggle()
    {
        if (Input.GetKeyDown(CrouchKey))
        {
            if (!Crouching)
            {
                Crouching = true;
                CharController.height = 1.2f;
            }
            else
            {
                Crouching = false;
                CharController.height = 2f;
            }
        }
        if (Input.GetKeyDown(CrouchHoldKey))
        {
            if (!Crouching)
            {
                Crouching = true;
                CharController.height = 1.2f;
            }
            else
            {
                Crouching = false;
                CharController.height = 2f;
            }
        }
        if (Input.GetKeyUp(CrouchHoldKey))
        {
            if (!Crouching)
            {
                Crouching = true;
                CharController.height = 1.2f;
            }
            else
            {
                Crouching = false;
                CharController.height = 2f;
            }
        }
    }

    private void WalkToggle()
    {
        if (Input.GetKeyDown(WalkKey) && !Input.GetKeyDown(WalkHoldKey))
        {
            if (!Walking)
            {
                Walking = true;
                MovementSpeed = SetMovementSpeed*.60f;
            }
            else
            {
                Walking = false;
                MovementSpeed = SetMovementSpeed;
            }
        }
        if (Input.GetKey(WalkHoldKey) && !Walking)
        {
            Walking = true;
            MovementSpeed = SetMovementSpeed * .60f;
        }
        if (Input.GetKeyUp(WalkHoldKey))
        {
            Walking = false;
            MovementSpeed = SetMovementSpeed;
        }
    }

    void Shoot()
    {
        if (Recoil > 0)
        {
            Recoil -= 1 * Time.deltaTime;
        }
        if (GunRefresh > 0)
        {
            GunRefresh -= 1 * Time.deltaTime;
        }
        if (Input.GetKeyDown(FireGun))
        {
            GunAnimator.SetTrigger("PullTrigger");
            if (Ammo <= 0)
            {
                MyAudio.PlayOneShot(GunEmpty, 1);
            }
        }
        if (Input.GetKey(FireGun) && CanShoot && GunRefresh <= 0 && Ammo > 0)
        {
            MuzzleFlareLight.SetActive(true);
            GameObject Flash = Instantiate(MuzzleFlare, ShootingPoint.transform.position, PlayerCamera.transform.rotation);
            float FlashRandomScale = Random.Range(.4f, .6f);
            Flash.transform.localScale = new Vector3(FlashRandomScale, FlashRandomScale, FlashRandomScale);
            Flash.transform.Rotate(Random.Range(0,360), 90, 0);
            GunAnimator.SetTrigger("Fire");
            Ammo--;
            MyAudio.PlayOneShot(GunShot, 1);
            GunRefresh = TimeBetweenShots;
            RaycastHit Hit;
            Vector3 ShotOffSet = new Vector3(Random.Range(-RecoilIntensity,RecoilIntensity) * Recoil, Random.Range(-RecoilIntensity, RecoilIntensity)*Recoil, 0);
           // Debug.DrawRay(ShootingPoint.transform.position, (PlayerCamera.transform.forward * 100)+ShotOffSet, Color.red, 10);
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward+ShotOffSet, out Hit))
            {
                if(Hit.collider.tag == "Enemy")
                {
                    Hit.collider.GetComponent<Monster>().HP -= 1;
                }
                Instantiate(Dust, Hit.point, PlayerCamera.transform.rotation);
            }
            Recoil += .25f;
        }
        if (Input.GetKeyUp(FireGun) || Ammo <= 0)
        {
            GunAnimator.SetTrigger("EndFire");
        }
        if (Recoil > 1) { Recoil = 1; }
    }

}
