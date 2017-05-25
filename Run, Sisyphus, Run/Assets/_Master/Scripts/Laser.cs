using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Laser : MonoBehaviour {

    public int moveSpeed = 130;
    bool m_facingRight;
    public int dmg = 0;

    public bool friendly = false;


    private void Start()
    {
        if (friendly)
        {
            m_facingRight = FindObjectOfType<PlatformerCharacter2D>().ReturnFacing();
        }
        else if (!friendly)
        {
            if (FindObjectOfType<PlatformerCharacter2D>().transform.position.x > FindObjectOfType<Enemy>().transform.position.x)
                m_facingRight = true;
            else
                m_facingRight = false;
        }
        //m_facingRight = FindObjectOfType<PlatformerCharacter2D>().ReturnFacing();

        if (!m_facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        
    }

    void Update ()
    {
        if (m_facingRight)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }

        Destroy(gameObject, 1);
	}

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Enemy hit = coll.gameObject.GetComponent<Enemy>();
        if(hit == null)
        {
            Player hitPlayer = coll.gameObject.GetComponent<Player>();
            if(hitPlayer == null)
            {
                Destroy(gameObject);
            }
            if (!friendly)
            {
                hitPlayer.DamagePlayer(dmg);
                Destroy(gameObject);
            }
                
        }
        else
        {
            hit.DamageEnemy(dmg);
            Destroy(gameObject);
        }
    }
}
