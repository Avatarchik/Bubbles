using UnityEngine;
using System.Collections;
using System;

public class Ball : MonoBehaviour
{
    public static ITextureManager textureManager;
    public static Menu menu;

    private float speed = 30f;
    private TextureSize size;
    private MeshRenderer cachedRenderer;

    private void Awake()
    {
        cachedRenderer = GetComponent<MeshRenderer>();
    }

    public void Init(TextureSize size, int actualSize, Vector3 pos)
    {
        transform.position = pos;
        transform.localScale = Vector3.one * actualSize;

        this.size = size;

        speed = 300f / (int)size;
        var texture = textureManager.GetTexture(size, Utils.GetRandomEnum<TextureColor>());
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
        gameObject.SetActive(false);
    }
}