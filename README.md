# Article Api

### Projede Kullanılar Tasarım Desenleri :

- Singleton (Nesne örneğinin bir kere oluşturup tüm isteklerde bu instance'ı kullanmak için)
- Repository (Veritabanı sorgu işlemlerinin bir merkezden yapılmasını sağlamak için)


### Projede Kullanılar Teknoloji & Kütüphaneler :

- Asp.Net Core 2.2.0
- EntityFramework Core 2.2.6
- NLog AspNetCore


### Kurulum

- Kurulum için ConnectionString'in kontrol edilip uygulamanın çalıştırılması yeterlidir.
- Veritabanı, tablo ve datalar ilk çalışmada Migrate edilir

**![appsttngs](https://user-images.githubusercontent.com/6877358/62597960-50df8b00-b8f0-11e9-9c59-177aef6de5b5.png)**


### Test

- Proje içerisindeki Article.postman_collection.json dosyası Postman uygulamasına import edilerek test edilebilir.

![howtouse](https://user-images.githubusercontent.com/6877358/62602019-61493300-b8fb-11e9-92e9-7f155a4bedab.gif)



### RoadMap

- Entity ile modelleri maplemek için AutoMapper kullanılabilir
- Validation eklenebilir
- Authentication için IdentityServer kullanılabilir
- Loglama için DatabaseLogger eklenebilir
- Built-in Dependency Injection yerine 3. parti bir container kullanılabilir
- Transaction Scope Abstraction'ı eklenebilir
- Permission yapısı eklenebilir
- Api Versiyonlama eklenebilir
