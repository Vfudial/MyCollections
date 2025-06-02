namespace MyCollections
{
    public static class MenuFunctions
    {
        public static void PrintMenu(int choice, string[] menu)
        {
            Console.Clear();
            for (int i = 0; i < menu.Length; ++i)
            {
                if (i == choice)
                {
                    for (int j = 0; j < menu.Length; ++j)
                    {
                        if (i == j)
                        {
                            Console.WriteLine(menu[j].Replace("  ", "=>"));
                            continue;
                        }
                        Console.WriteLine(menu[j]);
                    }
                }
            }
            Console.WriteLine("\v");
        }
        public static int Menu(string[] menu)
        {

            Console.Clear();
            Console.CursorVisible = false;
            int choice = 0;
            while (true)
            {
                PrintMenu(choice, menu);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (choice == 0)
                        {
                            choice = menu.Length - 1;
                            break;
                        }
                        choice--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (choice == menu.Length - 1)
                        {
                            choice = 0;
                            break;
                        }
                        choice++;
                        break;
                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;
                        return choice + 1;
                }
            }
        }
    }
}