using System;
using NBitcoin;

Network network = Network.Main;

// Eigener privater Schlüssel
Key meineKey = new Key();

// Eigene Native-SegWit-Adresse
BitcoinAddress meineAdresse =
    meineKey.PubKey.GetAddress(
        ScriptPubKeyType.Segwit,
        network);

// Empfängeradresse
BitcoinAddress empfaenger =
    BitcoinAddress.Create(
        "bc1qar0srrr7xfkvy5l643lydnw9re59gtzzwf5mdq",
        network);

// TXID der Transaktion, die den UTXO erzeugt hat
uint256 vorherigerTxHash =
    uint256.Parse(
        "e9dc8fece9f4a4c7a9c5e6a4b3d2e1f0" +
        "a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6");

// Position des Outputs in der Vorgängertransaktion
uint outputIndex = 0;

// Wert des verfügbaren UTXOs
Money utxoBetrag = Money.Coins(0.001m);

// Einlösebedingung des UTXOs
Script scriptPubKey = meineAdresse.ScriptPubKey;

// UTXO als Coin darstellen
Coin coin = new Coin(
    new OutPoint(vorherigerTxHash, outputIndex),
    new TxOut(utxoBetrag, scriptPubKey));

// TransactionBuilder für das Mainnet erzeugen
TransactionBuilder builder =
    network.CreateTransactionBuilder();

// Transaktion aufbauen und signieren
Transaction tx = builder
    .AddKeys(meineKey)
    .AddCoins(coin)
    .Send(
        empfaenger,
        Money.Coins(0.0008m))
    .SetChange(meineAdresse)
    .SendFees(Money.Coins(0.00005m))
    .BuildTransaction(true);

// Transaktion lokal überprüfen
if (!builder.Verify(tx, out var fehler))
{
    foreach (var f in fehler)
    {
        Console.WriteLine("Fehler: " + f);
    }
}
else
{
    Console.WriteLine("Transaktion gültig!");
}

// Transaktions-ID ausgeben
Console.WriteLine("TXID: " + tx.GetHash());

// Serialisierte Transaktion ausgeben
Console.WriteLine("Hex:  " + tx.ToHex());
