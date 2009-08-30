﻿#region Usings

using System.Collections.Generic;

using ICSharpCode.Data.Core.Interfaces;
using ICSharpCode.Data.EDMDesigner.Core.EDMObjects.SSDL;
using ICSharpCode.Data.EDMDesigner.Core.EDMObjects.SSDL.EntityType;
using ICSharpCode.Data.EDMDesigner.Core.EDMObjects.SSDL.Association;
using ICSharpCode.Data.EDMDesigner.Core.EDMObjects.SSDL.Function;
using ICSharpCode.Data.EDMDesigner.Core.EDMObjects.SSDL.Property;
using ICSharpCode.Data.EDMDesigner.Core.EDMObjects.Common;

#endregion

namespace ICSharpCode.Data.EDMDesigner.Core.ObjectModelConverters
{
    internal class SSDLConverter
    {
        #region Public methods

        public static SSDLContainer CreateSSDLContainer(IDatabase database, string modelNamespace)
        {
            SSDLContainer ssdlContainer = new SSDLContainer()
            {
                Name = database.Name.Replace(".", string.Empty) + "StoreContainer",
                Namespace = modelNamespace,
                Provider = database.Datasource.DatabaseDriver.ProviderName,
                ProviderManifestToken = database.Datasource.ProviderManifestToken
            };

            List<ITable> tablesAndViews = new List<ITable>();
            tablesAndViews.AddRange(database.Tables);
            tablesAndViews.AddRange(database.Views);

            foreach (ITable table in tablesAndViews)
            {
                if (!table.IsSelected)
                    continue;

                EntityType entityType = CreateSSDLEntityType(table);
                ssdlContainer.EntityTypes.Add(entityType);

                if (table.Constraints != null)
                {
                    foreach (IConstraint constraint in table.Constraints)
                    {
                        if (constraint.FKTableName != table.TableName)
                            continue;

                        Association association = CreateSSDLAssociation(constraint);
                        ssdlContainer.AssociationSets.Add(association);
                    }
                }
            }

            foreach (IProcedure procedure in database.Procedures)
            {
                if (!procedure.IsSelected)
                    continue;
                
                Function function = CreateSSDLFunction(procedure);
                ssdlContainer.Functions.Add(function);
            }

            return ssdlContainer;
        }

        #endregion

        #region Private methods

        private static EntityType CreateSSDLEntityType(ITable table)
        {
            EntityType entityType = new EntityType()
            {
                Name = table.TableName,
                EntitySetName = table.TableName,
                Schema = table.SchemaName
            };

            if (table is IView)
            {
                entityType.StoreType = StoreType.Views;
                entityType.DefiningQuery = (table as IView).DefiningQuery;
            }
            else
                entityType.StoreType = StoreType.Tables;

            foreach (IColumn column in table.Items)
            {
                entityType.Properties.Add(CreateSSDLProperty(column, entityType));
            }

            return entityType;
        }

        private static Function CreateSSDLFunction(IProcedure procedure)
        {
            Function function = new Function()
            {
                Name = procedure.Name,
                Schema = procedure.SchemaName,
                Aggregate = false,
                BuiltIn = false,
                NiladicFunction = false,
                IsComposable = false,
                ParameterTypeSemantics = ParameterTypeSemantics.AllowImplicitConversion
            };

            foreach (IProcedureParameter procedureParameter in procedure.Items)
            {
                FunctionParameter functionParameter = new FunctionParameter()
                {
                    Name = procedureParameter.Name,
                    Type = procedureParameter.DataType,
                    Mode = (ParameterMode)procedureParameter.ParameterMode
                };

                function.Parameters.Add(functionParameter);
            }

            return function;
        }

        private static Property CreateSSDLProperty(IColumn column, EntityType entityType)
        {
            Property property = new Property(entityType)
            {
                Name = column.Name,
                Type = column.DataType,
                IsKey = column.IsPrimaryKey,
                Nullable = column.IsNullable,
                // FixedLength
                // Collation
                // DefaultValue
                // Unicode
            };

            if (column.Length > 0)
                property.MaxLength = column.Length;

            //if (column.Precision != 0)
            //    property.Precision = column.Precision;

            //if (column.Scale != 0)
            //    property.Scale = column.Scale;

            if (column.IsIdentity)
                property.StoreGeneratedPattern = StoreGeneratedPattern.Identity;
            else if (column.IsComputed)
                property.StoreGeneratedPattern = StoreGeneratedPattern.Computed;

            return property;
        }

        private static Association CreateSSDLAssociation(IConstraint constraint)
        {
            Association association = new Association()
            {
                Name = constraint.Name,
                AssociationSetName = constraint.Name
            };

            Role role1 = new Role()
            {
                Name = constraint.PKTableName,
                Cardinality = (Cardinality)constraint.PKCardinality
            };

            Property role1Property = CreateSSDLProperty(constraint.PKColumn, CreateSSDLEntityType(constraint.PKTable));
            role1.Property = role1Property;
            role1.Type = role1Property.EntityType;

            association.Role1 = role1;
            association.PrincipalRole = role1;

            Role role2 = new Role()
            {
                Name = constraint.FKTableName,
                Cardinality = (Cardinality)constraint.FKCardinality
            }; 

            Property role2Property = CreateSSDLProperty(constraint.FKColumn, CreateSSDLEntityType(constraint.FKTable));
            role2.Property = role2Property;
            role2.Type = role2Property.EntityType;

            association.Role2 = role2;
            association.DependantRole = role2;

            return association;
        }

        #endregion
    }
}