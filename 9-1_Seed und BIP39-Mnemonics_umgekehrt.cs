using System;
using NBitcoin;

string worte = "monitor pulse believe draw age water carpet camp invite defy sight permit";

Mnemonic bestehend = new Mnemonic(worte, Wordlist.English);
Console.WriteLine("Gültig: " + bestehend.IsValidChecksum);
