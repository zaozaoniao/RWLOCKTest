using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace webcore2.Controllers
{
    [ApiController]
   // [Route("[controller]")]
    public class LockCompareController : ControllerBase
    {
        public static DictionaryWapper<string, string> dw = new DictionaryWapper<string, string>();
       
        public static SynchronizedCache sc = new SynchronizedCache();
        static LockCompareController()
        {
            // 定义成静态构造函数，防止事件多次注册； 或者可以采用自定义event的 add remove块：https://blog.csdn.net/enternalstar/article/details/108372447
            // dw.OnValueChanged += new EventHandler<ValueChangedEventArgs<string>>(OnConfigUsedChanged);

        }

        [Route("monitorlock")]
        [HttpGet]
        public void Monitorlock([FromQuery] string key, [FromQuery] string value)
        {
            dw[key] = value;
        }

        [Route("rwlock")]
        [HttpGet]
        public void ReadWriteLock([FromQuery]string key,[FromQuery]string value)
        {
            //if (sc.AddOrUpdate(key, value) == SynchronizedCache.AddOrUpdateStatus.Updated)
            //{
            //    OnConfigUsedChanged("ReadWriteLockSlim", new ValueChangedEventArgs<string>(key));
            //}
            sc.AddOrUpdate(key, value);
        } 
        private static void OnConfigUsedChanged(object sender, ValueChangedEventArgs<string> e)
        {
            // TODO  Trigger Changed Event
            Console.WriteLine($"{sender} :{e.Key} changed,please pay attention.");
        }
    }
}
