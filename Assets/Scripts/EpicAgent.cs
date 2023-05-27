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
    public GameObject model;
    Rigidbody cRigidbody;
    Unit unit;
    GameManager gameManager;


    void Start() {
        cRigidbody = GetComponent<Rigidbody>();
        unit = GetComponent<Unit>();
        gameManager = unit.gameManager;

        unit.onReset += onUnitReset;
        unit.onRoundEnd += onUnitRoundEnd;
    }

    private void onUnitReset(Unit unit, ResetReason reason) {
        if(reason == ResetReason.roundStarted) {
            EndEpisode();
        }
    }

    private void onUnitRoundEnd(bool hasWon, bool directly) {
        if (hasWon) {
            AddReward(1);
            if(directly) {
                AddReward(gameManager.scoreToWin * gameManager.scoreToWin / (Time.time - gameManager.RoundStartTime));
            }
        }
        else {
            AddReward(-1);
            if (directly) {
                var reward = Math.Min(0, -GetCumulativeReward());
                AddReward(reward);
            }
        }
    }

    public override void OnEpisodeBegin() {
        this.cRigidbody.angularVelocity = Vector3.zero;
        this.cRigidbody.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor) {
        var localVelocity = transform.InverseTransformDirection(cRigidbody.velocity);
        var normalizedVelocity = cRigidbody.velocity.normalized;
        //sensor.AddObservation(transform.localPosition.x);
        //sensor.AddObservation(transform.localPosition.y);
        //sensor.AddObservation(normalizedVelocity.x);
        //sensor.AddObservation(normalizedVelocity.z);
        //sensor.AddObservation(cRigidbody.velocity.magnitude);
    }

    private void Update() {
        if(cRigidbody.velocity != Vector3.zero) {
            model.transform.rotation = Quaternion.LookRotation(cRigidbody.velocity, transform.up);
        }
    }

    private void FixedUpdate() {
        AddReward(-0.001f * Time.deltaTime);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) {
        var continuousActions = actionBuffers.ContinuousActions;
        var forward = Mathf.Clamp(continuousActions[0], -1f, 1f);
        var right = Mathf.Clamp(continuousActions[1], -1f, 1f);

        var dirToGo = transform.forward * forward;
        dirToGo += transform.right * right;


        cRigidbody.AddForce(dirToGo * (float)unit.Speed, ForceMode.VelocityChange);

        if(transform.position.y < ((gameManager?.FloorY ?? 0f) - 1)) {
            unit.ResetUnit(ResetReason.died);
        }

    }

    private void OnTriggerEnter(Collider other) {
        
        var powerup = other.GetComponent<Powerup>();
        if(powerup != null) {
            switch (powerup.type) {
                case Powerup.Type.Bad:
                    var reward = Math.Max(Math.Min(-0.50f, -GetCumulativeReward()), -gameManager.scoreToWin);
                    AddReward(reward);
                    break;
                case Powerup.Type.Good:
                    AddReward(Math.Max(0.10f,1f/gameManager.scoreToWin));
                    break;
                case Powerup.Type.Speed:
                    AddReward(0.01f);
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Wall") {
            AddReward(-0.001f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
