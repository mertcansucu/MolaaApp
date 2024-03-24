using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MolaaApp.Data.Abstract;
using MolaaApp.Data.Concrete;
using MolaaApp.Data.Concrete.EfCore;
using MolaaApp.Models;

var builder = WebApplication.CreateBuilder(args);

//IEmailSender, SmtpEmailSender burda ben IEmailSender çağırıp SmtpEmailSender ile mesaj oluşturucam bunun yerine api kullanarak yapmak istersemde SmtpEmailSender yerine onu yazıcam
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i => new SmtpEmailSender(
    builder.Configuration["EmailSender:Host"],
    builder.Configuration.GetValue<int>("EmailSender:Port"),    
    builder.Configuration.GetValue<bool>("EmailSender:EnableSSl"),    
    builder.Configuration["EmailSender:Username"],   
    builder.Configuration["EmailSender:Password"])
);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:SqLite_Connection"]));

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

//**AddDefaultTokenProviders() bu bize->parola yenileme,sıfırlama,email sıfırlama email onaylı hale getirmek için bize token bilgisi üretir

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    
    options.User.RequireUniqueEmail = true;

    options.User.AllowedUserNameCharacters = "abcdefghiıjklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    //kullanıcı login işleminde ben kullanıcıya 5 hak vericem eğer 5 seferde login olamazsa hesabını 5 dk kitliyicem ve kullanıcı giriş yapamıyacak
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    //giriş için email onayı olmasını istediğim için bunu aktif etmem lazım başlangıçta bu false du nem bunu aktif yaptım
    options.SignIn.RequireConfirmedEmail = true;

});

builder.Services.ConfigureApplicationCookie(options =>{//cookie ayarları
    options.LoginPath = "/Account/Login";//default halide bu login girişi için buraya yönlendirir beni
    options.AccessDeniedPath = "/Account/AccessDenied";//giriş yapan kullanıcı rolü yani yetkisi yetersizse sayfalara erişmesini engelliyorum ve kullanıcıya yetkisiz kişi olduğunu söylüyorum,usercontrollera eklediğim kod ile yetkisiz biri yetkisi dışında bir yere gitmek istediğinde bu sayfaya yönlendirilecek
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);//ben bu yazdığım iki kodla diyorum ki kullanıcı 30 gün içerisinde giriş yapsın veya yapmasın cookisini sakla sonra sil, ama diyelim 15. gün ben giriş yaptım süre 30 gün olarak yeniden başlar sonra kayıt silinir ve kullanıcı yeniden giriş yaparak giriş yapmalıdır
    

});

builder.Services.AddScoped<IPostRepository, EfPostRepository>();//ben burda diyorum ki IPostRepository ben sanalı göderdiğimde sen bana EfPostRepository ile gerçek halini bana gönder,AddScoped olmasının nedeni her http reqository aynı nesneyi gönderir yani her http requestinde bir nesne yolla
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<ILikeRepository, EfLikeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//kimlik doğrulama işlemleri
app.UseAuthentication();

app.UseAuthorization();

//ben ekranda url kısmını değiştirmek istiyorum onun için eklemeler yapıcam ve benim şstediğim şekli:
//post icin iliskilendirme
app.MapControllerRoute(
    name: "post_details",
    pattern: "posts/details/{url}",//posts sabit yer diğer kısım ise sayfanın urlsini çekmek olacak
    defaults: new {controller = "Posts",action="Details"}
);
app.MapControllerRoute(
    name: "user_profile",
    pattern: "profile/{username}",
    defaults: new {controller = "Account",action="Profile"}
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

IdentitySeedData.IdentityTestUser(app);//IdentitySeedData yani test verilerini ekledim

app.Run();
