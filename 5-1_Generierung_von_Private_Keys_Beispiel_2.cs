using System;
using NBitcoin;

// Neuen privaten Schlüssel erzeugen
Key privateKey = new Key();

// Privaten Schlüssel als Hex ausgeben
string originalHex = privateKey.ToHex();

// WIF aus dem gerade erzeugten Schlüssel erstellen
BitcoinSecret wif = privateKey.GetWif(Network.Main);
string wifString = wif.ToString();

Console.WriteLine("Neu erzeugter Schlüssel:");
Console.WriteLine("Hex: " + originalHex);
Console.WriteLine("WIF: " + wifString);
Console.WriteLine();

/*
Den privaten Schlüssel aus dem gerade erzeugten
WIF-String wiederherstellen.
*/
Key wiederhergestellt =
    Key.Parse(wifString, Network.Main);

string wiederhergestelltesHex =
    wiederhergestellt.ToHex();

Console.WriteLine("Aus diesem WIF wiederhergestellt:");
Console.WriteLine("Hex: " + wiederhergestelltesHex);
Console.WriteLine();

Console.WriteLine(
    "Schlüssel identisch: " +
    (originalHex == wiederhergestelltesHex));
