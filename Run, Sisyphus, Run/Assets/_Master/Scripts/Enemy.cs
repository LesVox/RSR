using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats
    {
        public int Health = 100;

        public float delayJump = 0.5f;
        public float delayShoot = 0.7f;
        public float delayMove = 0.5f;

        public float delayJumpMax = 1.5f;
        public float delayShootMax = 1.7f;

        public float desiredDist = 5.0f;
        public float maxDist = 10.0f;

        public float damageDealt = 0;
        public float timeAlive = 0;
        public float fitness = 0;

        
    }

    public EnemyStats stats = new EnemyStats();

    public Rigidbody2D rb;

    private float JumpForce = 400f;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void DamageEnemy(int damage)
    {
        stats.Health -= damage;
        if (stats.Health <= 0)
        {
            GameMaster.KillEnemy(this);
        }
    }

    public void Jump()
    {
        rb.AddForce(new Vector2(0f, JumpForce));
    }

    public void IncreaseDamageDealt(int damage)
    {
        stats.damageDealt += damage;
    }

    public void IncreasTimeAlive()
    {
        stats.timeAlive += Time.deltaTime;
    }

    public float CalculateFitness()
    {
        stats.fitness = stats.damageDealt + stats.timeAlive;

        return stats.fitness;
    }

}
