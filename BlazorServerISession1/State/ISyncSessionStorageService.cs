using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerISession1.State
{
    public interface ISyncSessionStorageService
    {
        T GetItem<T>(string key);

        /// <summary>
        /// Retrieve the specified data from session storage as a <see cref="string"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>The data associated with the specified <paramref name="key"/> as a <see cref="string"/></returns>
        string GetItemAsString(string key);

        void SetItem<T>(string key, T data);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in session storage with the specified <paramref name="key"/>. Does not serialize the value before storing.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The string to be saved</param>
        /// <returns></returns>
        void SetItemAsString(string key, string data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
