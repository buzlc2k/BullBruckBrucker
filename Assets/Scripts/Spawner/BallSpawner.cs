using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class BallSpawner : SingletonMono<BallSpawner>
    {
        [SerializeField] private int maxBallsSpawnedPerFrame = 200;
        [SerializeField] private int maxActiveBalls = 1000;

        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform ballHolder;

        private ObjectPooler<BallController> ballPooler;

        private Queue<SpawnRequest> spawnQueue = new();
        private bool isProcessingQueue = false;
        
        private int activeBallCount = 0;

        [System.Serializable]
        public struct SpawnRequest
        {
            public Vector3 position;
            public Vector3 direction;

            public SpawnRequest(Vector3 pos, Vector3 dir)
            {
                position = pos;
                direction = dir;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            InitializePooler();
        }

        private void InitializePooler()
        {
            ballPooler = new ObjectPooler<BallController>(ballPrefab.GetComponent<BallController>(), ballHolder, 1000);
        }

        private void Update()
        {
            if (!isProcessingQueue && spawnQueue.Count > 0)
                StartCoroutine(ProcessSpawnQueue());
        }

        public void SpawnSingleBall(Vector3 position, Vector3 direction)
        {
            spawnQueue.Enqueue(new SpawnRequest(position, direction));
        }

        public void SpawnMultipleBalls(List<SpawnRequest> requests)
        {
            foreach (var request in requests)
                spawnQueue.Enqueue(request);
        }

        public void SpawnBallImmediate(Vector3 position, Vector3 direction)
        {
            SpawnBallInternal(position, direction);
        }

        private IEnumerator ProcessSpawnQueue()
        {
            isProcessingQueue = true;

            while (spawnQueue.Count > 0)
            {
                int ballsSpawnedThisFrame = 0;

                while (spawnQueue.Count > 0 && ballsSpawnedThisFrame < maxBallsSpawnedPerFrame)
                {
                    var request = spawnQueue.Dequeue();
                    SpawnBallInternal(request.position, request.direction);
                    ballsSpawnedThisFrame++;
                }

                yield return null;
            }

            isProcessingQueue = false;
        }

        private void SpawnBallInternal(Vector3 position, Vector3 direction)
        {
            if (activeBallCount >= maxActiveBalls)
                return;

            var ballSpawned = ballPooler.Get(position, direction);
            
            activeBallCount++;
            
            ballSpawned.ReleaseCallback += OnBallReleased;
            LevelManager.Instance.AddBall(ballSpawned.gameObject);
        }
        
        private void OnBallReleased(GameObject ball)
        {
            activeBallCount--;

            LevelManager.Instance.RemoveBall(ball);
        }
    }
}