using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Sails.Service
{
    public class ServiceRunner
    {
        public static void Entry()
        {
            AliLog.Logger log = new AliLog.Logger();
            HostFactory.Run(x =>
            {
                x.Service<BaseService>(s =>
                {
                    s.ConstructUsing(name => new BaseService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(1); //restart the service after 1 minute                        
                    rc.SetResetPeriod(1); //set the reset interval to one day
                });

                var serviceName = Assembly.GetEntryAssembly().GetName().Name;
                serviceName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
               
                x.StartAutomaticallyDelayed();
                x.SetDescription(serviceName);
                x.SetDisplayName(serviceName);
                x.SetServiceName(serviceName);
            });
        }
    }
}
