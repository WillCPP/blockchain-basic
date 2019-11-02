using System;

namespace blockchain_basic
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain valueCoin = new Blockchain();
            Console.WriteLine($"Is Chain Valid: {valueCoin.IsValid()}");
            valueCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Alice,receiver:Bob,amount:10}"));
            Console.WriteLine($"Is Chain Valid: {valueCoin.IsValid()}");
        }
    }
}
