using System.Collections;
using UnityEngine;
using Zenject;

public class GameLogic : MonoBehaviour, IInitializable
{
    public static bool IsServer
    {
        get; private set;
    }

    [Inject]
    private ITextureManager textureManager;
    [Inject]
    private Menu menu;

    [Inject]
    private TCPClient client;
    [Inject]
    private TCPServer server;

    private float timeSpawn = 1f;
    private float timer = 0f;

    private int spawnCount = 1;
    private int currentLevel = 1;

    [Inject]
    private Spawner spawner;

    public void Initialize()
    {
        menu.playGame += () =>
        {
            IsServer = true;
            server.StartServer();
        };

        menu.connectGame += (s) =>
        {
            IsServer = false;
            client.StartConnect(s);
        };
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= timeSpawn)
        {
            Spawn();
            timer = 0f;
        }

    }

    private void Spawn()
    {
        if (spawnCount % (10 * currentLevel) == 0)
        {
            timeSpawn -= 0.1f;

            currentLevel++;

            if (timeSpawn < 0)
            {
                timeSpawn = 0.1f;
            }

            StartCoroutine(StartNewWave());

        }
        else
        {
            for (int i = 0; i < currentLevel; i++)
            {
                spawner.SpawnBubble();
                spawnCount++;
            }
        }
    }

    private IEnumerator StartNewWave()
    {
        while (spawner.HasActive)
        {
            yield return null;
        }

        textureManager.NewLevelPrepare(OnCompete);
    }

    private void OnCompete()
    {
        spawner.SpawnBubble();
    }
}