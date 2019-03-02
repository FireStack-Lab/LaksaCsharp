using LaksaCsharp.Account;
using LaksaCsharp.BlockChain;
using LaksaCsharp.Contract;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaTest.Contract
{
    [TestClass]
    public class ContractTest
    {
        [TestMethod]
        public void GetAddressForContract()
        {
            Transaction transaction = new Transaction();

            transaction.SenderPubKey = "0246E7178DC8253201101E18FD6F6EB9972451D121FC57AA2A06DD5C111E58DC6A";
            transaction.Nonce = "19";
            String address = ContractFactory.GetAddressForContract(transaction);
            Assert.AreEqual(address.ToLower(), "8f14cb1735b2b5fba397bea1c223d65d12b9a887");
        }

        [TestMethod]
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

            List<Values> init =
                new List<Values>() {
                    new Values() { Type="Uint32", Value="0" , VName ="_scilla_version"  },
                    new Values() { Type="ByStr20", Value="0x9bfec715a6bd658fcb62b0f8cc9bfa2ade71434a" , VName ="owner"  },
                };

            Wallet wallet = new Wallet();
            wallet.AddByPrivateKey("e19d05c5452598e24caad4a0d85a49146f7be089515c905ae6a19e8a578a6930");
            ContractFactory factory = new ContractFactory()
            {
                Provider = new HttpProvider("https://dev-api.zilliqa.com/"),
                Signer = wallet
            };

            LaksaCsharp.Contract.Contract contract = factory.AtContract("a7c88a90eb79740fc730397557f77f36fd52a04c", code, (Values[])init.ToArray(), "");
            CallParams param = new CallParams()
            {
                Amount = "0",
                GasLimit = "10000",
                GasPrice = "1000000000",
                SenderPubKey = "0246e7178dc8253201101e18fd6f6eb9972451d121fc57aa2a06dd5c111e58dc6a",
                Version = Wallet.Pack(2, 8).ToString()
            };
            Transition transition = new Transition()
            {
                Name = "getHello",
                Params = new Field[] { }
            };
            LaksaCsharp.Transaction.Transaction transitionResult = contract.Call(transition, (Values[])init.ToArray(), param, 300, 3);
        }

        [TestMethod]
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
                Provider = new HttpProvider("https://dev-api.zilliqa.com/"),
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
