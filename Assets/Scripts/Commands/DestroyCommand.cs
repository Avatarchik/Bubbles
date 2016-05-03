using UnityEngine;
using System.Collections;

public class DestroyCommand : Command
{
    private GameObject g;

    public DestroyCommand(GameObject g)
    {
        this.g = g;
    }

    public override void Execute()
    {
        g.SetActive(false);
    }
}

