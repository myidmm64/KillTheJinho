using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawner = null;
    [SerializeField]
    private GameObject []enemyPrefab = null;
    private int randomEnemyChoose = 0;
    private float SpawnDelay = 3f;
    public bool Skilling = false;
    [SerializeField]
    private Text scoreText = null;
    private long score = 0;
    private int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
        UpdateUI();
    }
    public void UpdateUI()
    {
        scoreText.text = string.Format("SCORE\n{0}", score);
    }
    public void ScoreUp(long addscore)
    {
        score += addscore;
        UpdateUI();
    }
    private IEnumerator EnemySpawn()
    {
        while (true)
        {
            GameObject enemy;
            randomEnemyChoose = Random.Range(0, 3);
            if(score > 80 * level)
            {
                level++;
                if (SpawnDelay < 0.5f)
                    SpawnDelay = 0.5f;
                else
                    SpawnDelay -= 1f;
                if (SpawnDelay < 0.5f)
                    SpawnDelay = 0.5f;
            }
            enemy = Instantiate(enemyPrefab[randomEnemyChoose], spawner.transform);
            enemy.transform.SetParent(null);
            yield return new WaitForSeconds(SpawnDelay);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
