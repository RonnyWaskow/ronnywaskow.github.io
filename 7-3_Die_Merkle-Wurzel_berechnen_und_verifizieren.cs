using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using NBitcoin;
using NBitcoin.Crypto;

// --- Einen echten Block mit mehreren Transaktionen laden ---
// (roh von mempool.space, dann von NBitcoin geparst)
int hoehe = 100000;
using HttpClient http = new HttpClient();

string blockHash = (await http.GetStringAsync(
    "https://mempool.space/api/block-height/" + hoehe)).Trim();
byte[] rohBlock = await http.GetByteArrayAsync(
    "https://mempool.space/api/block/" + blockHash + "/raw");
Block block = Block.Load(rohBlock, Network.Main);

// TXIDs sammeln und Wurzel selbst berechnen
var txids = block.Transactions.Select(tx => tx.GetHash()).ToList();
uint256 manuell = BerechneMerkleRoot(txids);

// --- Drei Wege zur selben Wurzel vergleichen ---
Console.WriteLine($"Block {hoehe} mit {txids.Count} Transaktionen");
Console.WriteLine("NBitcoin:  " + block.GetMerkleRoot().Hash);   // eingebaut
Console.WriteLine("Manuell:   " + manuell);                      // selbst berechnet
Console.WriteLine("Im Header: " + block.Header.HashMerkleRoot);  // aus dem Block

// --- Selbsttest: gerade, ungerade und mehrere Ebenen absichern ---
Console.WriteLine("\nSelbsttest 1..7 Blätter (manuell == NBitcoin):");
for (int n = 1; n <= 7; n++)
{
    var blaetter = Enumerable.Range(0, n)
        .Select(i => Hashes.DoubleSHA256(Encoding.UTF8.GetBytes("blatt" + i)))
        .ToList();

    bool ok = BerechneMerkleRoot(blaetter) == MerkleNode.GetRoot(blaetter).Hash;
    Console.WriteLine($"  n={n}: {(ok ? "OK" : "FEHLER")}");
}

// --- Hilfsmethoden ---

// Zwei Hashes zu einem Elternhash kombinieren
static uint256 KnotenHash(uint256 links, uint256 rechts)
{
    byte[] daten = links.ToBytes().Concat(rechts.ToBytes()).ToArray();
    return Hashes.DoubleSHA256(daten);
}

// Merkle-Wurzel aus einer Liste von TXIDs berechnen
static uint256 BerechneMerkleRoot(List<uint256> txids)
{
    if (txids.Count == 0) return uint256.Zero;

    var ebene = new List<uint256>(txids);
    while (ebene.Count > 1)
    {
        // Ungerade Anzahl: letzten Knoten duplizieren
        if (ebene.Count % 2 == 1)
            ebene.Add(ebene[^1]);

        var naechste = new List<uint256>();
        for (int i = 0; i < ebene.Count; i += 2)
            naechste.Add(KnotenHash(ebene[i], ebene[i + 1]));

        ebene = naechste;
    }
    return ebene[0];
}
