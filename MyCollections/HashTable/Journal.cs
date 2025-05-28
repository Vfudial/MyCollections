using Emojis;

namespace MyCollections;

public class Journal<T> where T : IInit, new()
{
    List<string> journal = new();

    public void WriteRecord(object source, CollectionHandlerEventArgs<T> args)
    {
        journal.Add($"{args.Type} элемент {args.Item}");
    }

    public void PrintJournal()
    {
        if (journal.Count == 0)
        {
            Console.WriteLine("Журнал пуст");
            return;
        }

        foreach (string record in journal)
        {
            Console.WriteLine(record);
        }
    }
}