using System;
using NBitcoin;

// Genesis-Block bereitstellen (wie im ersten Codeblock von 7.1)
Block genesis = Network.Main.GetGenesis();

// Proof-of-Work des Genesis-Blocks prüfen
bool gueltig = genesis.Header.CheckProofOfWork();
Console.WriteLine("PoW gueltig: " + gueltig); // True

// Den Hash mit dem Target vergleichen (manuell)
uint256 blockHash = genesis.Header.GetHash();
uint256 zielwert  = genesis.Header.Bits.ToUInt256();

Console.WriteLine("Hash:     " + blockHash);
Console.WriteLine("Zielwert: " + zielwert);
Console.WriteLine("Hash <= Zielwert: " + (blockHash <= zielwert));
