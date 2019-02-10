﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.Commands.Sql.DataClassification
{
    public class DefinitionsCommon
    {
        internal const string ParentResourceColumnParameterSet = "ParentResourceColumnParameterSet";
        internal const string ColumnParameterSet = "ColumnParameterSet";
        internal const string ParentResourceParameterSet = "ParentResourceParameterSet";
        internal const string DatabaseParameterSet = "DatabaseParameterSet";
        internal const string SqlDatabaseSensitivityClassification = "SqlDatabaseSensitivityClassification";
        internal const string ManagedDatabaseSensitivityClassification = "ManagedDatabaseSensitivityClassification";
        internal const string ResourceGroupNameHelpMessage = "The name of the resource group.";
        internal const string ServerNameHelpMessage = "SQL server name.";
        internal const string InstanceNameHelpMessage = "SQL Managed Instance name.";
        internal const string DatabaseNameHelpMessage = "SQL Database name.";
        internal const string LabelNameHelpMessage = "A name that describes the sensitivity of the data  stored in the column.";
        internal const string InformationTypeHelpMessage = "A name that describes the information type of the data  stored in the column.";
        internal const string ManagedDatabaseNameHelpMessage = "SQL Managed Database name.";
        internal const string SqlDatabaseInputObjectHelpMessage = "The SQL database object.";
        internal const string ManagedDatabaseInputObjectHelpMessage = "The managed database object.";
        internal const string SqlDatabaseSensitivityClassificationInputObjectHelpMessage = "An object representing a SQL Database Sensitivity Classification";
        internal const string ManagedDatabaseSensitivityClassificationInputObjectHelpMessage = "An object representing a SQL Managed Database Sensitivity Classification";
        internal const string SchemaNameHelpMessage = "Name of schema.";
        internal const string TableNameHelpMessage = "Name of table.";
        internal const string ColumnNameHelpMessage = "Name of column.";
    }
}
