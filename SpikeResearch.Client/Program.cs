using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpikeResearch.Contracts.Managers;
using SpikeResearch.Utilities;

namespace SpikeResearch.Client
{
    public class Program : ServiceBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods


        static void Main(string[] args)
        {
            var program = new Program();
        }

        public Program()
        {
            PrintHeading("Spike Research");

            Console.WriteLine("1. GitHub");
            Console.WriteLine("2. Google Docs");
            Console.WriteLine("3: Exit");

            switch (GetNumberInput(3))
            {
                case "1":
                    GitHub();
                    break;
                case "2":
                    GoogleDocs();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:

                    break;
            }

            ExitApp();
        }

        public void GitHub()
        {
            Console.WriteLine("Now Working on GitHub");
            var gitHubClient = new GitHubClient();
        }

        public void GoogleDocs()
        {
            
        }

        public void ExitApp()
        {
            Console.WriteLine("Press any key to start over.");
            Console.ReadLine();
            Console.Clear();
            var program = new Program();
        }

        #region HelperMethods

        

        #endregion

        #endregion
    }
}
