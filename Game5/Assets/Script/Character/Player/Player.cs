using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player :  MonoBehaviour
{
    [HideInInspector] public SpriteRenderer mySR;
    [HideInInspector] public Rigidbody2D myrigid;
    [HideInInspector] public Animator myanim;
    [HideInInspector] public PlayerHurt playerhurt;
    [HideInInspector] public PlayerAttack playerAttack;
    [HideInInspector] public SkillSet playerSkill;
    [HideInInspector] public Collider2D mycollider;
    [HideInInspector] public bool isAlve = true;
    public DataScriptObject playerdata;
    private int enemycount;
    float move_x, move_y;
    [HideInInspector] public float maxhealth, maxmana, health, mana, speed;
    //public float exp, exptolevel;
    //[HideInInspector] public int level;

    public float dashVelocity, dashCD;
    public Vector3 distanceDir;
    bool canDash;
    public static bool near = false;
    [Header("Pick Up")]
    Collider2D[] pickup;
    public Collider2D[] interactCollider;
    [HideInInspector] public float rangePickup = 1.2f;
    public LayerMask pickupMask, interactLayer;
    private void Awake()
    {
        mySR = GetComponent<SpriteRenderer>();
        myrigid = GetComponent<Rigidbody2D>();
        mycollider = GetComponent<Collider2D>();
        myanim = GetComponent<Animator>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerhurt = GetComponentInParent<PlayerHurt>();
        playerSkill = GetComponentInParent<PlayerSkill>();
    }
    private void Start()
    {
        health = PlayerPrefs.GetFloat("heath");
        PlayerPrefs.SetFloat("currenthealth", health);
        mana = PlayerPrefs.GetFloat("mana");
        PlayerPrefs.SetFloat("currentmana", mana);
        //exp = PlayerPrefs.GetFloat("Exp");
        //exptolevel = PlayerPrefs.GetFloat("Explevelup");
        //level = PlayerPrefs.GetInt("level");
    }
    private void OnDisable()
    {
        //playerdata.levelUp.expToLvl = exptolevel;
        //playerdata.levelUp.exp = exp;
        //playerdata.levelUp.level = level;
    }
    private void Update()
    {
        maxhealth = playerdata.basicStats.health;
        maxmana = playerdata.basicStats.mana;
        PlayerPrefs.SetFloat("currentmana", mana);
        PlayerPrefs.SetFloat("currenthealth", health);
        speed = PlayerPrefs.GetFloat("speed") + (PlayerPrefs.GetFloat("speed") * PlayerPrefs.GetFloat("bounsSpeed")) / 100;
        if (isAlve)
        {
            playerAttack.FindEnemy();
            playerhurt.RegenRecover();
            PickUp();
            InteractablePlayer();
            StartCoroutine(HandleSkillInput());
            DashBtn();
        }
    }
    private void FixedUpdate()
    {
        move_x = Input.GetAxis("Horizontal");
        move_y = Input.GetAxis("Vertical");
        Move();
        if (health >= PlayerPrefs.GetFloat("heath"))
            health = PlayerPrefs.GetFloat("heath");
        if (mana >= PlayerPrefs.GetFloat("mana"))
            mana = PlayerPrefs.GetFloat("mana");
 
    }
    #region move & flip
    private void Move()
    {
        Vector2 newvelocity = new Vector2(move_x, move_y).normalized;
        if (move_x != 0 || move_y != 0 && !playerAttack.isCanAttack) 
        {
            if (playerAttack.isCanAttack)
                myrigid.velocity = Vector3.zero;
            else
            {
                myanim.Play("Walk");
                myanim.SetFloat("MoveX", move_x);
                myanim.SetFloat("MoveY", move_y);
                myanim.SetBool("Walking", true);
                myrigid.velocity = (newvelocity * speed * Time.deltaTime);
            }
        }
        else if (move_x == 0 && move_y == 0) 
        {
            myanim.SetBool("Walking", false);
            myrigid.velocity = Vector2.zero;
        }
    }
    private void FlipCharacter()
    {
        if (move_x < 0)
            mySR.flipX = false;
        else if (move_x > 0)
            mySR.flipX = true;
    }
    public void SetPosition(Vector3 cords, Vector2 direction)
    {
        Vector2 currentMove = new Vector2(move_x, move_y);
        this.transform.position = cords;
        currentMove = direction;
    }
    #endregion

    #region Dash
    private void DashBtn()
    {
        if (Input.GetKeyDown(KeyCode.R) && !canDash)
        {
            Dash();
        }
    }
    private void Dash()
    {
        canDash = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position);
        ParticleSystem clone1 = Instantiate(AssetManager.instance.assetData.dashInEffect, transform.position, Quaternion.identity);
        clone1.transform.parent = transform.parent;
        if (direction.magnitude < dashVelocity)
            transform.Translate(direction);
        else
            transform.Translate(direction.normalized * dashVelocity);
        mySR.enabled = false;
        StartCoroutine(DashTime(1f));
        mySR.enabled = true;
        ParticleSystem clone2 = Instantiate(AssetManager.instance.assetData.dashOutEffect, transform.position, Quaternion.identity);
        clone2.transform.parent = transform.parent;
        StartCoroutine(DashDelayCD());
    }
    IEnumerator DashTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
    IEnumerator DashDelayCD()
    {
        yield return new WaitForSeconds(dashCD);
        canDash = false;
    }
    #endregion

    #region PickUp
    void PickUp()
    {
        pickup = Physics2D.OverlapCircleAll(transform.position, rangePickup, pickupMask);
        if (pickup.Length == 0)
            return;
        if (pickup.Length > 0)
            foreach (var collider in pickup)
            {
                if (collider.gameObject.TryGetComponent<LootItem>(out LootItem loot))
                    loot.StartCoroutine(loot.MoveCourtine());
            }
    }
    void InteractablePlayer()
    {
        interactCollider = Physics2D.OverlapCircleAll(transform.position, 1.4f, interactLayer);
        if (interactCollider.Length == 0)
        {
            near = false;
            return;
        }
        near = true;
        if (Input.GetKeyDown(KeyCode.Z) && near && interactCollider.Length > 0 && !DialogueManager.isDialogueOpen && !DialogueManager.instance.dialogueBox.activeSelf) 
            foreach (var collider in interactCollider)
            {
                if (collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                    interactable.Interact();
            }
    }
    #endregion

    #region Level System
    /*public void AddExperience(float expToAdd)
    {
        PlayerPrefs.SetFloat("Exp", PlayerPrefs.GetFloat("Exp") + expToAdd + (expToAdd * PlayerPrefs.GetFloat("extraExp")) / 100);
        while (PlayerPrefs.GetFloat("Exp") >= PlayerPrefs.GetFloat("Explevelup"))
            LevelUp();
        exptolevel = PlayerPrefs.GetFloat("Explevelup");
        exp = PlayerPrefs.GetFloat("Exp");
    }
    public void LevelUp()
    {
        PlayerPrefs.SetFloat("Exp", PlayerPrefs.GetFloat("Exp") - PlayerPrefs.GetFloat("Explevelup"));
        PlayerPrefs.SetInt("skillpoint", PlayerPrefs.GetInt("skillpoint") + 1);
        PlayerPrefs.SetFloat("Explevelup", PlayerPrefs.GetFloat("Explevelup") * 1.20f);
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        level = PlayerPrefs.GetInt("level");
    }*/
    #endregion

    #region Attack && Skill
    public void P_StopAttack()
    {
        playerAttack.isCanAttack = false;
    }
    IEnumerator HandleSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.T) && !playerAttack.isCanAttack && mana >= playerSkill.ultimateskill.skillMpCost)
            //&& !playerAttack.skillUI.isUltimateSkillCD)
        {
            playerAttack.isCanAttack = true;
            playerSkill.UltimateSkill();
            //playerAttack.skillUI.UseUltimateSkill();
            yield return new WaitForSeconds(1.05f);
            playerAttack.isCanAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.Z) && !playerAttack.isCanAttack && !near) //!playerAttack.skillUI.isAttackCD
        { 
            playerAttack.isCanAttack = true;
            playerSkill.NormalAttack();
            //playerAttack.skillUI.UseAttackNormal();
            //AudioManager.instance.PlaySfx("fireball");
        }
    }
    #endregion
}

