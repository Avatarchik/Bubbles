using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandHadler : MonoBehaviour
{
    public static CommandHadler Instance
    {
        get; private set;
    }

    private Queue<Command> commands = new Queue<Command>();

    private void Awake()
    {
        Instance = this;
    }

    public void PostCommand(Command c)
    {
        commands.Enqueue(c);
    }

    private void Update()
    {
        if (commands.Count > 0)
            commands.Dequeue().Execute();
    }
}
