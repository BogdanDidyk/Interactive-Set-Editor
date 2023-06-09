﻿namespace InteractiveSetEditor
{
    public static class IEnumerableExtension
    {
        public static void PrintData<T>(this IEnumerable<T> data, int highlightedIndex = 0, ConsoleColor highlightedColor = ConsoleColor.Green)
        {
            for (int i = 0; i < data.Count(); i++)
            {
                Console.ForegroundColor = (i == highlightedIndex) ? highlightedColor : ConsoleColor.Gray;
                Console.Write(data.ElementAt(i) + "  ");
            }

            Console.ResetColor();
        }
    }

    public static class ConsoleReader
    {
        public static T ReadValue<T>()
        {
            Console.Clear();
            Console.ResetColor();
            Console.CursorVisible = true;

            Console.Write($"New [{typeof(T)}] Value: ");
            T value;

            try
            {
                value = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            catch (Exception)
            {
                value = default;
            }
            Console.CursorVisible = false;

            return value;
        }
    }

    public static class InteractiveSetEditor<T>
    {
        private static void ShowManual()
        {
            Console.WriteLine("Press → and ← to move pointer");
            Console.WriteLine("Press SPACE to change current value");
            Console.WriteLine("Press TAB to add new value");
            Console.WriteLine("Press DEL or BACKSPACE to clear current value");
            Console.WriteLine("Press ENTER to complete edition");
            Console.WriteLine();
            Console.WriteLine("(Press any key to start)");
            Console.ReadKey();
        }

        public static void EditItems(List<T> set)
        {
            ShowManual();

            Console.CursorVisible = false;
            int index = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                set.PrintData(index);

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.RightArrow)
                {
                    ++index;
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    --index;
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    set[index] = ConsoleReader.ReadValue<T>();
                }
                else if (key == ConsoleKey.Tab)
                {
                    set.Add(ConsoleReader.ReadValue<T>());
                }
                else if ((key == ConsoleKey.Delete || key == ConsoleKey.Backspace) && set.Count != 1)
                {
                    set.RemoveAt(index);
                }

                index = (set.Count + index) % set.Count;
            }
            while (key != ConsoleKey.Enter);

            Console.ResetColor();
            Console.Clear();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<int> set = new List<int>() { 26, 10, -13, 17, 3 };
            List<int> copy = new List<int>(set);

            InteractiveSetEditor<int>.EditItems(set);

            Console.WriteLine($"Original: {string.Join(", ", copy)}");
            Console.WriteLine($"Edited: {string.Join(", ", set)}");

            Console.Read();
        }
    }
}