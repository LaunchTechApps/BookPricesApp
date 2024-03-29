using System;
namespace BookPricesApp.GUI.Utils;

public static class MessageBoxQueue
{
    private static Queue<string> _queue = new();
    public static void Add(string message)
    {
        _queue.Enqueue(message);
        showMessages();
    }

    private static bool _running = false;
    private static void showMessages()
    {
        if (_running) 
        { 
            return; 
        }

        _running = true;

        while (_queue.Count != 0)
        {
            var message = _queue.Dequeue();
            MessageBox.Show(message);
        }

        _running = false;
    }
}
