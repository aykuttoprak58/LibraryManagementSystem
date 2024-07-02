# LibraryManagementSystem

Kullanıcıların kütüphaneden kitap ödünç alma ve iade etme işlemlerinin takibini gerçekleştimek amacıyla geliştirilmiştir.

LibrarySql.sql dosyasından veritabanını yükleyebiirsiniz.

Kayıt işlemi yapıldıktan sonra admin yetkisi sadece Members tablosunda bulunan MemberShipType kolonunu Admin olarak değiştirilerek kullanıcıya admin yetkis verilir.

Aşağıdaki gibi 3 tip  Üyelik Türü vardır.
1.Öğrenci ( kullanıcı adı : berk58 şifre : 123456)
2.Vatandaş ( kullanıcı adı : ahmet58 şifre : 123456)
3.Admin ( kullanıcı adı : karahan58 şifre : 123456)

Öğrencinin 4 adet Kitap alma hakkı vardır ve kitap iade tarihi aldığı günden 30 gün sonradır.
Vatandaşın 2 adet Kitap alma hakkı vardır ve kitap iade tarihi aldığı günden 14 gün sonradır.

Kullanıcı kitabı geç iade ettiğinde ceza yemektedir.2 defa üst üste ceza yediğinde kullanıcı hesabı pasif hale gelir ve giriş yapamaz.30 gün sonra tekrar aktif hale gelir.

Kullanıcının ceza sayısı 4 olduğunda hesabı kalıcı olarak iptal edilir.

Aşağıdaki gibi uyarı mesajları gelmektedir.

1. 7 GÜN ÖNCE KİTAP İADE HATIRLATMA MESAJI
2. KİTAP İADESİ İÇİN SON GÜN İADE HATIRLATMA MESAJI
3. GEÇ İADE CEZA MESAJI
4. 2 KEZ ÜST ÜSTE GEÇ İADE CEZA MESAJI
5. 4 KEZ ÜST ÜSTE GEÇ İADE CEZA MESAJI
6. ŞİFREMİ UNUTTUM MESAJI

Kullanıcı şifresini sıfırlamak istediğinde email adresine passwordresettoken gönderilir kullanıcı bu numarayaı ve yeni şifresini girerek şifresini başarıyla günceller.

# Önemli Not
Uyarı Maillerin kendi hesabınıza gelmesi için
Gmail hesabınızın 2-Step Verification kısmından App passwords üretmelisiniz
MyQuery Class'ının içindeki  SendEmail1 - SendEmail7 methodlarında bulunan aşağıdaki değişkenleri kendi gmail ve app pasword şifreniz ile doldurunuz 

 string fromMail = "kendi gmail hesabınız";
 string fromPassword = "app posword şifreniz";

# Kullanılan Teknolojiler
1.  .NET 8
2.  Authorization ve Authentication
3.  HangFire
4.  BackGround Service
5.  Ado.Net
