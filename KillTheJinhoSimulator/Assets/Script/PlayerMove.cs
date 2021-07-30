using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    [SerializeField]
    private float MaxSpeed = 0f;
    private SpriteRenderer spriteRenderer = null;
    [SerializeField]
    private float Speed = 2f;
    [SerializeField]
    private float JumpSpeed = 0f;
    private RaycastHit2D rayHit;
    private RaycastHit2D enemyRay;
    private bool isJump = false;
    [SerializeField]
    private int JumpAble = 2;
    [SerializeField]
    private float IchiNokaTaSpeed = 0f;
    [SerializeField]
    private AudioClip IchiNoKaTaAudio = null;
    private SoundManager soundManager = null;
    private bool IchiNoKaTaing = false;
    [SerializeField]
    private GameObject Lightning1Prefab = null;
    [SerializeField]
    private Transform leftLightning1Position = null;
    [SerializeField]
    private Transform rightLightning1Position = null;
    private int LeftMove = -1;

    private float curTime = 0f;
    private float coolTime = 0.5f;

    private GameManager gameManager = null;
    [SerializeField]
    private int life = 0;
    private bool isdead = false;
    private Animator animator = null;
    [SerializeField]
    private GameObject attackEffectPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Move(int RightOrLeft)
    {
        LeftMove = RightOrLeft;
        if (RightOrLeft == -1)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
        rigid.AddForce(Vector2.right * Speed * RightOrLeft * 3f  ,ForceMode2D.Impulse);
        if(rigid.velocity.x > MaxSpeed)
            rigid.velocity = new Vector2(MaxSpeed,rigid.velocity.y);
        else if (rigid.velocity.x < MaxSpeed *(-1))
            rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);

    }
    private void Jump()
    {
        JumpAble--;
        rigid.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < -21)
            gameObject.transform.position = new Vector2 (-19f,gameObject.transform.position.y);
        if (gameObject.transform.position.x > 33)
            gameObject.transform.position = new Vector2(32f,gameObject.transform.position.y);
        if (Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
        }
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));
        rayHit = Physics2D.Raycast(rigid.position, Vector2.down,1,LayerMask.GetMask("Platform"));
        enemyRay = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Enemy"));
        if (rayHit.collider != null || enemyRay.collider != null)
        {
            JumpAble = 2;
        }
        if (Input.GetButtonDown("Jump") && JumpAble != 1 && IchiNoKaTaing == false)
            Jump();
        if (Input.GetKeyDown(KeyCode.R) && IchiNoKaTaing == false && rayHit.distance > 0.1f)
        {
            Time.timeScale = 0;
            IchiNoKaTa();
        }
        if(curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                //평타애니메이션 실행
                Attack();
                curTime = coolTime;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }
    private void Attack()
    {
        GameObject attack;
        if (LeftMove < 0f)
            attack = Instantiate(attackEffectPrefab, leftLightning1Position);
        else
            attack = Instantiate(attackEffectPrefab, rightLightning1Position);
        attack.transform.SetParent(null);
    }
    private void FixedUpdate()
    {
        if (IchiNoKaTaing) return;
        if (Input.GetKey(KeyCode.RightArrow))
            Move(1);
        if (Input.GetKey(KeyCode.LeftArrow))
            Move(-1);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (isdead) return;
            isdead = true;
            StartCoroutine(Damaged());
        }
    }

    private IEnumerator Damaged()
    {
        life--;
        if(life <= 0)
        {
            Destroy(gameObject);
        }
        for(int i =0; i<4; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        isdead = false;
    }
    private void IchiNoKaTa()
    {
        spriteRenderer.color = Color.white;
        soundManager.BgSoundStop();
        IchiNoKaTaing = true;
        gameObject.layer = 10;
        gameManager.Skilling = true;
        soundManager.SFXPlay("IchiNoKaTa", IchiNoKaTaAudio);
        StartCoroutine(IchiNoKaTaAttack());
    }
    private IEnumerator IchiNoKaTaAttack()
    {
        yield return new WaitForSecondsRealtime(7f);
        Time.timeScale = 1;
        GameObject Lightning1;
        animator.SetBool("IchinoKaTa", true);
        if (LeftMove < 0f)
            Lightning1 = Instantiate(Lightning1Prefab, leftLightning1Position);
        else
            Lightning1 = Instantiate(Lightning1Prefab, rightLightning1Position);
        Lightning1.transform.SetParent(null);

        if (spriteRenderer.flipX == true)
        {
            transform.position = new Vector2(gameObject.transform.position.x + 10f, gameObject.transform.position.y);
            //rigid.AddForce(Vector2.right * IchiNokaTaSpeed, ForceMode2D.Impulse);
        }

        else
        {
            transform.position = new Vector2(gameObject.transform.position.x - 10f, gameObject.transform.position.y);
            //rigid.AddForce(Vector2.right * IchiNokaTaSpeed * (-1), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.3f);
        IchiNoKaTaing = false;
        animator.SetBool("IchinoKaTa", false);
        gameManager.Skilling = false;
        yield return new WaitForSeconds(0.2f);
        gameObject.layer = 9;
        soundManager.BgSoundPlay();
    }
}
