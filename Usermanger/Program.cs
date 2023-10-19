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
        Description = "ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�",
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
#region ע�����
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
#region JWTУ��
//�ڶ��������Ӽ�Ȩ�߼�
JwtTokenOptions tokenOptions = new ();
builder.Configuration.Bind("JwtTokenOptions", tokenOptions);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
 .AddJwtBearer(options =>  //���������õļ�Ȩ���߼�
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         //JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
         ValidateIssuer = true,//�Ƿ���֤Issuer
         ValidateAudience = true,//�Ƿ���֤Audience
         ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
         ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
         ValidAudience = tokenOptions.Audience,//
         ValidIssuer = tokenOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//�õ�SecurityKey 
     };
 });
#endregion
/*builder.Services.AddDefaultIdentity<Role>()
    .AddRoles<Role>()*/
#region ����
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
});
#endregion
// ע���Զ�����Ȩ�������
/*builder.Services.AddSingleton<IAuthorizationHandler,CustomAuthorizationHandler>();*/
// ����Զ������
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CustomRolePolicy", policy =>
             policy.Requirements.Add(new CustomAuthorizationRequirement(new List<string> { "����Ա", "���", "�鳤" ,"����","�ɲ�"})));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//�Զ����м��
app.UseMiddleware<RequestLimitMiddleware>();
app.UseMiddleware<MarkDownViewerMiddleware>();
#region ��֤��Ȩ
app.UseAuthentication();
app.UseAuthorization();

#endregion

app.MapControllers();

app.Run();
