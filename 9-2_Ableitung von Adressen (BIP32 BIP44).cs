using System;
using NBitcoin;

// Mnemonic aus dem Testvektor, hier als abandon-Beispiel
string worte =
    "abandon abandon abandon abandon abandon abandon " +
    "abandon abandon abandon abandon abandon about";
Mnemonic bestehend = new Mnemonic(worte, Wordlist.English);

// Seed aus dem Mnemonic ableiten (optionale Passphrase, hier leer)
byte[] seed = bestehend.DeriveSeed();

Console.WriteLine("Seed (64 Byte):");
Console.WriteLine(Convert.ToHexString(seed).ToLower());

// Master Key aus dem Mnemonic ableiten
ExtKey masterKey = bestehend.DeriveExtKey();
 
Console.WriteLine("Master Private Key:");
Console.WriteLine(masterKey.PrivateKey.ToHex());
 
// Als xprv (Base58Check, beginnt mit 'xprv')
Console.WriteLine("xprv: " + masterKey.GetWif(Network.Main));
