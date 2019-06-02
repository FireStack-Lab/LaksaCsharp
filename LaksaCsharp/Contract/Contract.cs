using Google.Protobuf.WellKnownTypes;
using LaksaCsharp.Account;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Contract
{
    public class Contract
    {
        public static string NIL_ADDRESS = "0x0000000000000000000000000000000000000000";

        private ContractFactory contractFactory;
        private Values[] init;
        private string abi;
        private List<LaksaCsharp.BlockChain.State> state;
        private string address;
        private string code;
        private ContractStatus contractStatus;

        private Wallet signer;
        private HttpProvider provider;

        public Contract(ContractFactory factory, string code, string abi, string address, Values[] init, List<LaksaCsharp.BlockChain.State> state)
        {
            this.contractFactory = factory;
            this.provider = factory.Provider;
            this.signer = factory.Signer;
            if (!string.IsNullOrEmpty(address))
            {
                this.abi = abi;
                this.address = Account.Account.NormaliseAddress(address);
                this.init = init;
                this.state = state;
                this.code = code;
                this.contractStatus = ContractStatus.Deployed;
            }
            else
            {
                this.abi = abi;
                this.code = code;
                this.init = init;
                this.contractStatus = ContractStatus.Initialised;
            }
        }

        public Tuple<Transaction.Transaction, Contract> Deploy(DeployParams param, int attempts, int interval)
        {
            if (string.IsNullOrEmpty(code) || init == null || init.Count() <= 0)
            {
                throw new Exception("Cannot deploy without code or initialisation parameters.");
            }
            Transaction.Transaction transaction = new Transaction.Transaction();
            transaction.ID = param.ID;
            transaction.Version = param.Version;
            transaction.Nonce = param.Nonce;
            transaction.GasPrice = param.GasPrice;
            transaction.GasLimit = param.GasLimit;
            transaction.SenderPubKey = param.SenderPubKey;
            transaction.ToAddr = NIL_ADDRESS;
            transaction.Amount = "0";
            transaction.Code = this.code.Replace("/\\", "");
            transaction.Data = JsonConvert.SerializeObject(this.init).Replace("/\\", "");
            transaction.Provider = this.provider;
            transaction = this.PrepareTx(transaction, attempts, interval);
            if (transaction.IsRejected())
            {
                this.contractStatus = ContractStatus.Rejected;
                return Tuple.Create<Transaction.Transaction, Contract>(transaction, this);
            }

            this.contractStatus = ContractStatus.Deployed;
            this.address = ContractFactory.GetAddressForContract(transaction);
            return Tuple.Create<Transaction.Transaction, Contract>(transaction, this);
        }
        private class Data
        {
            [JsonProperty("_tag")]
            public string Tag { get; set; }
            [JsonProperty("params")]
            public Values[] Params { get; set; }
        }

        public Transaction.Transaction Call(string transition, Values[] args, CallParams param, int attempts, int interval)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("Contract has not been deployed!");
            }

            Transaction.Transaction transaction = new Transaction.Transaction();
            transaction.ID = param.ID;
            transaction.Version = param.Version;
            transaction.Nonce = param.Nonce;
            transaction.GasPrice = param.GasPrice;
            transaction.GasLimit = param.GasLimit;
            transaction.SenderPubKey = param.SenderPubKey;
            transaction.ToAddr = NIL_ADDRESS;
            transaction.Amount = "0";
            transaction.Code = this.code.Replace("/\\", "");
            transaction.Data = JsonConvert.SerializeObject(new Data() { Tag = transition, Params = args });
            transaction.Provider = this.provider;
            return this.PrepareTx(transaction, attempts, interval);

        }


        public Transaction.Transaction PrepareTx(Transaction.Transaction tx, int attempts, int interval)
        {
            tx = signer.Sign(tx);
            try
            {
                CreateTxResult createTxResult = provider.CreateTransaction(tx.ToTransactionPayload()).Result;
                tx.Confirm(createTxResult.TranID, attempts, interval);
            }
            catch (IOException e)
            {
                tx.Status = TxStatus.Rejected;
            }
            catch (Exception e)
            {
            }
            return tx;
        }

        public bool IsInitialised()
        {
            return ContractStatus.Initialised == this.contractStatus;
        }

        public bool IsDeployed()
        {
            return ContractStatus.Deployed == this.contractStatus;
        }

        public bool IsRejected()
        {
            return ContractStatus.Rejected == this.contractStatus;
        }

        public List<LaksaCsharp.BlockChain.State> GetState()
        {

            if (ContractStatus.Deployed != this.contractStatus)
            {
                new List<LaksaCsharp.BlockChain.State>();
            }

            try
            {
                return this.provider.GetSmartContractState(this.address.ToLower().Replace("0x", "")).Result;
            }
            catch (IOException e)
            {
            }

            return state;
        }
    }
}
