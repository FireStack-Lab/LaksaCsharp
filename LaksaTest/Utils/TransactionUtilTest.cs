using LaksaCsharp.Transaction;
using LaksaCsharp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaTest.Utils
{
    [TestFixture]
    public class TransactionUtilTest
    {

        [Test]
        public void EncodeTransactionProto()
        {
            TxParams txParams = new TxParams();
            txParams.Version = "0";
            txParams.Nonce = "0";
            txParams.ToAddr = "2E3C9B415B19AE4035503A06192A0FAD76E04243";
            txParams.SenderPubKey = "0246e7178dc8253201101e18fd6f6eb9972451d121fc57aa2a06dd5c111e58dc6a";
            txParams.Amount = "10000";
            txParams.GasPrice = "100";
            txParams.GasLimit = "1000";
            txParams.Code = "";
            txParams.Data = "";

            TransactionUtil util = new TransactionUtil();
            byte[] bytes = util.EncodeTransactionProto(txParams);
            Console.WriteLine(ByteUtil.ByteArrayToHexString(bytes));
        }
    }
}
