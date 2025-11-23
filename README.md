# QuizApp

Yapılanlar: <br>

- Veritabanı tabloları için gerekli modeller oluşturuldu.
- Her bir model için ayrı ayrı repositoryler oluşturuldu.
- Modeller ApplicationDbContext'te tanıtıldı.
- Farklı işlem grupları için gerekli servisler oluşturuldu. (User servisi, Question servisi gibi)
- Veritabanı değişikliklerinde senkronizasyonu sağlamak için Unit of Work yapısı kuruldu. Repositoryler buraya dahil edildi.
- User ve Question controllerları oluşturuldu.
- Adminlerin soru oluşturması ve raporlanmış soruları görebilmesi için gerekli endpointler controllerlara eklendi.
- Kullanıcıların kategori bazlı soruları çekebilmesi, soruları cevaplandırabilmesi ve raporlayabilmesi için endpointler oluşturuldu.
- Auth0 implementasyonu için gerekli güncellemeler yapıldı.
