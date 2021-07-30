using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgImageMove : MonoBehaviour
{
    private GameManager gameManager = null;
    private SpriteRenderer spriteRenderer = null;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Skilling == true)
            spriteRenderer.color = new Color(60 / 255f, 90 / 255f, 200 / 255f);
        else
            spriteRenderer.color = Color.white;

    }
}
