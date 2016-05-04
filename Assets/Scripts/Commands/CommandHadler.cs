using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class CommandHadler : MonoBehaviour
{
    [Inject]
    public TCPServer server;

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
        if (commands.Count <= 0)
        {
            return;
        }
        var command = commands.Dequeue();
        command.Execute();

        if (GameLogic.IsServer)
        {
            server.Send(command);
        }
    }
}
