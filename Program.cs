using Newtonsoft.Json;
using System;

namespace blockchain_basic
{
    public class Program
    {
        public static int Port = 7777;
        public static P2PServer Server = new P2PServer();
        public static P2PClient Client = new P2PClient();
        public static Blockchain ValueCoin = new Blockchain();
        static void Main(string[] args)
        {
            Server.Start();

            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();

            Console.WriteLine($"Is Chain Valid: {ValueCoin.IsValid()}");

            startTime = DateTime.Now;
            //valueCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Alice,receiver:Bob,amount:10}"));
            ValueCoin.CreateTransaction(new Transaction("Alice","Bob",10));
            ValueCoin.ProcessPendingTransaction("Charlie");
            ValueCoin.ProcessPendingTransaction("Charlie");
            endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");

            Console.WriteLine($"Is Chain Valid: {ValueCoin.IsValid()}");

            Console.WriteLine($"Alice balance: {ValueCoin.GetBalance("Alice")}");
            Console.WriteLine($"Bob balance: {ValueCoin.GetBalance("Bob")}");
            Console.WriteLine($"Charlie balance: {ValueCoin.GetBalance("Charlie")}");

            Console.WriteLine(JsonConvert.SerializeObject(ValueCoin, Formatting.Indented));
        }
    }
}
