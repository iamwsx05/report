using Microsoft.Practices.Unity;
using Report.Itf;
using System;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Report.Ui
{
    public class ProxyUploadSb : IDisposable
    {
        public ItfUploadSb Service = null;

        public ProxyUploadSb()
        {
            if (GlobalAppConfig.RunningMode == 2)
            {
                Service = Function.UnitySection("unity.xml", "unityReport", "uploadsb").Resolve<ItfUploadSb>();
            }
            else if (GlobalAppConfig.RunningMode == 3)
            {
                try
                {
                    Service = WcfEndpoint.Fac<ItfUploadSb>().CreateChannel(WcfEndpoint.HisEndpointAddress(this.GetType().Name));
                    Service.Verify();
                }
                catch
                {
                    if (WcfEndpoint.AllowChange)
                    {
                        WcfEndpoint.ChangeServer();
                        Service = WcfEndpoint.Fac<ItfUploadSb>().CreateChannel(WcfEndpoint.HisEndpointAddress(this.GetType().Name));
                    }
                }
            }
        }

        public void Dispose()
        {
            if (Service != null)
            {
                Service.Dispose();
                Service = null;
            }
        }
    }
}
