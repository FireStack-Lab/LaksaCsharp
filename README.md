# LaksaCsharp
LaksaCsharp -- Zilliqa Blockchain C# Library 

The project is still under development.

## Quick Start

More docs can be found in https://apidocs.zilliqa.com/


## Supports

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






