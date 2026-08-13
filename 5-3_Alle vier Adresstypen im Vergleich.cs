using System;
using NBitcoin;

Key privateKey = new Key();
 
Console.WriteLine("Legacy   (1...): " +
    privateKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main));
 
Console.WriteLine("P2SH-SW  (3...): " +
    privateKey.GetAddress(ScriptPubKeyType.SegwitP2SH, Network.Main));
 
Console.WriteLine("Bech32  (bc1q): " +
    privateKey.GetAddress(ScriptPubKeyType.Segwit, Network.Main));
 
Console.WriteLine("Taproot (bc1p): " +
    privateKey.GetAddress(ScriptPubKeyType.TaprootBIP86, Network.Main));
