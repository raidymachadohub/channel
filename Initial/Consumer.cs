using System.Threading.Channels;

namespace Initial;

public class Consumer
{
    private readonly Channel<string> _channel;

    public Consumer(Channel<string> channel)
    {
        _channel = channel;
    }

    public async void CallMultiThread()
    {
        await foreach (var guid in _channel.Reader.ReadAllAsync())
        {
            Console.WriteLine(guid);
        }
    }
}