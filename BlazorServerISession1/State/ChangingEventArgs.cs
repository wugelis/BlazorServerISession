using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerISession1.State
{
    public class ChangingEventArgs: ChangedEventArgs
    {
        public bool Cancel { get; set; }
    }
}
