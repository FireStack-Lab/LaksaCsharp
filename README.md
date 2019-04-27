# LaksaCsharp
LaksaCsharp -- Zilliqa Blockchain C# Library 

The project is still under development.

## Quick Start

More docs can be found in https://apidocs.zilliqa.com/

### Generate large amount of addresses

```c#
using LaksaCsharp.Crypto;
using LaksaCsharp.Utils;
using Org.BouncyCastle.Math;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            while (n < 100)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine("generate nth keypair:");
                ECKeyPair keyPair = KeyTools.GenerateKeyPair();
                BigInteger privateInteger = keyPair.PrivateKey;
                BigInteger publicInteger = keyPair.PublicKey;
                Console.WriteLine("private key is: " + ByteUtil.ByteArrayToHexString(privateInteger.ToByteArray()));
                Console.WriteLine("public key is: " + ByteUtil.ByteArrayToHexString(publicInteger.ToByteArray()));
                Console.WriteLine("address is: " + KeyTools.GetAddressFromPublicKey(ByteUtil.ByteArrayToHexString(publicInteger.toByteArray())));
            }
        }
    }
}

```

### Validate an address

```c#
using LaksaCsharp.Utils;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "2624B9EA4B1CD740630F6BF2FEA82AAC0067070B";
            bool isAddress = Validation.IsAddress(address);
            Console.WriteLine("is address: " + isAddress);
        }
    }
}
```

### Validate checksum address 

```c#
using LaksaCsharp.Utils;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string checksumAddress = "0x4BAF5faDA8e5Db92C3d3242618c5B47133AE003C";
            bool isChecksumAddress = Validation.IsValidChecksumAddress(checksumAddress);
            Console.WriteLine("is checksum address: " + isChecksumAddress);
        }
    }
}
```

### Transaction operation (include construct transaction, calculate transaction fee, do serialization, sign a transaction, broadcast)

```c#
using LaksaCsharp.Account;
using LaksaCsharp.Contract;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using LaksaCsharp.Utils;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Wallet wallet = new Wallet();
            string ptivateKey = "e19d05c5452598e24caad4a0d85a49146f7be089515c905ae6a19e8a578a6930";
            // Populate the wallet with an account
            string address = wallet.AddByPrivateKey(ptivateKey);
            Console.WriteLine("address is: " + address);

            HttpProvider provider = new HttpProvider("https://api.zilliqa.com");
            //get balance
            BalanceResult balanceResult = provider.GetBalance(address).Result;
            Console.WriteLine("balance is: " + balanceResult.Balance);

            //construct non-contract transaction
            Transaction transaction = new Transaction();

            transaction.Version = Wallet.Pack(333, 8).ToString();
            transaction.ToAddr = "4baf5fada8e5db92c3d3242618c5b47133ae003c";
            transaction.SenderPubKey = "0246e7178dc8253201101e18fd6f6eb9972451d121fc57aa2a06dd5c111e58dc6a";
            transaction.Amount = "10000000";
            transaction.GasPrice = "1000000000";
            transaction.GasLimit = "1";
            transaction.Code = "";
            transaction.Data = "";
            transaction.Provider = new HttpProvider("https://dev-api.zilliqa.com/");

            //sign transaction
            transaction = wallet.Sign(transaction);
            Console.WriteLine("signature is: " + transaction.Signature);

            //broadcast transaction
            CreateTxResult result = TransactionFactory.CreateTransaction(transaction);
            Console.WriteLine(result);

            //hello-world contract
            String code = "scilla_version 0\n" +
                    "\n" +
                    "    (* HelloWorld contract *)\n" +
                    "\n" +
                    "    import ListUtils\n" +
                    "\n" +
                    "    (***************************************************)\n" +
                    "    (*               Associated library                *)\n" +
                    "    (***************************************************)\n" +
                    "    library HelloWorld\n" +
                    "\n" +
                    "    let one_msg =\n" +
                    "      fun (msg : Message) =>\n" +
                    "      let nil_msg = Nil {Message} in\n" +
                    "      Cons {Message} msg nil_msg\n" +
                    "\n" +
                    "    let not_owner_code = Int32 1\n" +
                    "    let set_hello_code = Int32 2\n" +
                    "\n" +
                    "    (***************************************************)\n" +
                    "    (*             The contract definition             *)\n" +
                    "    (***************************************************)\n" +
                    "\n" +
                    "    contract HelloWorld\n" +
                    "    (owner: ByStr20)\n" +
                    "\n" +
                    "    field welcome_msg : String = \"\"\n" +
                    "\n" +
                    "    transition setHello (msg : String)\n" +
                    "      is_owner = builtin eq owner _sender;\n" +
                    "      match is_owner with\n" +
                    "      | False =>\n" +
                    "        msg = {_tag : \"TransactionOperation\"; _recipient : _sender; _amount : Uint128 0; code : not_owner_code};\n" +
                    "        msgs = one_msg msg;\n" +
                    "        send msgs\n" +
                    "      | True =>\n" +
                    "        welcome_msg := msg;\n" +
                    "        msg = {_tag : \"TransactionOperation\"; _recipient : _sender; _amount : Uint128 0; code : set_hello_code};\n" +
                    "        msgs = one_msg msg;\n" +
                    "        send msgs\n" +
                    "      end\n" +
                    "    end\n" +
                    "\n" +
                    "\n" +
                    "    transition getHello ()\n" +
                    "        r <- welcome_msg;\n" +
                    "        e = {_eventname: \"getHello()\"; msg: r};\n" +
                    "        event e\n" +
                    "    end";
            List<Values> init =
                new List<Values>() {
                    new Values() { Type="Uint32", Value="0" , VName ="_scilla_version"  },
                    new Values() { Type="ByStr20", Value="0x9bfec715a6bd658fcb62b0f8cc9bfa2ade71434a" , VName ="owner"  },
                };
            ContractFactory factory = new ContractFactory()
            {
                Provider = new HttpProvider("https://api.zilliqa.com/"),
                Signer = wallet
            };

            Contract contract = factory.NewContract(code, (Values[])init.ToArray(), "");
            DeployParams deployParams = new DeployParams()
            {

                GasLimit = "10000",
                GasPrice = "1000000000",
                SenderPubKey = "0246e7178dc8253201101e18fd6f6eb9972451d121fc57aa2a06dd5c111e58dc6a",
                Version = Wallet.Pack(2, 8).ToString()
            };

            //deploy contract, this will take a while to track transaction util it been confirmed or failed
            Tuple<Transaction, Contract> deployResult = contract.Deploy(deployParams, 300, 3);
            Console.WriteLine("result is: " + deployResult);
            Console.WriteLine("cumulative gas is: " + deployResult.Item1.Receipt.Cumulative_gas);

            //calculate transaction fee
            string transactionFee = new BigInteger(deployResult.Item1.Receipt.Cumulative_gas).Multiply(new BigInteger(deployResult.Item1.GasPrice)).ToString();
            Console.WriteLine("transaction fee is: " + transactionFee);
        }
    }
}

```

