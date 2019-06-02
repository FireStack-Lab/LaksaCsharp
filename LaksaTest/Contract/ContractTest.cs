using LaksaCsharp.Account;
using LaksaCsharp.BlockChain;
using LaksaCsharp.Contract;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using NUnit.Framework;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaTest.Contract
{
    [TestFixture]
    public class ContractTest
    {
        [Test]
        public void GetAddressForContract()
        {
            Transaction transaction = new Transaction();

            transaction.SenderPubKey = "0246E7178DC8253201101E18FD6F6EB9972451D121FC57AA2A06DD5C111E58DC6A";
            transaction.Nonce = "19";
            String address = ContractFactory.GetAddressForContract(transaction);
            Assert.AreEqual(address.ToLower(), "8f14cb1735b2b5fba397bea1c223d65d12b9a887");
        }

        [Test]
        public void Call()
        {
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
                    "        msg = {_tag : \"Main\"; _recipient : _sender; _amount : Uint128 0; code : not_owner_code};\n" +
                    "        msgs = one_msg msg;\n" +
                    "        send msgs\n" +
                    "      | True =>\n" +
                    "        welcome_msg := msg;\n" +
                    "        msg = {_tag : \"Main\"; _recipient : _sender; _amount : Uint128 0; code : set_hello_code};\n" +
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
            List<Values> init = new List<Values>() {
                new  Values(){ VName="_scilla_version",Type="Uint32",Value="0"},
                 new  Values(){ VName="owner",Type="ByStr20",Value="0x9bfec715a6bd658fcb62b0f8cc9bfa2ade71434a"},
                  new  Values(){ VName="total_tokens",Type="Uint128",Value="1000000000"},
                   new  Values(){ VName="decimals",Type="Uint32",Value="0"},
                    new  Values(){ VName="name",Type="String",Value="BobCoin"},
                     new  Values(){ VName="symbol",Type="String",Value="BOB"},
            };
            Wallet wallet = new Wallet();
            wallet.AddByPrivateKey("e19d05c5452598e24caad4a0d85a49146f7be089515c905ae6a19e8a578a6930");
            ContractFactory factory = new ContractFactory()
            {
                Provider = new HttpProvider("https://dev-api.zilliqa.com/"),
                Signer = wallet
            };
            LaksaCsharp.Contract.Contract contract = factory.AtContract("zil1h4cesgy498wyyvsdkj7g2zygp0xj920jw2hyx0", code, (Values[])init.ToArray(), "");
            int nonce = int.Parse(factory.Provider.GetBalance("9bfec715a6bd658fcb62b0f8cc9bfa2ade71434a").Result.Nonce);
            CallParams param = new CallParams()
            {
                Nonce = (nonce + 1).ToString(),
                Version = Wallet.Pack(333, 1).ToString(),
                GasPrice = "1000000000",
                GasLimit = "1000",
                SenderPubKey = "0246e7178dc8253201101e18fd6f6eb9972451d121fc57aa2a06dd5c111e58dc6a",
                Amount = "0"

            };
            List<Values> values = new List<Values>() {
                 new  Values(){ VName="to",Type="ByStr20",Value="0x381f4008505e940ad7681ec3468a719060caf796"},
                  new  Values(){ VName="tokens",Type="Uint128",Value="10"},
            };

            LaksaCsharp.Transaction.Transaction transaction = contract.Call("Transfer", (Values[])values.ToArray(), param, 3000, 3);
        }

        [Test]
        public void Deploy()
        {
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
                "        msg = {_tag : \"Main\"; _recipient : _sender; _amount : Uint128 0; code : not_owner_code};\n" +
                "        msgs = one_msg msg;\n" +
                "        send msgs\n" +
                "      | True =>\n" +
                "        welcome_msg := msg;\n" +
                "        msg = {_tag : \"Main\"; _recipient : _sender; _amount : Uint128 0; code : set_hello_code};\n" +
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

            Wallet wallet = new Wallet();
            wallet.AddByPrivateKey("e19d05c5452598e24caad4a0d85a49146f7be089515c905ae6a19e8a578a6930");
            ContractFactory factory = new ContractFactory()
            {
                Provider = new HttpProvider("https://api.zilliqa.com/"),
                Signer = wallet
            };

            LaksaCsharp.Contract.Contract contract = factory.NewContract(code, (Values[])init.ToArray(), "");
            DeployParams deployParams = new DeployParams()
            {

                GasLimit = "10000",
                GasPrice = "1000000000",
                SenderPubKey = "0246e7178dc8253201101e18fd6f6eb9972451d121fc57aa2a06dd5c111e58dc6a",
                Version = Wallet.Pack(2, 8).ToString()
            };

            Tuple<Transaction, LaksaCsharp.Contract.Contract> result = contract.Deploy(deployParams, 300, 3);

            Console.WriteLine("result is: " + result.ToString());

            String transactionFee = new BigInteger(result.Item1.Receipt.Cumulative_gas).Multiply(new BigInteger(result.Item1.GasPrice)).ToString();
            Console.WriteLine("transaction fee is: " + transactionFee);
        }
    }
}
