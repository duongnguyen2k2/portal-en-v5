using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.BuildFile
{
    public class BuildFile
    {
        public string? TABLE_CATALOG { get; set; }
        public string? TABLE_SCHEMA { get; set; }
        public string? TABLE_NAME { get; set; }
        public string? COLUMN_NAME { get; set; }
        public string? ORDINAL_POSITION { get; set; }
        public string? COLUMN_DEFAULT { get; set; }
        public string? IS_NULLABLE { get; set; }
        public string? DATA_TYPE { get; set; }
        public string? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public string? CHARACTER_OCTET_LENGTH { get; set; }
        public string? NUMERIC_PRECISION { get; set; }
        public string? NUMERIC_PRECISION_RADIX { get; set; }
        public string? NUMERIC_SCALE { get; set; }
    }

    public class SearchBuildFile {
        public string? ControllerName { get; set; }
        public string? TableName { get; set; }
        public string? Stored_Name { get; set; }
        public string? AngularFolder { get; set; }        
        public string? Title { get; set; }        
        public string? KeyDatabase { get; set; }        
    }

    public class LinkBuildFile
    {
        public string? Controller { get; set; }
        public string? Models { get; set; }        
        public string? ModesService { get; set; }
        public string? View { get; set; }
        public string? AngularModels { get; set; }
        public string? AngularModesService { get; set; }
        public string? AngularComponent { get; set; }
        public string? AngularHTML { get; set; }        
        public string? Database { get; set; }

        
    }

    public class ConvertAtt
    {
        public string? DBColumnName { get; set; }
        public string? DBDataType { get; set; }
        public string? API_SQL_DataType { get; set; }
        public string? API_Oracle_DataType { get; set; }
        public string? API_Angular_DataType { get; set; }
        public string? API_Data { get; set; }

    }

    public class BuildContnetDataTable
    {
        public string? Str_1 { get; set; }
        public string? Str_2 { get; set; }
    }
    public class BuildContnet {
        public string? ModelsContent {get;set;}
        public string? AngularModelsContent {get;set;}
        public BuildContnetDataTable ModelsServiceContentSaveItem { get;set;}
        public string? ModelsServiceContentGetItem { get;set;}
        public string? SQL_Params { get;set;}
        public string? SQL_SaveItem { get;set;}
        public string? SQL_SaveItemInsert { get;set;}
        public string? SQL_SaveItemInsertVal { get;set;}
        public string? ORACLE_SaveItem { get;set;}
    }

    public class BuildReplace {
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }


}
