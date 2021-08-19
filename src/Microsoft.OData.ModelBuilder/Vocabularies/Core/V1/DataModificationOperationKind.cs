// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// This is an auto generated file. Please run the template to modify it.
// <auto-generated />

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.ModelBuilder.Core.V1
{
    /// <summary>
    /// Org.OData.Core.V1.DataModificationOperationKind
    /// </summary>
    public enum DataModificationOperationKind
    {
        /// <summary>
        /// Insert new instance
        /// </summary>
        insert,

        /// <summary>
        /// Update existing instance
        /// </summary>
        update,

        /// <summary>
        /// Insert new instance or update it if it already exists
        /// </summary>
        upsert,

        /// <summary>
        /// Delete existing instance
        /// </summary>
        delete,

        /// <summary>
        /// Invoke action or function
        /// </summary>
        invoke,

        /// <summary>
        /// Add link between entities
        /// </summary>
        link,

        /// <summary>
        /// Remove link between entities
        /// </summary>
        unlink,
    }
}