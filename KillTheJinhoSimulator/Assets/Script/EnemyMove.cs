using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    private int HP = 0;
    private SpriteRenderer spriteRenderer = null;
    private bool isDamaged = false;
    private int LeftMove = 0;
    private Rigidbody2D rigid = null;
    [SerializeField]
    private float Speed = 0f;
    [SerializeField]
    private float MaxSpeed = 0f;
    private float RandomDelay = 0f;
    private int RandomMove = 0;
    private GameManager gameManager = null;
    [SerializeField]
    private long score = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Randoms());
    }
    private IEnumerator Randoms()
    {
        while (true)
        {
            RandomDelay = Random.Range(0.5f, 2f);
            RandomMove = Random.Range(-1, 2);
            Move(RandomMove);
            yield return new WaitForSeconds(RandomDelay);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("IchiNoKaTa"))
        {
            if (isDamaged) return;
            isDamaged = true;
            HP -= 100;
            Damaged();
        }
        if (collision.CompareTag("PlayerAttack"))
        {
            if (isDamaged) return;
            isDamaged = true;
            Damaged();
        }
    }
    private void Move(int RightOrLeft)
    {
        LeftMove = RightOrLeft;
        if (RightOrLeft == -1)
            spriteRenderer.flipX = false;
        else if (RightOrLeft == 1)
            spriteRenderer.flipX = true;
        rigid.AddForce(Vector2.right * Speed * RightOrLeft * 3f, ForceMode2D.Impulse);
        if (rigid.velocity.x > MaxSpeed)
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < MaxSpeed * (-1))
            rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);

    }
    private void Damaged()
    {
        HP--;
        if (HP <= 0)
        {
            gameManager.ScoreUp(score);
            Destroy(gameObject);
        }
        StartCoroutine( ColorChange());
    }
    private IEnumerator ColorChange()
    {
        for (int i = 0; i < 4; i++) { 
        spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        isDamaged = false;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
