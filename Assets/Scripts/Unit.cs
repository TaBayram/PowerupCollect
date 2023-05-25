using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ResetReason
{
    roundStarted=1,
    died = 2,
}

public class Unit : MonoBehaviour, IUnit
{
    public string playerName;
    [SerializeField]
    private double speed;
    private double originalSpeed;
    private int score;
    public int winCount = 0;
    public int lossCount = 0;
    private List<Coroutine> coroutines = new List<Coroutine>();
    public List<PowerupPickupData> powerupPickupData= new List<PowerupPickupData>();

    public Action<Unit> onScoreChange;
    public Action<bool> onRoundEnd;
    public Action<Unit, ResetReason> onReset;

    public GameManager gameManager;

    private bool isAI = true;
    public float totalDistance = 0;
    private Vector3 previousLocation;


    public double Speed { get => speed; set => speed = value; }
    public int Score { get => score; set { score = value; onScoreChange?.Invoke(this); } }

    private void Start() {
        originalSpeed = speed;
        speed = 0;
    }

    private void FixedUpdate() {
        if (isAI) {
            RecordDistance();
        }
    }
    private void RecordDistance() {
        totalDistance += Vector3.Distance(transform.position, previousLocation);
        previousLocation = transform.position;
    }

    public void OnPowerupCollected(Powerup.Type type) {
        for (int i = 0; i <= powerupPickupData.Count; i++) {
            if (i == powerupPickupData.Count) {
                powerupPickupData.Add(new PowerupPickupData(type));
                break;
            }
            else if (powerupPickupData[i].type == type) {
                powerupPickupData[i].count += 1;
                break;
            }
        }
    }

    public void BuffSpeedTimed(double amount, double duration) {
        coroutines.Add(StartCoroutine(IncreaseSpeed(amount, duration)));
    }

    private IEnumerator IncreaseSpeed(double amount, double duration) {
        var modifiedSpeed = amount * originalSpeed;
        Speed += modifiedSpeed;
        yield return new WaitForSeconds((float)duration);
        Speed -= modifiedSpeed;
    }

    public void ResetUnit(ResetReason resetReason) {
        for (int i = 0; i < coroutines.Count; i++) {
            StopCoroutine(coroutines[i]);
        }
        coroutines.Clear();
        Speed = originalSpeed;
        Score = 0;
        transform.position = gameManager.GetRandomSpawnLocation();
        onReset?.Invoke(this, resetReason);
        
    }

    internal void Won() {
        winCount++;
        onRoundEnd?.Invoke(true);
    }

    internal void Lost() {
        lossCount++;
        onRoundEnd?.Invoke(false);
    }


}
[Serializable]
public class PowerupPickupData
{
    public Powerup.Type type;

    public int count;

    public PowerupPickupData(Powerup.Type type) {
        this.type = type;
        count = 1;
    }

    public PowerupPickupData(Powerup.Type type, int count) {
        this.type = type;
        this.count = count;
    }
}