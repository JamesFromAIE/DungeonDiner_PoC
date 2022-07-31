using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsManager : Singleton<DropsManager>
{
    public void DropLoot(Vector3 position, List<DropProbability> drops)
    {
        if (drops.Count <= 0) return;

        List<ScriptableDrops> weightedDrops = new List<ScriptableDrops>();

        foreach (DropProbability drop in drops)
        {
            for (int i = 0; i < drop.Weight; i++)
            {
                weightedDrops.Add(drop.Drop);
            }
        }

        int randAmount = Random.Range(0, 3);

        for (int i = 0; i < randAmount; i++)
        {
            int randIndex = Random.Range(0, weightedDrops.Count);

            ScriptableDrops randDrop = weightedDrops[randIndex];

            EnemyDrop newDrop = DropsPool.Instance.GetDropObject();

            newDrop.Init(randDrop, position);
        }
    }
}
