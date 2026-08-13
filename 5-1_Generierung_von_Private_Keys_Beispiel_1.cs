using System;
using NBitcoin;
 
// Sicherer Zufalls-Key per Betriebssystem-RNG
Key privateKey = new Key();
 
// Den rohen Schlüssel als Hex-String ausgeben (32 Byte = 64 Zeichen)
Console.WriteLine("Hex:  " + privateKey.ToHex());
 
// Im WIF-Format (Wallet Import Format, Base58Check-kodiert)
BitcoinSecret wif = privateKey.GetWif(Network.Main);
Console.WriteLine("WIF:  " + wif);
