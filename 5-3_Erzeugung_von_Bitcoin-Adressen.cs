using System;
using NBitcoin;

// Privaten und öffentlichen Schlüssel erzeugen
Key privateKey = new Key();
PubKey publicKey = privateKey.PubKey;

// Privaten Schlüssel als WIF darstellen
BitcoinSecret wif =
    privateKey.GetWif(Network.Main);

// Native SegWit-Adresse (bc1q) ableiten
BitcoinAddress bech32Adresse =
    publicKey.GetAddress(
        ScriptPubKeyType.Segwit,
        Network.Main);

// Ergebnisse ausgeben
Console.WriteLine("Privater Schlüssel: " +
    privateKey.ToHex());

Console.WriteLine("WIF: " + wif);

Console.WriteLine("Öffentlicher Schlüssel: " +
    publicKey.ToHex());

Console.WriteLine("Native-SegWit-Adresse: " +
    bech32Adresse);
