using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI1 : MonoBehaviour {

    public Transform target;

    //How many times per second we will update our path.
    public float updateRate = 2f;

    //Caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //The calculated path
    public Path path;

    //AI spd per sec
    public float speed = 300f;
    public ForceMode2D fMode;

    public bool mayMove;

    [HideInInspector]
    public bool pathIsEnded = false;

    //The max distance from the AI to a waypoint for it to continue to the waypoint.
    public float nextWayPointDistance = 3;

    //The Waypoint we are currently moving towards.
    private int currentWaypoint = 0;

     void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null)
        {
            Debug.Log("No Player found");
            return;
        }

        //Start a new Path to the target position return the result to the ONPathComplete method.
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        if(target == null)
        {
            //TODO: Insert a player search here
            yield break;
        }

        //Start a new Path to the target position return the result to the ONPathComplete method.
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());

    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path, did it have an error?" + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            //TODO: Insert a player search here
            return;
        }

        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir.y = 0;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI
        if (mayMove)
            rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWayPointDistance)
        {
            currentWaypoint++;
            return;
        }
        

    }

}
