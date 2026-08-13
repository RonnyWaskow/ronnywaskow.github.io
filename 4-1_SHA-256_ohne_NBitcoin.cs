using System;
using System.Security.Cryptography;
using System.Text;

string eingabe = "Bitcoin";
byte[] daten = Encoding.UTF8.GetBytes(eingabe);

/* SHA-256 berechnen (statische Methode, seit .NET 5) */
byte[] hash = SHA256.HashData(daten);

// Bytes in einen Hex-String umwandeln
string hex = Convert.ToHexString(hash).ToLower();

Console.WriteLine(hex);
