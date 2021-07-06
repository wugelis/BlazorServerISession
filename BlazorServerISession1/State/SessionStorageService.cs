using BlazorServerISession1.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorServerISession1.State
{
    public class SessionStorageService: ISessionStorageService, ISyncSessionStorageService
    {
        private readonly IBrowserStorageProvider _storageProvider;
        private readonly IJsonSerializer _jsonSerializer;

        public SessionStorageService(IBrowserStorageProvider storageProvider, IJsonSerializer jsonSerializer)
        {
            _storageProvider = storageProvider;
            _jsonSerializer = jsonSerializer;
        }

        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        public T GetItem<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = _storageProvider.GetItem(key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            try
            {
                return _jsonSerializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException e) when (e.Path == "$" && typeof(T) == typeof(string))
            {
                return (T)(object)serialisedData;
            }
        }

        public string GetItemAsString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _storageProvider.GetItem(key);
        }

        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException($"Key 名稱: {nameof(key)} 不可為空值!");

            var serializedData = await _storageProvider.GetItemAsync(key).ConfigureAwait(false);

            if(string.IsNullOrWhiteSpace(serializedData))
            {
                return default;
            }

            try
            {
                return _jsonSerializer.Deserialize<T>(serializedData);
            }
            catch(JsonException e)
            {
                return (T)(object)serializedData;
            }
        }

        public void SetItem<T>(string key, T data)
        {
            throw new NotImplementedException();
        }

        public void SetItemAsString(string key, string data)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException($"Key 名稱: {nameof(key)} 不可為空值!");

            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var e = RaiseOnChangingSync(key, data);

            if (e.Cancel)
                return;

            _storageProvider.SetItem(key, data);

            RaiseOnChanged(key, e.OldValue, data);
        }

        public async ValueTask SetItemAsync<T>(string key, T data)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException($"Key 名稱: {nameof(key)} 不可為空值!");

            var e = await RaiseOnChangingAsync(key, data).ConfigureAwait(false);

            if (e.Cancel)
                return;

            var serialisedData = _jsonSerializer.Serialize(data);
            await _storageProvider.SetItemAsync(key, serialisedData).ConfigureAwait(false);

            RaiseOnChanged(key, e.OldValue, data);
        }

        private object GetItemInternal(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException($"Key 名稱: {nameof(key)} 不可為空值!");

            var serialisedData = _storageProvider.GetItem(key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            try
            {
                return _jsonSerializer.Deserialize<object>(serialisedData);
            }
            catch (JsonException)
            {
                return serialisedData;
            }
        }

        private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = await GetItemInternalAsync<object>(key).ConfigureAwait(false),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }

        private async Task<T> GetItemInternalAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _storageProvider.GetItemAsync(key).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;
            try
            {
                return _jsonSerializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException)
            {
                return (T)(object)serialisedData;
            }
        }

        private void RaiseOnChanged(string key, object oldValue, object data)
        {
            var e = new ChangedEventArgs
            {
                Key = key,
                OldValue = oldValue,
                NewValue = data
            };

            Changed?.Invoke(this, e);
        }

        private ChangingEventArgs RaiseOnChangingSync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = GetItemInternal(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }
    }
}
