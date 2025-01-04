using System.Collections.ObjectModel;

namespace Xabbo.Custom;

internal class HandItemInfo(string name, int id)
{
    public string Name { get; set; } = name;
    public int Id { get; set; } = id;
}

internal class HandItemLibrary
{
    private static HandItemLibrary _instance; // Singleton instance
    private readonly List<HandItemInfo> _handItems;

    public ReadOnlyCollection<HandItemInfo> HandItemList { get; }

    private HandItemLibrary()
    {
        _handItems = new List<HandItemInfo>();
        PopulateHandItems();
        HandItemList = _handItems.AsReadOnly();
    }

    public static HandItemLibrary GetInstance()
    {
        // Instantiate and populate hand items only once
        return _instance ??= new HandItemLibrary();
    }

    private void AddHandItem(string name, int id)
    {
        _handItems.Add(new HandItemInfo(name, id));
    }

    public void PopulateHandItems()
    {
        AddHandItem("None", 0);
        AddHandItem("Tè Verde", 1);
        AddHandItem("Carota", 3);
        AddHandItem("Gelato alla Vaniglia", 4);
        AddHandItem("Caffè", 8);
        AddHandItem("Decaffeinato", 9);
        AddHandItem("Tè", 10);
        AddHandItem("Cioccolata", 11);
        AddHandItem("Caffè Macchiato", 12);
        AddHandItem("Espresso", 13);
        AddHandItem("Cola", 14);
        AddHandItem("Cioccolata Calda", 15);
        AddHandItem("Gassosa", 16);
        AddHandItem("Chinotto", 17);
        AddHandItem("Bicchiere d'acqua", 18);
        AddHandItem("Acqua Gassata", 19);
        AddHandItem("Macchina fotografica", 20);
        AddHandItem("Hamburger", 21);
        AddHandItem("Succo di Lime", 22);
        AddHandItem("Infuso di Radice", 23);
        AddHandItem("Deliziose Bollicine", 24);
        AddHandItem("Pozione d'Amore", 25);
        AddHandItem("Ghiacciolo", 26);
        AddHandItem("Té", 27);
        AddHandItem("Sake", 28);
        AddHandItem("Succo di pomodoro", 29);
        AddHandItem("Liquido radioattivo", 30);
        AddHandItem("FrizzLime", 31);
        AddHandItem("FrizzLime", 32);
        AddHandItem("Cola", 33);
        AddHandItem("Pesce", 34);
        AddHandItem("FrizzLime", 35);
        AddHandItem("Pera", 36);
        AddHandItem("Pesca deliziosa", 37);
        AddHandItem("Arancia", 38);
        AddHandItem("Fetta di Formaggio", 39);
        AddHandItem("Succo d'arancia", 40);
        AddHandItem("Sumppi-kuppi", 41);
        AddHandItem("Succo d'arancia", 42);
        AddHandItem("Limonata", 43);
        AddHandItem("Drink da Fine del Mondo!", 44);
        AddHandItem("Biscotto Divertente all'Arancia", 45);
        AddHandItem("Biscotto Divertente al Pistacchio", 46);
        AddHandItem("Biscotto Divertente alla Fragola", 47);
        AddHandItem("Lecca-lecca", 48);
        AddHandItem("Bottiglia di Succo Spumeggiante", 50);
        AddHandItem("Caffè Espresso", 53);
        AddHandItem("Pepsi", 55);
        AddHandItem("Bibita alla Ciliegia", 57);
        AddHandItem("Tazza di Sangue", 58);
        AddHandItem("Castagne", 60);
        AddHandItem("Acqua Avvelenata", 62);
        AddHandItem("Pop Corn", 63);
        AddHandItem("Banana Drink", 66);
        AddHandItem("Coscia di pollo", 70);
        AddHandItem("Toast", 71);
        AddHandItem("Zabaione", 73);
        AddHandItem("Calice", 74);
        AddHandItem("Gelato alla fragola", 75);
        AddHandItem("Gelato alla menta", 76);
        AddHandItem("Gelato al Cioccolato", 77);
        AddHandItem("Zucchero Filato Rosa", 79);
        AddHandItem("Zucchero Filato Azzurro", 80);
        AddHandItem("Hot Dog", 81);
        AddHandItem("Binocolo", 82);
        AddHandItem("Mela succosa", 83);
        AddHandItem("Omino di Pan di Zenzero", 84);
        AddHandItem("Americano", 85);
        AddHandItem("Frappuccino", 86);
        AddHandItem("Secchiello d'Acqua", 87);
        AddHandItem("Succo Bobba Invecchiato", 88);
        AddHandItem("Cupcake", 89);
        AddHandItem("Succo di Fragola", 90);
        AddHandItem("Tè Verde", 91);
        AddHandItem("Caramella Blu", 92);
        AddHandItem("Caramella Rossa", 93);
        AddHandItem("Lecca-Lecca Rosa", 94);
        AddHandItem("Lecca-Lecca Verde", 95);
        AddHandItem("Fetta di Torta", 96);
        AddHandItem("Croissant", 97);
        AddHandItem("Pomodoro", 98);
        AddHandItem("Melanzana", 99);
        AddHandItem("Cavolo", 100);
        AddHandItem("Bottiglia di Succo Frizzante", 101);
        AddHandItem("Energy Drink", 102);
        AddHandItem("Banana", 103);
        AddHandItem("Avocado", 104);
        AddHandItem("Uva", 105);
        AddHandItem("Frullato", 106);
        AddHandItem("Succo di verdura", 107);
        AddHandItem("Burger", 109);
        AddHandItem("Letterina", 110);
        AddHandItem("Granchio", 111);
        AddHandItem("Peperoncino Rosso", 112);
        AddHandItem("Frullato di agrumi", 113);
        AddHandItem("Frullato verde", 114);
        AddHandItem("Frullato di Frutti di bosco", 115);
        AddHandItem("Limone", 116);
        AddHandItem("Cookie", 117);
        AddHandItem("Ramune Rosa", 118);
        AddHandItem("Ramune Blu", 119);
        AddHandItem("Granita di Mirtilli", 120);
        AddHandItem("Granita di Fragole", 121);
        AddHandItem("Takoyaki", 122);
        AddHandItem("Borscht", 123);
        AddHandItem("Te con Uva Macchiata", 124);
        AddHandItem("Te alla Menta", 125);
        AddHandItem("Bubble Fragola", 126);
        AddHandItem("Cono Gelato", 127);
        AddHandItem("Gelato al Carbone", 128);
        AddHandItem("Yogurt", 129);
        AddHandItem("Formaggio", 130);
        AddHandItem("Pane", 131);
        AddHandItem("Gamberetto", 132);
        AddHandItem("Broccoli", 133);
        AddHandItem("Anguria", 134);
        AddHandItem("Donut", 135);
        AddHandItem("Salsiccia", 136);
        AddHandItem("Ghiacciolo", 137);
        AddHandItem("Patatine (aperte)", 138);
        AddHandItem("Pistola a Raggi", 182);
        AddHandItem("Rosa", 1000);
        AddHandItem("Rosa Nera", 1001);
        AddHandItem("Girasole", 1002);
        AddHandItem("Libro Rosso", 1003);
        AddHandItem("Libro Blu", 1004);
        AddHandItem("Libro Verde", 1005);
        AddHandItem("Fiore Giallo", 1006);
        AddHandItem("Margherita Azzurra", 1007);
        AddHandItem("Margherita Gialla", 1008);
        AddHandItem("Margherita Rosa", 1009);
        AddHandItem("Cartellina", 1011);
        AddHandItem("Pillole", 1013);
        AddHandItem("Siringa", 1014);
        AddHandItem("Rifiuti Tossici", 1015);
        AddHandItem("Fiore Bolly", 1019);
        AddHandItem("Giacinto Rosa", 1021);
        AddHandItem("Giacinto Azzurro", 1022);
        AddHandItem("Stella di Natale", 1023);
        AddHandItem("Budino", 1024);
        AddHandItem("Caramella", 1025);
        AddHandItem("Regalo", 1026);
        AddHandItem("Candela", 1027);
        AddHandItem("Ciotola di Cereali", 1028);
        AddHandItem("Palloncino", 1029);
        AddHandItem("HiPad", 1030);
        AddHandItem("Torcia Habbo-lympix", 1031);
        AddHandItem("Sindaco Tom", 1032);
        AddHandItem("UFO", 1033);
        AddHandItem("Oggetto alieno", 1034);
        AddHandItem("Chiave inglese", 1035);
        AddHandItem("Papera di gomma", 1036);
        AddHandItem("Serpente", 1037);
        AddHandItem("Bastone", 1038);
        AddHandItem("Mano Mozzata", 1039);
        AddHandItem("Cuore", 1040);
        AddHandItem("Calamaro", 1041);
        AddHandItem("Pupù di Pipistrello", 1042);
        AddHandItem("Verme", 1043);
        AddHandItem("Ratto Morto", 1044);
        AddHandItem("Dentiera", 1045);
        AddHandItem("Crema Clearasil", 1046);
        AddHandItem("Palla di Ferro", 1047);
        AddHandItem("Bandierina Ditch the Label Nera", 1048);
        AddHandItem("Martello", 1049);
        AddHandItem("Uovo di Pasqua Luminescente", 1050);
        AddHandItem("Pennello", 1051);
        AddHandItem("Bandierina Ditch the Label Bianca", 1052);
        AddHandItem("Anatra", 1053);
        AddHandItem("Palloncino Arancione", 1054);
        AddHandItem("Palloncino Verde", 1055);
        AddHandItem("Palloncino Blu", 1056);
        AddHandItem("Palloncino Rosa", 1057);
        AddHandItem("Lanterna", 1058);
        AddHandItem("Carta Igienica", 1059);
        AddHandItem("Spray", 1060);
        AddHandItem("Crisantemo", 1061);
        AddHandItem("Caramella Teschio Rosa", 1062);
        AddHandItem("Caramella Teschio Verde", 1063);
        AddHandItem("Caramella Teschio Blu", 1064);
        AddHandItem("Bambola Giocattolo", 1065);
        AddHandItem("Orsetto Giocattolo", 1066);
        AddHandItem("Soldato Giocattolo", 1067);
        AddHandItem("Manga", 1068);
        AddHandItem("Fumetto", 1069);
        AddHandItem("Biglietto Giallo", 1070);
        AddHandItem("HiPad Dorato", 1071);
        AddHandItem("Bussola", 1072);
        AddHandItem("Uovo di Dinosauro", 1073);
        AddHandItem("Allosaurus Verde", 1074);
        AddHandItem("Triceratopi Gialli", 1075);
        AddHandItem("Saurolophus Viola", 1076);
        AddHandItem("Asciugamano", 1077);
        AddHandItem("Spiedino di Lucertola", 1078);
        AddHandItem("Cervo Volante", 1079);
        AddHandItem("Scarabeo Rinoceronte", 1080);
        AddHandItem("Annaffiatoio", 1081);
        AddHandItem("Bandiera Pride", 1082);
        AddHandItem("Zucca Spaventosa", 1083);
        AddHandItem("Borsa della spesa", 1084);
        AddHandItem("DVD d'Azione", 1085);
        AddHandItem("DVD Thriller", 1086);
        AddHandItem("Quaderno", 1087);
        AddHandItem("Matita", 1088);
        AddHandItem("Patatine (chiuse)", 1089);
        AddHandItem("Mano Vuota", 999999999);

    }
}
