using UnityEngine;
using System.Collections;

public class SpawnCommand : Command {

    private GameLogic s;
    private SpawnParams p;

    public SpawnCommand(GameLogic s,SpawnParams p)
    {
        this.s = s;
        this.p = p;
    }

    public override void Execute()
    {
        s.Spawn(p);
    }
}
