using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFNB_UWP
{
    internal class ExpiringItem<TValue>
    {
        TValue _value;
        int _lifetime;
        public ExpiringItem(TValue value, int lifetime)
        {
            _value = value;
            _lifetime = lifetime;
        }
        public TValue Value()
        {
            return _value;
        }
        public int Lifetime()
        {
            return _lifetime;
        }
        public void ResetAgeTo(int maxLifetime)
        {
            _lifetime = maxLifetime;
        }
        public void Age()
        {
            _lifetime--;
        }
    }

    internal class DictionaryWithExpiration<TKey, TValue> : IDictionary<TKey, TValue>
    {


        private Dictionary<TKey, ExpiringItem<TValue>> _storage = new Dictionary<TKey, ExpiringItem<TValue>>();

        private int _maxLifetime;

        private Func<TKey, TValue> _dataSource;

        public DictionaryWithExpiration(int maxLifetime, Func<TKey, TValue> dataSource) {
            _maxLifetime = maxLifetime;
            _dataSource = dataSource;
        }

        public TValue this[TKey key] {

            get
            {
                if (_storage.ContainsKey(key))
                {
                    _storage[key].ResetAgeTo(_maxLifetime);
                    return _storage[key].Value();
                }
                _storage[key] = new ExpiringItem<TValue>(_dataSource(key), _maxLifetime);
                RunGarbageCollection();
                return _storage[key].Value();
            }
            set
            {
                _storage[key] = new ExpiringItem<TValue>(value, _maxLifetime);
                RunGarbageCollection();
            }

        }


        public void RunGarbageCollection() {
            foreach (TKey key in _storage.Keys.ToList()) {
                _storage[key].Age();
                if (_storage[key].Lifetime() <= 0) { 
                    _storage.Remove(key);
                }
            }
        }

        public ICollection<TKey> Keys => _storage.Keys;

        public ICollection<TValue> Values() { 
            List<TValue> values = new List<TValue>();
            foreach (TKey key in _storage.Keys) {
                values.Add(_storage[key].Value());
            }
            return values;
        }

        public int Count => _storage.Count();

        public bool IsReadOnly => false;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new NotImplementedException();

        public void Add(TKey key, TValue value)
        {
            _storage[key] = new ExpiringItem<TValue>(value, _maxLifetime);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new DictionaryWithExpirationEnumerator<TKey, TValue>(this);
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class DictionaryWithExpirationEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {

        private int _position = -1;
        private int _size = 0;
        private List<TKey> _keys;

        DictionaryWithExpiration<TKey, TValue> _dictionary;

        KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public DictionaryWithExpirationEnumerator(DictionaryWithExpiration<TKey, TValue> dictionary) {
            _dictionary = dictionary;
            _size = _dictionary.Count;
            _keys = _dictionary.Keys.ToList();
        }

        KeyValuePair<TKey, TValue> Current()
        {
            return new KeyValuePair<TKey, TValue>(_keys[_position], _dictionary[_keys[_position]]);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            _position++;
            return _position < _size;
        }

        public void Reset()
        {
            _position = -1;
        }
}


    }