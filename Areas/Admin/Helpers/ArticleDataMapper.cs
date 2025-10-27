using API.Areas.Admin.Models.Articles;
using API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using static API.Areas.Admin.Models.ManagerFile.ManagerFile;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

#nullable enable

namespace API.Areas.Admin.Helpers
{
    /// <summary>
    /// Maps DataRow to Articles model with variant support for flexibility across different queries.
    /// Designed for reuse in controllers handling Articles or similar entities.
    /// </summary>
    public static class ArticleDataMapper
    {
        /// <summary>
        /// Variants for different mapping scenarios (e.g., full fields vs minimal).
        /// </summary>
        public enum ArticleVariant
        {
            Full,    // E.g., GetItem: All fields including EN, IdCoQuan, etc.
            Tmp,     // E.g., GetArticleTMPByLink: Basic + LinkRoot, LikeN.
            ByAlias, // E.g., GetItemByAlias: Standard + FeaturedHome, FlagEdit.
            Log      // E.g., GetItemLogArticle: Minimal, no culture fields.
        }

        /// <summary>
        /// Options for customizing mapping behavior.
        /// </summary>
        public class ArticleMappingOptions
        {
            public string? SecretId { get; set; }
            public bool ResetTmpFields { get; set; } = false; // E.g., clear if Title == "article_tmp"
            public double DefaultMoney { get; set; } = 0.0;
            // Extendable: Add more for future (e.g., custom fallbacks).
        }

