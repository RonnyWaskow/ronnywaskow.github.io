using System;
using System.Diagnostics;
using System.Security.Cryptography;
 
byte[] daten = new byte[80]; // Dummy-Header
long   hashCount = 0;
var    uhr = Stopwatch.StartNew();
 
// 3 Sekunden lang hashen
while (uhr.Elapsed.TotalSeconds < 3.0)
{
    SHA256.HashData(SHA256.HashData(daten));
    hashCount++;
}
uhr.Stop();
 
double hashrate = hashCount / uhr.Elapsed.TotalSeconds;
Console.WriteLine($"Hashrate: {hashrate / 1_000_000:F2} MH/s");
Console.WriteLine($"Hashes:   {hashCount:N0} in {uhr.Elapsed.TotalSeconds:F1}s");
