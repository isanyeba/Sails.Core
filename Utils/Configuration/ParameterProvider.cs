using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    /// <summary>
	/// 提供读取或写入参数的支持
	/// </summary>
	public abstract class ParameterProvider : IDisposable
    {
        #region 构造和析构

        
        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        protected bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }
        #endregion

        /// <summary>
        /// 读取给定名称的参数值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns></returns>
        protected abstract object GetParameterA(string name, Type parameterType, int index = 0);

        public object GetParameter(string name, Type parameterType, int index = 0)
        {
            object result = null;
            if (result != null)
                return result;

            return GetParameterA(name, parameterType, index);
        }

        /// <summary>
        /// 为给定名称的参数赋值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public abstract bool SetParameter(string name, object value);

        /// <summary>
        /// 将所有参数保存至目标源
        /// </summary>
        public abstract void SaveToSource();

        /// <summary>
        /// 判断给定的类型是否有一个ParameterProvider作为参数的构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasParameterProviderCtor(Type type)
        {
            var ctors = type.GetConstructors();
            foreach (var ctor in ctors)
            {
                var param = ctor.GetParameters();
                if (param.Length == 1 && param[0].ParameterType.IsAssignableFrom(typeof(ParameterProvider)))
                    return true;
            }
            return false;
        }

        ///// <summary>
        ///// 获取或设置给定名称的参数值
        ///// </summary>
        ///// <param name="name">参数名称</param>
        ///// <returns></returns>
        //public object this[string name]
        //{
        //    get
        //    {
        //        return GetParameterA(name);
        //    }
        //}

        /// <summary>
        /// 获取该对象是否已经被释放
        /// </summary>
        public bool Disposed
        {
            get
            {
                return _disposed;
            }
        }
    }

    public delegate void ParameterProviderMethodHandler(ParameterProvider provider);
}
