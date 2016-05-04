using System;
using System.Collections;
using System.Threading;

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