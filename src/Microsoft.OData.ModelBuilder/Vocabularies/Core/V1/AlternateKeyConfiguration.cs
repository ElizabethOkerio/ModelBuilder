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
    /// Org.OData.Core.V1.AlternateKey
    /// </summary>
    public partial class AlternateKeyConfiguration : IRecord
    {
        private readonly Dictionary<string, object> _dynamicProperties = new Dictionary<string, object>();
        private readonly HashSet<PropertyRefConfiguration> _key = new HashSet<PropertyRefConfiguration>();

        /// <summary>
        /// Creates a new instance of <see cref="AlternateKeyConfiguration"/>
        /// </summary>
        public AlternateKeyConfiguration()
        {
        }

        /// <summary>
        /// Dynamic properties.
        /// </summary>
        /// <param name="name">The name to set</param>
        /// <param name="value">The value to set</param>
        /// <returns><see cref="AlternateKeyConfiguration"/></returns>
        public AlternateKeyConfiguration HasDynamicProperty(string name, object value)
        {
            _dynamicProperties[name] = value;
            return this;
        }

        /// <summary>
        /// The set of properties that make up this key
        /// </summary>
        /// <param name="keyConfiguration">The configuration to set</param>
        /// <returns><see cref="AlternateKeyConfiguration"/></returns>
        public AlternateKeyConfiguration HasKey(Func<PropertyRefConfiguration, PropertyRefConfiguration> keyConfiguration)
        {
            var instance = new PropertyRefConfiguration();
            instance = keyConfiguration?.Invoke(instance);
            return HasKey(instance);
        }

        /// <summary>
        /// The set of properties that make up this key
        /// </summary>
        /// <param name="key">The value(s) to set</param>
        /// <returns><see cref="AlternateKeyConfiguration"/></returns>
        public AlternateKeyConfiguration HasKey(params PropertyRefConfiguration[] key)
        {
            _key.UnionWith(key);
            return this;
        }

        /// <inheritdoc/>
        public IEdmExpression ToEdmExpression()
        {
            var properties = new List<IEdmPropertyConstructor>();

            if (_key.Any())
            {
                var collection = _key.Select(item => item.ToEdmExpression()).Where(item => item != null);
                if (collection.Any())
                {
                    properties.Add(new EdmPropertyConstructor("Key", new EdmCollectionExpression(collection)));
                }
            }

            properties.AddRange(_dynamicProperties.ToEdmProperties());

            if (!properties.Any())
            {
                return null;
            }

            return new EdmRecordExpression(properties);
        }
    }
}