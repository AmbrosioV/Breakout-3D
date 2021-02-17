using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public BarAgent agent;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Brick"))
        {
            agent.AddReward(1.0f);
            col.gameObject.SetActive(false);
            if (GameObject.FindGameObjectsWithTag("Brick").Length == 0)
            {
                agent.EndEpisode();
            }
        }
    }
}
