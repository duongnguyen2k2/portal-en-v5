using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DocumentFormat.OpenXml.Drawing;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Data;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Excel;
using API.Models.Mysql;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.Word;
using API.Areas.Admin.Models.Articles;
namespace API.Controllers
{
    public class MysqlController : Controller
    {
        public async Task<IActionResult> Index()
        {
            // open a connection asynchronously
            using var connectMysql = new MySqlConnection(Startup.ConnectionStringMysql);
            using var connectMssql = new SqlConnection(Startup.ConnectionString);

            await connectMysql.OpenAsync();

            // create a DB command and set the SQL statement with parameters
            using var command = connectMysql.CreateCommand();
            command.CommandText = @"SELECT
	                                t.term_id,
	                                t.slug,
	                                r.parent,
	                                t.term_order,
                                CASE
		
		                                WHEN NAME LIKE '%{:vi}%' 
		                                AND NAME LIKE '%{:en}%' THEN
			                                TRIM(
				                                SUBSTRING(
					                                NAME,
					                                LOCATE( '{:vi}', NAME ) + 5,
					                                LOCATE( '{:}', NAME ) - ( LOCATE( '{:vi}', NAME ) + 5 ) 
				                                )) ELSE NAME 
		                                END AS TitleVN,
	                                CASE
			
			                                WHEN NAME LIKE '%{:vi}%' 
			                                AND NAME LIKE '%{:en}%' THEN
				                                TRIM(
					                                SUBSTRING(
						                                NAME,
						                                LOCATE( '{:en}', NAME ) + 5,
						                                LOCATE(
							                                '{:}',
							                                NAME,
						                                LOCATE( '{:en}', NAME )) - ( LOCATE( '{:en}', NAME ) + 5 ) 
					                                )) ELSE '' 
			                                END AS TitleEN 
		                                FROM
			                                daktip_terms t
			                                LEFT JOIN daktip_term_taxonomy r ON t.term_id = r.term_id";
            //command.Parameters.AddWithValue("@post", "post");
            // execute the command and read the results
            using var reader = await command.ExecuteReaderAsync(); // Thực thi câu lệnh SQL

            List<API.Models.Mysql.DaktipTerm> ListItems = new List<Models.Mysql.DaktipTerm>();

            while (reader.Read())
            {
                API.Models.Mysql.DaktipTerm Post = new API.Models.Mysql.DaktipTerm()
                {
                    term_id = reader.GetInt32("term_id"),
                    slug = reader.GetString("slug"),
                    parent = reader.GetInt32("parent"),
                    TitleVn = reader.GetString("TitleVN"),
                    TitleEn = reader.GetString("TitleEn"),
                    term_order = reader.GetInt32("term_order")
                };
                ListItems.Add(Post);

            }
            await reader.DisposeAsync();
            var oldToNewIdMap = new Dictionary<int, int> { { 0, 0 } };


            await connectMssql.OpenAsync();
            string aliasEn;
            foreach (var item in ListItems)
            {
                aliasEn = item.TitleEn == null ? "" : API.Models.MyHelper.StringHelper.UrlFriendly(item.TitleEn);

                using var insertCmd = connectMssql.CreateCommand();
                insertCmd.CommandText = "INSERT INTO CategoriesArticles (Title, TitleEN, Alias, AliasEN, ParentId, Ordering) OUTPUT INSERTED.Id VALUES (@title, @titleEn, @alias, @aliasEn, NULL, @ordering)";
                insertCmd.Parameters.AddWithValue("@title", item.TitleVn);
                insertCmd.Parameters.AddWithValue("@titleEn", item.TitleEn);
                insertCmd.Parameters.AddWithValue("@alias", item.slug);
                insertCmd.Parameters.AddWithValue("@aliasEn", aliasEn);
                insertCmd.Parameters.AddWithValue("@ordering", item.term_order);

                var newId = (int)await insertCmd.ExecuteScalarAsync();
                oldToNewIdMap[item.term_id] = newId;
            }
            await reader.DisposeAsync();


            foreach (var item in ListItems)
            {
                using var cmd = connectMssql.CreateCommand();
                cmd.CommandText = @"UPDATE CategoriesArticles
                                    SET ParentId = @newParentId
                                    WHERE Id = @newId";
                cmd.Parameters.AddWithValue("@newId", oldToNewIdMap[item.term_id]);
                cmd.Parameters.AddWithValue("@newParentId", oldToNewIdMap[item.parent]);

                //await cmd.ExecuteNonQueryAsync();
            }

            return Json(new API.Models.MsgSuccess() { Data = ListItems });
        }
        public async Task<IActionResult> InsertCategories()
        {
            using var connectMssql = new SqlConnection(Startup.ConnectionString);
            await connectMssql.OpenAsync();
            using var command = connectMssql.CreateCommand();
            command.CommandText = @"Select Id, Title, Alias, Description, ParentId, Status, Deleted, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Ordering, Hits from CategoriesArticles";
            using var reader = await command.ExecuteReaderAsync();
            List<API.Models.Mysql.Categories> ListItems = new List<Models.Mysql.Categories>();

            while (reader.Read())
            {
                API.Models.Mysql.Categories category = new API.Models.Mysql.Categories()
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Alias = reader.GetString("Alias"),
                    Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    ParentId = reader.GetInt32("ParentId"),
                    Status = reader.GetBoolean("Status"),
                    Deleted = reader.GetBoolean("Deleted"),
                    CreatedBy = reader.GetInt32("CreatedBy"),
                    CreatedDate = reader.GetDateTime("CreatedDate"),
                    ModifiedBy = reader.GetInt32("ModifiedBy"),
                    ModifiedDate = reader.IsDBNull("ModifiedDate") ? null : reader.GetDateTime("ModifiedDate"),
                    Ordering = reader.GetInt32("Ordering"),
                    Hits = reader.GetInt32("Hits")
                };
                ListItems.Add(category);

            }
            await reader.DisposeAsync();
            //var oldToNewIdMap = new Dictionary<int, int> { { 0, 0} };
            foreach (var item in ListItems)
            {
                using var cmd = connectMssql.CreateCommand();
                cmd.CommandText = @"INSERT INTO NewCategoriesArticles(Id, TitleVN, AliasVN, Description, ParentId, Status, Deleted, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Ordering, Hits) 
                                        VALUES (@Id, @titleVN, @aliasVN, @description, @parentId, @status, @deleted, @createBy, @createDate, @modifiedBy, @modifiedDate, @ordering, @hits)";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@titleVN", item.Title ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@aliasVN", item.Alias ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@description", item.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@parentId", item.ParentId);
                cmd.Parameters.AddWithValue("@status", item.Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@createBy", item.CreatedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@createDate", item.CreatedDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", item.ModifiedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedDate", item.ModifiedDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ordering", item.Ordering ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@hits", item.Hits ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@deleted", item.Deleted ?? (object)DBNull.Value);


                //await cmd.ExecuteNonQueryAsync();

            }

            return Json(new API.Models.MsgSuccess() { Data = ListItems });
        }
        public async Task<Dictionary<int, int>> InsertArticle()
        {
            using var connectMysql = new MySqlConnection(Startup.ConnectionStringMysql);

            await connectMysql.OpenAsync();
            using var commandMysql = connectMysql.CreateCommand();
            commandMysql.CommandText = @"SELECT term_id from daktip_terms";

            using var reader1 = await commandMysql.ExecuteReaderAsync(); // Thực thi câu lệnh SQL

            List<API.Models.Mysql.DaktipTerm> ListItems1 = new List<Models.Mysql.DaktipTerm>();

            while (reader1.Read())
            {
                API.Models.Mysql.DaktipTerm Post = new API.Models.Mysql.DaktipTerm()
                {
                    term_id = reader1.GetInt32("term_id"),
                };
                ListItems1.Add(Post);

            }
            await reader1.DisposeAsync();


            using var connectMssql = new SqlConnection(Startup.ConnectionString);
            await connectMssql.OpenAsync();
            using var commandMssql = connectMssql.CreateCommand();
            commandMssql.CommandText = @"
                                          SELECT *
                                          FROM CategoriesArticles
                                          ORDER BY Id
                                          OFFSET 65 ROWS";
            using var reader2 = await commandMssql.ExecuteReaderAsync();
            List<API.Models.Mysql.Categories> ListItems2 = new List<Models.Mysql.Categories>();
            while (reader2.Read())
            {
                API.Models.Mysql.Categories category = new API.Models.Mysql.Categories()
                {
                    Id = reader2.GetInt32("Id"),
                };
                ListItems2.Add(category);

            }
            await reader2.DisposeAsync();

            var dict = new Dictionary<int, int>();

            for (int i = 0; i < ListItems1.Count && i < ListItems2.Count; i++)
            {
                dict[ListItems1[i].term_id] = ListItems2[i].Id;
            }

            return dict;

        }

