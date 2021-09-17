using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace webcore2
{
    public class ValueChangedEventArgs<TK> : EventArgs
    {
        public TK Key { get; set; }
        public ValueChangedEventArgs(TK key)
        {
            Key = key;
        }
    }

    public class DictionaryWapper<TKey, TValue>
    {
        public object objLock = new object();

        private  readonly Dictionary<TKey, TValue> _dict;
        public event EventHandler<ValueChangedEventArgs<TKey>> OnValueChanged;
        public DictionaryWapper()
        {
            _dict = new Dictionary<TKey, TValue> { };
        }
        public TValue this[TKey Key]
        {
            get { return _dict[Key]; }
            set
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                lock (objLock)
                {
                    try
                    {
                        if (_dict.TryGetValue(Key, out TValue result))
                        {

                            if (!result.Equals(value))
                            {
                              //  OnValueChanged(this, new ValueChangedEventArgs<TKey>(Key));
                                _dict[Key] = value;
                            }
                        }
                        else
                            _dict[Key] = value;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"检测变更失败，{ex}");
                    }
                }
                sw.Stop();
                Debug.WriteLine($"monitorlock: {sw.ElapsedMilliseconds}");
            }
        }
    }
}
