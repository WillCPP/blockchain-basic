using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace blockchain_basic
{
    public class P2PServer: WebSocketBehavior
    {
        bool chainSynced = false;
        WebSocketServer server = null;

        public void Start()
        {
            server = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            server.AddWebSocketService<P2PServer>("/Blockchain");
            server.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "HELLO SERVER")
            {
                Console.WriteLine(e.Data);
                Send("HELLO CLIENT");
            }
            else
            {
                Blockchain blockchain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (blockchain.IsValid() && blockchain.Chain.Count > Program.ValueCoin.Chain.Count)
                {
                    List<Transaction> transactions = new List<Transaction>();
                    transactions.AddRange(blockchain.PendingTransactions);
                    transactions.AddRange(Program.ValueCoin.PendingTransactions);

                    blockchain.PendingTransactions = transactions;
                    Program.ValueCoin = blockchain;
                }

                if (!chainSynced)
                {
                    Send(JsonConvert.SerializeObject(Program.ValueCoin));
                    chainSynced = true;
                }
            }
        }
    }
}
