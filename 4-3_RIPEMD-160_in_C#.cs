using System;
using System.Text;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;

string eingabe = "Bitcoin";
byte[] daten = Encoding.UTF8.GetBytes(eingabe);

// Hash160 = RIPEMD160(SHA256(daten))
byte[] hash160 = Hashes.Hash160RawBytes(daten);

string hex = Convert.ToHexString(hash160).ToLower();
Console.WriteLine(hex);
