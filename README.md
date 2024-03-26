## Techcareer.net Backend Bootcamp Bitirme Projesi

### Proje Tanımı:

MolaaApp, kullanıcıların içerik paylaşımı yapabildiği, etkinlik veya toplantı düzenleyebildiği, paylaşılan içeriklere beğeni ve yorum yapabildiği bir web uygulamasıdır. Uygulama, kullanıcıların kimlik doğrulaması gerektiren bir dizi işlevi gerçekleştirebileceği etkileşimli bir ortam sunmaktadır.

### Proje Özellikleri ve Açıklamaları:

1. Kullanıcı Girişi ve Kayıt Olma

Kullanıcılar, Gmail SMTP kullanarak kayıt olabilirler. Kayıt işlemi tamamlandıktan sonra kullanıcıya bir onay e-postası gönderilir ve kullanıcı onayladıktan sonra uygulamaya giriş yapabilir. Şifre unuttum işleviyle kullanıcılar şifre sıfırlama talebi oluşturabilirler ve yeni bir şifre oluşturmak için e-posta yoluyla bir bağlantı gönderilir.

2. Kullanıcı Sayfası ve Profil Düzenleme

Kullanıcılar ikiye ayrılıyor, admin rolündeki kullanıcı ve rolsüz kullanıcı şeklinde. Rolsüz kullanıcı kendi profilini düzenleyebilir, içerik veya toplantı veya içerik ekleyebilir, içeriklere yorum ve beğeni atabilir. Admin rolündeki kullanıcı diğer kullanıcıların profillerini düzenleyebilir, silebilir ve diğer kullanıcılara roller atayabilir.

3. Rol Bazlı Yetkilendirme

Kullanıcılara roller atanır ve bu roller belirli işlevlere erişim sağlar. Admin rolü, kullanıcıları yönetme, düzenleme ve silme gibi yetkilere sahiptir. Rolsüz kullanıcılar sadece kendi profili, gönderileri, toplantı veya etkinliklerini düzenleyebilir.

4. Gönderi Yönetimi

Kullanıcılar, gönderi ekleme, silme, güncelleme ve beğenme durumu gibi işlemleri gerçekleştirebilirler. Rolsüz kullanıcılar sadece kendi gönderilerini düzenleyebilir veya silebilir. Beğeni ve yorum işlemleri için jQuery kullanılarak sayfa yenilenmeden gerçekleştirilir.

5. Toplantı-Etkinlik Yönetimi

Kullanıcılar, toplantı veya etkinlik paylaşabilir ve katılım durumlarını işaretleyebilirler. AJAX kullanılarak sayfa yenilenmeden katılım işlemleri gerçekleştirilir. Toplantıları düzenleme ve silme işlemleri, admin veya toplantıyı paylaşan kullanıcılar tarafından gerçekleştirilebilir.

6. Ortak Layout Kullanımı

Ortak bir layout kullanmanın birçok avantajı vardır:
Kod tekrarını önler: Ortak bir layout kullanmak, aynı kod parçalarının her sayfada tekrar tekrar yazılmasını önler. Bu, kodun daha düzenli ve okunabilir olmasını sağlar.
Bakımı kolaylaştırır: Tüm sayfalar aynı layout’u kullandığı için, layout’ta yapılan bir değişiklik otomatik olarak tüm sayfalara yansır. Bu, bakım sürecini büyük ölçüde kolaylaştırır.
Tutarlı bir kullanıcı deneyimi sağlar: Ortak bir layout kullanmak, tüm sayfaların aynı görünüm ve hissiyatı korumasını sağlar. Bu, kullanıcıların siteyi kullanırken karşılaştıkları deneyimin tutarlı olmasını sağlar.
Verimliliği artırır: Ortak bir layout kullanmak, geliştirme sürecini hızlandırır. Çünkü geliştiricilerin her sayfa için ayrı ayrı tasarım yapmasına gerek kalmaz.

7. Güvenlik İşlemleri

