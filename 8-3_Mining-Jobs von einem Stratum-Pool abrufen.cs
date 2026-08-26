using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;

// --- Verbindung zum Solo-Pool public-pool.io (kein Account nötig) ---
string poolHost = "public-pool.io";
int    poolPort = 21496;               // Alternativ-Port: 3333

using TcpClient tcp = new TcpClient();

// Verbindungsversuch mit 10-Sekunden-Timeout, damit eine blockierte
// Verbindung als klarer Fehler auftaucht statt als endloser Hänger
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await tcp.ConnectAsync(poolHost, poolPort, cts.Token);
Console.WriteLine($"Verbunden mit {poolHost}:{poolPort}");

using var reader = new StreamReader(tcp.GetStream());
using var writer = new StreamWriter(tcp.GetStream()) { AutoFlush = true };

// --- Phase 1: Subscribe ---
string subscribe =
    "{\"id\": 1, \"method\": \"mining.subscribe\"," +
    " \"params\": [\"BitcoinBuch/1.0\"]}";
await writer.WriteLineAsync(subscribe);
Console.WriteLine("Subscribe: " + (await reader.ReadLineAsync() ?? ""));

// --- Phase 2: Authorize ---
// Username = eigene Mining-Adresse + Worker-Name (wie in Kapitel 11),
// Passwort ist bei Solo-Pools beliebig ("x").
string adresse = "bc1q02l3j44tmnhqk9tfw3eh09h34z93a4ez94hpdc";
string authorize =
    "{\"id\": 2, \"method\": \"mining.authorize\"," +
    " \"params\": [\"" + adresse + ".worker1\", \"x\"]}";
await writer.WriteLineAsync(authorize);

// --- Phase 3: Alle Pool-Nachrichten mitlesen, bis das erste
//     echte Arbeitspaket (mining.notify) kommt ---
while (true)
{
    string zeile = await reader.ReadLineAsync() ?? "";
    if (zeile.Length == 0) continue;
    Console.WriteLine("<< " + zeile);

    if (zeile.Contains("mining.notify"))
    {
        using JsonDocument doc = JsonDocument.Parse(zeile);
        JsonElement param = doc.RootElement.GetProperty("params");

        string jobId    = param[0].GetString()!;
        string prevHash = param[1].GetString()!;
        string nbits    = param[6].GetString()!;
        string ntime    = param[7].GetString()!;

        Console.WriteLine($"\nJob-ID:   {jobId}");
        Console.WriteLine($"PrevHash: {prevHash}");
        Console.WriteLine($"nBits:    {nbits}");
        Console.WriteLine($"nTime:    {ntime}");
        break;
    }
}
