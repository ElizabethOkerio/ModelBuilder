﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.OData.ModelBuilder.Config;

namespace Microsoft.OData.ModelBuilder.Annotations
{
    /// <summary>
    /// Represents an annotation to add the queryable restrictions on an EDM property, including not filterable, 
    /// not sortable, not navigable, not expandable, not countable, automatically expand.
    /// </summary>
    public class QueryableRestrictionsAnnotation
    {
        /// <summary>
        /// Initializes a new instance of <see cref="QueryableRestrictionsAnnotation"/> class.
        /// </summary>
        /// <param name="restrictions">The queryable restrictions for the EDM property.</param>
        public QueryableRestrictionsAnnotation(QueryableRestrictions restrictions)
        {
            Restrictions = restrictions ?? throw new ArgumentNullException(nameof(restrictions));
        }

        /// <summary>
        /// Gets the restrictions for the EDM property.
        /// </summary>
        public QueryableRestrictions Restrictions { get; }
    }
}