        public async Task<IActionResult> DataDumpArtcle()
        {
            using var connectMysql = new MySqlConnection(Startup.ConnectionStringMysql);
            await connectMysql.OpenAsync();
            using var commandMysql = connectMysql.CreateCommand();
            commandMysql.CommandText = @"SELECT
	                                        p.*,
	                                        r.term_taxonomy_id

                                        FROM
	                                        daktip_posts p
	                                        LEFT JOIN daktip_term_relationships r ON p.ID = r.object_id
	                                        LEFT JOIN daktip_term_taxonomy t ON r.term_taxonomy_id = t.term_taxonomy_id 
                                        WHERE
	                                        r.term_taxonomy_id IS NOT NULL AND p.post_type = 'post' AND p.post_title REGEXP '^[A-Za-z0-9 ,.!?''''()\-]+$'";
            using var reader = await commandMysql.ExecuteReaderAsync();
            List<DaktipPost> daktipPosts = new List<DaktipPost>();

            while (reader.Read())
            {
                DaktipPost post = new DaktipPost()
                {
                    ID = reader.GetInt32("ID"),
                    post_author = reader.GetInt32("post_author"),
                    term_taxonomy_id = reader.GetInt32("term_taxonomy_id"),
                    post_title = reader.GetString("post_title"),
                    post_content = reader.GetString("post_content"),
                    post_excerpt = reader.GetString("post_excerpt"),
                    post_name = reader.GetString("post_name"),
                    post_status = reader.GetString("post_status"),
                    post_date = reader.GetDateTime("post_date"),
                    post_modified = reader.GetDateTime("post_modified"),
                };
                daktipPosts.Add(post);
            }
            await reader.DisposeAsync();


            var ListId = await InsertArticle();
            using var connectMssql = new SqlConnection(Startup.ConnectionString);
            await connectMssql.OpenAsync();
            foreach (var post in daktipPosts)
            {
                Boolean Status = true;
                if (post.post_status == "draft" || post.post_status == "pending")
                {
                    Status = false;
                }
                using var commandMssql = connectMssql.CreateCommand();
                commandMssql.CommandText = @"INSERT INTO Articles(IdOld,Title,Alias,IntroText,FullText,Title_EN, Alias_EN, IntroText_EN, FullText_EN, CatId, CreatedDate, ModifiedDate, AuthorId,CreatedBy,IdCoQuan,Status,Hot,FlagEdit,StaticPage) 
                                                            VALUES(@Id,@title_en, @alias_en, @intro_en, @fulltext_en,@title_en, @alias_en, @intro_en, @fulltext_en, @catId, @createDate, @modifiedDate, @authorId,@CreatedBy,132,@Status,0,0,0)";

                commandMssql.Parameters.AddWithValue("@Id", post.ID);
                commandMssql.Parameters.AddWithValue("@title_en", post.post_title);
                commandMssql.Parameters.AddWithValue("@alias_en", post.post_name);
                commandMssql.Parameters.AddWithValue("@intro_en", post.post_excerpt);
                commandMssql.Parameters.AddWithValue("@fulltext_en", post.post_content);
                commandMssql.Parameters.AddWithValue("@catId", ListId[post.term_taxonomy_id]);
                commandMssql.Parameters.AddWithValue("@createDate", post.post_date);
                commandMssql.Parameters.AddWithValue("@modifiedDate", post.post_modified);
                commandMssql.Parameters.AddWithValue("@authorId", post.post_author);
                commandMssql.Parameters.AddWithValue("@CreatedBy", post.post_author);
                commandMssql.Parameters.AddWithValue("@Status", Status);

                //await commandMssql.ExecuteNonQueryAsync();
            }


            return Json(new API.Models.MsgSuccess() { Data = daktipPosts });
        }
        public async Task<IActionResult> DataDumpArtcleVN()
        {
            using var connectMysql = new MySqlConnection(Startup.ConnectionStringMysql);
            await connectMysql.OpenAsync();
            using var commandMysql = connectMysql.CreateCommand();
            commandMysql.CommandText = @"SELECT
                                            p.*,
                                            r.term_taxonomy_id
                                        FROM
                                            daktip_posts p
                                            LEFT JOIN daktip_term_relationships r ON p.ID = r.object_id
                                            LEFT JOIN daktip_term_taxonomy t ON r.term_taxonomy_id = t.term_taxonomy_id 
                                        WHERE
                                            r.term_taxonomy_id IS NOT NULL 
                                            AND p.post_type = 'post'
                                            AND NOT p.post_title REGEXP '^[A-Za-z0-9 ,.!?''''()\\-]+$'";
            using var reader = await commandMysql.ExecuteReaderAsync();
            List<DaktipPost> daktipPosts = new List<DaktipPost>();

            while (reader.Read())
            {
                DaktipPost post = new DaktipPost()
                {
                    ID = reader.GetInt32("ID"),
                    post_author = reader.GetInt32("post_author"),
                    term_taxonomy_id = reader.GetInt32("term_taxonomy_id"),
                    post_title = reader.GetString("post_title"),
                    post_content = reader.GetString("post_content"),
                    post_excerpt = reader.GetString("post_excerpt"),
                    post_name = reader.GetString("post_name"),
                    post_status = reader.GetString("post_status"),
                    post_date = reader.GetDateTime("post_date"),
                    post_modified = reader.GetDateTime("post_modified"),
                };
                daktipPosts.Add(post);
            }
            await reader.DisposeAsync();


            var ListId = await InsertArticle();
            using var connectMssql = new SqlConnection(Startup.ConnectionString);
            await connectMssql.OpenAsync();
            foreach (var post in daktipPosts)
            {
                Boolean Status = true;
                if(post.post_status== "draft" || post.post_status== "pending")
                {
                    Status = false;
                }
               
                using var commandMssql = connectMssql.CreateCommand();
                
                commandMssql.CommandText = @"INSERT INTO Articles(IdOld,Title, Alias, IntroText, FullText, CatId, CreatedDate, ModifiedDate, AuthorId,CreatedBy,IdCoQuan,Status,Hot,FlagEdit,StaticPage) 
                                                          VALUES(@Id,@title, @alias, @intro, @fulltext, @catId, @createDate, @modifiedDate, @authorId,@CreatedBy,132,@Status,0,0,0)";
                commandMssql.Parameters.AddWithValue("@Id", post.ID);
                commandMssql.Parameters.AddWithValue("@title", post.post_title);
                commandMssql.Parameters.AddWithValue("@alias", post.post_name);
                commandMssql.Parameters.AddWithValue("@intro", post.post_excerpt);
                commandMssql.Parameters.AddWithValue("@fulltext", post.post_content);
                commandMssql.Parameters.AddWithValue("@catId", ListId[post.term_taxonomy_id]);
                commandMssql.Parameters.AddWithValue("@createDate", post.post_date);
                commandMssql.Parameters.AddWithValue("@modifiedDate", post.post_modified);
                commandMssql.Parameters.AddWithValue("@authorId", post.post_author);
                commandMssql.Parameters.AddWithValue("@CreatedBy", post.post_author);
                commandMssql.Parameters.AddWithValue("@Status", Status);

                //await commandMssql.ExecuteNonQueryAsync();
                
            }


            return Json(new API.Models.MsgSuccess() { Data = daktipPosts });
        }



