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

    public int scoreToWin = 20;
    public int round = 0;
    public List<RoundData> roundDatas = new List<RoundData>();

    public float roundStartTime = 0;


    public float FloorY { get => enviroment.transform.position.y; }

    private void Start() {
        units = enviroment.GetComponentsInChildren<Unit>().ToList();
        foreach (var unit in units) {
            unit.onScoreChange += onUnitScoreChange;
        }
    }

    public Vector3 GetRandomSpawnLocation() {
        return spawnLocations[Random.Range(0, spawnLocations.Count)].position;
    }

    public void onUnitScoreChange(Unit unit) {
        if(unit.Score >= scoreToWin) {
            EndRound(unit);
        }
    }

    public void ResetRound() {
        foreach (var unit in units) {
            unit.ResetUnit();
        }
        roundStartTime = Time.time;
        round++;
    }

    public void EndRound(Unit winner) {
        var data = new RoundData(round, winner, Time.time - roundStartTime);
        winner.Won();
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
        data.Record(scoreToWin);

        ResetRound();

        
    }


}
