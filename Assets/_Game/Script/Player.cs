using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private int kunaiCapacity = 3;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private float attackDamage;

    public bool isGrounded = true;
    public bool isJumping = false;
    public bool isAttack = false;

    private float horizontal;

    private int coin = 0;
    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    private Vector3 savePoint;
    void Update()
    {
        if (IsDead)
        {
            return;
        }
        isGrounded = CheckGrounded();
        //SetMove(horizontal);
        // horizontal có giá trị từ -1 đến 1, -1 khi ấn sang trái và 1 khi ấn sang phải
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isGrounded)
        {
            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
        }
        //attack
        if (Input.GetKeyDown(KeyCode.C))
        {
            Attack();
        }
        //throw
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Throw();
        }
        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }
        // Moving 
        if (Mathf.Abs(horizontal) >= 0.1f)
        {
            rb.velocity = new Vector2(horizontal*speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0,horizontal>0?0:180,0));
        }
        //idle
        else if (isGrounded && !isAttack)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;

        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
        UIManager.instance.SetCoin(coin);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f,Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        //if (hit.collider != null)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return hit.collider != null;
    }
    public void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAtack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public void Throw()
    {
        if (kunaiCapacity > 0)
        {
            ChangeAnim("throw");
            isAttack = true;
            Invoke(nameof(ResetAttack), 0.5f);         
            Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
            kunaiCapacity--;
            if (kunaiCapacity == 0)
            {
                ChangeAnim("Idle");
                Invoke(nameof(CreatKunai), 10f);
            }
        } 
    }

    private void CreatKunai()
    {
        kunaiCapacity = 3;
    }
    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }
    public void Jump()
    {
        ChangeAnim("jumpin");
        isJumping = true;
        rb.AddForce(jumpForce*Vector2.up);
    }
    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    private void ActiveAtack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;

            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "DeadZone")
        {
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f);
        }
        if (collision.tag == "FlashBottle")
        {
            Destroy(collision.gameObject);
            UIManager.instance.WaterPoition();
        }
        if (collision.tag == "HPbottle")
        {
            Destroy(collision.gameObject);
            HealingPlayer();         
        }
    }
    private void HealingPlayer()
    {
        this.hp += 30;
        if (hp>100)
        {
            hp = 100;
        }
        healthBar.SetNewHp(hp);
    }
    public void SetMove(float horizontal)
    {
        
        this.horizontal = horizontal;
    }
}
