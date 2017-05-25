using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    int genePool = 10;

    public float timerMax;
    public float playerJumpTimer;
    public float playerJumpTimerMax;

    public float playerShootTimer;
    public float playerShootTimerMax;

    public float playerMoveTimer;
    public float playerMoveTimerMax;

    private bool jumpTimerRunning;
    private bool moveTimerRunning;
    private bool shootTimerRunning;

    List<Enemy> PromisingElements = new List<Enemy>();

    public Transform enemyPrefab;
    public Transform enemySpawnPoint;

    Platformer2DUserControl actionHolder;

    Enemy spawn;
    Player user;

    public int spawnDelay = 2;

    private void Awake()
    {
        user = FindObjectOfType<Player>();
        
    }

    public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
    }

    public IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(spawnDelay);

        Transform foe = Instantiate(enemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
        spawn = foe.GetComponent<Enemy>();
        if(spawn != null)
        {
            int n;
            for (n = 0; n < genePool; n++)
            {
                spawn.stats.delayJump += instance.PromisingElements[n].stats.delayJump;
                spawn.stats.delayShoot += instance.PromisingElements[n].stats.delayShoot;
                spawn.stats.delayMove += instance.PromisingElements[n].stats.delayMove;
                spawn.stats.delayJumpMax += instance.PromisingElements[n].stats.delayJumpMax;
                spawn.stats.delayShootMax += instance.PromisingElements[n].stats.delayShootMax;

                spawn.stats.desiredDist += instance.PromisingElements[n].stats.desiredDist;
                spawn.stats.maxDist += instance.PromisingElements[n].stats.maxDist;
            }

            spawn.stats.delayJump = spawn.stats.delayJump * Random.Range(0.9f, 1.1f) / n;
            spawn.stats.delayShoot = spawn.stats.delayShoot * Random.Range(0.9f, 1.1f) / n;
            spawn.stats.delayMove = spawn.stats.delayMove * Random.Range(0.9f, 1.1f) / n;
            spawn.stats.delayJumpMax = spawn.stats.delayJumpMax * Random.Range(0.9f, 1.1f) / n;
            spawn.stats.delayShootMax = spawn.stats.delayShootMax * Random.Range(0.9f, 1.1f) / n;

            spawn.stats.desiredDist = spawn.stats.desiredDist * Random.Range(0.9f, 1.1f) / n;
            spawn.stats.maxDist = spawn.stats.maxDist * Random.Range(0.9f, 1.1f) / n;
        }

    }

    public static void KillEnemy(Enemy enemy)
    {
        //Time to Calculate Fitness!
        int n;
        for (n = 0; n < instance.genePool; n++)
        {
            if (enemy.CalculateFitness() >= instance.PromisingElements[n].CalculateFitness() || instance.PromisingElements[n] == null)
            {
                instance.PromisingElements.Insert(n, enemy);
            }
        }
        Destroy(enemy.gameObject);
        instance.StartCoroutine(instance.RespawnEnemy());
    }

    private void Update()
    {
        if (spawn != null)
        {
            spawn.IncreasTimeAlive();
        }
    }

    private void FixedUpdate()
    {
        //Checking if the player has done anything. Checking for shots is done in the weapon script.
        actionHolder = FindObjectOfType<Platformer2DUserControl>();
        if (actionHolder.regJump)
        {
            playerJumpTimeCheck();
        }
        if (actionHolder.regH != 0)
        {
            playerMoveTimeCheck();
        }

        //Countdowns and action execution
        if (jumpTimerRunning)
        {
            playerJumpTimer += Time.deltaTime;
            if(playerJumpTimer >= spawn.stats.delayJump)
            {
                spawn.Jump();
                playerJumpTimer = 0;
                jumpTimerRunning = false;
            }
        }

        //If the player is too far away or we have been standing still for too long since the player did anything, spawn ("enemy") move into its desired distance
        if (moveTimerRunning)
        {
            playerMoveTimer += Time.deltaTime;
        }
        Debug.Log("Spawn is = " + spawn.ToString());
        Debug.Log("User is = " + user.ToString());
        if (playerMoveTimer >= spawn.stats.delayMove || user.transform.position.sqrMagnitude - spawn.transform.position.sqrMagnitude > spawn.stats.maxDist)
        {
            EnemyAI1 spawnAI = spawn.GetComponent<EnemyAI1>();
            if(spawnAI != null)
            {
                spawnAI.mayMove = true;
            }
            playerMoveTimer = 0;
            moveTimerRunning = false;
        }
        else
        {
            EnemyAI1 spawnAI = spawn.GetComponent<EnemyAI1>();
            if (spawnAI != null)
            {
                spawnAI.mayMove = false;
            }
        }

        if (shootTimerRunning)
        {
            playerShootTimer += Time.deltaTime;
            if(playerShootTimer >= spawn.stats.delayShoot)
            {
                //TODO: perform action
                playerShootTimer = 0;
                shootTimerRunning = false;
            }
        }
    }

    #region actionTimer

    public void playerJumpTimeCheck()
    {
        if(playerJumpTimer <= 0 && !jumpTimerRunning)
        {
            jumpTimerRunning = true;
            return;
        }

        return;
    }

    public void playerMoveTimeCheck()
    {
        if (playerMoveTimer <= 0 && !moveTimerRunning)
        {
            moveTimerRunning = true;
            return;
        }

        return;
    }

    public void playerShootTimeCheck()
    {
        if (playerShootTimer <= 0 && !shootTimerRunning)
        {
            shootTimerRunning = true;
            return;
        }

        return;
    }
    #endregion

}
