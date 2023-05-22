using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    public string name;
    public Type type;

    [DontCreateProperty]
    public bool isUsed = false;

    public double lifeTime = 5;

    public abstract bool Pickup(Collider other);

    private void Start() {
        
    }

    private void Update() {
        lifeTime-=Time.deltaTime;
        if(lifeTime < 0 && !isUsed) {
            Remove();
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (Pickup(other)) {
            Remove();
        }
    }


    protected void Remove() {
        Destroy(gameObject);
    }
    

    public enum Type {
        Bad,
        Good,
        Speed
    }

}
