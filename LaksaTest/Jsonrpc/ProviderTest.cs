using LaksaCsharp.BlockChain;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LaksaTest.Jsonrpc
{
    [TestFixture]
    public class ProviderTest
    {
        [Test]
        public void GetNetworkId()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String networkId = client.GetNetworkId().Result;
            Assert.AreEqual("1", networkId);
        }

        [Test]
        public void GetDSBlockListing()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BlockList blockList = client.GetDSBlockListing(1).Result;
            Assert.IsNotNull(blockList);
        }

        [Test]
        public void GetTxBlockListing()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BlockList blockList = client.GetTxBlockListing(1).Result;
            Assert.IsNotNull(blockList);
        }

        [Test]
        public void GetBlockchainInfo()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BlockchainInfo blockchainInfo = client.GetBlockchainInfo().Result;
            Assert.IsNotNull(blockchainInfo);
        }


        [Test]
        public void GetDsBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            DsBlock dsBlock = client.GetDsBlock("1").Result;
            Assert.IsNotNull(dsBlock);
            Assert.IsTrue(dsBlock.Header.Difficulty == 3);
        }


        [Test]
        public void GetNumDSBlocks()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String Result = client.GetNumDSBlocks().Result;
            Assert.IsNotNull(Result);
        }


        [Test]
        public void GetTxBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            TxBlock txBlock = client.GetTxBlock("40").Result;
            Assert.IsNotNull(txBlock);
            Assert.AreEqual(3, txBlock.Body.MicroBlockInfos.Count());
        }

        [Test]
        public void GetLatestDsBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            DsBlock dsBlock = client.GetLatestDsBlock().Result;
            Assert.IsNotNull(dsBlock);
        }

        [Test]
        public void GetLatestTxBlock()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            TxBlock txBlock = client.GetLatestTxBlock().Result;
            Assert.IsNotNull(txBlock);
        }

        [Test]
        public void GetBalance()
        {
            HttpProvider client = new HttpProvider("https://dev-api.zilliqa.com/");
            BalanceResult balance = client.GetBalance("4baf5fada8e5db92c3d3242618c5b47133ae003c".ToLower()).Result;
            Assert.IsNotNull(balance.Balance);
        }

        [Test]
        public void GetBalance32()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            BalanceResult balance = client.GetBalance32("zil1z6rpmumewzrmdz44wu9hgvdwrs5xgptlzd6kec").Result;
            Assert.NotNull(balance);
        }

        [Test]
        public void GetSmartContractCode()
        {
            HttpProvider client = new HttpProvider("https://dev-api.zilliqa.com/");
            String code = client.GetSmartContractCode("4baf5fada8e5db92c3d3242618c5b47133ae003c").Result.Code;
            Console.WriteLine(code);
        }

        [Test]
        public void GetMinimumGasPrice()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String price = client.GetMinimumGasPrice().Result;
            Console.WriteLine(price);

        }


        [Test]
        public void GetTransaction()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            Transaction transaction = client.GetTransaction("ce918e4c77ed40f3a23588bd3c380458b43be168935d468e2e6f680724e71474").Result;
            Console.WriteLine(transaction);
        }

        [Test]
        public void GetTransaction32()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            Transaction transaction = client.GetTransaction32("ce918e4c77ed40f3a23588bd3c380458b43be168935d468e2e6f680724e71474").Result;
            Console.WriteLine(transaction);
        }

        [Test]
        public void GetRecentTransactions()
        {
            HttpProvider client = new HttpProvider("https://dev-api.zilliqa.com/");
            TransactionList transactionList = client.GetRecentTransactions().Result;
            Console.WriteLine(transactionList);
        }

        [Test]
        public void GetSmartContractState()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            List<State> stateList = client.GetSmartContractState("D6606D02DFF929593312D8D0D36105E376F95AA0").Result;
            Console.WriteLine(stateList);
        }

        [Test]
        public void GetNumTxnsTxEpoch()
        {
            HttpProvider client = new HttpProvider("https://api.zilliqa.com/");
            String lastEpoch = client.GetNumTxnsTxEpoch().Result;
            List<State> lastStateList = client.GetSmartContractState("D6606D02DFF929593312D8D0D36105E376F95AA0").Result;

            Console.WriteLine("last epoch is " + lastEpoch);
            Console.WriteLine("last state:" + lastStateList);

            int n = 0;

            while (n < 2)
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
