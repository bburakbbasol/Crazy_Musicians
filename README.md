# 🎸 Crazy Musicians API

Bu proje, **ASP.NET Core Web API** kullanılarak geliştirilmiş bir **Müzisyen Yönetim Sistemi** uygulamasıdır. Kullanıcılar **müzisyen ekleyebilir, güncelleyebilir, silebilir ve müzisyen detaylarını görüntüleyebilirler**.

---

## 🚀 Proje Özellikleri

✅ **Müzisyen Yönetimi**  
- Müzisyen ekleme, düzenleme, silme ve listeleme  
- Müzisyen detaylarını görüntüleme  

✅ **Arama ve Filtreleme**  
- İsme veya mesleğe göre müzisyen arama  
- Sayfalama desteğiyle listeleme  

✅ **RESTful API Desteği**  
- `GET`, `POST`, `PUT`, `PATCH` ve `DELETE` HTTP metodlarıyla CRUD işlemleri  

---

## 🛠 Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|-----------|---------|
| **ASP.NET Core Web API** | Backend geliştirme |
| **C# ve LINQ** | Veri işleme ve sorgulama |
| **JSON Patch** | Parçalı güncelleme işlemleri |

---

## 🔧 API Kullanımı

### 🎵 Tüm Müzisyenleri Getir
```http
GET /api/musicians
```

### 🎤 Belirli Bir Müzisyeni Getir
```http
GET /api/musicians/{id}
```

### 🎼 İsme Göre Müzisyen Ara
```http
GET /api/musicians/Name/{name}
```

### 🎹 Yeni Müzisyen Ekle
```http
POST /api/musicians
```
**Body:**
```json
{
  "name": "Yeni Müzisyen",
  "job": "Yeni Meslek",
  "description": "Açıklama"
}
```

### 🎷 Müzisyen Bilgilerini Güncelle (Tam Güncelleme)
```http
PUT /update?id={id}
```
**Body:**
```json
{
  "id": 1,
  "name": "Güncellenmiş Müzisyen",
  "job": "Güncellenmiş Meslek",
  "description": "Yeni Açıklama"
}
```

### 🥁 Müzisyen Bilgilerini Güncelle (Parçalı Güncelleme)
```http
PATCH /reschedule/{id}
```
**Body:**
```json
[
  {
    "op": "replace",
    "path": "/name",
    "value": "Yeni İsim"
  }
]
```

### 🎺 Müzisyeni Sil
```http
DELETE /delete/{id}
```

### 🔍 Müzisyen Arama
```http
GET /api/musicians/Search?keyword={keyword}&page={page}&pageSize={pageSize}
```

---

## 🚀 Kurulum ve Çalıştırma

#### 1. Projeyi Klonla
```bash
git clone https://github.com/bburakbbasol/Crazy-Musicians.git
```

#### 2. Projeyi Aç ve Çalıştır
```bash
cd Crazy-Musicians
dotnet run
```

API artık çalışıyor ve müzisyenleri yönetebilirsiniz! 🎶

