using API.Areas.Admin.Models.RecaptchaUsage;
using API.Areas.Admin.Models.Services;
using API.Filters;
using API.MiddleWares;
using API.Models;
using API.Models.ManagerFiles;
using API.Models.MyHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GoogleService = API.Models.MyHelper.GoogleService;

var builder = WebApplication.CreateBuilder(args);

// SET GLOBAL CONFIG FROM BUILDER
API.Startup._config = builder.Configuration;
API.Startup.Configuration = builder.Configuration;
API.Startup.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
API.Startup.ConnectionStringSTP = builder.Configuration.GetConnectionString("STPConnection");
API.Startup.ConnectionStringMysql = builder.Configuration.GetConnectionString("DefaultMySQL");
API.Startup.reCAPTCHASiteKey = builder.Configuration["reCAPTCHASiteKey"];
API.Startup.reCAPTCHASecretKey = builder.Configuration["reCAPTCHASecretKey"];

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Filters, Razor, MVC, Newtonsoft
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(SampleActionFilter));
})
.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
.AddDataAnnotationsLocalization()
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// Razor runtime compilation
var mvcBuilder = builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
if (builder.Environment.IsDevelopment())
{
    // mvcBuilder.AddRazorRuntimeCompilation();
}

// Background Services
builder.Services.AddHostedService<TempFileCleanupService>();

builder.Services.AddHttpClient();

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Form size limits
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = 937280000;
    x.MultipartBodyLengthLimit = 937280000;
    x.MultipartHeadersLengthLimit = 937280000;
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 83728000000;
});

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
});

// Authentication & Authorization
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(bearerOptions =>
{
    var paramsValidation = bearerOptions.TokenValidationParameters;
    paramsValidation.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    paramsValidation.ValidAudience = builder.Configuration["Jwt:Issuer"];
    paramsValidation.ValidIssuer = builder.Configuration["Jwt:Issuer"];
    paramsValidation.ValidateIssuerSigningKey = true;
    paramsValidation.ValidateLifetime = true;
    paramsValidation.ClockSkew = TimeSpan.Zero;
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddDistributedMemoryCache();

// Add by Tayd - reCAPTCHA v3 sites PYN - START

// Đăng ký IHttpContextAccessor (built-in)
builder.Services.AddHttpContextAccessor();
// Đăng ký IRecaptchaQuotaService RecaptchaQuotaService
builder.Services.AddScoped<IRecaptchaQuotaService, RecaptchaQuotaService>();
// Đăng ký Google service (Scoped cho per-request)

builder.Services.AddScoped<IGoogleService, GoogleService>();
// Đăng ký BackgroundService (Hosted Service)
builder.Services.AddHostedService<RecaptchaResetService>();

// Add by Tayd - reCAPTCHA v3 sites PYN  - END

var app = builder.Build();

// MIDDLEWARE PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Localization
//var supportedCultures = new[]
//{
//    new CultureInfo("vi"),
//    new CultureInfo("vi-VN"),
//    new CultureInfo("en"),
//    new CultureInfo("en-US")
//};

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseCookiePolicy();
//app.UseRouting();
//app.UseSession();

// Localization should be after session but before authentication
//app.UseRequestLocalization(new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("vi"),
//    SupportedCultures = supportedCultures,
//    SupportedUICultures = supportedCultures,
//    RequestCultureProviders = new List<IRequestCultureProvider>
//    {
//        new CookieRequestCultureProvider(),
//        new AcceptLanguageHeaderRequestCultureProvider()
//    }
//});

// Localization
var supportedCultures = new[]
{
    new CultureInfo("vi"),
    new CultureInfo("vi-VN"),
    new CultureInfo("en"),
    new CultureInfo("en-US")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("vi-VN"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider(),
        new QueryStringRequestCultureProvider()
    }
};

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseSession();

app.Use(async (context, next) =>
{
    if (string.IsNullOrEmpty(context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]))
    {
        var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("vi-VN"));
        context.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, cookieValue);
    }
    await next();
});

