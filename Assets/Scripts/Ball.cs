using UnityEngine;

public class Ball : MonoBehaviour
{
    public static ITextureManager textureManager;
    public static Menu menu;

    private float speed = 30f;
    private BallSize size;

    private MeshRenderer cachedRenderer;
    private GameObject cachedGo;

    private SpawnParams spawnParams;

    public bool ActiveSelf
    {
        get { return cachedGo.activeSelf; }
    }

    private void Awake()
    {
        cachedGo = gameObject;
        cachedRenderer = GetComponent<MeshRenderer>();
    }

    public void Init(SpawnParams spawnParams)
    {
        this.spawnParams = spawnParams;

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
        CommandHadler.Instance.PostCommand(new DestroyCommand(spawnParams.id));
    }
}