### Know a smart contract deposit

```c#
using LaksaCsharp.BlockChain;
using LaksaCsharp.Jsonrpc;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            string lastEpoch = client.GetNumTxnsTxEpoch().Result;
            List<State> lastStateList = client.GetSmartContractState("D6606D02DFF929593312D8D0D36105E376F95AA0").Result;

            Console.WriteLine("last epoch is " + lastEpoch);
            Console.WriteLine("last state:" + lastStateList);

            int n = 0;

            while (true)
            {
                string epoch = client.GetNumTxnsTxEpoch().Result;
                Console.WriteLine(n + "th current epoch is: " + epoch);
                if (lastEpoch != epoch)
                {
                    Console.WriteLine("epoch hash changed");
                    List<State> stateList = client.GetSmartContractState("D6606D02DFF929593312D8D0D36105E376F95AA0").Result;
                    Console.WriteLine("last state: " + lastStateList);
                    Console.WriteLine("current state: " + stateList);
                    lastEpoch = epoch;
                    lastStateList = stateList;
                }
                Thread.Sleep(3000);
                n += 1;
            }
        }
    }
}

```



### Account API

- [x] FromFile
- [x] ToFile

### Wallet API

- [x] CreateAccount
- [x] AddByPrivateKey addByKeyStore
- [x] Remove
- [x] SetDefault
- [x] SignTransaction (default account)
- [x] SignTransactionWith (specific account)

### TransactionFactory Transaction

- [x] SendTransaction
- [x] TrackTx
- [x] Confirm
- [x] IsPending isInitialised isConfirmed isRejected

### ContractFactory Contract

- [x] Deploy
- [x] Call
- [x] IsInitialised isDeployed isRejected
- [x] GetState
- [x] GetAddressForContract


### Crypto API

- [x] GetDerivedKey (PBKDF2 and Scrypt)
- [x] GeneratePrivateKey
- [x] Schnorr.sign
- [x] Schnorr.verify
- [x] GetPublicKeyFromPrivateKey
- [x] GetPublicKeyFromPrivateKey
- [x] GetAddressFromPublicKey
- [x] GetAddressFromPrivateKey
- [x] EncryptPrivateKey
- [x] DecryptPrivateKey

### JSON-RPC API

#### Blockchain-related methods

- [x] GetNetworkId
- [x] GetBlockchainInfo
- [x] GetShardingStructure
- [x] GetDsBlock
- [x] GetLatestDsBlock
- [x] GetNumDSBlocks
- [x] GetDSBlockRate
- [x] GetDSBlockListing
- [x] GetTxBlock
- [x] GetLatestTxBlock
- [x] GetNumTxBlocks
- [x] GetTxBlockRate
- [x] GetTxBlockListing
- [x] GetNumTransactions
- [x] GetTransactionRate
- [x] GetCurrentMiniEpoch
- [x] GetCurrentDSEpoch
- [x] GetPrevDifficulty
- [x] GetPrevDSDifficulty

#### Transaction-related methods

- [x] CreateTransaction
- [x] GetTransaction
- [x] GetRecentTransactions
- [x] GetTransactionsForTxBlock
- [x] GetNumTxnsTxEpoch
- [x] GetNumTxnsDSEpoch
- [x] GetMinimumGasPrice

#### Contract-related methods

- [x] GetSmartContractCode
- [x] GetSmartContractInit
- [x] GetSmartContractState
- [x] GetSmartContracts
- [x] GetContractAddressFromTransactionID

#### Account-related methods

- [x] GetBalance

### Validation

- [x] IsAddress
- [x] IsPublicjKey
- [x] IsPrivateKey
- [x] IsSignature

### Util

- [x] ByteArrayToHexString
- [x] HexStringToByteArray
- [x] GenerateMac
- [x] IsByteString
- [x] EncodeTransactionProto
- [x] ToChecksumAddress
- [x] IsValidChecksumAddress






