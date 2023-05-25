using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public string gameName;
    public GameObject enviroment;
    public PowerupSpawner powerupSpawner;
    public List<Transform> spawnLocations = new List<Transform>();
    public List<Unit> units = new List<Unit>();

    public List<RoundData> roundDatas = new List<RoundData>();
    public int roundTimeLimit = 60;
    public int roundWinTimeDecrement = 10;
    public int minRoundTime = 0;
    public int scoreToWin = 20;
    public int round = 0;

    public float roundStartTime = 0;

    public bool isTraining = true;
    public bool gameStarted = false;
    public float FloorY { get => enviroment.transform.position.y; }

    private void Start() {
        units = enviroment.GetComponentsInChildren<Unit>().ToList();
        foreach (var unit in units) {
            unit.onScoreChange += onUnitScoreChange;
        }
        minRoundTime = minRoundTime == 0 ? (int)Mathf.Max(roundTimeLimit * 0.10f, 30): minRoundTime;
        StartCoroutine(StartGame());
    }

    private void Update() {
        if(roundTimeLimit != 0 && roundTimeLimit < Time.time - roundStartTime) {
            EndRound(null);
        }
    }

    private IEnumerator StartGame() {
        yield return new WaitForSeconds(1);
        StartRound();
        gameStarted = true;
    }


    public Vector3 GetRandomSpawnLocation() {
        return spawnLocations[Random.Range(0, spawnLocations.Count)].position;
    }

    public void onUnitScoreChange(Unit unit) {
        if(unit.Score >= scoreToWin) {
            EndRound(unit);
        }
    }

    public void StartRound() {
        ResetRound();
        roundStartTime = Time.time;
        round++;
    }

    public void EndRound(Unit winner) {
        var data = new RoundData(round, winner, Time.time - roundStartTime, scoreToWin, roundTimeLimit);
        winner?.Won();
        foreach (var unit in units) {
            if (winner != unit) {
                unit.Lost();
            }
            data.pickupDatas.Add(unit.playerName, new List<PowerupPickupData>(unit.powerupPickupData));
            data.scoreDatas.Add(unit.playerName, unit.Score);
            unit.powerupPickupData.Clear();
            data.totalDistance += unit.totalDistance;
            unit.totalDistance = 0;
        }
        data.units.AddRange(units);
        roundDatas.Add(data);
        //Debug.Log(gameName + "\n" + data.Print() + "\n ----------");
        if (isTraining) {
            data.Record();
        }
        if (winner != null) {
            roundTimeLimit = Mathf.Max(minRoundTime, roundTimeLimit - roundWinTimeDecrement);
        }

        StartRound();
    }

    public void ResetRound() {
        foreach (var unit in units) {
            unit.ResetUnit(ResetReason.roundStarted);
        }
    }



}
