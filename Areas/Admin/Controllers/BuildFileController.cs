using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System.IO;
using System.Text;
using API.Areas.Admin.Models.BuildFile;

namespace API.Controllers
{
    [Area("Admin")]
    public class BuildFileController : Controller
    {       
        [HttpPost]
        public IActionResult TaoFile([FromBody]SearchBuildFile dto)
        {
            try
            {
                List<BuildFile> listBuildFile = BuildFileService.GetList(dto.TableName);
                List<ConvertAtt> listConvertAtrt = BuildFileService.GetListConvertAtt();
                BuildFileService Service = new BuildFileService();
                LinkBuildFile link = new LinkBuildFile();
                BuildContnet Content = new BuildContnet();
                string ModelsServiceContentSaveItem_Str_1 = "\"@flag\"";
                string ModelsServiceContentSaveItem_Str_2 = "\"SaveItem\"";
                string tab_service = "\r\n \t\t\t\t\t\t";
                string tab_service_angular = "\r\n \t";
                for (int i = 0; i < listBuildFile.Count(); i++){
                    string IS_NULL = "?";
                    ModelsServiceContentSaveItem_Str_1 = ModelsServiceContentSaveItem_Str_1 + ",\"@" + listBuildFile[i].COLUMN_NAME+ "\"";
                    ModelsServiceContentSaveItem_Str_2 = ModelsServiceContentSaveItem_Str_2 + ",dto." + listBuildFile[i].COLUMN_NAME;
                    if (i == listBuildFile.Count() - 1) { tab_service = ""; tab_service_angular = ""; }
                    if (i == 0)
                    {
                        //Content.SQL_SaveItem = listBuildFile[i].COLUMN_NAME + " = @" + listBuildFile[i].COLUMN_NAME; ;
                    }
                    else if (i == 1) {
                        Content.SQL_SaveItem = listBuildFile[i].COLUMN_NAME + " = @" + listBuildFile[i].COLUMN_NAME; 
                        Content.SQL_SaveItemInsert =  listBuildFile[i].COLUMN_NAME;
                        Content.SQL_SaveItemInsertVal =  "@"+listBuildFile[i].COLUMN_NAME;
                    }
                    else
                    {
                        if (listBuildFile[i].COLUMN_NAME == "CreatedBy")
                        {
                            Content.SQL_SaveItemInsert = Content.SQL_SaveItemInsert + "," + listBuildFile[i].COLUMN_NAME; ;
                            Content.SQL_SaveItemInsertVal = Content.SQL_SaveItemInsertVal + ",@" + listBuildFile[i].COLUMN_NAME;
                        }
                        else if (listBuildFile[i].COLUMN_NAME == "CreatedDate")
                        {
                            Content.SQL_SaveItemInsert = Content.SQL_SaveItemInsert + "," + listBuildFile[i].COLUMN_NAME; ;
                            Content.SQL_SaveItemInsertVal = Content.SQL_SaveItemInsertVal + ",GETDATE()";
                        }
                        else if (listBuildFile[i].COLUMN_NAME == "ModifiedBy")
                        {
                            Content.SQL_SaveItem = Content.SQL_SaveItem + "," + listBuildFile[i].COLUMN_NAME + " = @" + listBuildFile[i].COLUMN_NAME;
                        }
                        else if (listBuildFile[i].COLUMN_NAME == "ModifiedDate")
                        {
                            Content.SQL_SaveItem = Content.SQL_SaveItem + "," + listBuildFile[i].COLUMN_NAME + " = GETDATE()";
                        } else if (listBuildFile[i].COLUMN_NAME == "Deleted") {
                            Content.SQL_SaveItemInsert = Content.SQL_SaveItemInsert + "," + listBuildFile[i].COLUMN_NAME; ;
                            Content.SQL_SaveItemInsertVal = Content.SQL_SaveItemInsertVal + ",0" ;
                            Content.SQL_SaveItem = Content.SQL_SaveItem + "," + listBuildFile[i].COLUMN_NAME + " = 0";
                        }
                        else {
                            Content.SQL_SaveItemInsert = Content.SQL_SaveItemInsert + "," + listBuildFile[i].COLUMN_NAME; ;
                            Content.SQL_SaveItemInsertVal = Content.SQL_SaveItemInsertVal + ",@" + listBuildFile[i].COLUMN_NAME;
                            Content.SQL_SaveItem = Content.SQL_SaveItem + "," + listBuildFile[i].COLUMN_NAME + " = @" + listBuildFile[i].COLUMN_NAME;

                        }
                       
                    }
                    
                    for (int j = 0; j < listConvertAtrt.Count(); j++) {
                        if (listBuildFile[i].DATA_TYPE == listConvertAtrt[j].DBDataType) {
                            Content.AngularModelsContent = Content.AngularModelsContent + listBuildFile[i].COLUMN_NAME +":"+ listConvertAtrt[j].API_Angular_DataType+ "="+ listConvertAtrt[j].API_Data+ ";"+ tab_service_angular;
                            if (listBuildFile[i].IS_NULLABLE == "YES")
                            {
                                if (listConvertAtrt[j].API_SQL_DataType == "string") {
                                    IS_NULL = "";
                                }
                                Content.ModelsContent = Content.ModelsContent + "public " + listConvertAtrt[j].API_SQL_DataType + IS_NULL+ " " + listBuildFile[i].COLUMN_NAME + " { get; set; }\r\n \t\t";                                
                                Content.ModelsServiceContentGetItem = Content.ModelsServiceContentGetItem + listBuildFile[i].COLUMN_NAME + " = (" + listConvertAtrt[j].API_SQL_DataType + IS_NULL+ ")((r[" + "\"" + listBuildFile[i].COLUMN_NAME + "\"" + "] == System.DBNull.Value) ? "+ "null" + " : r[" + "\"" + listBuildFile[i].COLUMN_NAME + "\"" + "]),"+tab_service;
                            }
                            else {
                                Content.ModelsContent = Content.ModelsContent + "public " + listConvertAtrt[j].API_SQL_DataType + " " + listBuildFile[i].COLUMN_NAME + " { get; set; }\r\n \t\t";
                                Content.ModelsServiceContentGetItem = Content.ModelsServiceContentGetItem + listBuildFile[i].COLUMN_NAME + " = (" + listConvertAtrt[j].API_SQL_DataType + ")r[" + "\"" + listBuildFile[i].COLUMN_NAME + "\"" + "],"+tab_service;
                            }

                            //-- Buil  code database
                            if (listConvertAtrt[j].API_SQL_DataType == "string")
                            {
                                Content.SQL_Params = Content.SQL_Params + "@" + listBuildFile[i].COLUMN_NAME + " " + listBuildFile[i].DATA_TYPE + "(" + listBuildFile[i].CHARACTER_MAXIMUM_LENGTH + ")=null,  \r\n \t";
                            }                           
                            else
                            {
                                Content.SQL_Params = Content.SQL_Params + "@" + listBuildFile[i].COLUMN_NAME + " " + listBuildFile[i].DATA_TYPE + "="+ listConvertAtrt[j].API_Data + ",  \r\n \t";
                            }
                            //-- End Buil  code database

                        }
                    }
                }
                Content.ModelsServiceContentSaveItem = new BuildContnetDataTable()
                {
                    Str_1 = ModelsServiceContentSaveItem_Str_1,
                    Str_2 = ModelsServiceContentSaveItem_Str_2
                };                
                
                string DestLinkAngM = dto.AngularFolder + "/" + dto.AngularFolder;
                string DestLinkAngMS = dto.AngularFolder + "/" + dto.AngularFolder + ".service";
                string DestLinkAngComponent = dto.AngularFolder + "/" + dto.AngularFolder + ".component";
                string DestLinkAngularHTML = dto.AngularFolder + "/" + dto.AngularFolder + ".component";
               
                //link.AngularModels = Service.BuildCodeFile(dto, Content, "Angular/Angular", DestLinkAngM, dto.AngularFolder + "/", "BuildAngularModels",".ts");
                //link.AngularModesService = Service.BuildCodeFile(dto, Content, "Angular/Angular.service", DestLinkAngMS, dto.AngularFolder + "/", "AngularModesService", ".ts");
                //link.AngularComponent = Service.BuildCodeFile(dto, Content, "Angular/Angular.component", DestLinkAngComponent, dto.AngularFolder + "/", "AngularComponent", ".ts");
                //link.AngularHTML = Service.BuildCodeFile(dto, Content, "Angular/Angular.component", DestLinkAngularHTML, dto.AngularFolder + "/", "AngularHTML", ".html");
                link.Controller = Service.BuildCodeFile(dto, Content, "Controller", dto.ControllerName+ "Controller", "", "BuildController",".cs");
                link.Models = Service.BuildCodeFile(dto, Content, "Models", dto.ControllerName + "/"+ dto.ControllerName, dto.ControllerName, "BuildModels", ".cs");
                link.ModesService = Service.BuildCodeFile(dto, Content, "ModelsService", dto.ControllerName + "/" + dto.ControllerName+ "Service", dto.ControllerName, "BuildModelsService", ".cs");             
                link.Database = Service.BuildCodeFile(dto, Content, "Database", dto.ControllerName, "", "Database", ".sql");             
               
                return Json(listBuildFile);
               
            }
            catch (Exception e)
            {
                return Json(new Models.MsgError() { Msg = e.Message });
            }
        }
    }
}