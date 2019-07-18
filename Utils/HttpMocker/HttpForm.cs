using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public class HttpForm: Dictionary<string, string>
    {
        public new string this[string name]
        {
            set
            {
                if (name == null) return;
                if (this.ContainsKey(name))
                {
                    this[name] = value;
                }
                else
                {
                    this.Add(name, value);
                }
            }
        }
    }
}
