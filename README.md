# Training Catalog API

REST ilkelerine bağlı kalarak eğitim içeriklerini yönetmek için tasarlanmış bir .NET 9 tabanlı servis.
Sunucu, eğitim ve kategori verilerini SQLite üzerinde saklar, doğrulama kurallarını FluentValidation
ile uygular ve tüm CRUD akışını Swagger üzerinden belgelendirir.

## Öne Çıkan Özellikler
- Eğitim kayıtlarını sayfalı olarak listeleme, detay görüntüleme, oluşturma, güncelleme ve silme uç noktaları.
- Kategori yönetimi için CRUD servisleri ve eğitimlerle bire-çok ilişki.
- FluentValidation destekli alan kontrolleri ve ProblemDetails tabanlı hata yanıtları.
- İlk çalıştırmada EF Core migration'larının otomatik uygulanması ve hazır Swagger arayüzü.

## Teknoloji Yığını
| Bileşen | Sürüm | Not |
| --- | --- | --- |
| ASP.NET Core Web API | net9.0 | Minimal hosting modeli ve `MapControllers` kullanımı.【F:training-catalog-api.csproj†L1-L22】【F:Program.cs†L12-L70】 |
| EF Core + SQLite | 9.0.x | `AppDbContext` aracılığıyla eğitim ve kategori tablolarını yönetir.【F:Data/AppDbContext.cs†L6-L48】 |
| FluentValidation.AspNetCore | 11.3.1 | DTO tabanlı doğrulama zincirleri sağlar.【F:training-catalog-api.csproj†L10-L20】【F:Validators/Training/TrainingCreateDtoValidator.cs†L7-L35】 |
| Swashbuckle (Swagger) | 9.0.4 | Otomatik API belgesi üretimi ve test konsolu.【F:training-catalog-api.csproj†L10-L20】【F:Program.cs†L52-L68】 |

## Proje Yapısı
```
training-catalog-api/
├── Controller/        # HTTP uç noktaları
├── DTO/               # İstek/yanıt veri transfer nesneleri
├── Models/            # EF Core varlıkları
├── Repositories/      # Veri erişim katmanı
├── Services/          # İş kuralları ve mapping katmanı
├── Validators/        # FluentValidation kuralları
├── Data/AppDbContext  # DbContext tanımı ve fluent API konfigurasyonu
└── Program.cs         # Servis kayıtları, middleware zinciri, otomatik migration
```

