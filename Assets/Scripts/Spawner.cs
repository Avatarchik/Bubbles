using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnArea;
    [SerializeField]
    private TextureManager textureManager;
    [SerializeField]
    private Menu menu;

    private GameObject prefab;

    private float timeSpawn = 1f;

    private float timer = 0f;

    private int spawnCount = 1;
    private int currentLevel = 1;

    private readonly List<Ball> bubbleshCache = new List<Ball>();

    private bool Inited = false;

    private IEnumerator Start()
    {
        yield return StartCoroutine(LoadPrefabFromAssetBundle());

        Ball.textureManager = textureManager;
        Ball.menu = menu;

        menu.playGame += () => { Inited = true; };
        menu.connectGame += (s) => { Inited = true; };
    }

    private IEnumerator LoadPrefabFromAssetBundle()
    {
        var bundleUrl = string.Format("file:///{0}/{1}", Application.dataPath, "resources");

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

    private void Update()
    {
        if (!Inited)
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
                SpawnBubble();
            }
        }
    }

    private IEnumerator StartNewWave()
    {
        while (bubbleshCache.Any(b => b.gameObject.activeSelf))
        {
            yield return null;
        }

        textureManager.ClearAllTextures();
        textureManager.StartPrepareTextures();
        textureManager.preparingComplete += Instance_preparingComplete;
    }

    private void Instance_preparingComplete()
    {
        SpawnBubble();
    }

    private void SpawnBubble()
    {
        var paramsSpawn = GetRandomBubbleParams();

        CommandHadler.Instance.PostCommand(new SpawnCommand(this, paramsSpawn));
    }

    private SpawnParams GetRandomBubbleParams()
    {
        var spawnParams = new SpawnParams();

        spawnParams.size = Utils.GetRandomEnum<TextureSize>();
        spawnParams.actualSize = (int)spawnParams.size * 16;

        var spawnSize = spawnArea.localScale.x / 2f;

        var xRandom = Random.Range(spawnArea.position.x - spawnSize + spawnParams.actualSize / 2f,
            spawnArea.position.x + spawnSize - spawnParams.actualSize / 2f);
        spawnParams.pos = new Vector3(xRandom, spawnArea.position.y + spawnParams.actualSize / 2f, 0f);

        return spawnParams;
    }

    public void Spawn(SpawnParams spawnParams)
    {
        Ball ball;

        if (bubbleshCache.Any(b => !b.gameObject.activeSelf))
        {
            ball = bubbleshCache.First(b => !b.gameObject.activeSelf);
        }
        else
        {
            var go = GameObject.Instantiate(prefab);
            ball = go.GetComponent<Ball>();

            bubbleshCache.Add(ball);
        }

        ball.gameObject.SetActive(true);
        ball.Init(spawnParams);

        spawnCount++;
    }
}

public class SpawnParams
{
    public int actualSize;
    public Vector3 pos;
    public TextureSize size;
}
