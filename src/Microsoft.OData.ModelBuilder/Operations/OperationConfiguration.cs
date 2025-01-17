﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.OData.ModelBuilder
{
    /// <summary>
    /// Represents an Operation (Edm Function or Edm Action) that is exposed in the Edm model
    /// </summary>
    public abstract class OperationConfiguration
    {
        private List<ParameterConfiguration> _parameters = new List<ParameterConfiguration>();
        private BindingParameterConfiguration _bindingParameter;
        private string _namespace;

        /// <summary>
        /// Initializes a new instance of <see cref="OperationConfiguration" /> class.
        /// </summary>
        /// <param name="builder">The ODataModelBuilder to which this OperationConfiguration should be added.</param>
        /// <param name="name">The name of this OperationConfiguration.</param>
        internal OperationConfiguration(ODataModelBuilder builder, string name)
        {
            Name = name;
            ModelBuilder = builder;
        }

        /// <summary>
        /// Gets a value indicating whether operation links follow OData conventions.
        /// </summary>
        public bool FollowsConventions { get; protected set; }

        /// <summary>
        /// The Name of the operation
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The Title of the operation. When customized, the title of the operation
        /// will be sent back when the OData client asks for an entity or a feed in
        /// JSON full metadata.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Kind of operation, which can be either Action or Function
        /// </summary>
        public abstract OperationKind Kind { get; }

        /// <summary>
        /// Does the operation have side-effects.
        /// </summary>
        public abstract bool IsSideEffecting { get; }

        /// <summary>
        /// The FullyQualifiedName is the Name further qualified using the Namespace.
        /// </summary>
        public string FullyQualifiedName => Namespace + "." + Name;

        /// <summary>
        /// The Namespace by default is the ModelBuilder's Namespace.
        /// </summary>
        public string Namespace
        {
            get { return _namespace ?? ModelBuilder.Namespace; }
            set { _namespace = value; }
        }

        /// <summary>
        /// The type returned when the operation is invoked.
        /// </summary>
        public IEdmTypeConfiguration ReturnType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the return is nullable or not.
        /// </summary>
        public bool ReturnNullable { get; set; }

        /// <summary>
        /// The Navigation Source that are returned from.
        /// </summary>
        public NavigationSourceConfiguration NavigationSource { get; set; }

        /// <summary>
        /// The EntitySetPathExpression that entities are returned from.
        /// </summary>
        public IEnumerable<string> EntitySetPath { get; internal set; }

        /// <summary>
        /// Vocabulary builders to annotate this <see cref="OperationConfiguration"/>
        /// </summary> 
        public Dictionary<Type, VocabularyTermConfiguration> VocabularyTermConfigurations { get; } = new Dictionary<Type, VocabularyTermConfiguration>();

        /// <summary>
        /// Get the bindingParameter.
        /// <remarks>Null means the operation has no bindingParameter.</remarks>
        /// </summary>
        public virtual BindingParameterConfiguration BindingParameter
        {
            get { return _bindingParameter; }
        }

        /// <summary>
        /// The parameters the operation takes
        /// </summary>
        public virtual IEnumerable<ParameterConfiguration> Parameters
        {
            get
            {
                if (_bindingParameter != null)
                {
                    yield return _bindingParameter;
                }
                foreach (ParameterConfiguration parameter in _parameters)
                {
                    yield return parameter;
                }
            }
        }

        /// <summary>
        /// Can the operation be bound to a URL representing the BindingParameter.
        /// </summary>
        public virtual bool IsBindable => _bindingParameter != null;

        /// <summary>
        /// Sets the return type to a single EntityType instance.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entitySetName">The entitySetName which contains the return EntityType instance.</param>
        internal void ReturnsFromEntitySetImplementation(Type entityType, string entitySetName)
        {
            EntityTypeConfiguration entity = ModelBuilder.AddEntityType(entityType);
            NavigationSource = ModelBuilder.AddEntitySet(entitySetName, entity);
            ReturnType = ModelBuilder.GetTypeConfigurationOrNull(entityType);
            ReturnNullable = true;
        }

        /// <summary>
        /// Sets the return type to a collection of EntityType instances.
        /// </summary>
        /// <param name="elementEntityType">The element entity type.</param>
        /// <param name="entitySetName">The entitySetName which contains the returned EntityType instances.</param>
        internal void ReturnsCollectionFromEntitySetImplementation(Type elementEntityType, string entitySetName)
        {
            EntityTypeConfiguration entity = ModelBuilder.AddEntityType(elementEntityType);
            NavigationSource = ModelBuilder.AddEntitySet(entitySetName, entity);
            IEdmTypeConfiguration elementType = ModelBuilder.GetTypeConfigurationOrNull(elementEntityType);
            Type clrCollectionType = typeof(IEnumerable<>).MakeGenericType(elementEntityType);
            ReturnType = new CollectionTypeConfiguration(elementType, clrCollectionType);
            ReturnNullable = true;
        }

        /// <summary>
        /// Sets the return type to a single EntityType instance.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entitySetPath">The entitySetPath which contains the return EntityType instance.</param>
        internal void ReturnsEntityViaEntitySetPathImplementation(Type entityType, IEnumerable<string> entitySetPath)
        {
            ReturnType = ModelBuilder.GetTypeConfigurationOrNull(entityType);
            EntitySetPath = entitySetPath;
            ReturnNullable = true;
        }

        /// <summary>
        /// Sets the return type to a collection of EntityType instances.
        /// </summary>
        /// <param name="clrElementEntityType">The element entity type.</param>
        /// <param name="entitySetPath">The entitySetPath which contains the returned EntityType instances.</param>
        internal void ReturnsCollectionViaEntitySetPathImplementation(Type clrElementEntityType, IEnumerable<string> entitySetPath)
        {
            IEdmTypeConfiguration elementType = ModelBuilder.GetTypeConfigurationOrNull(clrElementEntityType);
            Type clrCollectionType = typeof(IEnumerable<>).MakeGenericType(clrElementEntityType);
            ReturnType = new CollectionTypeConfiguration(elementType, clrCollectionType);
            EntitySetPath = entitySetPath;
            ReturnNullable = true;
        }

        /// <summary>
        /// Established the return type of the operation.
        /// <remarks>Used when the return type is a single Primitive or ComplexType.</remarks>
        /// </summary>
        internal void ReturnsImplementation(Type clrReturnType)
        {
            IEdmTypeConfiguration configuration = GetOperationTypeConfiguration(clrReturnType);
            ReturnType = configuration;
            ReturnNullable = TypeHelper.IsNullable(clrReturnType);
        }

        /// <summary>
        /// Establishes the return type of the operation
        /// <remarks>Used when the return type is a collection of either Primitive or ComplexTypes.</remarks>
        /// </summary>
        internal void ReturnsCollectionImplementation(Type clrElementType)
        {
            // TODO: I don't like this temporary solution that says the CLR type of the collection is IEnumerable<T>.
            // It basically has no meaning. That said the CLR type is meaningful for IEdmTypeConfiguration
            // because I still think it is useful for IEdmPrimitiveTypes too.
            // You can imagine the override of this that takes a delegate using the correct CLR type for the return type.
            Type clrCollectionType = typeof(IEnumerable<>).MakeGenericType(clrElementType);
            IEdmTypeConfiguration edmElementType = GetOperationTypeConfiguration(clrElementType);
            ReturnType = new CollectionTypeConfiguration(edmElementType, clrCollectionType);
            ReturnNullable = TypeHelper.IsNullable(clrElementType);
        }

        /// <summary>
        /// Specifies the bindingParameter name and type.
        /// </summary>
        internal void SetBindingParameterImplementation(string name, IEdmTypeConfiguration bindingParameterType)
        {
            _bindingParameter = new BindingParameterConfiguration(name, bindingParameterType);
        }

        /// <summary>
        /// Adds a new non-binding parameter.
        /// </summary>
        public ParameterConfiguration AddParameter(string name, IEdmTypeConfiguration parameterType)
        {
            ParameterConfiguration parameter = new NonbindingParameterConfiguration(name, parameterType);
            _parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new non-binding parameter
        /// </summary>  
        public ParameterConfiguration Parameter(Type clrParameterType, string name)
        {
            if (clrParameterType == null)
            {
                throw Error.ArgumentNull("clrParameterType");
            }

            IEdmTypeConfiguration parameterType = GetOperationTypeConfiguration(clrParameterType);
            return AddParameter(name, parameterType);
        }

        /// <summary>
        /// Adds a new non-binding parameter
        /// </summary>
        public ParameterConfiguration Parameter<TParameter>(string name)
        {
            return this.Parameter(typeof(TParameter), name);
        }

        /// <summary>
        /// Adds a new non-binding collection parameter
        /// </summary>
        public ParameterConfiguration CollectionParameter<TElementType>(string name)
        {
            Type elementType = typeof(TElementType);
            IEdmTypeConfiguration elementTypeConfiguration = GetOperationTypeConfiguration(typeof(TElementType));
            CollectionTypeConfiguration parameterType = new CollectionTypeConfiguration(elementTypeConfiguration, typeof(IEnumerable<>).MakeGenericType(elementType));
            return AddParameter(name, parameterType);
        }

        /// <summary>
        /// Adds a new non-binding entity type parameter.
        /// </summary>
        public ParameterConfiguration EntityParameter<TEntityType>(string name) where TEntityType : class
        {
            Type entityType = typeof(TEntityType);
            IEdmTypeConfiguration parameterType =
                ModelBuilder.StructuralTypes.FirstOrDefault(t => t.ClrType == entityType) ??
                ModelBuilder.AddEntityType(entityType);

            return AddParameter(name, parameterType);
        }

        /// <summary>
        /// Adds a new non-binding collection of entity type parameter.
        /// </summary>
        public ParameterConfiguration CollectionEntityParameter<TElementEntityType>(string name) where TElementEntityType : class
        {
            Type elementType = typeof(TElementEntityType);
            IEdmTypeConfiguration elementTypeConfiguration =
                ModelBuilder.StructuralTypes.FirstOrDefault(t => t.ClrType == elementType) ??
                ModelBuilder.AddEntityType(elementType);

            CollectionTypeConfiguration parameterType = new CollectionTypeConfiguration(elementTypeConfiguration,
                typeof(IEnumerable<>).MakeGenericType(elementType));

            return AddParameter(name, parameterType);
        }

        /// <summary>
        /// Gets or sets the <see cref="ODataModelBuilder"/> used to create this configuration.
        /// </summary>
        protected ODataModelBuilder ModelBuilder { get; set; }

        private IEdmTypeConfiguration GetOperationTypeConfiguration(Type clrType)
        {
            Type type = TypeHelper.GetUnderlyingTypeOrSelf(clrType);
            IEdmTypeConfiguration edmTypeConfiguration;

            if (TypeHelper.IsEnum(type))
            {
                edmTypeConfiguration = ModelBuilder.GetTypeConfigurationOrNull(type);

                if (edmTypeConfiguration != null && TypeHelper.IsNullable(clrType))
                {
                    edmTypeConfiguration = ((EnumTypeConfiguration)edmTypeConfiguration).GetNullableEnumTypeConfiguration();
                }
            }
            else
            {
                edmTypeConfiguration = ModelBuilder.GetTypeConfigurationOrNull(clrType);
            }

            if (edmTypeConfiguration == null)
            {
                if (TypeHelper.IsEnum(type))
                {
                    EnumTypeConfiguration enumTypeConfiguration = ModelBuilder.AddEnumType(type);

                    if (TypeHelper.IsNullable(clrType))
                    {
                        edmTypeConfiguration = enumTypeConfiguration.GetNullableEnumTypeConfiguration();
                    }
                    else
                    {
                        edmTypeConfiguration = enumTypeConfiguration;
                    }
                }
                else
                {
                    edmTypeConfiguration = ModelBuilder.AddComplexType(clrType);
                }
            }

            return edmTypeConfiguration;
        }
    }
}
