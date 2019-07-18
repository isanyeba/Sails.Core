using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public abstract class ParameterSetting : IDisposable
    {
        #region 构造和析构
        public ParameterSetting()
        {

        }

        public ParameterSetting(string configFile)
        {
            this.Initialize(configFile);
        }

        //==

        public void Dispose()
        {
            Dispose(true);
        }
        public event EventHandler Disposed;
        bool _disposed = false;

        [Browsable(false)]
        public bool IsDisposed
        {
            get { return _disposed; }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                ParameterError = null;
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
                if (Disposed != null)
                    Disposed(this, EventArgs.Empty);
            }

        }
        #endregion

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="provider">参数存储器，如果为null将使用默认的参数存储器(即读取App.Config)</param>
        public virtual void Initialize(string appconfigFile)
        {
            if (appconfigFile == null || appconfigFile == string.Empty)
                appconfigFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (File.Exists(appconfigFile))
            {
                var appConofigProvider = new AppConfigProvider(appconfigFile);
                Initialize(new AppConfigProvider[] { appConofigProvider });
                appConofigProvider.Dispose();
                appConofigProvider = null;
            }
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="provider">参数存储器，如果为null将使用默认的参数存储器(即读取App.Config)</param>
        public virtual void Initialize(params ParameterProvider[] provider)
        {
            List<ParameterProvider> providers = new List<ParameterProvider>();
            if (provider != null && provider.Length > 0)
                providers.AddRange(provider);

            Assembly ass = Assembly.GetCallingAssembly();
            string configFile1 = null;
            if (ass != null) configFile1 = ass.Location + ".config";
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string configFile2 = null;
            if (entryAssembly != null) configFile2 = entryAssembly.Location + ".config";

            if (configFile1 != null && File.Exists(configFile1))
                providers.Add(new AppConfigProvider(configFile1));
            if (configFile2 != null && configFile2 != configFile1 && File.Exists(configFile2))
                providers.Add(new AppConfigProvider(configFile2));

            Type Type = this.GetType();
            FieldInfo[] fields = Type.GetFields();
            var providerArray = providers.ToArray();
            foreach (FieldInfo field in fields)
            {
                SetFieldValue(field, GetParameter(field.Name, providerArray, field.FieldType));
            }
            provider = null;
        }

        /// <summary>
        /// 給指定对象的指定字段赋值
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="value">值</param>
        /// <param name="target">指定对象</param>
        /// <returns></returns>
        public static ParameterErrorEnum SetFieldValue(FieldInfo field, object value, object target)
        {
            if (field == null) return ParameterErrorEnum.NoFieldFoundInObject;
            if (IsNull(value)) return ParameterErrorEnum.ParameterNotFound;
            ParameterErrorEnum result = ParameterErrorEnum.UnknownParameterType;
            var isArray = field.FieldType.IsArray;
            if (isArray)
            {
                var elementType = field.FieldType.GetElementType();
                var array = (Array)value;
                if (array.Length == 0) return ParameterErrorEnum.ParameterNotFound;
                if (ConvertMapping.ContainsToObjectType(elementType))
                {
                    Array ret = Array.CreateInstance(elementType, array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        var obj = array.GetValue(i);
                        object convertedValue = null;
                        try
                        {
                            if (obj.GetType() == elementType)
                                convertedValue = obj;
                            else
                                convertedValue = ConvertMapping.ConvertToObject(elementType, obj.ToString().Trim());
                            ret.SetValue(convertedValue, i);
                        }
                        catch
                        {
                            result = ParameterErrorEnum.ExceptionAppeared;
                            break;
                        }

                    }
                    try
                    {
                        field.SetValue(target, ret);
                        result = ParameterErrorEnum.NoError;
                    }
                    catch
                    {
                        result = ParameterErrorEnum.SetParameterError;
                    }
                }
                return result;
            }
            else
            {
                if (ConvertMapping.ContainsToObjectType(field.FieldType))
                {
                    object convertedValue = null;
                    try
                    {
                        if (value.GetType() == field.FieldType)
                            convertedValue = value;
                        else
                            convertedValue = ConvertMapping.ConvertToObject(field.FieldType, value.ToString().Trim());
                    }
                    catch
                    {
                        result = ParameterErrorEnum.ExceptionAppeared;
                    }
                    try
                    {
                        field.SetValue(target, convertedValue);
                        result = ParameterErrorEnum.NoError;
                    }
                    catch
                    {
                        result = ParameterErrorEnum.SetParameterError;
                    }

                }
                else
                {
                    result = ParameterErrorEnum.UnknownParameterType;
                }

                return result;
            }
        }

        public static bool IsNull(object target)
        {
            return target == null || target == DBNull.Value;
        }
        /// <summary>
        /// 将参数保存到指定的参数提供器中
        /// </summary>
        /// <param name="provider">参数存储器，如果为null将使用默认的参数存储器(即读取App.Config)</param>
        public virtual void SaveToSource(string appconfigFile)
        {
            if (appconfigFile == null || appconfigFile == string.Empty)
                appconfigFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            SaveToSource(new AppConfigProvider[] { new AppConfigProvider(appconfigFile) });
        }

        /// <summary>
        /// 将参数保存到指定的参数提供器中
        /// </summary>
        /// <param name="provider">参数存储器，如果为null将使用默认的参数存储器(即读取App.Config)</param>
        public virtual void SaveToSource(params ParameterProvider[] provider)
        {
            ParameterProvider prov = null;
            bool needDispose = false;
            if (provider == null || provider.Length == 0)
            {
                prov = new AppConfigProvider(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                needDispose = true;
            }
            else
                prov = provider[0];

            Type Type = this.GetType();

            FieldInfo[] fields = Type.GetFields();
            foreach (FieldInfo item in fields)
            {
                prov.SetParameter(item.Name, item.GetValue(this));
            }

            prov.SaveToSource();
            if (needDispose)
            {
                prov.Dispose();
                prov = null;
            }
        }

        //==========

        /// <summary>
        /// 对指定参数的参数赋值
        /// </summary>
        /// <param name="field">参数信息</param>
        /// <param name="value">目标值</param>
        /// <returns></returns>
        protected virtual bool SetFieldValue(FieldInfo field, object value)
        {
            try
            {
                ParameterErrorEnum errCode = SetFieldValue(field, value, this);
                if (errCode == ParameterErrorEnum.NoError)
                    return true;
                else
                {
                    RaiseParameterErrorEvent(this, field, value, errCode);
                    return false;
                }
            }
            catch//(Exception ex)
            {
                RaiseParameterErrorEvent(this, field, value, ParameterErrorEnum.ExceptionAppeared);
                return false;
            }
        }

        /// <summary>
        /// 从给定的参数存储器中读取指定名称的参数配置
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="provider">参数存储器</param>
        /// <returns></returns>
        protected virtual object GetParameter(string name, ParameterProvider[] provider, Type fieldType)
        {
            if (provider == null)
            {
                return null;
            }
            if (!fieldType.IsArray)
            {
                object result = null;
                foreach (ParameterProvider item in provider)
                {
                    if (item == null) continue;
                    result = item.GetParameter(name, fieldType);
                    if (result != null)
                        break;
                }
                return result;
            }
            else
            {
                var elementType = fieldType.GetElementType();
                List<object> list = new List<object>();
                foreach (ParameterProvider item in provider)
                {
                    if (item == null) continue;
                    int index = 0;
                    while (true)
                    {
                        var result = item.GetParameter(name, elementType, index++);
                        if (result == null) break;
                        list.Add(result);
                    }
                }
                return list.ToArray();

            }
        }

        /// <summary>
        /// 为给定类型对象提供字符串到对象的转换方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method">ConvertMethodHandler委托，这是一个输入string值，返回object对象的方法</param>
        public static void SetConvertMapping(Type type, Delegate method)
        {
            ConvertMapping.SetMappingToObject(type, method, true);
        }

        #region 事件
        /// <summary>
        /// 当为某个参数赋值失败时引发的事件
        /// </summary>
        public event ParameterErrorEventHandler ParameterError;

        /// <summary>
        /// 引发ParameterError事件
        /// </summary>
        /// <param name="target">事件源</param>
        /// <param name="field">产生赋值失败时的参数</param>
        /// <param name="value">产生赋值失败时的参数目标值</param>
        /// <param name="errorNum">错误类型</param>
        protected virtual void RaiseParameterErrorEvent(object target, FieldInfo field, object value, ParameterErrorEnum errorNum)
        {
            if (ParameterError != null)
                ParameterError(target, field, value, field.GetValue(target), errorNum);
        }

        /// <summary>
        /// 引发ParameterError事件
        /// </summary>
        /// <param name="target">事件源</param>
        /// <param name="field">产生赋值失败时的参数</param>
        /// <param name="value">产生赋值失败时的参数目标值</param>
        /// <param name="errorNum">错误类型</param>
        /// <param name="defaultValue">默认值</param>
        protected virtual void RaiseParameterErrorEvent(object target, FieldInfo field, object value, object defaultValue, ParameterErrorEnum errorNum)
        {
            if (ParameterError != null)
                ParameterError(target, field, value, defaultValue, errorNum);
        }
        #endregion

        #region 辅助
        /// <summary>
        /// 赋值失败的错误枚举
        /// </summary>
        public enum ParameterErrorEnum
        {
            /// <summary>
            /// 目标为空值
            /// </summary>
            ParameterNotFound = 0,
            /// <summary>
            /// 目标为非数字
            /// </summary>
            ParameterNotIsNumber = 1,
            /// <summary>
            /// 未知的字段格式
            /// </summary>
            UnknownParameterType = 2,
            /// <summary>
            /// 指定的值不是一个有效的DateTime格式
            /// </summary>
            ParameterNotIsDateTime = 3,
            /// <summary>
            /// 指定的字符串不是一个有效的Size格式
            /// </summary>
            ParameterNotIsSize,
            /// <summary>
            /// 指定的字符串不是一个有效的Point格式
            /// </summary>
            ParameterNotIsPoint,
            /// <summary>
            /// 指定的字符串不是一个有效的Rectangle格式
            /// </summary>
            ParameterNotIsRectangle,
            /// <summary>
            /// 指定的字符串不是一个有效的TimeSpan格式
            /// </summary>
            ParameterNotIsTimeSpan,
            /// <summary>
            /// 指定的字符串不是一个有效的数字数组格式
            /// </summary>
            ParameterNotIsNumArray,
            /// <summary>
            /// 指定的字符串不是一个有效的颜色格式
            /// </summary>
            ParameterNotIsColor,
            /// <summary>
            /// 参数设置成功，没有发生异常
            /// </summary>
            NoError,
            /// <summary>
            /// 在对象中找不到指定字段
            /// </summary>
            NoFieldFoundInObject,
            /// <summary>
            /// 发生异常
            /// </summary>
            ExceptionAppeared,
            /// <summary>
            /// 未找到对应的枚举值
            /// </summary>
            NoSuchEnumMemberFound,
            /// <summary>
            /// 在给定的对象中找不到指定的属性
            /// </summary>
            NoPropertyFoundInObject,
            /// <summary>
            /// 在给对象赋值的时候出错
            /// </summary>
            SetParameterError,

        }

        /// <summary>
        /// 设置值失败时引发的事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="field"></param>
        /// <param name="destValue"></param>
        /// <param name="defaultValue"></param>
        /// <param name="description"></param>
        public delegate void ParameterErrorEventHandler(object sender, MemberInfo field, object destValue, object defaultValue, ParameterErrorEnum description);

        #endregion
    }
}