Uygulama, ASP.NET Core Identity ile sağlanan kimlik doğrulama ve yetkilendirme işlemlerine dayanır. Ayrıca, AJAX kullanarak sayfa yenilenmeden işlemler gerçekleştirilir, böylece kullanıcı deneyimi artırılır ve verimlilik sağlanır. Kullanıcılara özel yetkiler atanır ve sayfalara erişim bu yetkilere göre belirlenir. Örneğin, admin veya belirli rollerdeki kullanıcılar belirli sayfalara erişebilirken, diğer kullanıcılar erişemeyebilirler. URL’ye erişmeye çalışsalar bile erişim izni olmayan kullanıcılar hata mesajı alırlar.
Ek Özellikler:
Şifre Politikası Ayarları: Uygulama, kullanıcıların şifrelerinin güvenliğini sağlamak için belirli kurallar uygular. Örneğin, şifrelerin belirli bir uzunlukta olması ve belirli karakterlerin gerekliliği gibi.
Kullanıcı Adı Kuralları: Kullanıcı adlarının belirli karakterler içermesine izin verilir. Bu, kullanıcıların daha esnek kullanıcı adları seçmelerini sağlar.
Kilitlenme Politikası: Kullanıcı giriş işlemlerinde başarısız denemelerin ardından hesapların belirli bir süreliğine kilitlenmesi sağlanır. Bu, kötü niyetli giriş denemelerine karşı koruma sağlar.
Onaylı E-posta Gerekliliği: Kullanıcıların hesaplarını etkinleştirmek için e-posta onayı gereklidir. Bu, uygulamanın güvenliğini ve kullanıcıların kimlik doğrulamasını sağlar.
Yukarıda belirtilen özellikler ve teknolojiler sayesinde, MolaaApp güvenilir, kullanıcı dostu ve etkili bir web uygulamasıdır.

8. View Component Kullanımı

View Component yapısını kullanarak belirli sayfalarda tekrar eden öğeleri veya işlemleri kolayca yönetilmesi sağlanır. Örneğin, bu projede, NewPosts adında bir View Component oluşturulmuş ve bu component, en yeni gönderileri listelemek için kullanılmıştır. Tabi ki admin rolündeki kullanıcı postu yayınlarsa o zaman listede görünür.

### Sonuç:

MolaaApp, kullanıcıların toplantıları yönetebildiği, içerik paylaşımı yapabildiği ve etkileşimli bir ortamda bulunduğu kapsamlı bir web uygulamasıdır. Kullanılan teknolojiler ve özellikler sayesinde güvenilir, kullanıcı dostu ve etkili bir deneyim sunar. Ayrıca, güvenlik önlemleri ve yetkilendirme mekanizmalarıyla veri güvenliği ve gizliliği sağlanmıştır.

Bu projede, Entity Framework Core ve Repository Pattern kullanılarak veritabanı işlemleri soyutlandı ve kod tekrarı önlenmiştir. Bu sayede, veritabanı işlemlerini gerçekleştiren kod parçaları merkezi bir yerde toplanmış ve bu işlemler uygulamanın farklı bölümlerinde kolayca kullanılmıştır. Bu yapı, kodun bakımını ve yönetimini kolaylaştırmıştır.
Proje kapsamında IComponent ve EFComponent adında iki farklı bileşen oluşturulmuştur. Bu bileşenler, Entity Framework bağlamının ve veri işlemlerinin soyutlanmasını sağlar. Böylece, veri tabanı işlemleri bağımsız bir bileşen aracılığıyla gerçekleştirilir ve kod tekrarı önlenir.
Kullanıcıların yetkilendirilmesi ve kimlik doğrulama işlemleri, ASP.NET Core Identity ile sağlanmıştır. Bu sayede, kullanıcıların belirli işlevlere erişimi kontrol edilmiş ve veri güvenliği sağlanmıştır. Ayrıca, AJAX kullanılarak sayfa yenilenmeden işlemler gerçekleştirilmiştir. Bu da kullanıcı deneyimini artırmış ve verimliliği sağlamıştır.

### Kullanılan Teknolojiler ve Versiyonlar:

- ASP.NET Core MVC (Version 7.0.11)
- Entity Framework Core (Version 7.0.11)
- Microsoft SQL Server
- Razor Pages
- HTML/CSS
- JavaScript
- jQuery
- Bootstrap

### Uygulamayı çalıştırmak için:

```console
git clone https://github.com/mertcansucu/MolaaApp.git
cd MolaaApp/
dotnet ef migrations add initialMigration
dotnet ef database update
dotnet watch run
```

### Default kullanıcı:

```json
{
  "email": "mrtcnscc@gmail.com",
  "parola": "Admin_123"
}
```

### Hazırlayan:

Mert Can Sucu (@mertcansucu)
