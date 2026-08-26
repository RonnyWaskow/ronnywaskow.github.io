using System;
using NBitcoin;

// 12 zufällige Wörter erzeugen (128 Bit Entropie)
Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);

Console.WriteLine("Mnemonic:");
Console.WriteLine(string.Join(" ", mnemonic.Words));
Console.WriteLine();
Console.WriteLine("Prüfsumme gültig: " + mnemonic.IsValidChecksum);
