using System;
using UnityEngine;

[Serializable]
public class SpawnCommand : Command
{
    private SpawnParams p;

    public SpawnCommand(SpawnParams p)
    {
        this.p = p;
    }

    public override void Execute()
    {
        GameObject.FindObjectOfType<Spawner>().Spawn(p);
    }
}
