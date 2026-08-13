using System;
using NBitcoin;
 
// Den Genesis-Block abrufen
Block genesis = Network.Main.GetGenesis();
BlockHeader header = genesis.Header;
 
// Die sechs Felder des Headers ausgeben
Console.WriteLine("Version:     " + header.Version);
Console.WriteLine("PrevHash:    " + header.HashPrevBlock);
Console.WriteLine("MerkleRoot:  " + header.HashMerkleRoot);
Console.WriteLine("Zeit:        " + header.BlockTime.UtcDateTime);
Console.WriteLine("Bits:        " + header.Bits);
Console.WriteLine("Nonce:       " + header.Nonce);
Console.WriteLine("BlockHash:   " + header.GetHash());
