﻿using System.Collections;
using System.Collections.Generic;
using NLog;

namespace SupportBank.ConsoleApp
{
  class Bank : IEnumerable<Account>
  {
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    private readonly Dictionary<string, Account> accounts = new Dictionary<string, Account>();

    public Bank(IEnumerable<Transaction> initialTransactions)
    {
      CreateAccountsFromTransactions(initialTransactions);
    }

    public Account this[string owner] => GetOrCreateAccount(owner);

    public IEnumerator<Account> GetEnumerator()
    {
      return accounts.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private void CreateAccountsFromTransactions(IEnumerable<Transaction> transactions)
    {
      foreach (var transaction in transactions)
      {
        GetOrCreateAccount(transaction.FromAccount).OutgoingTransactions.Add(transaction);
        GetOrCreateAccount(transaction.ToAccount).IncomingTransactions.Add(transaction);
      }
    }

    private Account GetOrCreateAccount(string owner)
    {
      if (accounts.ContainsKey(owner))
      {
        return accounts[owner];
      }

      logger.Debug($"Adding account for {owner}");
      var newAccount = new Account(owner);
      accounts[owner] = newAccount;
      return newAccount;
    }

  }
}