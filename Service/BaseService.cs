using Sails.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Service
{
    public class BaseService
    {
        string serviceName = "Sails.Service.Demo";
        List<IService> services;

        public BaseService()
        {
            serviceName = Assembly.GetEntryAssembly().GetName().Name;


            #region 目录引用

#pragma warning disable 0618

            //AppDomain.CurrentDomain.AppendPrivatePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libs"));
            //AppDomain.CurrentDomain.AppendPrivatePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));
            //AppDomain.CurrentDomain.AppendPrivatePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bins"));
            //AppDomain.CurrentDomain.AppendPrivatePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib"));
            
#pragma warning restore 0618

            #endregion

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Debug.WriteLine("begined");
        }

        internal bool Start()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
                if (Directory.Exists(configPath))
                {
                    string[] files = Directory.GetFiles(configPath);
                    services = LoadService(files);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal bool Stop()
        {
            try
            {
                if (services != null && services.Count != 0)
                {
                    foreach (var service in services)
                    {
                        service.Dispose();
                    }
                    services.Clear();
                }
                //winLOG.Write($"{serviceName}已退出!");
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        List<IService> LoadService(string[] files)
        {
            List<IService> serviceList = new List<IService>();
            foreach (string cfg in files)
            {
                AppConfigProvider provider = new AppConfigProvider(cfg);
                if (!provider.CanAccess)
                {
                    provider.Dispose();
                    //winLOG.Write($"加载服务:[{Path.GetFileNameWithoutExtension(cfg)}]失败,配置文件不可读!");
                    continue;
                }

                IConfig setting = new IConfig();
                setting.Initialize(provider);
                setting.Name = Path.GetFileNameWithoutExtension(cfg);
                IService service;

                #region 加载终端数据解释器
                if (setting.ServiceAssembly == null || setting.ServiceAssembly == string.Empty)
                {
                    throw new Exception($"解析服务[{setting.Name},必须加载服务配置ServiceAssembly!");
                }
                else
                {
                    try
                    {
                        service = (IService)AssemblyHelper.CreateInstance(setting.ServiceAssembly.Split(','));
                        if (service == null)
                            throw new Exception($"解析服务[{setting.Name},无法加载!找不到指定的程序集:{setting.ServiceAssembly}");
                        service.Initialize(provider);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"解析服务[{setting.Name},在加载时发生错误!", ex);
                    }
                }
                #endregion

                serviceList.Add(service);
            }
            return serviceList;
        }
        
        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //winLOG.Write("出现未处理的应用程序异常!", e.Exception);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //winLOG.Write("出现未处理的域异常!", (Exception)e.ExceptionObject);
        }
    }
}
