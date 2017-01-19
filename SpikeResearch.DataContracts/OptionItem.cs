using System;

namespace SpikeResearch.DataContracts
{
    public class OptionItem
    {
        public int Index { get; set; }

        public string Description { get; set; }

        public Action Action { get; set; }

        public OptionItem() { }

        public OptionItem(int index, string description, Action action)
        {
            Index = index;
            Description = description;
            Action = action;
        }
    }
}
