using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerISession1.State
{
    public interface IBrowserStorageProvider
    {
        string GetItem(string key);

        ValueTask<string> GetItemAsync(string key);

        void SetItem(string key, string data);

        ValueTask SetItemAsync(string key, string data);
    }
}
