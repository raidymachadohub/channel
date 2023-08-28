using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Initial;

public class Producer
{
    private readonly Channel<string> _channel;
    private readonly ConcurrentDictionary<string, string?> _concurrent;

    public Producer(Channel<string> channel,
        ConcurrentDictionary<string, string?> concurrent)
    {
        _channel = channel;
        _concurrent = concurrent;
    }

    private async Task Single()
    {
        foreach (var guid in Guids())
        {
            if (_concurrent.TryAdd(guid, default))
                await _channel.Writer.WriteAsync(guid);
        }
    }
    public void TwoMultiThread()
    {
        new Thread(() => { Single().Wait(); })
        {
            IsBackground = true
        }.Start();

        new Thread(() => { Single().Wait(); })
        {
            IsBackground = true
        }.Start();
    }

    private static IEnumerable<string> Guids() =>
        Enumerable.Repeat(0, 100).Select(_ => Guid.NewGuid().ToString());
}