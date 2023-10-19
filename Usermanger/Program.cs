using Dao.DaoIpml;
using Dao.IDao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model;
using Model.Entitys;
using Nest;
using Service;
using System.Text;
using UserManagement.Db;
using UserManagement.ServiceDev;
using UserManagement.ServiceDev.ES;
using UserManagement.ServiceDev.Jwt;
using Usermanger.AuthorizationSelf;
using Usermanger.EventDomain.Service;
using Usermanger.Filter;

var builder = WebApplication.CreateBuilder(args);
//builder.Host.UseServiceProviderFactory(new A)
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                     Id="Bearer"
                }
            },
            new string[] {}
        }
    });
});
#region 注册服务
builder.Services.AddScoped<IUserDao,UserDao>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IRoleDao,RoleDao>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<IPostDao, PostDao>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<IGroupDao,GroupDao>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<IUserRoleDao,UserRoleDao>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<IUserGroupDao,UserGroupDao>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<IRolePostDao,RolePostDao>();
builder.Services.AddScoped<RolePostService>();
builder.Services.AddScoped<IUserGroupDao, UserGroupDao>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<UserEventReviewService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<UserReviewService>();
builder.Services.AddScoped<UserGroupService>();
builder.Services.AddScoped<ICustomJWTService,CustomJWTService>();

//ES
builder.Services.AddScoped<ElasticClient>();
builder.Services.AddScoped<PeopleService>();
#endregion
//builder.Services.AddSingleton<ActionMiddleware>();
builder.Services.Configure<JwtTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));
builder.Services.AddMemoryCache();
builder.Services.Configure<MvcOptions>(op =>
{
    op.Filters.Add<JWTValidationFilter>();
    op.Filters.Add<RateActionFilter>();
    op.Filters.Add<MyExceptionFilter>();
});
#region JWT校验
//第二步，增加鉴权逻辑
JwtTokenOptions tokenOptions = new ();
builder.Configuration.Bind("JwtTokenOptions", tokenOptions);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
 .AddJwtBearer(options =>  //这里是配置的鉴权的逻辑
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         //JWT有一些默认的属性，就是给鉴权时就可以筛选了
         ValidateIssuer = true,//是否验证Issuer
         ValidateAudience = true,//是否验证Audience
         ValidateLifetime = true,//是否验证失效时间
         ValidateIssuerSigningKey = true,//是否验证SecurityKey
         ValidAudience = tokenOptions.Audience,//
         ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//拿到SecurityKey 
     };
 });
#endregion
/*builder.Services.AddDefaultIdentity<Role>()
    .AddRoles<Role>()*/
#region 跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
});
#endregion
// 注册自定义授权处理程序
/*builder.Services.AddSingleton<IAuthorizationHandler,CustomAuthorizationHandler>();*/
// 添加自定义策略
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CustomRolePolicy", policy =>
             policy.Requirements.Add(new CustomAuthorizationRequirement(new List<string> { "管理员", "书记", "块长" ,"主任","干部"})));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//自定义中间件
app.UseMiddleware<RequestLimitMiddleware>();
app.UseMiddleware<MarkDownViewerMiddleware>();
#region 认证授权
app.UseAuthentication();
app.UseAuthorization();

#endregion

app.MapControllers();

app.Run();
