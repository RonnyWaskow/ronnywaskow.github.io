using System;
using NBitcoin;

// Genesis-Block bereitstellen (wie im ersten Codeblock von 7.1)
Block genesis = Network.Main.GetGenesis();

// Beide Validierungen auf einmal:
// 1. Proof-of-Work des Headers
// 2. MerkleRoot stimmt mit Transaktionen überein
bool blockGueltig = genesis.Check();
Console.WriteLine("Block komplett gueltig: " + blockGueltig);

// Einzeln prüfen:
Console.WriteLine("PoW ok:        " + genesis.CheckProofOfWork());
Console.WriteLine("MerkleRoot ok: " + genesis.CheckMerkleRoot());