        /// <summary>
        /// Maps a DataRow to Articles, based on variant. Handles DBNull safely.
        /// Call after executing DB query and getting row[0].
        /// Only maps properties in Articles model that correspond to DB columns.
        /// </summary>
        /// <param name="row">DataRow from query result.</param>
        /// <param name="variant">Type of mapping (determines fields).</param>
        /// <param name="options">Optional config.</param>
        /// <returns>Mapped Articles or null if row invalid.</returns>
        public static Articles? MapRowToArticle(DataRow row, ArticleVariant variant, ArticleMappingOptions? options = null)
        {
            ArgumentNullException.ThrowIfNull(row);

            options ??= new ArticleMappingOptions();

            

            var id = SafeField(row, "Id", 0);  // PK always exists, but safe
            var article = new Articles
            {
                
                // Core fields: Use SafeField<T>(col, defaultValue) for multi-tenant safety
                Id = id,
                Title = SafeField<string?>(row, "Title", null),
                Alias = SafeField<string?>(row, "Alias", null),
                CatId = SafeField<int>(row, "CatId", 0),
                LikeN = SafeField<int>(row, "LikeN", 0),
                IntroText = SafeField<string?>(row, "IntroText", null),
                FullText = SafeField<string?>(row, "FullText", null),
                Status = SafeField<bool>(row, "Status", false),
                CreatedBy = SafeField<int?>(row, "CreatedBy", null),
                ModifiedBy = SafeField<int?>(row, "ModifiedBy", null),
                CreatedDate = SafeField<DateTime?>(row, "CreatedDate", DateTime.MinValue) ?? DateTime.MinValue,
                ModifiedDate = SafeField<DateTime?>(row, "ModifiedDate", DateTime.Now),
                Metadesc = SafeField<string?>(row, "Metadesc", null),
                Metakey = SafeField<string?>(row, "Metakey", null),
                Metadata = SafeField<string?>(row, "Metadata", null),
                Language = SafeField<string?>(row, "Language", null),
                Featured = SafeField<bool>(row, "Featured", false),
                Notification = SafeField<bool>(row, "Notification", false),
                FeaturedHome = SafeField<bool>(row, "FeaturedHome", false),
                Comment = SafeField<bool>(row, "Comment", false),
                StaticPage = SafeField<bool>(row, "StaticPage", false),  // Fallback false dù DB NO NULL
                Images = SafeField<string?>(row, "Images", null),
                FileItem = SafeField<string?>(row, "FileItem", null),
                Params = SafeField<string?>(row, "Params", null),
                Ordering = SafeField<int?>(row, "Ordering", null),
                Deleted = SafeField<bool>(row, "Deleted", false),
                Hit = SafeField<int>(row, "Hit", 0),
                AuthorId = SafeField<int>(row, "AuthorId", 0),
                IdCoQuan = SafeField<int>(row, "IdCoQuan", 0),
                Author = SafeField<string?>(row, "Author", null),
                PublishUp = SafeField<DateTime?>(row, "PublishUp", DateTime.MinValue) ?? DateTime.MinValue,
                Str_ListFile = SafeField<string?>(row, "Str_ListFile", null),
                Str_Link = SafeField<string?>(row, "Str_Link", null),
                RootNewsId = SafeField<int>(row, "RootNewsId", 0),
                QuantityImage = SafeField<int>(row, "QuantityImage", 0),
                Money = SafeField<double>(row, "Money", 0.0),
                IdLevel = SafeField<int>(row, "IdLevel", 0),
                CategoryTypeId = SafeField<int>(row, "CategoryTypeId", 0),
                LinkRoot = SafeField<string>(row, "LinkRoot", string.Empty),
                RootNewsFlag = SafeField<bool>(row, "RootNewsFlag", false),
                FlagEdit = SafeField<bool>(row, "FlagEdit", true),  // Fallback true dù DB NO NULL
                MetaMoney = SafeField<string?>(row, "MetaMoney", null),
                Metadata_EN = SafeField<string?>(row, "Metadata_EN", null),
                ArticlesStatusId = SafeField<int>(row, "ArticlesStatusId", 1),
                Ids = MyModels.Encode(id, options.SecretId),
                TotalRows = SafeField<int>(row, "TotalRows", 0),
                STT = SafeField<int>(row, "STT", 0),
                Address = SafeField<string>(row, "Address", string.Empty),
                AuthorRoot = SafeField<string>(row, "AuthorRoot", string.Empty),
                Category = SafeField<string>(row, "Category", string.Empty),
                Icon = SafeField<string>(row, "Icon", string.Empty),
                TenCoQuan = SafeField<string?>(row, "TenCoQuan", null),
                CodeCoQuan = SafeField<string?>(row, "CodeCoQuan", null),
                CreatedByName = SafeField<string?>(row, "CreatedByName", null),
                CreatedByFullName = SafeField<string?>(row, "CreatedByFullName", null),
                LevelTitle = SafeField<string?>(row, "LevelTitle", null),
                CategoryTypeTitle = SafeField<string>(row, "CategoryTypeTitle", string.Empty),
                DataFile = SafeField<string>(row, "DataFile", string.Empty),
                Link_EN = SafeField<string?>(row, "Link_EN", null),
                TenPhongBan = SafeField<string?>(row, "TenPhongBan", null),
                ModifiedByFullName = SafeField<string?>(row, "ModifiedByFullName", null),

                // Variant-specific fields: Set EN props directly (no fallback here; handle in override)
                Title_EN = SafeField<string?>(row, "Title_EN", null),
                Alias_EN = SafeField<string?>(row, "Alias_EN", null),
                IntroText_EN = SafeField<string?>(row, "IntroText_EN", null),
                FullText_EN = SafeField<string?>(row, "FullText_EN", null),
                Metadesc_EN = SafeField<string?>(row, "Metadesc_EN", null),
                Metakey_EN = SafeField<string?>(row, "Metakey_EN", null),
                FileItem_EN = SafeField<string?>(row, "FileItem_EN", null),

                // Computed fields: Safe PublishUp
                PublishUpShow = SafeField<DateTime?>(row, "PublishUp", DateTime.Now)?.ToString("dd/MM/yyyy") ?? DateTime.Now.ToString("dd/MM/yyyy")
            };


            // Initialize lists as per model defaults
            article.ListFile = !string.IsNullOrEmpty(article.Str_ListFile)
                ? JsonConvert.DeserializeObject<List<FileArticle>>(article.Str_ListFile) ?? new List<FileArticle>()
                : new List<FileArticle>();

            article.ListLinkArticle = !string.IsNullOrEmpty(article.Str_Link)
                ? JsonConvert.DeserializeObject<List<LinkArticle>>(article.Str_Link) ?? new List<LinkArticle> { new LinkArticle() }
                : new List<LinkArticle> { new LinkArticle() };


            article.ListDataFile = string.IsNullOrEmpty(article.DataFile)
                ? new List<TinymceFile>()
                : JsonConvert.DeserializeObject<List<TinymceFile>>(article.DataFile) ?? new List<TinymceFile>();


            // Defaults for complex objects
            article.MetadataCV = new API.Models.MetaData();
            article.MetadataCV_EN = new API.Models.MetaData();
            article.MetaMoneyCV = new MetaMoney();

            return article;
        }

