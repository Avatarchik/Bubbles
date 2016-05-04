using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private readonly List<Ball> bubbleshCache = new List<Ball>();

    [SerializeField]
    private Transform spawnArea;

    private GameObject prefab;

    [Inject]
    private CommandHadler commandHadler;
    [Inject]
    private ITextureManager textureManager;
    [Inject]
    private Menu menu;

    private IEnumerator Start()
    {
        yield return StartCoroutine(LoadPrefabFromAssetBundle());
    }

    public bool HasActive
    {
        get
        {
            return bubbleshCache.Any(b => b.ActiveSelf);
        }
    }

    public void SpawnBubble()
    {
        if (prefab == null)
        {
            return;
        }
        var paramsSpawn = GetRandomBubbleParams();
        commandHadler.PostCommand(new SpawnCommand(paramsSpawn));
    }

    private SpawnParams GetRandomBubbleParams()
    {
        var spawnParams = new SpawnParams();

        spawnParams.size = Utils.GetRandomEnum<BallSize>();
        spawnParams.actualSize = (int)spawnParams.size * 16;

        var spawnSize = spawnArea.localScale.x / 2f;

        var xRandom = Random.Range(
            spawnArea.position.x - spawnSize + spawnParams.actualSize / 2f,
            spawnArea.position.x + spawnSize - spawnParams.actualSize / 2f);

        spawnParams.posX = xRandom;
        spawnParams.posY = spawnArea.position.y + spawnParams.actualSize / 2f;
        spawnParams.color = Utils.GetRandomEnum<BallColor>();

        spawnParams.id = Guid.NewGuid().ToString();

        return spawnParams;
    }

    public void Spawn(SpawnParams spawnParams)
    {
        Ball ball;

        if (bubbleshCache.Any(b => !b.ActiveSelf))
        {
            ball = bubbleshCache.First(b => !b.ActiveSelf);
        }
        else
        {
            var go = (GameObject)Instantiate(prefab);
            ball = go.GetComponent<Ball>();
            bubbleshCache.Add(ball);
        }

        ball.gameObject.SetActive(true);
        ball.name = spawnParams.id;
        ball.Init(spawnParams, textureManager,menu, commandHadler);
    }

    public IEnumerator LoadPrefabFromAssetBundle()
    {
        var bundleUrl = string.Format("file:///{0}/{1}", Application.streamingAssetsPath, "resources");

        using (WWW www = new WWW(bundleUrl))
        {
            yield return www;

            if (www.error != null)
                throw new Exception("WWW download had an error:" + www.error);

            AssetBundle bundle = www.assetBundle;
            prefab = (GameObject)bundle.LoadAsset("ball");
            bundle.Unload(false);
        }

    }
}