using MicroService.EntityFramwork;
using MicroService.EntityFramwork.Mysql;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Module;
using Surging.Core.ProxyGenerator;
using Surging.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.IModules.Org
{
    /// <summary>
    /// 
    /// </summary>
   public class IntercepteModule: SystemModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public override void Initialize(CPlatformContainer serviceProvider)
        {
            base.Initialize(serviceProvider);
        }

        /// <summary>
        /// Inject dependent third-party components
        /// </summary>
        /// <param name="builder"></param>
        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
            base.RegisterBuilder(builder);
           
            builder.AddClientIntercepted(typeof(CacheProviderInterceptor));
        }
    }
}