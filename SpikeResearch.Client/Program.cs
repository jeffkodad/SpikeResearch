using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpikeResearch.Contracts.Managers;
using SpikeResearch.Utilities;

namespace SpikeResearch.Client
{
    public class Program
    {
        #region Fields

        private IGitHubManager _gitHubManager;

        #endregion

        #region Properties

        public IGitHubManager GitHubManager
        {
            get { return _gitHubManager ?? (_gitHubManager = ClassFactory.CreateClass<IGitHubManager>()); }
            set { _gitHubManager = value; }
        }

        #endregion

        #region Methods



        #endregion
        static void Main(string[] args)
        {
            var program = new Program();
        }

        public Program()
        {
            Console.WriteLine("Spike Research, press enter to continue.");
            Console.ReadLine();

            

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

    }
}
