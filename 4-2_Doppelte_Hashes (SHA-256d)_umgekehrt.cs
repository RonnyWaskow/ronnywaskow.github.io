using System;
using System.Text;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;

string eingabe = "Bitcoin";
byte[] daten = Encoding.UTF8.GetBytes(eingabe);

// Variante A: Ergebnis als rohe Hashbytes
byte[] roh = Hashes.DoubleSHA256RawBytes(daten);
string rohHex = Encoders.Hex.EncodeData(roh);

// Variante B: Ergebnis als NBitcoin-Typ uint256
uint256 hash = Hashes.DoubleSHA256(daten);

Console.WriteLine("RawBytes: " + rohHex);
Console.WriteLine("uint256:  " + hash);