## Geliştirme Gereksinimleri
- [.NET SDK 9.0.x](https://dotnet.microsoft.com/)
- SQLite 3 veya EF Core'un desteklediği alternatif bir sağlayıcı
- İsteğe bağlı araçlar: `dotnet-ef`, Postman/Insomnia, herhangi bir HTTP istemcisi

## Yapılandırma
Varsayılan yapılandırma `appsettings.json` dosyasında yer alır; SQLite dosyası proje kökündeki
`training.db` olarak tanımlanmıştır. Ek ortamlarda aşağıdaki değişkenler üzerinden bağlantı
parametrelerini özelleştirebilirsiniz.

```bash
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Data Source=training.db
```

CORS politikası varsayılan olarak `http://localhost:5173` adresine izin verir; farklı istemciler için
`Program.cs` içerisinde `CorsPolicy` tanımını güncelleyin.【F:Program.cs†L39-L47】

## Çalıştırma Adımları
1. Bağımlılıkları yükleyin:
   ```bash
   dotnet restore
   ```
2. Veritabanını hazırlayın (opsiyonel; uygulama başlatılırken de tetiklenir):
   ```bash
   dotnet ef database update
   ```
3. Geliştirme sunucusunu başlatın:
   ```bash
   dotnet watch run
   ```
   Varsayılan adresler `http://localhost:5212` ve `https://localhost:7236` olarak belirlenmiştir.【F:Properties/launchSettings.json†L4-L22】

İlk çalıştırmada `Program.cs` içerisinde tanımlanan scope sayesinde migration işlemi otomatik olarak
uygulanır.【F:Program.cs†L71-L76】

## Veri Modeli
### Training
| Alan | Tip | Kurallar |
| --- | --- | --- |
| `Id` | int | Otomatik artan birincil anahtar.【F:Models/Training.cs†L5-L19】 |
| `Title` | string (≤120) | Zorunlu; uzunluk sınırı FluentValidation ile kontrol edilir.【F:Models/Training.cs†L7-L18】【F:Validators/Training/TrainingCreateDtoValidator.cs†L11-L18】 |
| `ShortDescription` | string (≤280) | Zorunlu alan ve maksimum uzunluk doğrulaması.【F:Models/Training.cs†L7-L18】【F:Validators/Training/TrainingCreateDtoValidator.cs†L15-L18】 |
| `LongDescription` | string | Boş bırakılamaz.【F:Models/Training.cs†L7-L18】【F:Validators/Training/TrainingCreateDtoValidator.cs†L19-L20】 |
| `CategoryId` | int? | Opsiyonel yabancı anahtar; ilişkiler `AppDbContext` üzerinde tanımlanmıştır.【F:Models/Training.cs†L11-L12】【F:Data/AppDbContext.cs†L43-L47】 |
| `ImageUrl` | string? | URL formatı kontrol edilir.【F:Models/Training.cs†L13】【F:Validators/Training/TrainingCreateDtoValidator.cs†L27-L35】 |
| `StartDate` / `EndDate` | DateTime? | `EndDate >= StartDate` şartı sağlanmalıdır.【F:Models/Training.cs†L14-L15】【F:Validators/Training/TrainingCreateDtoValidator.cs†L22-L25】 |
| `IsPublished` | bool | Yayın durumunu ifade eder.【F:Models/Training.cs†L16】 |
| `CreatedAt` / `UpdatedAt` | DateTime? | Oluşturma ve güncelleme zaman damgaları.【F:Models/Training.cs†L17-L18】 |

### Category
| Alan | Tip | Kurallar |
| --- | --- | --- |
| `Id` | int | Birincil anahtar.【F:Models/Category.cs†L3-L8】 |
| `CategoryName` | string | Zorunlu alan; boş gönderim reddedilir.【F:Models/Category.cs†L5-L7】【F:Validators/Category/CategoryCreateDtoValidator.cs†L6-L11】 |
| `Trainings` | `ICollection<Training>` | İlişkili eğitimler otomatik yüklenebilir (include).【F:Models/Category.cs†L5-L7】【F:Data/AppDbContext.cs†L43-L47】 |

## Servis Akışı
`Controller` katmanı HTTP isteklerini alır, FluentValidation kontrollerinden geçen DTO'ları servis
katmanına iletir ve repository üzerinden veri erişimini tamamlar.【F:Controller/TrainingController.cs†L17-L73】【F:Controller/CategoryController.cs†L17-L74】【F:Services/Training/TrainingService.cs†L7-L84】【F:Services/Category/CategoryService.cs†L6-L62】
Sayfalama `TrainingService.GetAllAsync` içerisinde yapılır ve repository `Skip/Take` kullanarak
verileri döndürür.【F:Services/Training/TrainingService.cs†L49-L53】【F:Repositories/Training/TrainingRepository.cs†L44-L51】

## Uç Noktalar
| Metot | URL | Açıklama |
| --- | --- | --- |
| GET | `/api/trainings?pageNumber=1&pageSize=10` | Eğitimi sayfalı listeler, kayıt yoksa `204 No Content` döner.【F:Controller/TrainingController.cs†L17-L26】 |
| GET | `/api/trainings/{id}` | Tekil eğitim detayını döner; bulunamazsa `404 Not Found`.【F:Controller/TrainingController.cs†L28-L37】 |
| POST | `/api/trainings` | Yeni eğitim oluşturur, geçersiz veri geldiğinde `400 Bad Request`.【F:Controller/TrainingController.cs†L39-L55】 |
| PUT | `/api/trainings/{id}` | Mevcut kaydı günceller, başarıda `204 No Content`.【F:Controller/TrainingController.cs†L57-L66】 |
| DELETE | `/api/trainings/{id}` | Kaydı siler; bulunamazsa `404 Not Found`.【F:Controller/TrainingController.cs†L68-L73】 |
| GET | `/api/category` | Kategori listesini döner; kayıt yoksa `204 No Content`.【F:Controller/CategoryController.cs†L17-L26】 |
| GET | `/api/category/{id}` | Tekil kategori ve ilişkili eğitimleri döner.【F:Controller/CategoryController.cs†L28-L37】【F:Repositories/Category/CategoryRepository.cs†L31-L45】 |
| POST | `/api/category` | Kategori oluşturur, validasyon hatalarında `400 Bad Request`.【F:Controller/CategoryController.cs†L39-L55】 |
| PUT | `/api/category/{id}` | Kategori günceller; başarısızlıkta `404 Not Found`.【F:Controller/CategoryController.cs†L57-L67】 |
| DELETE | `/api/category/{id}` | Kategoriyi siler; bağlı eğitimler mevcutsa ilişkiden dolayı kısıt olabilir.【F:Controller/CategoryController.cs†L69-L73】【F:Data/AppDbContext.cs†L43-L47】 |

## Örnek İstek ve Yanıtlar
### Eğitim Listeleme
```http
GET /api/trainings?pageNumber=1&pageSize=3 HTTP/1.1
Host: localhost:5212
Accept: application/json
```
```json
[
  {
    "id": 1,
    "title": "React Temelleri",
    "shortDescription": "Component yapısı, router ve form doğrulama.",
    "longDescription": "Markdown ile zengin içerik desteği...",
    "categoryId": 2,
    "imageUrl": "https://example.com/react.png",
    "startDate": "2025-10-12T00:00:00",
    "endDate": "2025-10-14T00:00:00",
    "isPublished": true
  }
]
```

### Eğitim Detayı
```http
GET /api/trainings/1 HTTP/1.1
Host: localhost:5212
Accept: application/json
```
```json
{
  "id": 1,
  "title": "React Temelleri",
  "shortDescription": "Component yapısı, router ve form doğrulama.",
  "longDescription": "Markdown ile zengin içerik desteği...",
  "category": {
    "id": 2,
    "categoryName": "Frontend"
  },
  "isPublished": true,
  "startDate": "2025-10-12T00:00:00",
  "endDate": "2025-10-14T00:00:00"
}
```

### Eğitim Oluşturma
```http
POST /api/trainings HTTP/1.1
Host: localhost:5212
Content-Type: application/json
```
```json
{
  "title": "Azure Fundamentals",
  "shortDescription": "Bulut bilişime giriş",
  "longDescription": "Azure servislerine hızlı başlangıç...",
  "categoryId": 3,
  "imageUrl": "https://example.com/azure.png",
  "startDate": "2025-11-01T09:00:00",
  "endDate": "2025-11-02T17:00:00",
  "isPublished": false
}
```
Yanıt:
```http
HTTP/1.1 201 Created
Location: /api/trainings/5
```
```json
{
  "id": 5,
  "title": "Azure Fundamentals",
  "isPublished": false
}
```

## Hata Yönetimi
- `400 Bad Request`: FluentValidation hataları `ModelState` ile geri döner.
- `404 Not Found`: İstenen kaynak bulunamadığında verilir.
- `204 No Content`: Liste uç noktalarında veri olmadığında döner.
- Global hatalar `ProblemDetails` yapısıyla üretilir ve `UseExceptionHandler` üzerinden servis edilir.【F:Controller/TrainingController.cs†L39-L52】【F:Controller/CategoryController.cs†L39-L55】【F:Program.cs†L49-L60】

## Dokümantasyon ve Gözlemlenebilirlik
Uygulama ayağa kalktığında Swagger UI otomatik olarak servis edilir.
`https://localhost:7236/swagger` adresinden uç noktaları test edebilir,
`AddSwaggerGen` konfigürasyonunu genişleterek ek açıklamalar ekleyebilirsiniz.【F:Program.cs†L52-L68】

## API Yanıt Önizlemeleri
- ![Sayfalama yanıtı örneği](data/list-view.png)
- ![Detay yanıtı örneği](data/detail-view.png)
