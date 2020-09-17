﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.OData.ModelBuilder
{
    /// <summary>
    /// Default implementation of <see cref="IAssemblyResolver"/>
    /// </summary>
    public class DefaultAssemblyResolver : IAssemblyResolver
    {
        private Assembly[] _assemblies = GetAssembliesInteral();

        /// <summary>
        /// This static instance is used in the shared code in places where the request container context
        /// is not known or does not contain an instance of IWebApiAssembliesResolver.
        /// </summary>
        internal static IAssemblyResolver Default = new DefaultAssemblyResolver();

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        public IEnumerable<Assembly> Assemblies => _assemblies;

        private static Assembly[] GetAssembliesInteral()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
