using System;
using NBitcoin;
using NBitcoin.DataEncoders;

// --- Aus Block 1: definiert 'genesis' ---
Block genesis = Network.Main.GetGenesis();

// --- Block 2: eigenen Blockheader bauen und serialisieren ---
BlockHeader meinHeader = Network.Main.Consensus
    .ConsensusFactory.CreateBlockHeader();

meinHeader.Version        = 1;
meinHeader.HashPrevBlock  = genesis.GetHash();
meinHeader.HashMerkleRoot = uint256.Zero;
meinHeader.BlockTime      = DateTimeOffset.UtcNow;
meinHeader.Bits           = genesis.Header.Bits;
meinHeader.Nonce          = 0;

byte[] serialisiert = meinHeader.ToBytes();
Console.WriteLine("Länge: " + serialisiert.Length + " Byte");
Console.WriteLine("Hex:   " + Encoders.Hex.EncodeData(serialisiert));

// --- Gegenprobe: Round-Trip ---
BlockHeader gelesen = Network.Main.Consensus.ConsensusFactory.CreateBlockHeader();
gelesen.FromBytes(serialisiert);
Console.WriteLine("Round-Trip identisch: " +
    (gelesen.GetHash() == meinHeader.GetHash()));