        /// <summary>
        /// Deserializes JSON fields in Articles (or similar models) with safe error handling.
        /// Reusable for other entities with JSON props.
        /// </summary>
        /// <param name="item">Entity to process.</param>
        public static void DeserializeJsonFields(Articles item)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            item.ListFile = !string.IsNullOrEmpty(item.Str_ListFile)
                ? JsonSerializer.Deserialize<List<FileArticle>>(item.Str_ListFile, options) ?? new List<FileArticle>()
                : new List<FileArticle>();

            item.ListLinkArticle = !string.IsNullOrEmpty(item.Str_Link)
                ? JsonSerializer.Deserialize<List<LinkArticle>>(item.Str_Link, options) ?? new List<LinkArticle>()
                : new List<LinkArticle>();

            // Metadata handling (common pattern)
            item.MetadataCV = !string.IsNullOrEmpty(item.Metadata)
                ? JsonSerializer.Deserialize<MetaData>(item.Metadata, options) ?? CreateDefaultMetadata(item.Title ?? string.Empty)
                : CreateDefaultMetadata(item.Title ?? string.Empty);

            item.MetadataCV_EN = !string.IsNullOrEmpty(item.Metadata_EN)
                ? JsonSerializer.Deserialize<MetaData>(item.Metadata_EN, options) ?? CreateDefaultMetadata(item.Title_EN ?? string.Empty)
                : CreateDefaultMetadata(item.Title_EN ?? string.Empty);
        }

        private static MetaData CreateDefaultMetadata(string titleFallback) => new()
        {
            MetaTitle = titleFallback,
            MetaH1 = titleFallback,
            MetaH3 = titleFallback
        };

        /// <summary>
        /// Applies culture overrides for "en": Sets main fields to EN values if EN is not null/empty.
        /// If EN is null or empty, keeps the original (Vietnamese) value.
        /// For other cultures, no changes are made.
        /// </summary>
        /// <param name="item">Articles to override.</param>
        /// <param name="culture">Culture code (e.g., "en").</param>
        public static void ApplyCultureOverrides(Articles item, string culture)
        {
            if (culture != "en" || item == null) return;

            if (!string.IsNullOrEmpty(item.Title_EN)) item.Title = item.Title_EN;
            if (!string.IsNullOrEmpty(item.Alias_EN)) item.Alias = item.Alias_EN;
            if (!string.IsNullOrEmpty(item.IntroText_EN)) item.IntroText = item.IntroText_EN;
            if (!string.IsNullOrEmpty(item.FullText_EN)) item.FullText = item.FullText_EN;
            if (!string.IsNullOrEmpty(item.Metadata_EN)) item.Metadata = item.Metadata_EN;
            if (!string.IsNullOrEmpty(item.Metadesc_EN)) item.Metadesc = item.Metadesc_EN;
            if (!string.IsNullOrEmpty(item.Metakey_EN)) item.Metakey = item.Metakey_EN;
        }

        /// <summary>
        /// Safe field accessor: Returns value if column exists and not DBNull; else defaultValue.
        /// Handles multi-tenant missing columns without exception.
        /// Optional: Log missing column for tenant debugging.
        /// </summary>
        private static T SafeField<T>(DataRow row, string columnName, T defaultValue)
        {
            if (!row.Table.Columns.Contains(columnName))
            {
                // Optional: Log for debug, e.g., _logger?.LogDebug("Missing column '{Col}' for tenant {TenantId}", columnName, GetCurrentTenantId());
                return defaultValue;
            }

            var value = row[columnName];
            if (value == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                return (T)value;  // Cast safe vì checked DBNull
            }
            catch (InvalidCastException)
            {
                // Fallback if type mismatch (rare)
                return defaultValue;
            }
        }
        
    }

    // Interface for future extension (e.g., other mappers)
    public interface IEntityDataMapper<T>
    {
        T? MapRowToEntity(DataRow row, object? options = null);
        void DeserializeFields(T entity);
    }


}