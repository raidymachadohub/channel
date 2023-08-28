using System.Collections.Concurrent;
using System.Threading.Channels;
using Initial;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(Channel.CreateUnbounded<string>(new UnboundedChannelOptions() { SingleWriter = true }));
builder.Services.AddSingleton(_ => new ConcurrentDictionary<string, string>());

builder.Services.AddSingleton<Producer>();
builder.Services.AddSingleton<Consumer>();

var channel = Channel.CreateUnbounded<string>();
var concurrent = new ConcurrentDictionary<string, string?>();


var producer = new Producer(channel, concurrent);
var consumer = new Consumer(channel);

producer.TwoMultiThread();
consumer.CallMultiThread();

var app = builder.Build();
app.Run();