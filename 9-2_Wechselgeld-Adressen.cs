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

// BIP84-Pfad für die externen Adressen des ersten Kontos
var pfad84 = KeyPath.Parse("m/84'/0'/0'/0");

for (int i = 0; i < 3; i++)
{
    ExtKey kindKey = masterKey.Derive(pfad84).Derive((uint)i);
    BitcoinAddress adresse = kindKey.PrivateKey.GetAddress(
        ScriptPubKeyType.Segwit, Network.Main);

    Console.WriteLine($"Adresse {i}: {adresse}");
}

// Externe Adresse (empfangen): change = 0
ExtKey extern0 = masterKey.Derive(KeyPath.Parse("m/84'/0'/0'/0/0"));

// Interne Adresse (Wechselgeld): change = 1
ExtKey intern0 = masterKey.Derive(KeyPath.Parse("m/84'/0'/0'/1/0"));

Console.WriteLine("Empfangen:   " + extern0.PrivateKey.GetAddress(
    ScriptPubKeyType.Segwit, Network.Main));
Console.WriteLine("Wechselgeld: " + intern0.PrivateKey.GetAddress(
    ScriptPubKeyType.Segwit, Network.Main));
