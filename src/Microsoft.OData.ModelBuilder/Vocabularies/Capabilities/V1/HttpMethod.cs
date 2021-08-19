// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// This is an auto generated file. Please run the template to modify it.
// <auto-generated />

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.ModelBuilder.Capabilities.V1
{
    /// <summary>
    /// Org.OData.Capabilities.V1.HttpMethod
    /// </summary>
    [Flags]
    public enum HttpMethod
    {
        /// <summary>
        /// The HTTP GET Method
        /// </summary>
        GET,

        /// <summary>
        /// The HTTP PATCH Method
        /// </summary>
        PATCH,

        /// <summary>
        /// The HTTP PUT Method
        /// </summary>
        PUT,

        /// <summary>
        /// The HTTP POST Method
        /// </summary>
        POST,

        /// <summary>
        /// The HTTP DELETE Method
        /// </summary>
        DELETE,

        /// <summary>
        /// The HTTP OPTIONS Method
        /// </summary>
        OPTIONS,

        /// <summary>
        /// The HTTP HEAD Method
        /// </summary>
        HEAD,
    }
}