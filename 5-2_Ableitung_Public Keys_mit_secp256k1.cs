using System;
using NBitcoin;

// Neuen privaten Schlüssel erzeugen
Key privateKey = new Key();

// Öffentlichen Schlüssel ableiten
PubKey publicKey = privateKey.PubKey;

// Privaten Schlüssel als WIF darstellen
BitcoinSecret wif =
    privateKey.GetWif(Network.Main);

// Schlüsselpaar ausgeben
Console.WriteLine("Privater Schlüssel (Hex):");
Console.WriteLine(privateKey.ToHex());
Console.WriteLine();

Console.WriteLine("Privater Schlüssel (WIF):");
Console.WriteLine(wif);
Console.WriteLine();

Console.WriteLine("Öffentlicher Schlüssel (Hex):");
Console.WriteLine(publicKey.ToHex());
