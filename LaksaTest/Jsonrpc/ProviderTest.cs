using LaksaCsharp.BlockChain;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LaksaTest.Jsonrpc
{
    [TestClass]
    public class ProviderTest
    {
        [TestMethod]
        public void GetNetworkId()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String networkId = client.GetNetworkId().Result;
            Assert.AreEqual("1", networkId);
        }

        [TestMethod]
        public void GetDSBlockListing()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BlockList blockList = client.GetDSBlockListing(1).Result;
            Assert.IsNotNull(blockList);
        }

        [TestMethod]
        public void GetTxBlockListing()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BlockList blockList = client.GetTxBlockListing(1).Result;
            Assert.IsNotNull(blockList);
        }

        [TestMethod]
        public void GetBlockchainInfo()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BlockchainInfo blockchainInfo = client.GetBlockchainInfo().Result;
            Assert.IsNotNull(blockchainInfo);
        }


        [TestMethod]
        public void GetDsBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            DsBlock dsBlock = client.GetDsBlock("1").Result;
            Assert.IsNotNull(dsBlock);
            Assert.IsTrue(dsBlock.Header.Difficulty == 3);
        }


        [TestMethod]
        public void GetNumDSBlocks()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String Result = client.GetNumDSBlocks().Result;
            Assert.IsNotNull(Result);
        }


        [TestMethod]
        public void GetTxBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            TxBlock txBlock = client.GetTxBlock("40").Result;
            Assert.IsNotNull(txBlock);
            Assert.AreEqual(3, txBlock.Body.MicroBlockInfos.Count());
        }

        [TestMethod]
        public void GetLatestDsBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            DsBlock dsBlock = client.GetLatestDsBlock().Result;
            Assert.IsNotNull(dsBlock);
        }

        [TestMethod]
        public void GetLatestTxBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            TxBlock txBlock = client.GetLatestTxBlock().Result;
            Assert.IsNotNull(txBlock);
        }

        [TestMethod]
        public void GetBalance()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BalanceResult balance = client.GetBalance("E9C49CAF0D0BC9D7C769391E8BDA2028F824CF3D".ToLower()).Result;
            Assert.IsNotNull(balance.Balance);
        }

        [TestMethod]
        public void GetSmartContractCode()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String code = client.GetSmartContractCode("8cb841ef4f1f61d44271e167557e160434bd6d63").Result.Code;
            Console.WriteLine(code);
        }

        [TestMethod]
        public void GetMinimumGasPrice()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String price = client.GetMinimumGasPrice().Result;
            Console.WriteLine(price);

        }


        [TestMethod]
        public void GetTransaction()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            Transaction transaction = client.GetTransaction("0e8d4d5cc5f5a7747fdb004e625da02f177208a93728f72f679ae55e0ba5bc70").Result;
            Console.WriteLine(transaction);
        }

        [TestMethod]
        public void GetRecentTransactions()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            TransactionList transactionList = client.GetRecentTransactions().Result;
            Console.WriteLine(transactionList);
        }

        [TestMethod]
        public void GetSmartContractState()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            List<State> stateList = client.GetSmartContractState("D6606D02DFF929593312D8D0D36105E376F95AA0").Result;
            Console.WriteLine(stateList);
        }

        [TestMethod]
        public void GetNumTxnsTxEpoch()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String lastEpoch = client.GetNumTxnsTxEpoch().Result;
            List<State> lastStateList = client.GetSmartContractState("D6606D02DFF929593312D8D0D36105E376F95AA0").Result;

            Console.WriteLine("last epoch is " + lastEpoch);
            Console.WriteLine("last state:" + lastStateList);

            int n = 0;

            while (true)
            {
                String epoch = client.GetNumTxnsTxEpoch().Result;
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
