# Taşınmaz Yönetim Sistemi

Bu proje, **Angular 15** (frontend) ve **ASP.NET Core 8 Web API** (backend) teknolojileri kullanılarak geliştirilmiş, kullanıcı ve yönetici (admin) rollerine sahip bir taşınmaz (emlak) yönetim sistemidir. Sistem, kullanıcıların kendilerine ait taşınmaz kayıtlarını eklemesine, güncellemesine ve silmesine olanak tanırken, yöneticilere tüm kullanıcıları ve sistem loglarını yönetme yetkisi sağlar.


## 🚀 Teknolojiler

**Frontend:**
- Angular 15
- Bootstrap 5
- Reactive Forms
- OpenLayers (Harita entegrasyonu)

**Backend:**
- ASP.NET Core 8 Web API
- Entity Framework Core
- PostgreSQL
- JWT (JSON Web Token) ile Authentication
- Role-based Authorization (Admin / Kullanıcı)
- SHA256 + Salt ile şifreleme
- Loglama sistemi
- Excel ve PDF çıktı sistemleri

## 👥 Roller ve Yetkilendirme

| Rol        | Yetkiler                                                                 |
|------------|--------------------------------------------------------------------------|
| **Kullanıcı** | Kendi taşınmazlarını ekleme, güncelleme, silme, PDF/Excel çıktısı alma |
| **Admin**     | Tüm kullanıcıları ve taşınmazları listeleme, kullanıcı ekleme/silme, log kaydı görüntüleme, tüm taşınmazları haritada görüntüleme |

## 🧩 Proje Özellikleri

- 👤 JWT ile kullanıcı giriş ve rol kontrolü
- 🏠 Kullanıcının kendisine ait taşınmazları yönetmesi
- 🗺️ Harita üzerinde taşınmazların konumlarının gösterilmesi
- 📄 PDF ve Excel formatında veri çıktısı alma
- 🧑‍💼 Admin panelinden kullanıcı yönetimi
- 📋 Log kayıtlarının tutulması ve filtrelenmesi
- 🔒 Güvenlik için token bazlı route guard (Angular)

## 🛠️ Kurulum

### 1. Veritabanı
- PostgreSQL kurulu olmalı
- `appsettings.json` içerisindeki bağlantı ayarları güncellenmeli

### 2. Backend (ASP.NET Core)
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run

### 3. Frontend (Angular)

cd frontend
npm install
ng serve
