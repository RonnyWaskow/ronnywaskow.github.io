using System;
using NBitcoin;

var wallet = new MiniWallet(
    "abandon abandon abandon abandon abandon abandon " +
    "abandon abandon abandon abandon abandon about",
    Network.Main);   // Network.TestNet4 für Testnet-Adressen

Console.WriteLine("Adresse 0: " + wallet.Empfangsadresse(0));
Console.WriteLine("Adresse 1: " + wallet.Empfangsadresse(1));
Console.WriteLine("Wechsel 0: " + wallet.Wechselgeldadresse(0));

// In einem Top-Level-Programm steht die Klasse am Ende der Datei.
public class MiniWallet
{
    private readonly ExtKey _masterKey;
    private readonly Network _network;

    public MiniWallet(string mnemonic, Network network,
                      string passphrase = "")
    {
        var m = new Mnemonic(mnemonic, Wordlist.English);
        _masterKey = m.DeriveExtKey(passphrase);
        _network = network;
    }

    // Externe Empfangsadresse für Index i
    public BitcoinAddress Empfangsadresse(int index = 0)
    {
        var pfad = KeyPath.Parse($"m/84'/0'/0'/0/{index}");
        return _masterKey.Derive(pfad).PrivateKey
            .GetAddress(ScriptPubKeyType.Segwit, _network);
    }

    // Wechselgeld-Adresse für Index i
    public BitcoinAddress Wechselgeldadresse(int index = 0)
    {
        var pfad = KeyPath.Parse($"m/84'/0'/0'/1/{index}");
        return _masterKey.Derive(pfad).PrivateKey
            .GetAddress(ScriptPubKeyType.Segwit, _network);
    }

    // Privaten Schlüssel für eine externe Adresse holen
    public Key PrivaterSchluessel(int index = 0)
    {
        var pfad = KeyPath.Parse($"m/84'/0'/0'/0/{index}");
        return _masterKey.Derive(pfad).PrivateKey;
    }
}
