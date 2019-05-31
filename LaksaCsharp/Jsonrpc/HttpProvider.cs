using LaksaCsharp.BlockChain;
using LaksaCsharp.Transaction;
using LaksaCsharp.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Jsonrpc
{
    public class HttpProvider
    {
        private string url;

        public HttpProvider(string url)
        {
            this.url = url;
        }

        private Rep<R> Send<R, P>(string method, P para)
        {
            Req<P> req = new Req<P>();
            req.Id = "1";
            req.Jsonrpc = "2.0";
            req.Method = method;
            if (para != null)
            {
                req.Params = new P[] { para };
            }
            else
            {
                req.Params = new P[] { };
            }

            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            request.JsonSerializer = new JsonSerialize();
            request.AddJsonBody(req);
            IRestResponse response = client.Post(request);
            Rep<R> rep = JsonConvert.DeserializeObject<Rep<R>>(response.Content);

            return rep;
        }

        public Rep<string> GetNetworkId()
        {
            return Send<string, string>("GetNetworkId", "");
        }

        public Rep<BlockchainInfo> GetBlockchainInfo()
        {
            return Send<BlockchainInfo, string>("GetBlockchainInfo", "");
        }

        public Rep<ShardingStructure> GetShardingStructure()
        {
            return Send<ShardingStructure, string>("GetShardingStructure", "");
        }


        public Rep<BlockList> GetDSBlockListing(int pageNumber)
        {
            return Send<BlockList, int>("DSBlockListing", pageNumber);
        }


        public Rep<BlockList> GetTxBlockListing(int pageNumber)
        {
            return Send<BlockList, int>("TxBlockListing", pageNumber);
        }


        public Rep<string> GetNumDSBlocks()
        {
            return Send<string, string>("GetNumDSBlocks", "");
        }

        public Rep<double> GetDSBlockRate()
        {
            return Send<double, string>("GetDSBlockRate", "");
        }

        public Rep<BlockList> GetDSBlockListing()
        {
            return Send<BlockList, object>("DSBlockListing", 1);
        }

        public Rep<DsBlock> GetDsBlock(string blockNumber)
        {
            return Send<DsBlock, string>("GetDsBlock", blockNumber);
        }

        public Rep<TxBlock> GetTxBlock(string blockNumber)
        {
            return Send<TxBlock, string>("GetTxBlock", blockNumber);
        }

        public Rep<string> GetNumTxBlocks()
        {
            return Send<string, string>("GetNumTxBlocks", "");
        }

        public Rep<double> GetTxBlockRate()
        {
            return Send<double, string>("GetTxBlockRate", "");
        }


        public Rep<DsBlock> GetLatestDsBlock()
        {
            return Send<DsBlock, string>("GetLatestDsBlock", "");
        }

        public Rep<string> GetNumTransactions()
        {
            return Send<string, string>("GetNumTransactions", "");
        }

        public Rep<int> GetTransactionRate()
        {
            return Send<int, string>("GetTransactionRate", "");
        }

        public Rep<string> GetCurrentMiniEpoch()
        {
            return Send<string, string>("GetCurrentMiniEpoch", "");
        }

        public Rep<string> GetCurrentDSEpoch()
        {
            return Send<string, string>("GetCurrentDSEpoch", "");
        }

        public Rep<int> GetPrevDifficulty()
        {
            return Send<int, string>("GetPrevDifficulty", "");
        }

        public Rep<int> GetPrevDSDifficulty()
        {
            return Send<int, string>("GetPrevDSDifficulty", "");
        }

        public Rep<TxBlock> GetLatestTxBlock()
        {
            return Send<TxBlock, string>("GetLatestTxBlock", "");
        }

        //Account-related methods
        public Rep<BalanceResult> GetBalance(string address)
        {
            Rep<BalanceResult> result = Send<BalanceResult, string>("GetBalance", address);
            if (null == result.Result)
            {
                BalanceResult balanceResult = new BalanceResult();
                balanceResult.Balance = "0";
                balanceResult.Nonce = "0";
                result.Result = balanceResult;
            }

            return result;
        }

        public Rep<BalanceResult> GetBalance32(string address)
        {
            return GetBalance(Bech32.FromBech32Address(address));
        }

        //Contract-related methods todo need test
        public Rep<ContractResult> GetSmartContractCode(String address)
        {
            return Send<ContractResult, string>("GetSmartContractCode", address);
        }

        public Rep<List<Contract.Contract>> GetSmartContracts(string address)
        {
            return Send<List<Contract.Contract>, string>("GetSmartContracts", address);
        }

        public Rep<string> GetContractAddressFromTransactionID(string address)
        {
            return Send<string, string>("GetContractAddressFromTransactionID", address);
        }


        public Rep<List<LaksaCsharp.BlockChain.State>> GetSmartContractState(String address)
        {
            return Send<List<LaksaCsharp.BlockChain.State>, string>("GetSmartContractState", address);

        }

        public Rep<List<LaksaCsharp.BlockChain.State>> GetSmartContractInit(string address)
        {
            return Send<List<LaksaCsharp.BlockChain.State>, string>("GetSmartContractInit", address);
        }

        //Transaction-related methods
        public Rep<CreateTxResult> CreateTransaction(TransactionPayload payload)
        {
            return Send<CreateTxResult, TransactionPayload>("CreateTransaction", payload);
        }

        public Rep<string> GetMinimumGasPrice()
        {
            return Send<string, string>("GetMinimumGasPrice", "");
        }


        public Rep<Transaction.Transaction> GetTransaction(string hash)
        {
            return Send<Transaction.Transaction, string>("GetTransaction", hash);
        }

        public Rep<Transaction.Transaction> GetTransaction32(string hash)
        {
            Rep<Transaction.Transaction> rep = GetTransaction(hash);
            rep.Result.ToAddr = Bech32.ToBech32Address(rep.Result.ToAddr);
            return rep;
        }

        public Rep<TransactionList> GetRecentTransactions()
        {
            return Send<TransactionList, string>("GetRecentTransactions", "");
        }

        public Rep<List<List<string>>> GetTransactionsForTxBlock(string blockNum)
        {
            return Send<List<List<string>>, string>("GetTransactionsForTxBlock", blockNum);
        }

        public Rep<string> GetNumTxnsTxEpoch()
        {
            return Send<string, string>("GetNumTxnsTxEpoch", "");
        }

        public Rep<string> GetNumTxnsDSEpoch()
        {
            return Send<string, string>("GetNumTxnsDSEpoch", "");
        }
    }

}

public class BalanceResult
{
    public string Balance { get; set; }
    public string Nonce { get; set; }
}

public class ContractResult
{
    public string Code { get; set; }
}

public class CreateTxResult
{
    public string Info { get; set; }
    public string TranID { get; set; }


    public override string ToString()
    {
        return "CreateTxResult{" +
                "Info='" + Info + '\'' +
                ", TranID='" + TranID + '\'' +
                '}';
    }
}