app.UseRequestLocalization(localizationOptions);

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseMyAuthentication();
app.UseResponseCompression();


// Vary header
app.Use(async (context, next) =>
{
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" };
    await next();
});

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "api",
    areaName: "api",
    pattern: "api/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "image",
    areaName: "image",
    pattern: "Image/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "TrangChu",
    pattern: "trang-chu",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "DuThaoVanBan",
    pattern: "du-thao-van-ban.html",
    defaults: new { controller = "DuThaoVanBan", action = "Index" });

app.MapControllerRoute(
    name: "DanhSachHopTacXa",
    pattern: "danh-sach-hop-tac-xa.html",
    defaults: new { controller = "HTX", action = "Index" });

app.MapControllerRoute(
    name: "Albums",
    pattern: "albums.html",
    defaults: new { controller = "Albums", action = "Index" });

app.MapControllerRoute(
    name: "Audios_Detail",
    pattern: "truyen-thanh/{alias}-{id}.html",
    defaults: new { controller = "Audios", action = "Detail" });

app.MapControllerRoute(
    name: "CategoriesAudios",
    pattern: "loai-truyen-thanh/{alias}-{id}.html",
    defaults: new { controller = "Audios", action = "Index" });

app.MapControllerRoute(
    name: "Videos_Detail",
    pattern: "video/{alias}-{id}.html",
    defaults: new { controller = "Videos", action = "Detail" });

app.MapControllerRoute(
    name: "CategoriesVideos",
    pattern: "loai-videos/{alias}-{id}.html",
    defaults: new { controller = "Videos", action = "Index" });

app.MapControllerRoute(
    name: "VideosId",
    pattern: "videos.html",
    defaults: new { controller = "Videos", action = "Index", Id = 1, Alias = "videos" });

app.MapControllerRoute(
    name: "Contacts_LienHe",
    pattern: "lien-he.html",
    defaults: new { controller = "Contacts", action = "Detail" });

app.MapControllerRoute(
    name: "Contacts",
    pattern: "hoi-dap.html",
    defaults: new { controller = "Contacts", action = "Index" });

app.MapControllerRoute(
    name: "Contacts",
    pattern: "tra-loi-hoi-dap.html",
    defaults: new { controller = "Contacts", action = "List" });

app.MapControllerRoute(
    name: "TraCuuUsers",
    pattern: "tra-cuu-thong-tin.html",
    defaults: new { controller = "Users", action = "Index" });

app.MapControllerRoute(
    name: "HopTrucTuyen",
    pattern: "hop-truc-tuyen.html",
    defaults: new { controller = "Account", action = "HopTrucTuyen" });

app.MapControllerRoute(
    name: "HopTrucTuyenTaiLieu",
    pattern: "hop-truc-tuyen/tai-lieu.html",
    defaults: new { controller = "HopTrucTuyen", action = "TaiLieu" });

app.MapControllerRoute(
    name: "LichCongTac",
    pattern: "lich-cong-tac.html",
    defaults: new { controller = "WorkSchedules", action = "Index" });

app.MapControllerRoute(
    name: "Contacts",
    pattern: "cau-hoi-duoc-tra-loi.html",
    defaults: new { controller = "Contacts", action = "List" });

app.MapControllerRoute(
    name: "Reviews",
    pattern: "danh-gia-website.html",
    defaults: new { controller = "Reviews", action = "Index" });

app.MapControllerRoute(
    name: "Products_Categories",
    pattern: "loai-san-pham/{alias}-{id}.html",
    defaults: new { controller = "Products", action = "Index" });

app.MapControllerRoute(
    name: "Products_Detail",
    pattern: "san-pham/{alias}-{id}.html",
    defaults: new { controller = "Products", action = "Detail" });

app.MapControllerRoute(
    name: "SiteMap",
    pattern: "sitemap.html",
    defaults: new { controller = "Home", action = "SiteMap" });

app.MapControllerRoute(
    name: "Sitemap",
    pattern: "sitemap.xml",
    defaults: new { controller = "Sitemap", action = "Index" });

