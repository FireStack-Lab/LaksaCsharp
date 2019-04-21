using LaksaCsharp.Account;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaTest.Account
{
    [TestFixture]
    public class WalletTest
    {
        [Test]
        public void Sign()
        {
            Wallet wallet = new Wallet();
            wallet.Provider = new HttpProvider("https://dev-api.zilliqa.com/");
            string address = wallet.AddByPrivateKey("e19d05c5452598e24caad4a0d85a49146f7be089515c905ae6a19e8a578a6930");
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
            transaction = wallet.Sign(transaction);
            Console.WriteLine("signature is: " + transaction.Signature);
            CreateTxResult result = TransactionFactory.CreateTransaction(transaction);
            Console.WriteLine(result);
            Assert.NotNull(result);
            Assert.NotNull(result.TranID);
        }

        [Test]
        public void CreateAccount()
        {
            Wallet wallet = new Wallet();
            Assert.NotNull(wallet.CreateAccount());
        }
    }
}
