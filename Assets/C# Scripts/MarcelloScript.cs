using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarcelloScript : MonoBehaviour {

    public Vector3 velocity;
    public float moveSpeed, xDirection, boostSpeed, rotationSpeed, boostPrepTimer, boostPrepTimeMax, boostTimer, boostTimeMax, boostRecharge, boostRechargeMax
    ,moveAngle, shootAngle, attackSpeed, leftDirection, rightDirection, gunTimer, gunTimerMax, swordDamage, gunDamage, health, healthMax
    ,healthPercentage, hitTimer, hitTimerMax;
    public bool touchingGround, jetting, boostPrep, boosting, nextAttack, continuingAttack, upperCut, plunge, moveAttack, facingOpposite;
    public Rigidbody2D rb;
    public BoxCollider2D boxy;
    public Animator anim;
    public GameObject rig;
    public Vector2 boostDirection, newAttackLocation;
    //public GameObject sword, gunArm, gunArmIK, gun, bullet;
    public GameObject sword, gunArm, gun, bullet, swordArm, frontArmPosition, backArmPosition;
    public SpriteRenderer swordShoulder, gunShoulder, swordForearm, swordHand, gunForearm, gunHand, gunGun;
    public Quaternion baseGunRotation;
    public Image healthBar;
    public Transform[] rigPieces;
    public Shader spriteDefault, whiteSprite;
    public int attackNumber;
    // Use this for initialization
    void Start()
    {
        xDirection = transform.localScale.x;
        rightDirection = transform.localScale.x;
        leftDirection = transform.localScale.x * -1;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rig = GameObject.Find("Player_Rig");
        anim = rig.GetComponent<Animator>();
        boostPrepTimer = boostPrepTimeMax;
        boostTimer = boostTimeMax;
        moveAngle = 0;
        sword.GetComponent<BoxCollider2D>().enabled = false;
        nextAttack = true;
        shootAngle = gunArm.transform.rotation.z;
        baseGunRotation = gunArm.transform.rotation;
        gunArm.GetComponent<Animator>().enabled = false;
        health = healthMax;
        rigPieces = GetComponentsInChildren<Transform>();
        whiteSprite = Shader.Find("GUI/Text Shader");
        spriteDefault = Shader.Find("Sprites/Default");
    }

    // Update is called once per frame
    void Update()
    {
        velocity.x = Input.GetAxis("Left Horizontal");
        velocity.y = Input.GetAxis("Left Vertical");
        if (boosting == false && boostPrep == false)
        {
            if (Mathf.Abs(Input.GetAxis("Left Horizontal")) > .2f || Mathf.Abs(Input.GetAxis("Left Vertical")) > .2f){
                rb.MovePosition(transform.position + velocity * moveSpeed * Time.fixedDeltaTime);
            if (velocity.x > 0 && xDirection == rightDirection || velocity.x < 0 && xDirection == leftDirection)
            {
                facingOpposite = true;
            }
            else
            {
                facingOpposite = false;
            }
            }
        }
        boostDirection.x = Input.GetAxis("Left Horizontal");
        boostDirection.y = Input.GetAxis("Left Vertical");
        if (Input.GetButton("Boost") && boostTimer >= boostTimeMax && boostRecharge <= 0)
        {
            boostPrep = true;
            velocity.x = 0;
            boostPrepTimer -= Time.deltaTime;
        }
        else
        {
            boostPrep = false;
        }
        if (Input.GetButtonUp("Boost") && boostPrepTimer > 0 && boostRecharge <= 0)
        {
            boostPrepTimer = boostPrepTimeMax;
            boostRecharge = boostRechargeMax;
            boostPrep = false;
        }
        transform.localScale = new Vector2(-xDirection, transform.localScale.y);
        anim.SetFloat("Velocity X", Mathf.Abs(velocity.x));
        anim.SetFloat("Velocity Y", velocity.y);
        anim.SetBool("TouchingGround", touchingGround);
        anim.SetBool("BoostPrep", boostPrep);
        anim.SetBool("Boosting", boosting);
        anim.SetBool("Facing Opposite", facingOpposite);
        anim.SetInteger("Attack Number", attackNumber);
        if (boostPrepTimer <= 0 && boostRecharge <= 0)
        {
            boosting = true;
            boostPrep = false;
            boostPrepTimer = boostPrepTimeMax;
        }
        if (boosting == true)
        {
            rb.MovePosition(transform.position + transform.up * boostSpeed * Time.fixedDeltaTime);
            if (Mathf.Abs(Input.GetAxis("Left Horizontal")) > .2f || Mathf.Abs(Input.GetAxis("Left Vertical")) > .2f)
            {
                moveAngle = Mathf.Atan2(-boostDirection.x, boostDirection.y) * Mathf.Rad2Deg;
            }
            boostTimer -= Time.deltaTime;
            sword.GetComponent<BoxCollider2D>().enabled = true;
            //touchingGround = false;
            if (boostTimer <= 0 && boostRecharge <= 0)
            {
                boosting = false;
                boostTimer = boostTimeMax;
                boostRecharge = boostRechargeMax;
            }
        }
        else
        {
            moveAngle = 0;
            sword.GetComponent<BoxCollider2D>().enabled = false;
        }
        if(boostRecharge > 0){
            boostRecharge -= Time.deltaTime;
        }
        anim.SetBool("Continuing Attack", continuingAttack);
        anim.SetBool("Uppercut", upperCut);
        anim.SetBool("Plunge", plunge);
        if (Input.GetButtonDown("Attack") && boosting == false)
        {
            anim.SetTrigger("Attack");
            continuingAttack = true;
        }
        if (Input.GetButtonUp("Attack"))
        {
            anim.ResetTrigger("Attack");
        }
        if(Input.GetButtonDown("Uppercut") && boosting == false){
            upperCut = true;
            continuingAttack = true;
        }
        if(Input.GetButtonDown("Plunge") && boosting == false){
            plunge = true;
            continuingAttack = true;
        }
        if (moveAttack == true)
        {
            transform.position = Vector2.Lerp(transform.position, newAttackLocation, .1f);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(moveAngle, Vector3.forward), Time.fixedDeltaTime * rotationSpeed);
        if (Mathf.Abs(Input.GetAxis("Right Horizontal")) > .1f || Mathf.Abs(Input.GetAxis("Right Vertical")) > .1f)
        {
            gunArm.GetComponent<Animator>().ResetTrigger("Return");
            shootAngle = Mathf.Atan2(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical")) * Mathf.Rad2Deg;
            gunArm.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(shootAngle, Vector3.forward), .2f * Mathf.Rad2Deg);
            if (Input.GetAxis("Right Horizontal") < -.1f)
            {
                xDirection = rightDirection;
                swordArm.transform.position = backArmPosition.transform.position;
                gunArm.transform.position = frontArmPosition.transform.position;
                swordShoulder.sortingOrder = 1;
                swordForearm.sortingOrder = 2;
                swordHand.sortingOrder = 1;
                gunShoulder.sortingOrder = 8;
                gunForearm.sortingOrder = 9;
                gunHand.sortingOrder = 10;
                gunGun.sortingOrder = 9;
            }
            else if (Input.GetAxis("Right Horizontal") > .1f)
            {
                xDirection = leftDirection;
                swordArm.transform.position = frontArmPosition.transform.position;
                gunArm.transform.position = backArmPosition.transform.position;
                swordShoulder.sortingOrder = 8;
                swordForearm.sortingOrder = 9;
                swordHand.sortingOrder = 10;
                gunShoulder.sortingOrder = 1;
                gunForearm.sortingOrder = 2;
                gunHand.sortingOrder = 1;
                gunGun.sortingOrder = 0;
            }
            gunArm.GetComponent<Animator>().enabled = true;
            if (gunTimer <= 0)
            {
                GameObject thisBullet = Instantiate(bullet, gun.transform.position, Quaternion.identity);
                thisBullet.transform.up = Vector3.Normalize(new Vector3(gun.transform.right.x * xDirection, gun.transform.right.y * xDirection, 0));
                gunArm.GetComponent<Animator>().SetTrigger("Fire");
                gunTimer = gunTimerMax;
            }
            gunTimer -= Time.deltaTime;
        }
        else
        {
            gunTimer = gunTimerMax;
            gunArm.GetComponent<Animator>().ResetTrigger("Fire");
            gunArm.transform.rotation = Quaternion.Slerp(transform.rotation, baseGunRotation, .01f);
            gunArm.GetComponent<Animator>().SetTrigger("Return");
            gunArm.GetComponent<Animator>().enabled = false;
            if (Input.GetAxis("Left Horizontal") < -.1f)
            {
                xDirection = rightDirection;
                swordArm.transform.position = backArmPosition.transform.position;
                gunArm.transform.position =frontArmPosition.transform.position;
                swordShoulder.sortingOrder = 1;
                swordForearm.sortingOrder = 2;
                swordHand.sortingOrder = 1;
                gunShoulder.sortingOrder = 8;
                gunForearm.sortingOrder = 9;
                gunHand.sortingOrder = 10;
                gunGun.sortingOrder = 9;
            }
            else if (Input.GetAxis("Left Horizontal") > .1f)
            {
                xDirection = leftDirection;
                swordArm.transform.position = frontArmPosition.transform.position;
                gunArm.transform.position = backArmPosition.transform.position;
                swordShoulder.sortingOrder = 8;
                swordForearm.sortingOrder = 9;
                swordHand.sortingOrder = 10;
                gunShoulder.sortingOrder = 1;
                gunForearm.sortingOrder = 2;
                gunHand.sortingOrder = 1;
                gunGun.sortingOrder = 0;
            }
        }
        healthPercentage = health / healthMax;
        healthBar.fillAmount = healthPercentage;
        if(hitTimer >= hitTimerMax - .1f){
            foreach(Transform piece in rigPieces){
                if(piece.GetComponent<SpriteRenderer>() != null){
                    piece.GetComponent<SpriteRenderer>().material.shader = whiteSprite;
                }
            }
        }else{
            foreach (Transform piece in rigPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    piece.GetComponent<SpriteRenderer>().material.shader = spriteDefault;
                }
            }
        }
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
        if(attackNumber > 4){
            attackNumber = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            touchingGround = true;
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            touchingGround = true;
        }
        else
        {
            touchingGround = false;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            touchingGround = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Enemy Attack" && hitTimer <= 0)
        {
            if (hitTimer <= 0)
            health -= other.gameObject.GetComponent<ProjectileScript>().damage;
            hitTimer = hitTimerMax;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy Attack" && hitTimer <= 0)
        {
            if (hitTimer <= 0)
                health -= other.gameObject.GetComponent<ProjectileScript>().damage;
            hitTimer = hitTimerMax;
        }
    }
}
