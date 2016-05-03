using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public interface ITextureManager
{
    event Action preparingComplete;
    Texture GetTexture(TextureSize size, TextureColor color);
    void StartPrepareTextures();
    void ClearAllTextures();
}

public class TextureManager : MonoBehaviour, ITextureManager
{
    private Dictionary<ResourceKey, Texture> cache = new Dictionary<ResourceKey, Texture>();
    private const int BASE_SIZE = 32;

    public List<Texture> list = new List<Texture>();

    public void Awake()
    {
        StartPrepareTextures();
    }

    public event Action preparingComplete;

    public Texture GetTexture(TextureSize size, TextureColor color)
    {
        var key = new ResourceKey(size, color);

        return cache.ContainsKey(key) ? cache[key] : null;
    }

    public void StartPrepareTextures()
    {
        StartCoroutine(Prepare());
    }

    private IEnumerator Prepare()
    {
        foreach (TextureSize size in Enum.GetValues(typeof(TextureSize)))
        {
            foreach (TextureColor color in Enum.GetValues(typeof(TextureColor)))
            {
                var key = new ResourceKey(size, color);
                Texture texture = null;

                yield return StartCoroutine(CreateTexture(key, (t) =>
                {
                    texture = t;
                }));

                if (!cache.ContainsKey(key))
                {
                    cache.Add(key, texture);
                }
                else
                {
                    Destroy(texture);
                    texture = null;
                }
            }
        }
        yield return null;

        Debug.Log("prepared");

        if (preparingComplete != null)
        {
            preparingComplete();
        }
    }

    private IEnumerator CreateTexture(ResourceKey key, Action<Texture> callback)
    {
        var side = BASE_SIZE * (int)key.size;

        var texture = new Texture2D(side, side, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Trilinear,
            anisoLevel = 20
        };

        Color32[] colors = null;

        var t = new ThreadedAction(() =>
        {
            colors = new Color32[side * side];

            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    colors[j + i * side] = Color32.Lerp(Color.white, GetActualColor(key.color), (float)j / side);
                }
            }

        });

        yield return t.WaitForComplete();

        texture.SetPixels32(colors, 0);
        texture.Apply(false, true);

        callback(texture);
    }

    private Color GetActualColor(TextureColor color)
    {
        switch (color)
        {
            case TextureColor.Red:
                return Color.red;
            case TextureColor.Greed:
                return Color.green;
            case TextureColor.Blue:
                return Color.blue;
            default:
                throw new ArgumentOutOfRangeException("color", color, null);
        }
    }

    public void ClearAllTextures()
    {
        foreach (var texture in cache)
        {
            DestroyImmediate(texture.Value);
        }

        cache.Clear();
    }

    private class ResourceKey : IEquatable<ResourceKey>
    {
        public readonly TextureSize size;
        public readonly TextureColor color;

        public ResourceKey(TextureSize size, TextureColor color)
        {
            this.size = size;
            this.color = color;
        }

        public override int GetHashCode()
        {
            return (int)color + (int)size;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ResourceKey);
        }

        public bool Equals(ResourceKey other)
        {
            return color == other.color && size == other.size;
        }
    }
}


public enum TextureSize
{
    Small = 1,
    Normal = 2,
    Big = 4,
    Biggest = 8
}

public enum TextureColor
{
    Red = 0,
    Greed,
    Blue
}

public class ThreadedAction
{
    public ThreadedAction(Action action)
    {
        var thread = new Thread(() =>
        {
            if (action != null)
                action();
            _isDone = true;
        });
        thread.Start();
    }

    public IEnumerator WaitForComplete()
    {
        while (!_isDone)
            yield return null;
    }

    private bool _isDone;
}