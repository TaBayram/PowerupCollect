using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Unit unit;
    public GameObject model;
    private Vector3 playerVelocity;
    private CharacterController cController;


    void Start()
    {
        cController = GetComponent<CharacterController>();
        unit.onReset += onResetUnit;
    }


    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cController.Move(move * Time.deltaTime * (float)unit.Speed);

        if (move != Vector3.zero) {
            model.transform.rotation = Quaternion.LookRotation(move, transform.up);
        }

        var groundedPlayer = cController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += -10 * Time.deltaTime;
        cController.Move(playerVelocity * Time.deltaTime);

        if (transform.position.y < ((unit.gameManager?.FloorY ?? 0f) - 1)) {
            unit.ResetUnit(ResetReason.died);
        }

    }

    public void onResetUnit(Unit unit, ResetReason reason) {
        cController.enabled = false;
        transform.position = unit.gameManager.GetRandomSpawnLocation();
        cController.enabled = true;
    }
}
