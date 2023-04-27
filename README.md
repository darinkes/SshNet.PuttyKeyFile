SshNet.PuttyKeyFile
=============
[SSH.NET](https://github.com/sshnet/SSH.NET) Extension to read and use Authentication Keys in PuTTY-Format

[![License](https://img.shields.io/github/license/darinkes/SshNet.PuttyKeyFile)](https://github.com/darinkes/SshNet.PuttyKeyFile/blob/main/LICENSE)

![CodeQL](https://github.com/darinkes/SshNet.PuttyKeyFile/workflows/CodeQL/badge.svg)
![.NET-Ubuntu](https://github.com/darinkes/SshNet.PuttyKeyFile/workflows/.NET-Ubuntu/badge.svg)
![.NET-Windows](https://github.com/darinkes/SshNet.PuttyKeyFile/workflows/.NET-Windows/badge.svg)

## Status
WIP

Supports PPK v2 and v3

## .NET Frameworks

* .NET 4.0
* .NET 4.6.2
* netstandard 2.0

#### .NET 4.0 Note ####

PPK v3 encryption is not supported on .NET 4.0

## Keys
* ssh-ed25519
* ecdsa-sha2-nistp256
* ecdsa-sha2-nistp384
* ecdsa-sha2-nistp521
* ssh-rsa with 2048, 3072, 4096 or 8192 KeyLength

## Key Encryption
* None
* AES256-cbc

## Usage Example

```cs
var key = new PuttyKeyFile("my-key.ppk");

using var client = new SshClient("ssh.foo.com", "root", key);
client.Connect();
Console.WriteLine(client.RunCommand("hostname").Result);
```
