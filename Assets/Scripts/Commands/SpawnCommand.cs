using UnityEngine;
using System.Collections;

public class SpawnCommand : Command {

    private Spawner s;
    private SpawnParams p;

    public SpawnCommand(Spawner s,SpawnParams p)
    {
        this.s = s;
        this.p = p;
    }

    public override void Execute()
    {
        s.Spawn(p);
    }
}
