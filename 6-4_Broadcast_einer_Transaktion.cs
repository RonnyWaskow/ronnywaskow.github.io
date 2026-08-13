using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NBitcoin;

// ============================================================
//  Testnet4-Broadcast: von der eigenen Adresse bis ins Netz
// ============================================================

// Wir arbeiten AUSSCHLIESSLICH auf dem Testnet4. Dort sind
// Coins wertlos und kostenlos über einen Faucet erhältlich.
Network network = Network.TestNet4;

// Basis-URL der öffentlichen mempool.space-API für Testnet4.
const string apiBasis = "https://mempool.space/testnet4/api";

// ------------------------------------------------------------
// Schritt 1: Schlüssel laden oder (beim ersten Start) erzeugen
// ------------------------------------------------------------
// Tragen Sie hier nach dem ersten Lauf Ihren WIF-Schlüssel ein.
// Bleibt das Feld leer, erzeugt das Programm einen neuen Schlüssel,
// gibt WIF und Adresse aus und beendet sich. Sie schicken dann
// über einen Faucet Testcoins an die angezeigte Adresse.
const string meinWif = "";   // z.B. "cV..." (Testnet-WIF)

Key meineKey;

if (string.IsNullOrWhiteSpace(meinWif))
{
    meineKey = new Key();
    BitcoinAddress neueAdresse =
        meineKey.PubKey.GetAddress(ScriptPubKeyType.Segwit, network);

    Console.WriteLine("Neuen Schlüssel erzeugt. Bitte im Code eintragen:");
    Console.WriteLine("  WIF:     " + meineKey.GetWif(network));
    Console.WriteLine("  Adresse: " + neueAdresse);
    Console.WriteLine();
    Console.WriteLine("Schicken Sie Testnet4-Coins an diese Adresse");
    Console.WriteLine("(Testnet4-Faucet) und starten Sie danach erneut.");
    return;
}

// Vorhandenen Schlüssel aus dem WIF-String laden.
meineKey = new BitcoinSecret(meinWif, network).PrivateKey;
BitcoinAddress meineAdresse =
    meineKey.PubKey.GetAddress(ScriptPubKeyType.Segwit, network);

Console.WriteLine("Eigene Adresse: " + meineAdresse);

// Zieladresse für den Testversand. Voreingestellt: zurück an die
// eigene Adresse, damit Sie das Ergebnis leicht wiederfinden.
BitcoinAddress empfaenger = meineAdresse;

using HttpClient http = new HttpClient();

// ------------------------------------------------------------
// Schritt 2: Verfügbare UTXOs der eigenen Adresse abfragen
// ------------------------------------------------------------
string utxoJson = await http.GetStringAsync(
    apiBasis + "/address/" + meineAdresse + "/utxo");

using JsonDocument doc = JsonDocument.Parse(utxoJson);

List<Coin> coins = new List<Coin>();
Money summe = Money.Zero;

foreach (JsonElement u in doc.RootElement.EnumerateArray())
{
    uint256 txHash = uint256.Parse(u.GetProperty("txid").GetString());
    uint vout = (uint)u.GetProperty("vout").GetInt32();
    Money betrag = Money.Satoshis(u.GetProperty("value").GetInt64());

    // Bei SegWit genügen OutPoint, Betrag und ScriptPubKey der Adresse;
    // die vollständige Vorgänger-Transaktion wird nicht benötigt.
    coins.Add(new Coin(txHash, vout, betrag, meineAdresse.ScriptPubKey));
    summe += betrag;
}

if (coins.Count == 0)
{
    Console.WriteLine("Keine Guthaben gefunden. Bitte zuerst über");
    Console.WriteLine("einen Faucet Testnet4-Coins an die Adresse senden.");
    return;
}

Console.WriteLine($"Gefunden: {coins.Count} UTXO(s), zusammen {summe}");

// ------------------------------------------------------------
// Schritt 3: Aktuelle Gebührenrate abfragen
// ------------------------------------------------------------
// mempool.space liefert Empfehlungen in Satoshi pro virtuellem Byte.
long satProVByte = 1;   // sicherer Vorgabewert
try
{
    string feeJson = await http.GetStringAsync(
        apiBasis + "/v1/fees/recommended");
    using JsonDocument feeDoc = JsonDocument.Parse(feeJson);
    satProVByte = feeDoc.RootElement.GetProperty("minimumFee").GetInt64();
    if (satProVByte < 1) satProVByte = 1;
}
catch
{
    // Fällt auf den Vorgabewert zurück, falls die Abfrage scheitert.
}

FeeRate gebuehr = new FeeRate(Money.Satoshis(satProVByte), 1);
Console.WriteLine($"Gebührenrate: {satProVByte} sat/vByte");

// ------------------------------------------------------------
// Schritt 4: Transaktion bauen und signieren
// ------------------------------------------------------------
// SendAll + SubtractFees leert die Adresse an den Empfänger und
// zieht die Gebühr vom Betrag ab. So passt der Versand immer,
// unabhängig davon, wie viel der Faucet geschickt hat.
TransactionBuilder builder = network.CreateTransactionBuilder();

Transaction tx = builder
    .AddKeys(meineKey)
    .AddCoins(coins.ToArray())
    .SendAll(empfaenger)
    .SubtractFees()
    .SendEstimatedFees(gebuehr)
    .BuildTransaction(true);

// Transaktion lokal prüfen, bevor sie das Haus verlässt.
if (!builder.Verify(tx, out var fehlerListe))
{
    foreach (var f in fehlerListe)
        Console.WriteLine("Prüffehler: " + f);
    return;
}

Console.WriteLine("TXID (erwartet): " + tx.GetHash());

// ------------------------------------------------------------
// Schritt 5: Transaktion ans Netzwerk senden (Broadcast)
// ------------------------------------------------------------
string txHex = tx.ToHex();
var inhalt = new StringContent(txHex, Encoding.UTF8, "text/plain");
HttpResponseMessage antwort = await http.PostAsync(apiBasis + "/tx", inhalt);

string antwortText = await antwort.Content.ReadAsStringAsync();

if (antwort.IsSuccessStatusCode)
{
    Console.WriteLine("Broadcast erfolgreich!");
    Console.WriteLine("TXID: " + antwortText);
    Console.WriteLine("Ansehen: " +
        "https://mempool.space/testnet4/tx/" + antwortText);
}
else
{
    Console.WriteLine("Broadcast fehlgeschlagen: " + antwortText);
}
