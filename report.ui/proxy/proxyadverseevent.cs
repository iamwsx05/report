using Microsoft.Practices.Unity;
using Report.Itf;
using System;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Report.Ui
{
    public class ProxyAdverseEvent : IDisposable
    {
        public ItfAdverseEvent Service = null;

        public ProxyAdverseEvent()
        {
            if (GlobalAppConfig.RunningMode == 2)
            {
                Service = Function.UnitySection("unity.xml", "unityReport", "event").Resolve<ItfAdverseEvent>();
            }
            else if (GlobalAppConfig.RunningMode == 3)
            {
                try
                {
                    Service = WcfEndpoint.Fac<ItfAdverseEvent>().CreateChannel(WcfEndpoint.HisEndpointAddress(this.GetType().Name));
                    Service.Verify();
                }
                catch
                {
                    if (WcfEndpoint.AllowChange)
                    {
                        WcfEndpoint.ChangeServer();
                        Service = WcfEndpoint.Fac<ItfAdverseEvent>().CreateChannel(WcfEndpoint.HisEndpointAddress(this.GetType().Name));
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
