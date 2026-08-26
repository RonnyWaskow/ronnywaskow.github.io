using System;
using System.Net.Http;
using System.Text.Json;
using NBitcoin;

// Erste Empfangsadresse auf Testnet4 (tb1q...)
BitcoinAddress adresse = masterKey
    .Derive(KeyPath.Parse("m/84'/0'/0'/0/0"))
    .PrivateKey.GetAddress(ScriptPubKeyType.Segwit, Network.TestNet4);

Console.WriteLine("Adresse: " + adresse);

// Adresse auf Testnet4 abfragen
string url = $"https://mempool.space/testnet4/api/address/{adresse}";

using HttpClient http = new HttpClient();
string json = await http.GetStringAsync(url);
using JsonDocument doc = JsonDocument.Parse(json);

JsonElement chain = doc.RootElement.GetProperty("chain_stats");
long empfangen  = chain.GetProperty("funded_txo_sum").GetInt64();
long ausgegeben = chain.GetProperty("spent_txo_sum").GetInt64();
long guthaben   = empfangen - ausgegeben;   // das echte Guthaben

Console.WriteLine($"Empfangen:  {empfangen} sat");
Console.WriteLine($"Ausgegeben: {ausgegeben} sat");
Console.WriteLine($"Guthaben:   {guthaben} sat " +
    $"({guthaben / 100_000_000.0:F8} BTC)");
