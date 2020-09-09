﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.OData.ModelBuilder.Conventions.Attributes
{
    /// <summary>
    /// Base class for all attribute based conventions.
    /// </summary>
    public abstract class TypeAttributeConvention<TAttribute> : AttributeConvention
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeAttributeConvention{TAttribute}"/>.
        /// </summary>
        public TypeAttributeConvention()
            : base(attribute => attribute.GetType() == typeof(TAttribute), allowMultiple: false)
        {
        }
    }
}
