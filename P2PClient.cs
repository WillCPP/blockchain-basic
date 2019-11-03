using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace blockchain_basic
{
    public class P2PClient
    {
        IDictionary<string, WebSocket> sockets = new Dictionary<String, WebSocket>();

        public void Connect(string url)
        {
            if (!sockets.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);
                webSocket.OnMessage += (sender, e) => {
                    if (e.Data == "HELLO CLIENT")
                    {
                        Console.WriteLine(e.Data);
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
                    }
                };
                webSocket.Connect();
                webSocket.Send("HELLO SERVER");
                webSocket.Send(JsonConvert.SerializeObject(Program.ValueCoin));
                sockets.Add(url, webSocket);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in sockets)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in sockets)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in sockets)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in sockets)
            {
                item.Value.Close();
            }
        }
    }
}
