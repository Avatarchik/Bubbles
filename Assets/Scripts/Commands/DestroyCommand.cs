using System;
using UnityEngine;
using System.Collections;
[Serializable]
public class DestroyCommand : Command
{
    private string g;

    public DestroyCommand(string g)
    {
        this.g = g;
    }

    public override void Execute()
    {
        var go = GameObject.Find(g);

        if (go != null)
        {
            go.SetActive(false);
        }

    }
}

