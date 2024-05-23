using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;

    int currentWaypoint = 0;
    
    public WaveConfig WaveConfig
    {
        get => waveConfig;
        set => waveConfig = value;
    }

    private void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[0].position;
        currentWaypoint++;
    }
    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (currentWaypoint <= waypoints.Count - 1)
        {
            if (transform.position != waypoints[currentWaypoint].position)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, waveConfig.GetMoveSpeed * Time.deltaTime);
            }
            else currentWaypoint++;
        }
        else Destroy(gameObject);
    }
}
