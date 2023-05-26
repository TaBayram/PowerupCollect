using System.Collections.Generic;
using Unity.MLAgents;
using Unity;
using System;
using UnityEngine;

[Serializable]
public class RoundData
{
    public int index;
    public Unit winner;
    public float time;
    public Dictionary<string, List<PowerupPickupData>> pickupDatas;
    public Dictionary<string, int> scoreDatas;
    public List<Unit> units = new List<Unit>();
    public float totalDistance = 0;
    public float winScore;

    public RoundData(int index, Unit winner, float time, float winScore) {
        this.index = index;
        this.winner = winner;
        this.time = time;
        this.winScore = winScore;
        pickupDatas = new Dictionary<string, List<PowerupPickupData>>();
        scoreDatas = new Dictionary<string, int>();
    }


    public string Print() {
        string text = "Round " + index + ". Time " + Math.Round(time) + ". Winner "+ winner.playerName;

        foreach (var unit in units) {
            text += "\n\n Player " + unit.playerName + ". Score: " + scoreDatas[unit.playerName];
            text += "\n Wins " + unit.winCount + " Losses " + unit.lossCount;
            foreach (var pData in pickupDatas[unit.playerName]) {
                text += "\n Powerup " + pData.type + " amount " + pData.count;
            }
        }

        return text;

    }

    internal void Record() {
        var statsRecorder = Academy.Instance.StatsRecorder;

        float avgScore = 0;
        List<PowerupPickupData> avgPickupDatas = new List<PowerupPickupData>();
        foreach (var unit in units) {
            avgScore += scoreDatas[unit.playerName];
            foreach (var pData in pickupDatas[unit.playerName]) {
                bool exists = false;
                foreach (var avgData in avgPickupDatas) {
                    if(avgData.type == pData.type) {
                        avgData.count += pData.count;
                        exists = true;
                        break;
                    }
                }
                if(!exists) {
                    avgPickupDatas.Add(pData);
                }
            }
        }
        avgScore /= (units.Count * winScore);
        float avgVelocity = totalDistance / (units.Count * time);

        //statsRecorder.Add("Game/Round_HasWinner", winner ? 1 : 0);

        //statsRecorder.Add("Game/Round_TimeUsedRatio", time/maxTime);

        if (winner) {
            statsRecorder.Add("Game/Round_TimePerScore", (float)Math.Round(time)/winScore);

        }
        statsRecorder.Add("Game/Average_Score", (float)avgScore);
        foreach (var pData in avgPickupDatas) {
            statsRecorder.Add("Powerup/AvgCount_"+pData.type, pData.count / units.Count);
        }
        if (winner) {
            foreach(var pData in pickupDatas[winner.playerName]) {
                statsRecorder.Add("Powerup/WinnerCount_" + pData.type, pData.count);
            }
        }
        statsRecorder.Add("Game/Average_Velocity", avgVelocity);


    }
}