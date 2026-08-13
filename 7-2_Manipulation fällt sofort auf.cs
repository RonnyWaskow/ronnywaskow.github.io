using System;
using NBitcoin;

// Genesis-Block bereitstellen (wie im ersten Codeblock von 7.1)
Block genesis = Network.Main.GetGenesis();

// Echte Wurzel merken, dann absichtlich fälschen
uint256 echteWurzel = genesis.Header.HashMerkleRoot;
genesis.Header.HashMerkleRoot = uint256.Zero;   // gefälscht

Console.WriteLine("MerkleRoot ok (gefälscht): " + genesis.CheckMerkleRoot());
// Ausgabe: False

// Ursprünglichen Zustand wiederherstellen
genesis.Header.HashMerkleRoot = echteWurzel;

Console.WriteLine("MerkleRoot ok (echt):      " + genesis.CheckMerkleRoot());
