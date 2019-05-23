using LaksaCsharp.Crypto;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Transaction;
using LaksaCsharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Account
{
    public class Wallet
    {
        private IDictionary<string, Account> accounts = new Dictionary<string, Account>();
        private Account defaultAccount;

        public HttpProvider Provider
        {
            get; set;
        }
        public Wallet()
        {
            defaultAccount = null;
            Provider = new HttpProvider("https://api.zilliqa.com/");
        }

        public Wallet(IDictionary<string, Account> accounts, HttpProvider provider)
        {
            this.accounts = accounts;
            this.Provider = provider;

            if (accounts.Count > 0)
            {
                defaultAccount = accounts.FirstOrDefault().Value;
            }
            else
            {
                defaultAccount = null;
            }
        }

        public string CreateAccount()
        {
            Account account = new Account(KeyTools.GenerateKeyPair());
            this.accounts.Add(account.Address.ToUpper(), account);

            if (defaultAccount == null)
            {
                defaultAccount = account;
            }
            return account.Address;
        }

        public string AddByPrivateKey(string privateKey)
        {
            Account account = new Account(privateKey);
            this.accounts.Add(account.Address, account);
            if (defaultAccount == null)
            {
                defaultAccount = account;
            }
            return account.Address;
        }

        public string AddByKeyStore(string keystore, string passphrase)
        {
            Account account = Account.FromFile(keystore, passphrase);
            this.accounts.Add(account.Address, account);

            if (defaultAccount == null)
            {
                defaultAccount = account;
            }
            return account.Address;
        }

        public void SetDefault(string address)
        {
            this.defaultAccount = accounts.ContainsKey(address) ? accounts[address] : null;
        }

        public void Remove(string address)
        {
            Account toRemove = accounts.ContainsKey(address) ? accounts[address] : null;
            if (null != toRemove)
            {
                accounts.Remove(address);
                if (defaultAccount != null && defaultAccount.Address == toRemove.Address)
                {
                    if (accounts.Count > 0)
                    {
                        defaultAccount = accounts.FirstOrDefault().Value;
                    }
                    else
                    {
                        defaultAccount = null;
                    }
                }
            }
        }

        public Transaction.Transaction Sign(Transaction.Transaction transaction)
        {
            if (transaction.ToAddr.ToUpper().StartsWith("0X"))
            {
                transaction.ToAddr = transaction.ToAddr.Substring(2);
            }

            if (!Validation.IsBech32(transaction.ToAddr) && !Validation.IsValidChecksumAddress("0x" + transaction.ToAddr))
            {
                throw new Exception("not checksum address or bech32");
            }

            if (Validation.IsBech32(transaction.ToAddr))
            {
                transaction.ToAddr = Bech32.FromBech32Address(transaction.ToAddr);
            }

            if (Validation.IsValidChecksumAddress("0x" + transaction.ToAddr))
            {
                transaction.ToAddr = "0x" + transaction.ToAddr;
            }

            TxParams txParams = transaction.ToTransactionParam();

            if (txParams != null && !string.IsNullOrEmpty(txParams.SenderPubKey))
            {
                string address = KeyTools.GetAddressFromPublicKey(txParams.SenderPubKey).ToUpper();
                Account account = accounts[address];
                if (account == null)
                {
                    throw new Exception("Could not sign the transaction with" + address + "  as it does not exist");
                }
                return SignWith(transaction, account);
            }

            if (defaultAccount == null)
            {
                throw new Exception("This wallet has no default account.");
            }

            return this.SignWith(transaction, this.defaultAccount);

        }

        public Transaction.Transaction SignWith(Transaction.Transaction tx, Account signer)
        {
            BalanceResult result;
            if (signer == null)
            {
                throw new Exception("account not exists");
            }
            if (string.IsNullOrEmpty(tx.Nonce))
            {
                try
                {
                    result = this.Provider.GetBalance(signer.Address).Result;
                    tx.Nonce = result.Nonce + 1;
                }
                catch (Exception e)
                {
                    throw new Exception("cannot get nonce", e);
                }
            }
            tx.SenderPubKey = signer.GetPublicKey();
            byte[] message = tx.Bytes();
            Signature signature = Schnorr.Sign(signer.Keys, message);
            tx.Signature = (signature.ToString().ToLower());
            return tx;
        }

        public static int Pack(int a, int b)
        {
            return (a << 16) + b;
        }
    }
}
