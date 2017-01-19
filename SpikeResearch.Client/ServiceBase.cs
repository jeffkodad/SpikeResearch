using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpikeResearch.DataContracts;

namespace SpikeResearch.Client
{
    public class ServiceBase
    {
        #region Fields



        #endregion

        #region Properties

        #endregion

        #region Methods

        public void PrintHeading(string text)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine(text);
            Console.WriteLine("===========================================");
        }

        public void PrintSectionBreak()
        {
            Console.WriteLine("===========================================");
        }

        public string GetInput()
        {
            Console.WriteLine("Enter your input and press enter.");
            var input = Console.ReadLine();
            Console.WriteLine();
            return input;
        }

        public void WaitForAnyKey()
        {
            Console.ReadKey();
        }

        public string GetNumberInput(double maxNumber)
        {
            string _val = "";
            Console.Write("Enter your value: ");
            ConsoleKeyInfo key;
            bool read = true;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace)
                {
                    double val = 0;
                    bool _x = double.TryParse(key.KeyChar.ToString(), out val);
                    if (_x && val <= maxNumber && val != 0)
                    {
                        _val += key.KeyChar;
                        Console.Write(key.KeyChar);
                        Console.Write("\n\n");
                        read = false;
                    }
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (read);

            return _val;
        }

        public void ResetApp()
        {
            Console.Clear();
            var program = new Program();
        }

        public void DisplayAllObjectProperties(Object obj)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                Console.WriteLine($"{descriptor.Name}: {descriptor.GetValue(obj)}");
            }
        }

        public void DisplayOptionItemList<T>(string heading, List<OptionItem> list, T displayObject)
        {
            Console.Clear();
            PrintHeading(heading);
            if (displayObject != null)
            {
                DisplayAllObjectProperties(displayObject);
                PrintSectionBreak();
            }
            Console.WriteLine("Select an Option");
            foreach (var item in list.OrderBy(x => x.Index))
            {
                Console.WriteLine($"{item.Index}. {item.Description}");
            }
        }

        public void DisplayMessageAndWait(string message, Action action)
        {
            Console.WriteLine(message);
            WaitForAnyKey();
            action.Invoke();
        }

        public void CloseAppNow()
        {
            Environment.Exit(0);
        }

        public OptionItem GetResetItem(int index)
        {
            return new OptionItem(index, "Reset", new Action(ResetApp));
        }

        public OptionItem GetExitOption(int index)
        {
            return new OptionItem(index, "Exit", new Action(CloseAppNow));
        }

        #endregion
    }
}
