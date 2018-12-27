﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.CPlatform.Engines
{
   public interface IServiceEngineBuilder
    {
        void Build(ContainerBuilder serviceContainer);
    }
}
