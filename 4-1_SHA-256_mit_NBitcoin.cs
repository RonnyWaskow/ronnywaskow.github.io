using System;
using System.Text;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;

string eingabe = "Bitcoin";
byte[] daten = Encoding.UTF8.GetBytes(eingabe);

byte[] hash = Hashes.SHA256(daten);
string hex = Encoders.Hex.EncodeData(hash);

Console.WriteLine(hex);

