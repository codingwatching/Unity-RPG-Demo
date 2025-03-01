using System.Collections.Generic;
using UnityEngine;
using Items;

namespace SO
{
    [CreateAssetMenu(fileName = "DropListConfig_", menuName = "Unity RPG Project/DropListConfig", order = 5)]
    public class DropListConfig : ScriptableObject
    {
        public const int MAX_DROP_COUNT = 3;
        public List<Item> dropList = new List<Item>();

        public List<Item> GetDropItems(ProfessionAttribute enemyProgression, ref int golds)
        {
            List<Item> drops = new List<Item>();
            if (dropList.Count > 0)
            {
                golds += (int)Random.Range(enemyProgression.hp * 0.5f, enemyProgression.hp * 1.5f);
                int dropCount = Random.Range(1, MAX_DROP_COUNT);
                for (int i = 0; i < dropCount; i++)
                    drops.Add(dropList[Random.Range(0, dropList.Count)]);
            }

            return drops;
        }
    }
}