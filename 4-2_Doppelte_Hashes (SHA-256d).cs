using System;
using System.Security.Cryptography;
using System.Text;
using NBitcoin.DataEncoders;

string eingabe = "Bitcoin";

// Text in UTF-8-Bytes umwandeln
byte[] daten = Encoding.UTF8.GetBytes(eingabe);

// Erster SHA-256-Durchlauf
byte[] einfach = SHA256.HashData(daten);

// Zweiter SHA-256-Durchlauf
byte[] doppelt = SHA256.HashData(einfach);

// Ergebnis als Hexadezimalzeichenfolge anzeigen
string hex = Encoders.Hex.EncodeData(doppelt);

Console.WriteLine(hex);
