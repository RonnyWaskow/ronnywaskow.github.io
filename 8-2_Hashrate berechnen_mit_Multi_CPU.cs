using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;              // <-- für Interlocked
using System.Threading.Tasks;        // <-- für Parallel.For

byte[] daten = new byte[80]; // Dummy-Header
// Parallel-Mining auf allen CPU-Kernen (vereinfacht)
long gesamtHashes = 0;
var uhr = Stopwatch.StartNew();

Parallel.For(0L, 1_000_000L, i =>
{
    byte[] puffer = new byte[80]; // thread-lokaler Puffer
    BitConverter.TryWriteBytes(
        puffer.AsSpan(76), (uint)i);
    SHA256.HashData(SHA256.HashData(puffer));
    Interlocked.Increment(ref gesamtHashes);
});

Console.WriteLine($"Parallel: {gesamtHashes / uhr.Elapsed.TotalSeconds / 1e6:F1} MH/s");
