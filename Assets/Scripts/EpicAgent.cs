using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class EpicAgent : Agent
{
    public GameManager gameManager;
    Rigidbody cRigidbody;
    Unit unit;

    void Start() {
        cRigidbody = GetComponent<Rigidbody>();
        unit = GetComponent<Unit>();

    }

    public override void OnEpisodeBegin() {

        this.cRigidbody.angularVelocity = Vector3.zero;
        this.cRigidbody.velocity = Vector3.zero;
        this.transform.position = gameManager.GetRandomSpawnLocation();
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

    }

    public override void CollectObservations(VectorSensor sensor) {
        var localVelocity = transform.InverseTransformDirection(cRigidbody.velocity);
        var normalizedVelocity = cRigidbody.velocity.normalized;
        //sensor.AddObservation(normalizedVelocity.x);
        //sensor.AddObservation(normalizedVelocity.z);
    }

    private void FixedUpdate() {
        AddReward(-0.01f * Time.deltaTime);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) {
        // Actions, size = 2


        var continuousActions = actionBuffers.ContinuousActions;
        var forward = Mathf.Clamp(continuousActions[0], -1f, 1f);
        var right = Mathf.Clamp(continuousActions[1], -1f, 1f);
        var rotate = Mathf.Clamp(continuousActions[2], -1f, 1f);

        var dirToGo = transform.forward * forward;
        dirToGo += transform.right * right;
        var rotateDir = -transform.up * rotate;


        cRigidbody.AddForce(dirToGo * (float)unit.Speed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime *(float)unit.Speed);

    }

    private void OnTriggerEnter(Collider other) {
        
        var powerup = other.GetComponent<Powerup>();
        if(powerup != null) {
            switch (powerup.type) {
                case Powerup.Type.Bad:
                    var reward = Math.Min(-1, -GetCumulativeReward());
                    Debug.Log("Powerup "+powerup.type + " reward: "+ reward);
                    AddReward(reward);
                    EndEpisode();
                    break;
                case Powerup.Type.Good:
                    Debug.Log("Powerup " + powerup.type + " reward: " + 1 + " total "+ GetCumulativeReward());
                    AddReward(1);
                    break;
                case Powerup.Type.Speed:
                    Debug.Log("Powerup " + powerup.type + " reward: " + 0.05f + " total " + GetCumulativeReward());
                    AddReward(0.05f);
                    break;
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
