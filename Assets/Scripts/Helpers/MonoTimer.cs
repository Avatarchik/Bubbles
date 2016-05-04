using System;
using UnityEngine;
using System.Collections;

public class MonoTimer : MonoBehaviour
{
    public event Action onTick;

    public static MonoTimer timer;
    private float t = 0f;

    public static MonoTimer Timer
    {
        get
        {
            if (timer != null)
            {
                return timer;
            }

            var go = new GameObject("MonoTimer");
            DontDestroyOnLoad(go);
            timer = go.AddComponent<MonoTimer>();
            return timer;
        }
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t >= 1)
        {
            t = 0;
            if (onTick != null)
            {
                onTick();
            }
        }
    }
}
