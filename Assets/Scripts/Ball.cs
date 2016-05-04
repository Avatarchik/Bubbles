using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour
{
    private Menu menu;

    private float speed = 30f;
    private BallSize size;

    private MeshRenderer cachedRenderer;
    private GameObject cachedGo;

    private SpawnParams spawnParams;
    private CommandHadler commandHadler;

    public bool ActiveSelf
    {
        get { return cachedGo.activeSelf; }
    }

    private void Awake()
    {
        cachedGo = gameObject;
        cachedRenderer = GetComponent<MeshRenderer>();
    }

    public void Init(SpawnParams spawnParams , ITextureManager textureManager, Menu menu, CommandHadler commandHadler)
    {
        this.menu = menu;
        this.spawnParams = spawnParams;
        this.commandHadler = commandHadler;

        size = spawnParams.size;

        transform.position = new Vector3(spawnParams.posX, spawnParams.posY, 0f);
        transform.localScale = Vector3.one * spawnParams.actualSize;

        speed = 300f / (int)size;
        var texture = textureManager.GetTexture(size, spawnParams.color);
        cachedRenderer.material.SetTexture("_MainTex", texture);
    }

    private void OnMouseDown()
    {
        menu.AddScore((int)size * 100);
        DestroyBall();
    }

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
    }

    public void DestroyBall()
    {
        if (!GameLogic.IsServer)
        {
            return;
        }
        commandHadler.PostCommand(new DestroyCommand(spawnParams.id));
    }
}