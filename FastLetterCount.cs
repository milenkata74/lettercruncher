using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        string text = File.ReadAllText("large_text.txt"); 

        ConcurrentDictionary<char, int> letterCounts = new ConcurrentDictionary<char, int>();

        
        var textParts = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

       
        Parallel.ForEach(textParts, part =>
        {
            var localCount = new ConcurrentDictionary<char, int>();

            foreach (char c in part)
            {
                if (char.IsLetter(c)) 
                {
                    char lowerChar = char.ToLower(c);
                    localCount.AddOrUpdate(lowerChar, 1, (_, count) => count + 1);
                }
            }

           
            foreach (var kvp in localCount)
            {
                letterCounts.AddOrUpdate(kvp.Key, kvp.Value, (_, count) => count + kvp.Value);
            }
        });

        
        foreach (var kvp in letterCounts.OrderBy(kvp => kvp.Key))
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}
