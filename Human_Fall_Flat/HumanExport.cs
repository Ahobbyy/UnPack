using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HumanAPI;
using Multiplayer;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public sealed class HumanExport : EditorWindow
{
	public enum Languages
	{
		kEnglish,
		kFrench,
		kSpanish,
		kGerman,
		kRussian,
		kItalian,
		kChinese,
		kJapanese,
		kKorean,
		kBrazilian,
		kTurkish,
		kThai,
		kIndonesian,
		kPolish,
		kUkrainian,
		kPortuguese,
		kLithuanian
	}

	public class LanguageStrings
	{
		public string[] localisedStrings;

		public LanguageStrings(params string[] localisations)
		{
			localisedStrings = localisations;
		}
	}

	private enum StringIDs
	{
		kOpenWorkshopFolder = 0,
		kLevelTitle = 1,
		kLevelDescription = 2,
		kExportButton = 3,
		kValidationErrors = 4,
		kLoadScreenshot = 5,
		kLoadFromLevel = 6,
		kWindowName = 7,
		kDreamType = 8,
		kRecommendedPlayers = 9,
		kThemeTags = 10,
		kValidationFailure1 = 11,
		kValidationFailure2 = 12,
		kValidationFailure3 = 13,
		kValidationFailure4 = 14,
		kValidationFailure5 = 0xF,
		kValidationFailure6 = 0x10,
		kValidationFailure7 = 17,
		kValidationFailure8 = 18,
		kValidationFailure9 = 19,
		kValidationFailure10 = 20,
		kValidationFailure11 = 21,
		kValidationFailure12 = 22,
		kValidationFailure13 = 23,
		kValidationFailure14 = 24,
		kValidationFailure15 = 25,
		kTagLevelTypeOption1 = 26,
		kTagLevelTypeOption2 = 27,
		kTagLevelTypeOption3 = 28,
		kTagPlayer1 = 29,
		kTagPlayer2 = 30,
		kTagPlayer3 = 0x1F,
		kTagPlayer4 = 0x20,
		kTagPlayer5 = 33,
		kTagPlayer6 = 34,
		kTagPlayer7 = 35,
		kTagPlayer8 = 36,
		kExporting = 37,
		kLastTranslated = 37,
		kLevelTagName1 = 38,
		kFirstNonTranslated = 38,
		kLevelTagName2 = 39,
		kLevelTagName3 = 40,
		kLevelTagName4 = 41,
		kLevelTagName5 = 42,
		kLevelTagName6 = 43,
		kLevelTagName7 = 44,
		kLevelTagName8 = 45,
		kLevelTagName9 = 46,
		kLevelTagName10 = 47,
		kLevelTagName11 = 48
	}

	private enum IconSource
	{
		kNone,
		kLevel,
		kGame
	}

	public enum TagLevelType
	{
		kSinglePlayer,
		kMultiPlayer,
		kLobby
	}

	private const string kThumbnailFilename = "thumbnail.png";

	private const string kBundleFilename = "data";

	private const string kLevelsFolder = "Levels/";

	public const string kWorkshopFolderName = "Workshop/";

	private const int kMaxValidationStringSize = 4096;

	private const int kThumbNailDisplaySize = 256;

	private const float kAspectRatio = 1.77777779f;

	private const int kMaxPlayers = 8;

	private const string kLanguagePref = "HE-Lang";

	private const string kPrefabPath = "Assets/WorkShop/Prefabs";

	private const float kMinimumWindowHeight = 570f;

	private static string[] sLanguages = new string[17]
	{
		"ENGLISH", "FRANÇAIS", "ESPAÑOL", "DEUTSCH", "РУССКИЙ", "ITALIANO", "简体中文", "日本語", "한국어", "PORTUGUÊS BRASILEIRO",
		"TÜRKÇE", "ภาษาไทย", "BAHASA INDONESIA", "POLSKI", "УКРАЇНСЬКА", "PORTUGUÊS", "LIETUVIŲ KALBA"
	};

	private static Languages currentLanguage;

	private static readonly LanguageStrings[] sStrings = new LanguageStrings[38]
	{
		new LanguageStrings("Open Workshop folder", "Ouvrir le dossier de l'atelier", "Abrir carpeta de Workshop", "Workshop-Ordner öffnen", "Открыть папку мастерской", "Apri la cartella del laboratorio", "打开工坊文件夹", "ワークショップフォルダーを開く", "창작마당 폴더 열기", "Abrir pasta da oficina", "Atölye klasörünü aç", "เป\u0e34ดโฟลเดอร\u0e4cเว\u0e34ร\u0e4cกช\u0e47อป", "Folder Workshop Terbuka:", "Otwórz folder Warsztatu", "Відкрити вкладку майстерні", "Abrir pasta da Oficina", "Atidarykite dirbtuvių aplanką"),
		new LanguageStrings("Dream Title", "Titre du rêve", "Título de sueño", "Traum-Titel", "Название мечты", "Titolo del sogno", "梦境标题", "ドリームタイトル", "꿈 제목", "Título do Sonho", "Rüya Başlığı", "ช\u0e37\u0e48อความฝ\u0e31น", "Judul Dream", "Tytuł snu", "Назва мрії", "Título do sonho", "Sapnų antraštė"),
		new LanguageStrings("Dream Description", "Description du rêve", "Descripción de sueño", "Traum-Beschreibung", "Описание мечты", "Descrizione del sogno", "梦境描述", "ドリーム説明", "꿈 설명", "Descrição do Sonho", "Rüya Açıklaması", "รายละเอ\u0e35ยดของความฝ\u0e31น", "Deskripsi Dream", "Opis snu", "Опис мрії", "Descrição do sonho", "Sapnų aprašymas"),
		new LanguageStrings("Export Dream", "Exporter le rêve", "Exportar sueño ", "Traum exportieren", "Экспортировать мечту", "Esporta sogno", "导出梦境", "ドリームをエクスポート", "꿈 내보내기", "Exportar Sonho", "Rüyayı Dışarı Aktar", "ส\u0e48งออกความฝ\u0e31น", "Ekspor Dream", "Eksportuj sen", "Експортувати мрію", "Exportar sonho", "Exportuoti sapną"),
		new LanguageStrings("Export Errors", "Erreurs d'export", "Errores de exportación", "Export-Fehler", "Ошибки экспортирования", "Esporta errori", "导出错误", "エクスポートエラー", "오류 내보내기", "Erros ao exportar", "Dışa Aktarım Hataları", "ข\u0e49อผ\u0e34ดพลาดในการส\u0e48งออก", "Kesalahan Ekspor", "Błędy eksportowanego pliku", "Помилки експортування", "Erros de exportação", "Eksportavimo klaidos"),
		new LanguageStrings("Load Screenshot", "Charger la capture d'écran", "Cargar captura de pantalla", "Bildschirmfoto laden", "Загрузите скриншот", "Carica screenshot", "加载屏幕截图", "スクリーンショットロード", "스크린샷 불러오기", "Carregar captura de tela", "Ekran Görüntüsü Yükle", "โหลดภาพหน\u0e49าจอ", "Muat Cuplikan Layar", "Wczytaj zrzut ekranu", "Завантажити скріншот", "Carregar captura de imagem", "Įkelti ekrano vaizdo kopiją"),
		new LanguageStrings("Load Dream Image", "Charger l'image du rêve", "Cargar imagen de sueño", "Traumbild laden", "Загрузите изображение мечты", "Carica immagine sogno", "加载梦境图像", "ドリーム画像ロード", "꿈 이미지 불러오기", "Carregar imagem do sonho", "Rüya İmajı Yükle", "โหลดภาพความฝ\u0e31น", "Muat Gambar Dream", "Wczytaj obraz snu", "Завантажити зображення мрії", "Carregar imagem do sonho", "Ikelti sapnų paveikslėlį"),
		new LanguageStrings("Human Export", "Exportation", "Exportación humana", "Menschen-Export", "Экспорт человека", "Human Export", "人类导出", "ヒューマンエクスポート", "휴먼 내보내기", "Exportação humana", "İnsan Dışarı Aktarımı", "ส\u0e48งออกมน\u0e38ษย\u0e4c", "Ekspor Human", "Human Export", "Експорт людини", "Exportar humano", "Rankinis eksportas"),
		new LanguageStrings("Dream Type", "Type de rêve", "Tipo de sueño", "Traum-Typ", "Тип мечты", "Tipo di sogno", "梦境类型", "ドリームタイプ", "꿈 유형", "Tipo do Sonho", "Rüya Türü", "ประเภทความฝ\u0e31น", "Jenis Dream", "Rodzaj snu", "Тип мрії", "Tipo de sonho", "Sapnų tipas"),
		new LanguageStrings("Recommended Players", "Pour", "Jugadores recomendados", "Empfohlene Spieler", "Рекомендованные игроки", "Espellendo il giocatore…", "推荐玩家", "推奨プレイヤー", "추천한 플레이어", "Jogadores Recomendados", "Tavsiye Edilen Oyuncular", "ผ\u0e39\u0e49เล\u0e48นท\u0e35\u0e48แนะนำ", "Pemain Direkomendasikan", "Polecani gracze", "Рекомендовані гравці", "Jogadores recomendados", "Rekomenduojamas žaidėjų kiekis"),
		new LanguageStrings("Theme Tags", "Tags thématiques", "Etiquetas de tema", "Motiv-Schlagwörter", "Теги темы", "Tag dei temi", "主题标签", "テーマタグ", "테마 태그", "Marcadores de Tema", "Tema Etiketleri", "แท\u0e47กของธ\u0e35ม", "Tag Tema", "Tagi motywów", "Теги теми", "Tags do tema", "Temos žymos"),
		new LanguageStrings("There should be no cameras in the scene.", "Il ne doit pas y avoir de caméra dans la scène.", "No debería haber cámaras en la escena.", "In dieser Szene sollten keine Kameras sein.", "В месте действия не должно быть камер.", "Non dovrebbero esserci telecamere nella scena.", "场景中不应该有摄像头。", "シーン内でカメラは禁止されています。", "이 장면에 카메라가 없어야 합니다.", "Não devem existir câmeras na cena.", "Sahnede kamera olmamalıdır.", "ไม\u0e48ควรม\u0e35กล\u0e49องอย\u0e39\u0e48ในฉาก", "Sepertinya tidak ada kamera di scene.", "W scenie nie powinno być kamer.", "У місці дії не повинно бути камер.", "Não deve haver câmaras na cena.", "Scenoje neturi būti nei vienos kameros."),
		new LanguageStrings("There is more than one level in the scene.", "Il y a plus d'un niveau dans la scène.", "Hay más de un nivel en la escena.", "In dieser Szene ist mehr als ein Level.", "Место действия содержит больше одного уровня.", "C'è più di un livello nella scena.", "场景中有多个关卡。", "シーンにはレベルが1つ以上存在します。", "이 장면에 두 종류 이상의 레벨이 존재합니다.", "Existe mais de um nível na cena.", "Sahnede birden fazla seviye var.", "ในฉากม\u0e35มากกว\u0e48า 1 ด\u0e48าน", "Ada lebih dari satu level di scene.", "W scenie jest więcej niż jeden poziom.", "Місце дії містить більше одного рівня.", "A cena contém mais do que um nível.", "Scenoje yra daugiau nei vienas lygis."),
		new LanguageStrings("There are no level objects in the scene. Check Example scenes.", "Il n'y a pas d'objets dans la scène. Référez-vous à l'exemple.", "No hay objetos de nivel en la escena. Revisa las escenas de ejemplo.", "In dieser Szene sind keine Level-Objekte. Siehe Beispiel-Szenen.", "В месте действия отсутствуют объекты уровня. Ознакомьтесь с примерами.", "Non ci sono oggetti del livello nella scena. Controlla gli esempi di scene.", "场景中没有关卡对象。检查示例场景。", "シーン内ではレベルオブジェクトが存在しません。例を参照してください。", "이 장면에 레벨 관련 객체가 없습니다. 예제 장면을 확인하세요.", "Não existem objetos de nível na cena. Verifique cenas de exemplo.", "Sahnede seviye nesnesi yok. Örnek sahneleri kontrol et.", "ไม\u0e48ม\u0e35อ\u0e47อบเจกต\u0e4cด\u0e48านในฉากน\u0e35\u0e49 โปรดตรวจสอบฉากต\u0e31วอย\u0e48าง", "Tidak ada objek level di scene. Periksa scene Contoh.", "W scenie nie ma żadnych obiektów poziomu. Sprawdź sceny przykładowe.", "У місці дії відсутні об'єкти рівня. Погляньте на приклади.", "Não há objetos do nível na cena. Consulta os Exemplos da cena.", "Scenoje nėra lygio objektų. Pasižiūrėkite pavyzdines scenas."),
		new LanguageStrings("SpawnPoint not defined on level.", "Le SpawnPoint n'est pas défini.", "no definido en el nivel.", "SpawnPoint bei Level nicht definiert.", "На уровне не определена точка появления SpawnPoint.", "SpawnPoint nel livello non definito.", "关卡中还未定义‘重生点’。", "スポーンポイントがレベルで定義されていません。", "레벨 내에 생성 장소가 정의되지 않았습니다.", "SpawnPoint não definido no nível.", "Seviyede Başlama Noktası belirlenmemiş.", "ไม\u0e48ได\u0e49น\u0e34ยาม SpawnPoint ไว\u0e49ในด\u0e48าน", "SpawnPoint tidak ditentukan di level.", "Nie zdefiniowano punktu odradzania na poziomie.", "На рівні не визначено точку появи SpawnPoint.", "Ponto de Regeneração não definido no nível.", "„SpawnPoint“ lygyje nenustatytas"),
		new LanguageStrings("There are no lights in the level. Check Example scenes.", "Il n'y a pas d'éclairage dans la scène. Référez-vous à l'exemple.", "No hay luces en el nivel. Revisa las escenas de ejemplo.", "In diesem Level gibt es keine Lichter. Siehe Beispiel-Szenen.", "На уровне отсутствуют источники света. Ознакомьтесь с примерами.", "Non ci sono luci nel livello. Controlla le scene di esempio.", "当前关卡没有光线设置。检查示例场景。", "レベル内ではライトが存在しません。例を参照してください。", "이 레벨에 광원이 없습니다. 예제 장면을 확인하세요.", "Não existem luzes no nível. Verifique cenas de exemplo.", "Seviyede ışık yok. Örnek sahneleri kontrol et.", "ไม\u0e48ม\u0e35แสดงสว\u0e48างในด\u0e48าน โปรดตรวจสอบฉากต\u0e31วอย\u0e48าง", "Tidak ada cahaya di level tersebut. Periksa scene Contoh.", "Na poziomie występuje brak oświetlenia. Sprawdź sceny przykładowe.", "На рівні відсутні джерела світла. Погляньте на приклади.", "O nível não contém luzes. Consulta os Exemplos da cena.", "Lygyje nėra šviesų. Pasižiūrėkite pavyzdines scenas."),
		new LanguageStrings("WARNING: It is recommended to have a directional light with baking set to Realtime to represent the sun", "AVERTISSEMENT\u00a0: pour représenter le soleil, nous recommandons l'utilisation d'un éclairage directionnel dont la lightmap est définie en temps réel", "ADVERTENCIA: Se recomienda tener una luz direccional con Realtime baking para representar el sol.", "HINWEIS: Es wird empfohlen, ein gebackenes Richtungslicht in Echtzeit für die Sonne zu verwenden", "ПРЕДУПРЕЖДЕНИЕ: для имитации солнца рекомендуется установить направленный источник света со световым излучением в режиме реального времени.", "ATTENZIONE: è consigliabile possedere una luce direzionale con il baking impostato in tempo reale per raffigurare il sole", "警告！建议设置一个代表太阳的定向灯光源。", "警告：太陽を取り入れ、リアルタイムに保存する場合、指向性ライト使用することをお勧めします。", "경고: 실시잔으로 설정된 베이킹 세트와 지향성 광원을 사용해 태양을 표현하는 것을 권장합니다", "AVISO: É recomendado ter uma luz direcional com baking definido para Realtime para representar o sol", "UYARI: Güneşi temsil etmesi için ayarı Gerçek Zamanlı olarak belirlenmiş bir yön ışığı yerleştirilmesi tavsiye edilir.", "คำเต\u0e37อน: แนะนำให\u0e49ม\u0e35แสงแบบกำหนดท\u0e34ศทาง พร\u0e49อมต\u0e31\u0e49งค\u0e48าช\u0e38ดเบคก\u0e34\u0e49งเป\u0e47น \"เร\u0e35ยลไทม\u0e4c\" เพ\u0e37\u0e48อใช\u0e49แทนแสงแดด", "PERINGATAN: Disarankan memiliki cahaya direksional dengan set baking disetel ke Realtime untuk merepresentasikan matahari", "UWAGA: Zaleca się stosowanie kierunkowego światła z wypalaniem (Baking) w ustawieniu Realtime w celu uzyskania efektu słońca.", "УВАГА: для імітації сонця рекомендується встановити направлене джерело світла зі світловим випромінюванням в режимі реального часу.", "ATENÇÃO: Recomendamos que utilizes uma luz direcional com \"baking\" definido como Tempo Real para representar o Sol", "DĖMESIO: rekomenduojama turėti kryptinę šviesą ir „baking“ parametrui priskirti „Realtime“ funkciją, kad atrodytų, kaip saulė "),
		new LanguageStrings("There are no FallTriggers in the level.  Check Example scenes.", "Il n'y a pas de FallTriggers dans la scène. Référez-vous à l'exemple.", "No hay FallTriggers en el nivel. Revisa las escenas de ejemplo.", "In diesem Level gibt es keine FallTriggers.  Siehe Beispiel-Szenen.", "На уровне отсутствуют триггеры падения FallTriggers.  Ознакомьтесь с примерами.", "Non ci sono FallTriggers nel livello. Controlla le scene di esempio.", "当前关卡没有‘跌落触发’设置。检查示例场景。", "レベル内ではフォールトリガーが存在しません。例を参照してください。", "이 레벨에 낙하 트리거가 없습니다. 예제 장면을 확인하세요.", "Não existem FallTriggers no nível. Verifique cenas de exemplo.", "Seviyede Düşme Tetikleyici yok. Örnek sahneleri kontrol et.", "ไม\u0e48ม\u0e35 FallTriggers ในด\u0e48าน โปรดตรวจสอบฉากต\u0e31วอย\u0e48าง", "Tidak ada FallTrigger di level tersebut. Periksa scene Contoh.", "Na poziomie nie ma żadnych elementów typu FallTrigger. Sprawdź sceny przykładowe.", "На рівні відсутні тригери падіння FallTriggers.  Погляньте на приклади.", "O nível não contém FailTriggers. Consulta os Exemplos da cena.", "Lygyje nėra „FallTriggers“. Pasižiūrėkite pavyzdines scenas."),
		new LanguageStrings("There is no LevelPassTrigger in the level.  Check Example scenes.", "Il n'y a pas de LevelPassTriggers dans la scène. Référez-vous à l'exemple.", "No hay LevelPassTrigger en el nivel. Revisa las escenas de ejemplo.", "In diesem Level gibt es kein LevelPassTrigger.  Siehe Beispiel-Szenen.", "На уровне отсутствуют триггеры прохождения уровня LevelPassTrigger.  Ознакомьтесь с примерами.", "Non ci sono LevelPassTrigger nel livello. Controlla le scene di esempio.", "当前关卡没有‘通关触发’设置。检查示例场景。", "レベル内ではレベルパストリガーが存在しません。例を参照してください。", "이 레벨에 레벨 통과 트리거가 없습니다. 예제 장면을 확인하세요.", "Não existe LevelPassTrigger no nível. Verifique cenas de exemplo.", "Seviyede Seviye Geçme Tetikleyici yok. Örnek sahneleri kontrol et.", "ไม\u0e48ม\u0e35 LevelPassTrigger ในด\u0e48าน โปรดตรวจสอบฉากต\u0e31วอย\u0e48าง", "Tidak ada LevelPassTrigger di level tersebut. Periksa scene Contoh.", "Na poziomie nie ma żadnych elementów typu LevelPassTrigger. Sprawdź sceny przykładowe.", "На рівні відсутні тригери проходження рівня LevelPassTrigger.  Погляньте на приклади.", "O nível não contém LevelPassTriggers. Consulta os Exemplos da cena.", "Lygyje nėra „LevelPassTriggers“. Pasižiūrėkite pavyzdines scenas."),
		new LanguageStrings("Checkpoint {0} missing.", "Le point de contrôle {0} est absent.", "Falta el punto de control {0}.", "Kontrollpunkt {0} fehlt.", "Отсутствует контрольная точка {0}.", "Checkpoint {0} mancante.", "存档点 {0} 缺失。", "チェックポイント{0}が存在しません。", "체크포인트 {0}이(가) 없습니다.", "Ponto de verificação {0} ausente.", "{0} kontrol noktası eksik.", "ไม\u0e48ม\u0e35เช\u0e47กพอยต\u0e4c {0}", "Checkpoint {0} tidak ditemukan.", "Brak punktu kontrolnego {0}.", "Відсутня контрольна точка {0}.", "Ponto de controlo {0} em falta.", "Trūksta {0} tikrinimo taško."),
		new LanguageStrings("Dream Title cannot be empty.", "Le titre ne peut être laissé vide.", "El título del sueño no puede estar vacío.", "Traum-Titel darf nicht leer sein.", "Название мечты не может оставаться пустым.", "Il titolo del sogno non può essere vuoto.", "梦境标题不能为空。", "ドリームタイトルは空白にはできません。", "꿈 제목은 공란으로 둘 수 없습니다.", "Título do Sonho não pode ser vazio.", "Rüya Başlığı boş olamaz.", "ช\u0e37\u0e48อความฝ\u0e31นต\u0e49องไม\u0e48เว\u0e49นว\u0e48าง", "Judul Dream tidak boleh kosong.", "Pole tytułu snu nie może być puste.", "Назва мрії не може залишатися порожньою.", "O título do sonho não pode estar vazio.", "Sapnų antraštės laukelis negali būti tuščias."),
		new LanguageStrings("Dream Description cannot be empty.", "La description ne peut être laissée vide.", "La descripción del sueño no puede estar vacía.", "Traum-Beschreibung darf nicht leer sein.", "Описание мечты не может оставаться пустым.", "La descrizione del sogno non può essere vuota.", "梦境描述不能为空。", "ドリーム説明は空白にはできません。", "꿈 설명은 공란으로 둘 수 없습니다.", "Descrição do Sonho não pode ser vazio.", "Rüya Açıklaması boş olamaz.", "รายละเอ\u0e35ยดของความฝ\u0e31นต\u0e49องไม\u0e48เว\u0e49นว\u0e48าง", "Deskripsi Dream tidak boleh kosong.", "Pole opisu snu nie może być puste.", "Опис мрії не може залишатися порожнім.", "A descrição do sonho não pode estar vazia.", "Sapnų aprašymo laukelis negali būti tuščias."),
		new LanguageStrings("Invalid metadata.", "Métadonnées invalides.", "Metadatos no válidos", "Ungültige Metadaten.", "Неправильные метаданные.", "Metadati non validi.", "无效元数据。", "無効なメタデータ。", "잘못된 메타데이터입니다.", "Metadados inválidos.", "Geçersiz meta veri.", "ข\u0e49อม\u0e39ลเมตาไม\u0e48ถ\u0e39กต\u0e49อง", "Metadata tidak valid.", "Nieprawidłowe metadane.", "Неправильні метадані.", "Metadados inválidos.", "Neteisingi meta duomenys."),
		new LanguageStrings("Dream Image required.", "Image de rêve requise.", "Imagen de sueño requerida.", "Traumbild benötigt.", "Требуется изображение мечты.", "Necessaria immagine sogno", "需要描述图像。", "ドリーム画像が必要です。", "꿈 이미지가 필요합니다.", "Imagem do sonho necessária.", "Rüya İmajı gerekli.", "ต\u0e49องม\u0e35ร\u0e39ปภาพความฝ\u0e31น", "Gambar Dream harus ada.", "Wymagany jest obraz snu.", "Потрібне зображення мрії.", "Requer uma imagem do sonho.", "Reikalingas sapnų paveikslėlis."),
		new LanguageStrings("All game objects must be children of the level object. Check Example scenes.", "Tous les objets du jeu doivent être des enfants de l'objet du niveau. Vous pouvez vous référer aux exemples de scènes.", "Todos los objetos del juego deben ser vástagos del objeto de nivel. Consulta las escenas de ejemplo.", "Alle Spielobjekte müssen dem Level-Objekt untergeordnet sein. Siehe Beispiel-Szenen.", "Все игровые объекты должны быть дочерними для объектов уровня. Ознакомьтесь с примерами.", "Tutti gli oggetti di gioco devo derivare dagli oggetti del livello. Dai uno sguardo agli esempi.", "所有游戏对象必须是本关对象的子项。请查看示例场景。", "すべてのオブジェクトは子供レベルである必要があります。例を参照してください。", "모든 게임 오브젝트는 레벨 오브젝트에 종속되어야 합니다. 예시 장면을 확인하세요.", "Todos os objetos de jogo devem ser filhos do objecto de nível. Confira os exemplos.", "Tüm oyun nesneleri, seviye nesnesinin alt öğeleri olmalıdır. Örnek sahnelere bakabilirsin.", "ว\u0e31ตถ\u0e38ท\u0e31\u0e49งหมดในเกมต\u0e49องเป\u0e47นระด\u0e31บล\u0e48างของว\u0e31ตถ\u0e38ในด\u0e48าน ด\u0e39ฉากต\u0e31วอย\u0e48าง", "Semua objek game harus anak-anak dari objek level tersebut. Periksa scene Contoh.", "Wszystkie obiekty muszą być pochodną obiektu z poziomu. Sprawdź sceny przykładowe.", "Усі ігрові об'єкти повинні бути дочірніми для об'єктів рівня. Погляньте на приклади.", "Todo objeto de jogo deve ser filho do objeto de nível. Vê exemplos.", "Objekto lygije visi žaidimo objektai turi būti dukterinės klasės objektai. Pasžiurėkite pavyzdine scena."),
		new LanguageStrings("Object '{0}' has a NetBody but no RigidBody. A NetBody only needs to be on objects that have a RigidBody.", "L'objet '{0}' a un NetBody mais pas de RigidBody. Un NetBody n'est seulement nécessaire aux objets ayant un RigidBody.", "El objeto '{0}' tiene un NetBody pero no RigidBody. Un NetBody solo necesita estar en objetos que tengan un RigidBody.", "Objekt '{0}' hat einen NetBody, aber keinen RigidBody. Ein NetBody wird nur bei Objekten benötigt, die einen RigidBody haben.", "Объект '{0}' имеет NetBody, но не имеет RigidBody. NetBody должен быть только на тех объектах, которые имеют RigidBody.", "L'oggetto {0} ha un Netbody ma non ha un RigidBody. Il Netbody può essere solo in oggetti che hanno un RigidBody.", "对象'{0}'有 NetBody，但没有 RigidBody 。具有 RigidBody 的对象才需要 NetBody 。", "オブジェクト「{0}」にはNetBodyがありますが、RigidBodyはありません。NetBodyは、RigidBodyがあるオブジェクトのみに設置する必要があります。", "'{0}' 오브젝트에 NetBody가 있지만 RigidBody가 없습니다. NetBody는 RigidBody가 있는 오브젝트에 있어야 합니다.", "O objeto '{0}' tem um NetBody mas não tem um RigidBody. Um NetBody precisa estar em objetos com RigidBody.", "Nesne '{0}', NetBody'e sahip ama RigidBody'e sahip değil. Bir NetBody, yalnızca bir RigidBody'si olan nesnelerin üzerinde olmalıdır.", "ออบเจ\u0e47กต\u0e4c '{0}' ม\u0e35 NetBody แต\u0e48ไม\u0e48ม\u0e35 RigidBody โปรดทราบว\u0e48าออบเจ\u0e47กต\u0e4cจะม\u0e35 Netbody ได\u0e49ก\u0e47ต\u0e48อเม\u0e37\u0e48อม\u0e35 RigidBody ด\u0e49วย", "Objek '{0}' memiliki NetBody tetapi tidak memiliki RigidBody. NetBody hanya perlu berada di dalam objek yang memiliki RigidBody.", "Obiekt „{0}” ma właściwość „NetBody”, ale nie ma właściwości „RigidBody”. Parametr „NetBody” jest wymagany wyłącznie w obiektach mających właściwość „RigidBody”.", "Об'єкт '{0}' має NetBody, але не має RigidBody. NetBody повинен бути тільки на тих об'єктах, які мають RigidBody.", "O objeto '{0}' tem um NetBody mas não tem RigidBody. NetBody só precisa de estar em objetos que possuam um RigidBody.", "„{0}“ objektas turi „NetBody“, tačiau neturi „RigidBody“. „NetBody“ reikalingas tik tiems objektams, kurie turi „RigidBody“."),
		new LanguageStrings("Singleplayer", "Solo", "Un jugador", "Einzelspieler", "Одиночная игра", "Giocatore singolo", "单人模式", "シングルプレイヤー", "싱글 플레이", "Um jogador", "Tek Oyunculu", "เล\u0e48นคนเด\u0e35ยว", "Singleplayer", "Gra solowa", "Один гравець", "Um jogador", "Vienas žaidėjas"),
		new LanguageStrings("Multiplayer", "Multijoueur", "Multijugador", "Mehrspieler", "Многопользовательская игр", "Multigiocatore", "多人模式", "マルチプレイヤー", "멀티 플레이", "Multijogador", "Çok Oyunculu", "เล\u0e48นหลายคน", "Multiplayer", "Gra wieloosobowa", "Багато гравців", "Multijogador", "Keletas žaidėjų"),
		new LanguageStrings("Lobby", "Salon", "Vestíbulo", "Lobby", "Лобби", "Lobby", "大厅", "ロビー", "로비", "Lobby", "Lobi", "ล\u0e47อบบ\u0e35\u0e49", "Lobi", "Lobby", "Лобі", "Sala de espera", "Vestibiulis"),
		new LanguageStrings("1 Player", "1\u00a0joueur", "1 jugador", "1 Spieler", "1 игрок", "1 Giocatore", "1名玩家", "1プレイヤー", "플레이어 1", "1 Jogador", "1 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 1 คน", "1 Pemain", "1 gracz", "1 гравець", "1 jogador", "1 žaidėjas"),
		new LanguageStrings("2 Players", "2\u00a0joueurs", "2 jugadores", "2 Spieler", "2 игроков", "2 Giocatori", "2名玩家", "2プレイヤー", "플레이어 2", "2 Jogadores", "2 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 2 คน", "2 Pemain", "2 graczy", "2 гравці", "2 jogadores", "2 žaidėjai"),
		new LanguageStrings("3 Players", "3\u00a0joueurs", "3 jugadores", "3 Spieler", "3 игроков", "3 Giocatori", "3名玩家", "3プレイヤー", "플레이어 3", "3 Jogadores", "3 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 3 คน", "3 Pemain", "3 graczy", "3 гравці", "3 jogadores", "3 žaidėjai"),
		new LanguageStrings("4 Players", "4\u00a0joueurs", "4 jugadores", "4 Spieler", "4 игроков", "4 Giocatori", "4名玩家", "4プレイヤー", "플레이어 4", "4 Jogadores", "4 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 4 คน", "4 Pemain", "4 graczy", "4 гравці", "4 jogadores", "4 žaidėjai"),
		new LanguageStrings("5 Players", "5\u00a0joueurs", "5 jugadores", "5 Spieler", "5 игроков", "5 Giocatori", "5名玩家", "5プレイヤー", "플레이어 5", "5 Jogadores", "5 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 5 คน", "5 Pemain", "5 graczy", "5 гравців", "5 jogadores", "5 žaidėjai"),
		new LanguageStrings("6 Players", "6\u00a0joueurs", "6 jugadores", "6 Spieler", "6 игроков", "6 Giocatori", "6名玩家", "6プレイヤー", "플레이어 6", "6 Jogadores", "6 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 6 คน", "6 Pemain", "6 graczy", "6 гравців", "6 jogadores", "6 žaidėjai"),
		new LanguageStrings("7 Players", "7\u00a0joueurs", "7 jugadores", "7 Spieler", "7 игроков", "7 Giocatori", "7名玩家", "7プレイヤー", "플레이어 7", "7 Jogadores", "7 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 7 คน", "7 Pemain", "7 graczy", "7 гравців", "7 jogadores", "7 žaidėjai"),
		new LanguageStrings("8 Players", "8\u00a0joueurs", "8 jugadores", "8 Spieler", "8 игроков", "8 Giocatori", "8名玩家", "8プレイヤー", "플레이어 8", "8 Jogadores", "8 Oyuncu", "ผ\u0e39\u0e49เล\u0e48น 8 คน", "8 Pemain", "8 graczy", "8 гравців", "8 jogadores", "8 žaidėjai"),
		new LanguageStrings("Exporting…..", "Exportation en cours...", "Exportando…", "Export läuft...", "Экспортирование...", "Esportazione…", "正在导出......", "エクスポート中…", "내보내는 중...", "Exportando.....", "Dışarı aktarılıyor...", "กำล\u0e31งส\u0e48งออก…..", "Mengekspor...", "Eksportowanie...", "Експортується...", "A exportar…..", "Eksportuojama...")
	};

	private static readonly StringIDs[] sTagLevelTypeOptions = new StringIDs[2]
	{
		StringIDs.kTagLevelTypeOption1,
		StringIDs.kTagLevelTypeOption2
	};

	private static readonly StringIDs[] sTagPlayersOptions = new StringIDs[8]
	{
		StringIDs.kTagPlayer1,
		StringIDs.kTagPlayer2,
		StringIDs.kTagPlayer3,
		StringIDs.kTagPlayer4,
		StringIDs.kTagPlayer5,
		StringIDs.kTagPlayer6,
		StringIDs.kTagPlayer7,
		StringIDs.kTagPlayer8
	};

	private static readonly string[] sThemeFolderNames = new string[13]
	{
		"Mansion", "Train", "Carry", "Mountain", "Demolition", "Castle", "Water", "PowerPlant", "Aztec", "Dark",
		"Steam", "Christmas", "Ice"
	};

	private static string[] sThemeNames = new string[13]
	{
		"Mansion", "Train", "Carry", "Mountain", "Demolition", "Castle", "Water", "Power Plant", "Aztec", "Dark",
		"Steam", "Christmas", "Ice"
	};

	private static string[] sThemeFolderNamesPathed = new string[sThemeFolderNames.Length];

	private WorkshopItemMetadata metadata;

	private Texture2D thumbnail;

	private Scene lastScene;

	private string lastBundleName;

	private bool exportInProgress;

	private static string sWorkshopPath;

	private static string sWorkshopLevelsPath;

	private static DateTime sLastMetadataWrite;

	private static DateTime sLastIconRead;

	private static bool sNeedsValidate = true;

	private static bool sIsValid = false;

	private string validationString;

	private StringBuilder validationStringBuilder = new StringBuilder(4096);

	private static string sModPath;

	private static string sModThumbPath;

	private static string sThumbPath;

	private static bool sGameIconExists = false;

	private static bool sLevelIconExists = false;

	private static int sTagLevelTypeSelected = 1;

	private static int sTagNumberPlayersSelected = 7;

	private static string[] sTagLevelTypeOptionsStrings;

	private static string[] sTagNumberPlayersOptionsStrings;

	private static bool staticsInited;

	private bool isLobby;

	private bool isLobbyInit;

	private static HashSet<int> sThemeTags = new HashSet<int>();

	private static StringBuilder sThemeBuilder = new StringBuilder(128);

	private static int sThemesBitfield = 0;

	private const float kTimeBetweenValidates = 5f;

	private float mTimeUntilValidate;

	private bool readThemeTagsFromMeta;

	private Vector2 scroll;

	private static IconSource sIconSource { get; set; }

	private static string GetString(StringIDs id)
	{
		if (id > StringIDs.kExporting)
		{
			int num = (int)(id - 38);
			return sThemeNames[num];
		}
		return sStrings[(int)id].localisedStrings[(int)currentLanguage];
	}

	[MenuItem("Human/Human Export")]
	private static void Init()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		EditorWindow window = EditorWindow.GetWindow(typeof(HumanExport), false, GetString(StringIDs.kWindowName), true);
		Rect position = window.get_position();
		if (((Rect)(ref position)).get_height() < 570f)
		{
			Rect position2 = window.get_position();
			((Rect)(ref position2)).set_height(570f);
			window.set_position(position2);
		}
		window.Show();
	}

	private void ResetThemes()
	{
		readThemeTagsFromMeta = false;
		sThemesBitfield = 0;
	}

	private void InitThemes()
	{
		if (!staticsInited)
		{
			BuildThemeStringPathed();
		}
		MakeThemeList();
		BuildThemeString();
	}

	private void InitStatics()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (!staticsInited)
		{
			InitThemes();
			currentLanguage = (Languages)EditorPrefs.GetInt("HE-Lang");
			EditorSceneManager.remove_sceneOpened(new SceneOpenedCallback(EditorSceneManager_sceneOpened));
			EditorSceneManager.add_sceneOpened(new SceneOpenedCallback(EditorSceneManager_sceneOpened));
			EditorSceneManager.remove_sceneSaved(new SceneSavedCallback(EditorSceneManager_sceneSaved));
			EditorSceneManager.add_sceneSaved(new SceneSavedCallback(EditorSceneManager_sceneSaved));
			SetWorkshopPath();
			if (!Directory.Exists(sWorkshopPath))
			{
				Directory.CreateDirectory(sWorkshopPath);
			}
			sWorkshopLevelsPath = Path.Combine(sWorkshopPath, "Levels/");
			if (!Directory.Exists(sWorkshopLevelsPath))
			{
				Directory.CreateDirectory(sWorkshopLevelsPath);
			}
			BuildTagOptionStrings();
			staticsInited = true;
			CheckForLobby(force: true);
		}
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (EditorApplication.get_isPlaying())
		{
			return;
		}
		Scene activeScene = SceneManager.GetActiveScene();
		if (((Scene)(ref activeScene)).get_isDirty())
		{
			mTimeUntilValidate -= Time.get_deltaTime();
			if (mTimeUntilValidate < 0f)
			{
				NeedsValidate();
				mTimeUntilValidate = 5f;
			}
		}
	}

	private void EditorSceneManager_sceneSaved(Scene scene)
	{
		NeedsValidate();
	}

	private void EditorSceneManager_sceneOpened(Scene scene, OpenSceneMode mode)
	{
		SceneChange();
	}

	private static void BuildTagOptions(StringIDs[] ids, out string[] options)
	{
		int num = ids.Length;
		options = new string[num];
		for (int i = 0; i < num; i++)
		{
			options[i] = GetString(ids[i]);
		}
	}

	private static void BuildTagOptionStrings()
	{
		BuildTagOptions(sTagLevelTypeOptions, out sTagLevelTypeOptionsStrings);
		BuildTagOptions(sTagPlayersOptions, out sTagNumberPlayersOptionsStrings);
	}

	private void ReadThemeTagFromMeta(string themeTags)
	{
		if (readThemeTagsFromMeta)
		{
			return;
		}
		sThemesBitfield = 0;
		for (int i = 0; i < sThemeNames.Length; i++)
		{
			if (themeTags.IndexOf(sThemeNames[i], StringComparison.OrdinalIgnoreCase) >= 0)
			{
				sThemesBitfield |= 1 << i;
			}
		}
		BuildThemeString();
		readThemeTagsFromMeta = true;
	}

	private static void MakeThemeList()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (sThemesBitfield != 0)
		{
			return;
		}
		sThemeTags.Clear();
		sThemeBuilder.Length = 0;
		sThemesBitfield = 0;
		GameObject[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		foreach (GameObject val in array)
		{
			if ((int)((Object)val).get_hideFlags() == 8 || (int)((Object)val).get_hideFlags() == 61 || !EditorUtility.IsPersistent((Object)(object)val) || (int)PrefabUtility.GetPrefabType((Object)(object)val) == 0)
			{
				continue;
			}
			string assetOrScenePath = AssetDatabase.GetAssetOrScenePath((Object)(object)val);
			if (!assetOrScenePath.Contains("Assets/WorkShop/Prefabs"))
			{
				continue;
			}
			for (int j = 0; j < sThemeFolderNames.Length; j++)
			{
				if (assetOrScenePath.IndexOf(sThemeFolderNamesPathed[j], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					sThemesBitfield |= 1 << j;
					break;
				}
			}
		}
	}

	private static void BuildThemeStringPathed()
	{
		for (int i = 0; i < sThemeFolderNames.Length; i++)
		{
			string text = "/" + sThemeFolderNames[i] + "/";
			sThemeFolderNamesPathed[i] = text;
		}
	}

	private static void BuildThemeString()
	{
		sThemeBuilder.Length = 0;
		for (int i = 0; i < sThemeFolderNames.Length; i++)
		{
			if ((sThemesBitfield & (1 << i)) > 0)
			{
				if (sThemeBuilder.Length > 0)
				{
					sThemeBuilder.Append(", ");
				}
				sThemeBuilder.Append(sThemeNames[i]);
			}
		}
	}

	private static void SetWorkshopPath()
	{
		if (sWorkshopPath == null)
		{
			sWorkshopPath = Path.GetFullPath(Path.Combine(Application.get_persistentDataPath(), "Workshop/"));
			NeedsValidate();
		}
	}

	private void OpenWorkshopFolderWindow()
	{
		EditorUtility.RevealInFinder(sWorkshopLevelsPath);
	}

	private static void NeedsValidate()
	{
		sNeedsValidate = true;
	}

	private void ResetIconSource()
	{
		sIconSource = IconSource.kNone;
		sLastIconRead = default(DateTime);
	}

	private void ReadThumbnail(string thumbPath)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		if ((Object)(object)thumbnail != (Object)null)
		{
			Object.DestroyImmediate((Object)(object)thumbnail);
			thumbnail = null;
		}
		try
		{
			byte[] array = File.ReadAllBytes(thumbPath);
			if (array != null)
			{
				thumbnail = new Texture2D(1, 1, (TextureFormat)3, true, false);
				ImageConversion.LoadImage(thumbnail, array);
				TextureScale.Bilinear(thumbnail, 256, 144);
				thumbnail.Apply();
			}
		}
		catch
		{
		}
	}

	private bool UpdateIcon(string iconPath)
	{
		if (File.Exists(iconPath))
		{
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(iconPath);
			if ((Object)(object)thumbnail == (Object)null || lastWriteTimeUtc.CompareTo(sLastIconRead) > 0)
			{
				ReadThumbnail(iconPath);
				sLastIconRead = lastWriteTimeUtc;
			}
			return true;
		}
		return false;
	}

	private bool FindLatestIcon(string modThumbPath, string thumbPath)
	{
		if (!sGameIconExists && !sLevelIconExists)
		{
			return false;
		}
		if (sGameIconExists && sLevelIconExists)
		{
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(thumbPath);
			DateTime lastWriteTimeUtc2 = File.GetLastWriteTimeUtc(modThumbPath);
			sIconSource = ((lastWriteTimeUtc.CompareTo(lastWriteTimeUtc2) < 0) ? IconSource.kLevel : IconSource.kGame);
			return true;
		}
		sIconSource = ((!sGameIconExists) ? IconSource.kLevel : IconSource.kGame);
		return true;
	}

	private void LoadDefaultIcon()
	{
		thumbnail = null;
	}

	private void GetIconFromSource(string modThumbPath, string thumbPath)
	{
		sGameIconExists = File.Exists(thumbPath);
		sLevelIconExists = File.Exists(modThumbPath);
		bool flag = true;
		switch (sIconSource)
		{
		case IconSource.kNone:
			if (FindLatestIcon(modThumbPath, thumbPath))
			{
				GetIconFromSource(modThumbPath, thumbPath);
				return;
			}
			LoadDefaultIcon();
			break;
		case IconSource.kLevel:
			flag = UpdateIcon(modThumbPath);
			break;
		case IconSource.kGame:
			flag = UpdateIcon(thumbPath);
			break;
		}
		if (!flag)
		{
			sIconSource = IconSource.kNone;
			GetIconFromSource(modThumbPath, thumbPath);
		}
	}

	private void CheckIcon(string modThumbPath, string thumbPath)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (!exportInProgress)
		{
			Scene activeScene = SceneManager.GetActiveScene();
			if (!string.IsNullOrEmpty(((Scene)(ref activeScene)).get_name()))
			{
				GetIconFromSource(modThumbPath, thumbPath);
			}
		}
	}

	private void MakeEmptyMetadata(string sceneName)
	{
		metadata = new WorkshopItemMetadata
		{
			itemType = WorkshopItemType.Levels,
			title = sceneName,
			workshopId = 0uL
		};
	}

	private void CheckMetadata(string metaPath)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (exportInProgress)
		{
			return;
		}
		Scene activeScene = SceneManager.GetActiveScene();
		if (string.IsNullOrEmpty(((Scene)(ref activeScene)).get_name()))
		{
			MakeEmptyMetadata(((Scene)(ref activeScene)).get_name());
			return;
		}
		bool flag = false;
		_ = sLastMetadataWrite;
		if (metadata != null)
		{
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(Path.GetFullPath(Path.Combine(metaPath, "metadata.json")));
			if (lastWriteTimeUtc.CompareTo(sLastMetadataWrite) > 0)
			{
				flag = true;
				ResetThemes();
				sLastMetadataWrite = lastWriteTimeUtc;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			NeedsValidate();
			metadata = WorkshopItemMetadata.Load(metaPath);
			if (metadata == null)
			{
				MakeEmptyMetadata(((Scene)(ref activeScene)).get_name());
				return;
			}
			ReadThemeTagFromMeta(metadata.themeTags);
			sTagLevelTypeSelected = metadata.typeTags;
			sTagNumberPlayersSelected = metadata.playerTags;
		}
	}

	private static void MakeModAndThumbnailPaths(string bundleName, out string modPath, out string modThumbPath)
	{
		modPath = Path.Combine(sWorkshopPath, "Levels/" + bundleName);
		modThumbPath = Path.Combine(modPath, "thumbnail.png");
	}

	private static void MakeThumbnailPath(out string thumbPath)
	{
		thumbPath = Path.Combine(Application.get_persistentDataPath(), "thumbnail.png");
	}

	private void BuildIconUI()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		if ((Object)(object)thumbnail != (Object)null)
		{
			Rect aspectRect = GUILayoutUtility.GetAspectRect(1.77777779f, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width((float)((Texture)thumbnail).get_width()),
				GUILayout.Height((float)((Texture)thumbnail).get_height())
			});
			((Rect)(ref aspectRect)).set_center(new Vector2(EditorGUIUtility.get_currentViewWidth() / 2f, ((Rect)(ref aspectRect)).get_center().y));
			EditorGUI.DrawPreviewTexture(aspectRect, (Texture)(object)thumbnail);
		}
		else
		{
			GetIconFromSource(sModThumbPath, sThumbPath);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUI.set_enabled(sGameIconExists);
		GUIContent val = new GUIContent(GetString(StringIDs.kLoadScreenshot));
		if (GUI.Button(GUILayoutUtility.GetRect(val, GUI.get_skin().get_button()), val))
		{
			ResetIconSource();
			sIconSource = IconSource.kGame;
			GetIconFromSource(sModThumbPath, sThumbPath);
		}
		GUI.set_enabled(sLevelIconExists);
		val = new GUIContent(GetString(StringIDs.kLoadFromLevel));
		if (GUI.Button(GUILayoutUtility.GetRect(val, GUI.get_skin().get_button()), val))
		{
			ResetIconSource();
			sIconSource = IconSource.kLevel;
			GetIconFromSource(sModThumbPath, sThumbPath);
		}
		GUI.set_enabled(true);
		EditorGUILayout.EndHorizontal();
	}

	private bool BuildExportButtonUI()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.Space(20f);
		GUIContent val = new GUIContent(GetString(StringIDs.kExportButton));
		Rect rect = GUILayoutUtility.GetRect(val, GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
		((Rect)(ref rect)).set_center(new Vector2(EditorGUIUtility.get_currentViewWidth() / 2f, ((Rect)(ref rect)).get_center().y));
		return GUI.Button(rect, val);
	}

	private void BuildLanguageDropdown()
	{
		Languages languages = currentLanguage;
		currentLanguage = (Languages)EditorGUILayout.Popup((int)currentLanguage, sLanguages, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (languages != currentLanguage)
		{
			BuildTagOptionStrings();
			EditorPrefs.SetInt("HE-Lang", (int)currentLanguage);
			sNeedsValidate = true;
		}
	}

	private void BuildTagsUI(string bundleName)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		EditorGUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		string @string = GetString(StringIDs.kDreamType);
		string string2 = GetString(StringIDs.kRecommendedPlayers);
		if (isLobby)
		{
			sTagLevelTypeSelected = 2;
			sTagNumberPlayersSelected = 7;
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField(@string, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(EditorGUIUtility.get_labelWidth()) });
			EditorGUILayout.LabelField(GetString(StringIDs.kTagLevelTypeOption3), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField(string2, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(EditorGUIUtility.get_labelWidth()) });
			EditorGUILayout.LabelField(sTagNumberPlayersOptionsStrings[sTagNumberPlayersSelected], (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			sTagLevelTypeSelected = EditorGUILayout.Popup(@string, sTagLevelTypeSelected, sTagLevelTypeOptionsStrings, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			sTagNumberPlayersSelected = EditorGUILayout.Popup(string2, sTagNumberPlayersSelected, sTagNumberPlayersOptionsStrings, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		}
		EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.LabelField(GetString(StringIDs.kThemeTags), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(EditorGUIUtility.get_labelWidth()) });
		int num = 0;
		EditorGUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		for (int i = 0; i < 7; i++)
		{
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(100f) });
			int num2 = 0;
			while (num2 < 2)
			{
				if (num < sThemeNames.Length)
				{
					if (GUILayout.Toggle((sThemesBitfield & (1 << num)) > 0, sThemeNames[num], (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(100f) }))
					{
						sThemesBitfield |= 1 << num;
					}
					else
					{
						sThemesBitfield &= ~(1 << num);
					}
				}
				num2++;
				num++;
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}

	private void SceneChange()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		InitStatics();
		ResetThemes();
		InitThemes();
		lastScene = SceneManager.GetActiveScene();
		lastBundleName = ((Scene)(ref lastScene)).get_name().ToLowerInvariant();
		MakeThumbnailPath(out sThumbPath);
		MakeModAndThumbnailPaths(lastBundleName, out sModPath, out sModThumbPath);
		ResetIconSource();
		GetIconFromSource(sModThumbPath, sThumbPath);
		metadata = null;
		CheckMetadata(sModPath);
		CheckIcon(sModThumbPath, sThumbPath);
		NeedsValidate();
		CheckForLobby(force: true);
		((EditorWindow)this).Repaint();
	}

	private void OnGUI()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		if (exportInProgress)
		{
			EditorGUILayout.LabelField(GetString(StringIDs.kExporting), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			return;
		}
		EditorGUIUtility.set_labelWidth(200f);
		InitStatics();
		EditorGUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		BuildLanguageDropdown();
		Scene activeScene = SceneManager.GetActiveScene();
		string text = ((Scene)(ref activeScene)).get_name().ToLowerInvariant();
		MakeThumbnailPath(out sThumbPath);
		MakeModAndThumbnailPaths(text, out sModPath, out sModThumbPath);
		if (activeScene != lastScene || text != lastBundleName)
		{
			SceneChange();
		}
		if (sNeedsValidate)
		{
			sIsValid = Validate();
			sNeedsValidate = false;
		}
		if (GUILayout.Button(GetString(StringIDs.kOpenWorkshopFolder), (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			OpenWorkshopFolderWindow();
		}
		BuildIconUI();
		EditorGUILayout.LabelField(GetString(StringIDs.kLevelTitle), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUI.BeginChangeCheck();
		string text2 = EditorGUILayout.TextField(metadata.title, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (EditorGUI.EndChangeCheck() && text2.Length >= 129)
		{
			text2 = text2.Substring(0, 128);
		}
		if (!text2.Equals(metadata.title))
		{
			sNeedsValidate = true;
		}
		metadata.title = text2;
		EditorGUILayout.LabelField(GetString(StringIDs.kLevelDescription), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUI.BeginChangeCheck();
		string text3 = EditorGUILayout.TextArea(metadata.description, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(70f) });
		if (EditorGUI.EndChangeCheck() && text3.Length >= 8000)
		{
			text3 = text3.Substring(0, 7999);
		}
		if (!text3.Equals(metadata.description))
		{
			sNeedsValidate = true;
		}
		metadata.description = text3;
		BuildTagsUI(text);
		if (!string.IsNullOrEmpty(validationString))
		{
			Color contentColor = GUI.get_contentColor();
			GUI.set_contentColor(Color.get_red());
			EditorGUILayout.LabelField(GetString(StringIDs.kValidationErrors), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.TextArea(validationString, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_contentColor(contentColor);
		}
		GUI.set_enabled(sIsValid);
		bool num = BuildExportButtonUI();
		GUI.set_enabled(true);
		EditorGUILayout.EndVertical();
		if (num)
		{
			Export(activeScene);
		}
	}

	private void SetLevelHash()
	{
		BuiltinLevel builtinLevel = Object.FindObjectOfType<BuiltinLevel>();
		if (!((Object)(object)builtinLevel == (Object)null))
		{
			builtinLevel.netHash = (uint)Random.Range(0, int.MaxValue) | 0x80000000u;
			EditorUtility.SetDirty((Object)(object)builtinLevel);
		}
	}

	private void SetMetadata()
	{
		metadata.playerTags = sTagNumberPlayersSelected;
		BuildThemeString();
		metadata.themeTags = sThemeBuilder.ToString();
		metadata.typeTags = sTagLevelTypeSelected;
	}

	private void Export(Scene activeScene)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		IconSource iconSource = sIconSource;
		string externalVersionControl = EditorSettings.get_externalVersionControl();
		exportInProgress = true;
		SetLevelHash();
		SetMetadata();
		metadata.flags = 1;
		WorkshopItemMetadata workshopItemMetadata = JsonUtility.FromJson<WorkshopItemMetadata>(JsonUtility.ToJson((object)metadata));
		if (Validate())
		{
			RenderSettings.set_ambientMode((AmbientMode)3);
			RenderSettings.set_ambientLight(Color32.op_Implicit(new Color32((byte)108, (byte)123, (byte)150, byte.MaxValue)));
			RenderSettings.set_fogColor(Color32.op_Implicit(new Color32((byte)178, (byte)183, (byte)191, byte.MaxValue)));
			RenderSettings.set_fogDensity(0.01f);
			RenderSettings.set_fogMode((FogMode)3);
			EditorSettings.set_externalVersionControl("");
			PlayerSettings.set_colorSpace((ColorSpace)1);
			PlayerSettings.SetGraphicsAPIs((BuildTarget)5, (GraphicsDeviceType[])(object)new GraphicsDeviceType[2]
			{
				(GraphicsDeviceType)2,
				(GraphicsDeviceType)17
			});
			Lightmapping.set_bakedGI(false);
			Lightmapping.set_realtimeGI(true);
			if (EditorSceneManager.SaveModifiedScenesIfUserWantsTo((Scene[])(object)new Scene[1] { activeScene }))
			{
				BuildAssetBundleOptions val = (BuildAssetBundleOptions)0;
				val = (BuildAssetBundleOptions)(val | 1);
				AssetBundleBuild[] array = (AssetBundleBuild[])(object)new AssetBundleBuild[1]
				{
					new AssetBundleBuild
					{
						assetBundleName = "data",
						assetNames = new string[1] { ((Scene)(ref activeScene)).get_path() }
					}
				};
				Directory.CreateDirectory("AssetBundles");
				if ((Object)(object)BuildPipeline.BuildAssetBundles("AssetBundles", array, val, (BuildTarget)5) != (Object)null)
				{
					Directory.CreateDirectory(sModPath);
					workshopItemMetadata.Save(sModPath);
					if (iconSource != IconSource.kLevel)
					{
						string destFileName = Path.Combine(sModPath, "thumbnail.png");
						File.Copy(sThumbPath, destFileName, overwrite: true);
					}
					string outfile = Path.Combine(sModPath, "data");
					AssetBundleParser.ParseAssetBundle("AssetBundles/data", outfile);
				}
			}
			OnFocus();
		}
		EditorSettings.set_externalVersionControl(externalVersionControl);
		exportInProgress = false;
	}

	private void CheckForLobby(bool force = false)
	{
		if (!force && isLobbyInit)
		{
			return;
		}
		isLobby = (Object)(object)Object.FindObjectOfType<MultiplayerLobbyController>() != (Object)null;
		isLobbyInit = true;
		if (metadata != null)
		{
			if (isLobby)
			{
				metadata.itemType = WorkshopItemType.Lobbies;
				metadata.typeTags = 2;
			}
			else
			{
				metadata.itemType = WorkshopItemType.Levels;
				metadata.typeTags = 1;
			}
		}
		if (sTagLevelTypeSelected == 2)
		{
			sTagLevelTypeSelected = 1;
		}
	}

	private void OnProjectChange()
	{
		SceneChange();
		OnFocus();
	}

	private void OnFocus()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		InitStatics();
		Scene activeScene = SceneManager.GetActiveScene();
		if (!string.IsNullOrEmpty(((Scene)(ref activeScene)).get_name()))
		{
			string bundleName = ((Scene)(ref activeScene)).get_name().ToLowerInvariant();
			MakeThumbnailPath(out sThumbPath);
			MakeModAndThumbnailPaths(bundleName, out sModPath, out sModThumbPath);
			CheckMetadata(sModPath);
			CheckIcon(sModThumbPath, sThumbPath);
		}
	}

	private void OnLostFocus()
	{
	}

	private bool Validate()
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Invalid comparison between Unknown and I4
		bool result = true;
		validationStringBuilder.Length = 0;
		CheckForLobby(force: true);
		try
		{
			if (!isLobby && (Object)(object)Object.FindObjectOfType<Camera>() != (Object)null)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure1));
				result = false;
			}
			Level[] array = Object.FindObjectsOfType<Level>();
			if (array.Length > 1)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure2));
				result = false;
			}
			if (array.Length == 0)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure3));
				result = false;
			}
			Level level = array[0];
			if ((Object)(object)level.spawnPoint == (Object)null)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure4));
				result = false;
			}
			Light[] componentsInChildren = ((Component)level).GetComponentsInChildren<Light>();
			if (componentsInChildren.Length == 0)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure5));
				result = false;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			GameObject[] rootGameObjects = ((Scene)(ref activeScene)).GetRootGameObjects();
			foreach (GameObject val in rootGameObjects)
			{
				if (!((Object)(object)val.GetComponent<Level>() != (Object)null) && (!isLobby || !((Object)(object)val.GetComponent<MultiplayerLobbyController>() != (Object)null)))
				{
					validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure14));
					result = false;
				}
			}
			bool flag = false;
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				SerializedProperty val2 = new SerializedObject((Object)(object)componentsInChildren[j]).FindProperty("m_Lightmapping");
				flag |= (int)componentsInChildren[j].get_type() == 1 && val2.get_intValue() == 4;
			}
			if (!flag)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure6));
			}
			if ((Object)(object)((Component)level).GetComponentInChildren<FallTrigger>() == (Object)null)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure7));
				result = false;
			}
			if (!isLobby && (Object)(object)((Component)level).GetComponentInChildren<LevelPassTrigger>() == (Object)null)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure8));
				result = false;
			}
			Checkpoint[] componentsInChildren2 = ((Component)level).GetComponentsInChildren<Checkpoint>();
			Transform[] array2 = (Transform[])(object)new Transform[componentsInChildren2.Length];
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				array2[componentsInChildren2[k].number] = ((Component)componentsInChildren2[k]).get_transform();
			}
			for (int l = 0; l < array2.Length; l++)
			{
				if ((Object)(object)array2[l] == (Object)null)
				{
					validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure8));
				}
			}
			NetBody[] array3 = Object.FindObjectsOfType<NetBody>();
			foreach (NetBody netBody in array3)
			{
				if ((Object)(object)((Component)netBody).get_gameObject().GetComponent<Rigidbody>() == (Object)null)
				{
					string @string = GetString(StringIDs.kValidationFailure15);
					validationStringBuilder.AppendLine(string.Format(@string, ((Object)netBody).get_name()));
					result = false;
				}
			}
			if (metadata != null)
			{
				if (string.IsNullOrEmpty(metadata.title))
				{
					result = false;
					validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure10));
				}
				if (string.IsNullOrEmpty(metadata.description))
				{
					result = false;
					validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure11));
				}
			}
			else
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure12));
				result = false;
			}
			if ((Object)(object)thumbnail == (Object)null)
			{
				validationStringBuilder.AppendLine(GetString(StringIDs.kValidationFailure13));
				result = false;
			}
		}
		catch (Exception)
		{
		}
		validationString = validationStringBuilder.ToString();
		return result;
	}

	public HumanExport()
		: this()
	{
	}
}
