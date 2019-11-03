using blockchain_basic;
using System;
using System.Collections.Generic;

public class Blockchain {
    public IList<Block> Chain { get; set; }
    public IList<Transaction> PendingTransactions = new List<Transaction>();
    public int Difficulty { get; set; } = 2;
    public int Reward { get; set; } = 1;

    public Blockchain() {
        InitializeChain();
        AddGenesisBlock();
    }

    public void InitializeChain() {
        Chain = new List<Block>();
    }

    public Block CreateGenesisBlock() {
        return new Block(DateTime.Now, null, null);
    }

    public void AddGenesisBlock() {
        Chain.Add(CreateGenesisBlock());
    }

    public Block GetLatestBlock() {
        return Chain[Chain.Count - 1];
    }

    public void AddBlock(Block block) {
        Block latestBlock = GetLatestBlock();
        block.Index = latestBlock.Index + 1;
        block.PreviousHash = latestBlock.Hash;
        block.Mine(this.Difficulty);
        Chain.Add(block);
    }

    public bool IsValid() {
        for (int i = 1; i < Chain.Count; i++) {
            Block currenBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if(currenBlock.Hash != currenBlock.CalculateHash()) {
                return false;
            }

            if(currenBlock.PreviousHash != previousBlock.Hash) {
                return false;
            }
        }
        return true;
    }

    public void CreateTransaction(Transaction transaction)
    {
        PendingTransactions.Add(transaction);
    }

    public void ProcessPendingTransaction(string minerAddress)
    {
        Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
        AddBlock(block);

        PendingTransactions = new List<Transaction>();
        CreateTransaction(new Transaction(null, minerAddress, Reward));
    }

    public int GetBalance(string address)
    {
        int balance = 0;
        foreach (Block block in Chain)
        {
            if (block.Transactions != null)
            {
                foreach (Transaction transaction in block.Transactions)
                {
                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }
        }
        return balance;
    }
}