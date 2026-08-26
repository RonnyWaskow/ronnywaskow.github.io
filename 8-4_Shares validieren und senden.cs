using System;
using System.Globalization;
using System.Numerics;

// --- Teil 1: Ist ein gefundener Hash ein gültiger Share? ---

// Difficulty-1-Target von Bitcoin (der Bezugspunkt aller Schwierigkeiten)
BigInteger diff1Target = BigInteger.Parse(
    "00000000FFFF0000000000000000000000000000000000000000000000000000",
    NumberStyles.HexNumber);

long poolDifficulty = 100000;                       // vom Pool (set_difficulty)
BigInteger poolTarget = diff1Target / poolDifficulty;

// Zwei Beispiel-Hashes, wie sie aus der Mining-Schleife kämen
BigInteger hashTreffer = BigInteger.Parse(
    "0000000000000012340000000000000000000000000000000000000000000000",
    NumberStyles.HexNumber);
BigInteger hashDaneben = BigInteger.Parse(
    "00000000FFFFFFFF000000000000000000000000000000000000000000000000",
    NumberStyles.HexNumber);

// Pool-Target sauber als 64 Hex-Ziffern anzeigen
string targetHex = poolTarget.ToString("x64");
targetHex = targetHex.Substring(targetHex.Length - 64);

Console.WriteLine("Pool-Schwierigkeit: " + poolDifficulty);
Console.WriteLine("Pool-Target:        " + targetHex);
Console.WriteLine("Treffer gueltig:    " + (hashTreffer <= poolTarget));
Console.WriteLine("Daneben gueltig:    " + (hashDaneben <= poolTarget));