        public async Task<IActionResult> AddNewItem()
        {



            // open a connection asynchronously
            using var connection = new MySqlConnection(Startup.ConnectionStringMysql);
            await connection.OpenAsync();

            // create a DB command and set the SQL statement with parameters
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO test(title,status) VALUES (@title,@status)";
            command.Parameters.AddWithValue("@title", "Helo chao " + DateTime.Now);
            command.Parameters.AddWithValue("@status", 1);

            // execute the command and read the results
            int a = 1; //await command.ExecuteNonQueryAsync();// thực thi câu lệnh SQL

            long Id = 0;
            if (a == 1)
            {
                Id = command.LastInsertedId; // Lấy Id vừa insert
            }

            return Json(new API.Models.MsgSuccess() { Data = Id });
        }
    }


}

namespace API.Models.Mysql
{
    public class IdCategories
    {
        public int OldId { get; set; }
        public int NewId { get; set; }
    }
    public class DaktipTerm
    {

        public string slug { get; set; }
        public string TitleVn { get; set; }
        public string TitleEn { get; set; }
        public int term_id { get; set; }
        public int parent { get; set; }
        public int term_order { get; set; }
    }

    public class Categories
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public int ParentId { get; set; }
        public Boolean? Status { get; set; }
        public Boolean? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Ordering { get; set; }
        public int? Hits { get; set; }
    }
    public class DaktipPost
    {
        public int post_author { get; set; }
        public DateTime post_date { get; set; }
        public string post_title { get; set; }
        public string post_content { get; set; }
        public string post_excerpt { get; set; }
        public string post_name { get; set; }
        public string post_status { get; set; }
        public DateTime post_modified { get; set; }
        public int term_taxonomy_id { get; set; }
        public int ID { get; set; }
    }
}


//https://mysqlconnector.net/tutorials/connect-to-mysql/
//https://www.nuget.org/packages/MySqlConnector/


/*
 Bước 1: Tại File appsettings.json thêm connect
    "DefaultMySQL": "Server=103.159.50.19;User ID=kitchendeli_vn;Password=Eo6Nt@O1iOeL0Qo@;Database=kitchendeli_vn"
 Bước 2: Trong file Startup.cs thêm biến 

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public static string ConnectionStringMysql { get; private set; }


Bước 3: Tại hàm Configure thêm đoạn code lấy thông tin Mysql cấu hình trong appsettings.json

    ConnectionStringMysql = Configuration["ConnectionStrings:DefaultMySQL"];
  
 */