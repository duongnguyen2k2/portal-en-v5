using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.BuildFile;
using API.Models;
namespace API.Areas.Admin.Models.BuildFile
{
    public class BuildFileService
    {
        
        public static List<BuildFile> GetList(string TABLE_NAME)
        {            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "BuildFile",
                new string[] { "@flag", "@TABLE_NAME" }, new object[] { "GetList", TABLE_NAME });
            return (from r in tabl.AsEnumerable()
                    select new BuildFile
                    {
                        TABLE_NAME = (string)r["TABLE_NAME"],
                        COLUMN_NAME = (string)r["COLUMN_NAME"],
                        DATA_TYPE = (string)r["DATA_TYPE"],
                        IS_NULLABLE = (string)r["IS_NULLABLE"],
                        CHARACTER_MAXIMUM_LENGTH = (string)((r["CHARACTER_MAXIMUM_LENGTH"] == System.DBNull.Value) ? "" : r["CHARACTER_MAXIMUM_LENGTH"].ToString()),
                    }).ToList();

        }

        public string? BuildCodeFile(SearchBuildFile dto, BuildContnet Content,string SourceLink,string DestLink,string FolderLink,string Flag,string Ext)
        {
            string sourceFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BuildFile/"+SourceLink + Ext);
            string destFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BuildFileTemp/"+ DestLink + Ext);
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BuildFileTemp/"+ FolderLink)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BuildFileTemp/"+ FolderLink));
            }
            StringBuilder sb = new StringBuilder();

            using (StreamReader sr = new StreamReader(sourceFile))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {

                    if (Flag == "BuildAngularModels")
                    {
                        line = line.Replace("[[ControllerName]]", dto.ControllerName);
                        line = line.Replace("[[AngularModelsContent]]", Content.AngularModelsContent);
                    }
                    else if (Flag == "AngularModesService" || Flag == "AngularComponent")
                    {
                        line = line.Replace("[[ControllerName]]", dto.ControllerName);
                        line = line.Replace("[[AngularFolder]]", dto.AngularFolder);
                        line = line.Replace("[[title]]", "\"" + dto.Title + "\"");
                    }
                    else if (Flag == "AngularHTML")
                    {
                        //line = line.Replace("[[title]]", dto.Title);                        
                    }
                    else if (Flag == "BuildController")
                    {
                        line = line.Replace("{{ControllerName}}", dto.ControllerName);
                    }
                    else if (Flag == "BuildModels")
                    {
                        line = line.Replace("{{ControllerName}}", dto.ControllerName);
                        line = line.Replace("{{ModelsContent}}", Content.ModelsContent);
                    }
                    else if (Flag == "BuildModelsService")
                    {
                        line = line.Replace("{{ControllerName}}", dto.ControllerName);
                        line = line.Replace("{{Stored_Name}}", dto.Stored_Name);
                        line = line.Replace("{{ModelsServiceContentSaveItem_Str_1}}", Content.ModelsServiceContentSaveItem.Str_1);
                        line = line.Replace("{{ModelsServiceContentSaveItem_Str_2}}", Content.ModelsServiceContentSaveItem.Str_2);
                        line = line.Replace("{{ModelsServiceContentGetItem}}", Content.ModelsServiceContentGetItem);
                    }
                    else if (Flag == "Database") {
                        line = line.Replace("{{TableName}}", dto.TableName);
                        line = line.Replace("{{Stored_Name}}", dto.Stored_Name);
                        line = line.Replace("{{KeyDatabase}}", dto.KeyDatabase);
                        line = line.Replace("{{SQL_SaveItem}}", Content.SQL_SaveItem);
                        line = line.Replace("{{SQL_SaveItemInsert}}", Content.SQL_SaveItemInsert);
                        line = line.Replace("{{SQL_SaveItemInsertVal}}", Content.SQL_SaveItemInsertVal);
                        line = line.Replace("{{SQL_Params}}", Content.SQL_Params);
                    }
                    
                    sb.AppendLine(line);
                }
                sr.Close();
            }

            using (StreamWriter sWriter = new StreamWriter(destFile, false))
            {
                sWriter.Write(sb.ToString());
                sWriter.Flush();
                sWriter.Close();
            }
            return destFile;

        }
        

        public static List<ConvertAtt> GetListConvertAtt()
        {
            List<ConvertAtt> listItem = new List<ConvertAtt>(){};
            listItem.Add(new ConvertAtt() {DBDataType = "int",API_Oracle_DataType = "",API_SQL_DataType="int",API_Angular_DataType = "number",API_Data="0"});
            listItem.Add(new ConvertAtt() {DBDataType = "float", API_Oracle_DataType = "",API_SQL_DataType= "float", API_Angular_DataType = "number",API_Data="0"});
            listItem.Add(new ConvertAtt() {DBDataType = "nvarchar", API_Oracle_DataType = "",API_SQL_DataType="string",API_Angular_DataType = "string",API_Data= "\"\"" });
            listItem.Add(new ConvertAtt() {DBDataType = "varchar", API_Oracle_DataType = "",API_SQL_DataType="string",API_Angular_DataType = "string", API_Data = "\"\"" });
            listItem.Add(new ConvertAtt() {DBDataType = "bit", API_Oracle_DataType = "",API_SQL_DataType= "Boolean", API_Angular_DataType = "Boolean", API_Data = "true" });
            listItem.Add(new ConvertAtt() {DBDataType = "datetime", API_Oracle_DataType = "",API_SQL_DataType= "DateTime", API_Angular_DataType = "Date", API_Data ="null" });
            listItem.Add(new ConvertAtt() {DBDataType = "tinyint", API_Oracle_DataType = "",API_SQL_DataType= "Byte", API_Angular_DataType = "number", API_Data = "0" });
            return listItem;                        
        }

    }
}

