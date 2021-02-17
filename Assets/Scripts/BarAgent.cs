using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;

public class BarAgent : Agent
{
    public Transform ballTrasnform;
    public Rigidbody ballRigidBody;
    private float moveMultiplier = 0.5f;
    private float ballHitMultiplier = 500;
    private List<GameObject> bricks;

    private Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
    }

    public override void OnEpisodeBegin()
    {
        foreach (GameObject brickGO in bricks)
        {
            brickGO.SetActive(true);
        }

        transform.localPosition = new Vector3(0, -5.0f, Random.Range(-4.5f, 4.5f));
        rBody.velocity = Vector3.zero;

        ballTrasnform.localPosition = new Vector3(0, 2.5f, Random.Range(-4.5f, 4.5f));
        ballRigidBody.velocity = new Vector3(0, -7.0f, Random.Range(-2.0f, 2.0f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(rBody.velocity.z);
        //sensor.AddObservation(transform.localPosition.z);

        //sensor.AddObservation(ballTrasnform.localPosition.y);
        //sensor.AddObservation(ballTrasnform.localPosition.z);
        //sensor.AddObservation(ballRigidBody.velocity.y);
        //sensor.AddObservation(ballRigidBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.DiscreteActions[0] - 1;
        rBody.velocity += controlSignal * moveMultiplier;
        rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, 10.0f);
        
        if (ballTrasnform.localPosition.y < transform.localPosition.y - 1.0f)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetAxis("Horizontal") < -0.3f ? 0 :
            Input.GetAxis("Horizontal") > 0.3f ? 2 : 1;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            //Debug.DrawRay(transform.position, dir * 50,Color.red, 2.0f);
            dir = dir.normalized;
            ballRigidBody.AddForce(dir * ballHitMultiplier);
            AddReward(0.1f);
        }
    }

}
