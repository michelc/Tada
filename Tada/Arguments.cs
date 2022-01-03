using System.Collections.Generic;

namespace Tada
{
    public class Arguments
    {
        private List<string> Values { get; set; }

        public Arguments(string[] args)
        {
            Values = new List<string>(args);
        }

        public string Pop()
        {
            var pop = "";
            if (Values.Count > 0)
            {
                pop = Values[0];
                Values.RemoveAt(0);
            }

            return pop;
        }

        public string[] ToArray()
        {
            return Values.ToArray();
        }

        public string GetStringOption(string longOption, string shortOption = "")
        {
            if (shortOption == "") shortOption = longOption.Substring(0, 1);
            shortOption = "-" + shortOption;
            longOption = "--" + longOption;

            foreach (var a in Values)
            {
                if ((a == longOption) || (a == shortOption))
                {
                    var index = Values.FindIndex(x => x == a);
                    Values.RemoveAt(index);
                    if (index == Values.Count) return "";
                    var option = Values[index];
                    Values.RemoveAt(index);
                    return option;
                }
            }

            return "";
        }

        public bool GetBoolOption(string longOption, string shortOption = "")
        {
            if (shortOption == "") shortOption = longOption.Substring(0, 1);
            shortOption = "-" + shortOption;
            longOption = "--" + longOption;

            foreach (var a in Values)
            {
                if ((a == longOption) || (a == shortOption))
                {
                    Values.Remove(a);
                    return true;
                }
            }

            return false;
        }
    }
}
