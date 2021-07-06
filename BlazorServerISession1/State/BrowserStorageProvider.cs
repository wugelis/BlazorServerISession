using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerISession1.State
{
    /// <summary>
    /// 透過 Blazor 的 JSRuntime InterOp 機制存取瀏覽器的 session storage.
    /// </summary>
    public class BrowserStorageProvider : IBrowserStorageProvider
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;

        public BrowserStorageProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            _jSInProcessRuntime  = jSRuntime as IJSInProcessRuntime;
        }
        /// <summary>
        /// 在 Browser 端(前端) 透過 Mono Runtime 執行環境中，存取 Browser 的 window.sessionStorage.getItem() 方法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetItem(string key)
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<string>("sessionStorage.getItem", key);
        }
        /// <summary>
        /// (非同步方法): 在 Browser 端(前端) 透過 Mono Runtime 執行環境中，存取 Browser 的 window.sessionStorage.getItem() 方法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueTask<string> GetItemAsync(string key)
        {
            return _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", key);
        }
        /// <summary>
        /// 在 Browser 端(前端) 透過 Mono Runtime 執行環境中，存取 Browser 的 window.sessionStorage.setItem() 方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SetItem(string key, string data)
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoid("sessionStorage.setItem", key, data);
        }
        /// <summary>
        /// (非同步方法): 在 Browser 端(前端) 透過 Mono Runtime 執行環境中，存取 Browser 的 window.sessionStorage.setItem() 方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ValueTask SetItemAsync(string key, string data)
        {
            return _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", key, data);
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckForInProcessRuntime()
        {
            if (_jSInProcessRuntime == null)
            {
                throw new InvalidOperationException("目前的 IJSInProcessRuntime 不可使用。");
            }
        }
    }
}
