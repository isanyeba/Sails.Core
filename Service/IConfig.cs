using Sails.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Service
{
    public class IConfig : ParameterSetting
    {
        public string Name = "";
        public string ServiceAssembly;

        public string ProcessAdapter;
    }
}
