using System.Collections.Generic;
using Unity.MLAgents;
using Unity;
using System;
using UnityEngine;

[Serializable]
public class RoundData
{
    public int index;
    public float time;
    public List<PowerupPickupData> pickupDatas = new List<PowerupPickupData>();
    public List<int> scoreDatas = new List<int>();
    public List<Unit> units = new List<Unit>();
    public float totalDistance = 0;
    public float winScore;

    public List<PowerupPickupData> avgPickupDatas = new List<PowerupPickupData>();

    public RoundData(int index, float time, float winScore) {
        this.index = index;
        this.time = time;
        this.winScore = winScore;
    }

    internal void Record() {
        var statsRecorder = Academy.Instance.StatsRecorder;

        float avgScore = 0;
        for (int i = 0; i < scoreDatas.Count; i++) {
            avgScore += scoreDatas[i];
        }

        for (int i = 0; i < pickupDatas.Count; i++) {
            var data = pickupDatas[i];
            bool exists = false;
            foreach (var avgData in avgPickupDatas) {
                if (avgData.type == data.type) {
                    avgData.count += data.count;
                    exists = true;
                }
            }
            if (!exists) {
                var newAvgData = new PowerupPickupData(data.type, data.count);
                avgPickupDatas.Add(newAvgData);
            }
        }
        float totalPicked = 0;
        foreach (var avgData in avgPickupDatas) {
            totalPicked += avgData.count;
        }
        avgScore /= (units.Count * winScore);
        float avgVelocity = totalDistance / (units.Count * time);

        statsRecorder.Add("Game/Round_TimePerScore", (float)Math.Round(time)/winScore);
     
        statsRecorder.Add("Game/Average_Score", (float)avgScore);
        foreach (var pData in avgPickupDatas) {
            statsRecorder.Add("Powerup/WeightedCount_"+pData.type, pData.count / totalPicked);
        }
        statsRecorder.Add("Game/Average_Velocity", avgVelocity);


    }
}