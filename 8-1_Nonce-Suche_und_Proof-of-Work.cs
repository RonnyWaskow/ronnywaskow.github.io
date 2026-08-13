using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using NBitcoin;

// Kandidaten-Header aus dem Genesis-Block aufbauen
Block genesis    = Network.Main.GetGenesis();
BlockHeader tmpl = genesis.Header;

// 80-Byte Puffer einmalig serialisieren
byte[] header = tmpl.ToBytes();

// Ziel: Hash-Reversed muss mit 2 Nullbytes beginnen
// Das prüfen wir direkt auf den Raw-Bytes (letzten 2 Byte des SHA256d-Output)
const int zielNulls = 2;

Console.WriteLine("Mining-Simulation startet...");
var uhr = Stopwatch.StartNew();
long versuche = 0;

for (uint nonce = 0; nonce <= uint.MaxValue; nonce++)
{
    // Nonce als Little-Endian in den Puffer schreiben
    header[76] = (byte) nonce;
    header[77] = (byte)(nonce >>  8);
    header[78] = (byte)(nonce >> 16);
    header[79] = (byte)(nonce >> 24);

    // SHA-256d = SHA-256(SHA-256(header))
    byte[] hash = SHA256.HashData(SHA256.HashData(header));
    versuche++;

    // Prüfen: letzte N Bytes des Raw-Hash sind 0
    // (= erste N Bytes der angezeigten, reversed Hash)
    bool gefunden = true;
    for (int i = 0; i < zielNulls; i++)
        if (hash[31 - i] != 0) { gefunden = false; break; }

    if (gefunden)
    {
        uhr.Stop();
        // Für die Anzeige: Bytes umkehren (Bitcoin-Konvention)
string hashHex =         Convert.ToHexString(hash.Reverse().ToArray()).ToLower();
        Console.WriteLine($"Block gefunden!");
        Console.WriteLine($"Nonce:    {nonce:N0}");
        Console.WriteLine($"Hash:     {hashHex}");
        Console.WriteLine($"Versuche: {versuche:N0}");
        Console.WriteLine($"Zeit:     {uhr.ElapsedMilliseconds} ms");
        break;
    }
}
