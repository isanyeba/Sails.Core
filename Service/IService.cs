using Sails.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Service
{
    public abstract class IService : IDisposable
    {
        /// <summary>
        /// 初始化配置信息
        /// </summary>
        /// <param name="provider"></param>
        public virtual void Initialize(ParameterProvider provider = null)
        {

        }

        #region 构造和析构

        #region IDisposable
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        int disposedFlag;

        ~IService()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 获取该对象是否已经被释放
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public bool IsDisposed
        {
            get
            {
                return disposedFlag != 0;
            }
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (System.Threading.Interlocked.Increment(ref disposedFlag) != 1) return;
            if (disposing)
            {
                //在这里编写托管资源释放代码
                
            }
            //在这里编写非托管资源释放代码
        }

        #endregion
    }
}
