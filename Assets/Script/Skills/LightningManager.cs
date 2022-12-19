using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    public static LightningManager Instance { get; private set; }

    public Dictionary<int, int> Lightning { get; private set; }
    public float BasicDamage;
    public float AdditionalDamage;

    private void Awake() {
        Instance = this;
        Lightning = new Dictionary<int, int>();
    }

    public void HitOn(Transform obj) {
        int hash = obj.GetHashCode();
        if (Lightning.ContainsKey(hash)) {
            Lightning[hash]++;
        } else {
            Lightning.Add(hash, 1);
        }
        DataManager.Instance.takedamage(obj.transform, BasicDamage + (Lightning[hash] - 1) * AdditionalDamage);
    }
}