app.MapControllerRoute(
    name: "ThuTucHanhChinhXa",
    pattern: "thu-tuc-hanh-chinh-cap-xa.html",
    defaults: new { controller = "Home", action = "ThuTucHanhChinhXa" });

app.MapControllerRoute(
    name: "ThuTucHanhChinh",
    pattern: "thu-tuc-hanh-chinh.html",
    defaults: new { controller = "Home", action = "ThuTucHanhChinh" });

app.MapControllerRoute(
    name: "ThuTucHanhChinhHuyen",
    pattern: "thu-tuc-hanh-chinh-cap-huyen.html",
    defaults: new { controller = "Home", action = "ThuTucHanhChinhHuyen" });

app.MapControllerRoute(
    name: "Articles",
    pattern: "articles/{alias}.html",
    defaults: new { controller = "Articles", action = "Index" });

app.MapControllerRoute(
    name: "ArticlesCategories",
    pattern: "categories/{alias}-{id}.html",
    defaults: new { controller = "Articles", action = "GetByCat" });

app.MapControllerRoute(
    name: "Documents_Detail",
    pattern: "van-ban/{alias}-{id}.html",
    defaults: new { controller = "Documents", action = "Detail" });

app.MapControllerRoute(
    name: "Articles",
    pattern: "thong-bao.html",
    defaults: new { controller = "Articles", action = "Notification" });

app.MapControllerRoute(
    name: "Articles",
    pattern: "tin-noi-bat.html",
    defaults: new { controller = "Articles", action = "Featured" });

app.MapControllerRoute(
    name: "CategoriesArticlesGetListChildCat",
    pattern: "listchildcat/{alias}-{id}.html",
    defaults: new { controller = "CategoriesArticles", action = "GetListChildCat" });

app.MapControllerRoute(
    name: "CategoriesArticlesListChildCatNoImage",
    pattern: "ListChildCatNoImage/{alias}-{id}.html",
    defaults: new { controller = "CategoriesArticles", action = "ListChildCatNoImage" });

app.MapControllerRoute(
    name: "RSS",
    pattern: "rss",
    defaults: new { controller = "Rss", action = "Index" });

app.MapControllerRoute(
    name: "RSS_GetByCat",
    pattern: "rss/{alias}-{id}.rss",
    defaults: new { controller = "Rss", action = "GetByCat" });

app.MapControllerRoute(
    name: "Api",
    pattern: "api",
    defaults: new { controller = "Api", action = "Index" });
app.MapControllerRoute(
    name: "Api_GetByCat",
    pattern: "api/{alias}-{id}.json",
    defaults: new { controller = "Api", action = "GetByCat" });

app.MapControllerRoute(
    name: "DocumentRefers",
    pattern: "tai-lieu-tham-khao.html",
    defaults: new { controller = "DocumentRefers", action = "Index" });

app.MapControllerRoute(
    name: "DocumentForms",
    pattern: "mau-bieu-hanh-chinh.html",
    defaults: new { controller = "DocumentForms", action = "Index" });

app.MapControllerRoute(
    name: "DocumentImages",
    pattern: "Documents/{folderid}/{filenameid}/{filetitle}/{uid}",
    defaults: new { controller = "Documents", action = "DongBoCode" });

app.MapControllerRoute(
    name: "Documents",
    pattern: "van-ban.html",
    defaults: new { controller = "Documents", action = "Index" });



app.MapControllerRoute(
    name: "BanLanhDao",
    pattern: "ban-lanh-dao.html",
    defaults: new { controller = "Users", action = "BanLanhDao" });

app.MapControllerRoute(
    name: "Articles_Detail",
    pattern: "{alias}-{id}.html",
    defaults: new { controller = "Articles", action = "Detail" });

app.MapControllerRoute(
    name: "Articles_Detail",
    pattern: "-/{alias}",
    defaults: new { controller = "Home", action = "AliasDetail" });

app.MapControllerRoute(
    name: "Articles_Detail",
    pattern: "{alias}",
    defaults: new { controller = "Home", action = "AliasDetail" });



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();