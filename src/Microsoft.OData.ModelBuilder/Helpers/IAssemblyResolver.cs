﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.OData.ModelBuilder
{
    /// <summary>
    /// The Assembly resolver interface.
    /// </summary>
    public interface IAssemblyResolver
    {
        /// <summary>
        /// Gets a list of assemblies available for the application.
        /// </summary>
        /// <returns>A list of assemblies available for the application. </returns>
        IEnumerable<Assembly> Assemblies { get; }
    }
}
