using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerISession1.State
{
    public interface ISessionStorageService
    {
        ValueTask<T> GetItemAsync<T>(string key);

        ValueTask SetItemAsync<T>(string key, T data);
    }
}
