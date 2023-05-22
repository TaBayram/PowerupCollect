using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EpicAgent : Agent
{
    Rigidbody cRigidbody;
    Unit unit;

    void Start() {
        cRigidbody = GetComponent<Rigidbody>();
        unit = GetComponent<Unit>();

    }

    public override void OnEpisodeBegin() {

        this.cRigidbody.angularVelocity = Vector3.zero;
        this.cRigidbody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 0.5f, 0);



    }

    public override void CollectObservations(VectorSensor sensor) {
        var localVelocity = transform.InverseTransformDirection(cRigidbody.velocity);
        sensor.AddObservation(cRigidbody.velocity.x);
        sensor.AddObservation(cRigidbody.velocity.z);
    }

    public float forceMultiplier = 10;
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
                    Console.Write("Bad");
                    EndEpisode();
                    break;
                case Powerup.Type.Good:
                    AddReward(1);
                    break;
                case Powerup.Type.Speed:
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
