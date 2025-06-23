using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class ItemSpawner : SingletonMono<ItemSpawner>
    {
        [SerializeField] private List<GameObject> itemPrefabs;
        [SerializeField] private Transform itemHolder;
        private Dictionary<ItemID, ObjectPooler<ItemController>> itemPoolers;

        protected override void Awake()
        {
            InitializePooller();
        }

        private void InitializePooller()
        {
            itemPoolers = new();

            foreach (var itemPrefab in itemPrefabs)
            {
                var itemController = itemPrefab.GetComponent<ItemController>();
                itemPoolers.Add(itemController.Type, new ObjectPooler<ItemController>(itemController, itemHolder, 10));
            }

        }

        public void SpawnItem(ItemID type, Vector3 position)
        {
            if (!itemPoolers.TryGetValue(type, out var pooler)) return;

            var itemSpawned = pooler.Get(position);

            itemSpawned.ReleaseCallback += LevelManager.Instance.RemoveItem;
            LevelManager.Instance.AddItem(itemSpawned.gameObject);
        }
    }
}