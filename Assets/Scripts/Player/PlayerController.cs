 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Transform mainCamera, groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private SwordAttack swordCollider;
    [SerializeField] private Indicators indicators; 
    [SerializeField] private float radius, jumpHeight, maxSpeed = 3f, 
    gravity, attackTimeShot = 1, fallDamage = 5, flightTime;

    private PlayerRagdoll playerRagdooll;
    private CharacterController controller;
    private Vector3 velocity;
    private Animator anim;
    private bool isGrounded, water = false, CanAttack;

    private float speed = 3f, smoothTime = 0.2f, smooth;
    public bool moving;
    public float health = 100;

    void Start()
    {
        playerRagdooll = GetComponent<PlayerRagdoll>();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        swordCollider = GetComponentInChildren<SwordAttack>();

        //mainCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        CanAttack = true;
        //swordCollider.enabled = true;
        //isSwinging = true;
    }

    public void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized; //normalized что бы координаты не складывались

        //dir = Vector3.ClampMagnitude(dir, 1);
        if(dir.magnitude >= 0.1f && CanAttack) //magnitude  отслеживает сам вектор передвижения
        {
            float rotateAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotateAngle, ref smooth, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 move = Quaternion.Euler(0f, rotateAngle, 0f) * Vector3.forward; 

            controller.Move(move.normalized * speed * Time.deltaTime);
            anim.SetBool("Moving",true);
            moving = true;
            
        }
        else
        {
            anim.SetBool("Moving",false);
            moving = false;
        }

        
        horizontal = Mathf.Clamp01(Mathf.Abs(horizontal));
        vertical = Mathf.Clamp01(Mathf.Abs(vertical));
        

        if(Input.GetKey(KeyCode.LeftShift) && indicators.energyAmount > 20 || Input.GetKey(KeyCode.RightShift) )
        {
            horizontal = horizontal;
            speed = maxSpeed;
        }
        else 
        {
            horizontal /= 2;
            vertical /= 2;
            speed = maxSpeed / 2;
        }

        anim.SetFloat("Horizontal", horizontal, 0.05f, Time.deltaTime);
        anim.SetFloat("Vertical", vertical, 0.05f, Time.deltaTime);
    }

    
    void Attack()
    {
        if(Input.GetButtonDown("Fire1") && indicators.energyAmount > 25)
        {
            if(CanAttack)
            {
                indicators.energyAmount -= 20;
                CanAttack = false;
                anim.SetTrigger("Attack");
                swordCollider.isSwinging = true;
                swordCollider.hasHitEnemy = false;
                StartCoroutine(ResetAttack());
            }
        }            
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackTimeShot);
        CanAttack = true;
        swordCollider.isSwinging = false;
        //swordCollider.enabled = false;
        //isSwinging = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //print("Swiming");
        if(hit.gameObject.tag == "Water")
        {
            water = true;
            anim.SetBool("Swiming", true);
        }
        if(hit.gameObject.layer == 6)
        {
            water = false;
            anim.SetBool("Swiming", false);
        }
    }

    void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, radius, groundMask);

        if(Input.GetButtonDown("Jump") && isGrounded && CanAttack && indicators.energyAmount > 10)
        {                           // 10 * -25
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("Jump");
            indicators.energyAmount -= 10;
        }

        if(flightTime > 1 && isGrounded)
            TakeDamage(flightTime * fallDamage);

        if(!isGrounded)
            flightTime += Time.deltaTime;
        else flightTime = 0;


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);  
    }

    void Update()
    {
        Movement();
        Jumping();
        Attack();

        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        TargetShake.Shake(0.5f, 0.1f);

        if(health <= 0)
        {
            //print("death");
            gameObject.layer = 0;
            playerRagdooll.RagDools();
            losePanel.SetActive(true);
            //losePanel.GetComponent<Animator>().SetTrigger("Died");
            enabled = false;
            Invoke("Respawn", 10);

        }
        else
        {
            print("damage");
        }
    }

    void Respawn()
    {
        SceneManager.LoadScene(0);
    }
}
