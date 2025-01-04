﻿using System.Collections.ObjectModel;

namespace Xabbo.Custom;

internal class EffectInfo(string name, int id, bool staff)
{
    public string Name { get; set; } = name;
    public int Id { get; set; } = id;
    public bool Staff { get; set; } = staff;
}

internal class EffectLibrary
{
    private static EffectLibrary _instance; // Singleton instance
    private readonly List<EffectInfo> _effects;

    public ReadOnlyCollection<EffectInfo> EffectList { get; }

    private EffectLibrary()
    {
        _effects = new List<EffectInfo>();
        PopulateEffects();
        EffectList = _effects.AsReadOnly();
    }

    public static EffectLibrary GetInstance()
    {
        // Instantiate and populate effects only once
        return _instance ??= new EffectLibrary();
    }

    private void AddEffect(string name, int id, bool staff = false)
    {
        _effects.Add(new EffectInfo(name, id, staff));
    }

    private void PopulateEffects()
    {
        AddEffect("Hoverplank", 1, true);
        AddEffect("Hoverboard", 2, true);
        AddEffect("UFO blu", 3, true);
        AddEffect("Luccichio", 4, true);
        AddEffect("Torcia", 5, true);
        AddEffect("Jetpack", 6, true);
        AddEffect("Farfalle", 7, true);
        AddEffect("Lucciole", 8, true);
        AddEffect("Cuoricini bacio", 9, true);
        AddEffect("Mosche", 10, true);
        AddEffect("Raggi X", 11, true);
        AddEffect("Morto", 12, true);
        AddEffect("Fantasma", 13, true);
        AddEffect("Hoverboard rosa", 14, true);
        AddEffect("Hoverboard giallo", 15, true);
        AddEffect("Microfono", 16, true);
        AddEffect("UFO rosa", 17, true);
        AddEffect("UFO giallo", 18, true);
        AddEffect("Auto polizia", 19, true);
        AddEffect("Ambulanza", 20, true);
        AddEffect("Car dollar", 21, true);
        AddEffect("Car top fuel", 22, true);
        AddEffect("Uomo totem", 23, true);
        AddEffect("Drago totem di mare", 24, true);
        AddEffect("Aquila totem", 25, true);
        AddEffect("Mix totem", 26, true);
        AddEffect("Vichingo", 27, true);
        AddEffect("Splash", 28, true);
        AddEffect("Nuotata", 29, true);
        AddEffect("Pozzanghera", 30, true);
        AddEffect("Cheetos", 31, true);
        AddEffect("Minions", 32, true);
        AddEffect("BattleBanzai rosso", 33, true);
        AddEffect("BattleBanzai verde", 34, true);
        AddEffect("BattleBanzai blu", 35, true);
        AddEffect("BattleBanzai giallo", 36, true);
        AddEffect("Pozzanghera della palude", 37, true);
        AddEffect("Pattinaggio sul ghiaccio 1", 38, true);
        AddEffect("Pattinaggio sul ghiaccio 2", 39, true);
        AddEffect("Casco rosso", 40, true);
        AddEffect("Casco verde", 41, true);
        AddEffect("Casco blu", 42, true);
        AddEffect("Casco giallo", 43, true);
        AddEffect("Sims", 44, true);
        AddEffect("Pattinaggio sul ghiaccio con luccichio 1", 45, true);
        AddEffect("Pattinaggio sul ghiaccio con luccichio 2", 46, true);
        AddEffect("Megamind", 47, true);
        AddEffect("Dog Car", 48, true);
        AddEffect("Casco rosso luccicante", 49, true);
        AddEffect("Casco verde luccicante", 50, true);
        AddEffect("Pelato 1", 51, true);
        AddEffect("Pelato 2", 52, true);
        AddEffect("Uccellini rotanti sulla testa", 53, true);
        AddEffect("Macchina del coniglio pasquale", 54, true);
        AddEffect("Pattini a rotelle 1", 55, true);
        AddEffect("Pattini a rotelle 2", 56, true);
        AddEffect("Pattini a rotelle 3", 57, true);
        AddEffect("Pattini a rotelle 4", 58, true);
        AddEffect("Scintillante", 59, true);
        AddEffect("Testa di rana", 60, true);
        AddEffect("Effetto Disney 1", 61, true);
        AddEffect("Effetto Disney 2", 62, true);
        AddEffect("Effetto Disney 3", 63, true);
        AddEffect("Candela", 64, true);
        AddEffect("Cellulare", 65, true);
        AddEffect("Maghi di Waverly - Disney", 66, true);
        AddEffect("Pappagallo di Rio", 67, true);
        AddEffect("Coniglio Pasquale", 68, true);
        AddEffect("Volkswagen", 69, true);
        AddEffect("Kung Fu Panda", 70, true);
        AddEffect("Skateboard 1", 71, true);
        AddEffect("Skateboard 2", 72, true);
        AddEffect("Chupa Chups", 73, true);
        AddEffect("Ghirlanda hawaiana 1", 74, true);
        AddEffect("Ghirlanda hawaiana 2", 75, true);
        AddEffect("Ghirlanda hawaiana 3", 76, true);
        AddEffect("Sella del cavallo", 77, true);
        AddEffect("Perry", 78, true);
        AddEffect("Maschera Alien", 79, true);
        AddEffect("Bocca Gigante", 80, true);
        AddEffect("Maschera Creatura delle Paludi", 81, true);
        AddEffect("Maschera Macchia d'inchiostro", 82, true);
        AddEffect("Maschera del Mimo", 83, true);
        AddEffect("Maschera della Mummia", 84, true);
        AddEffect("Maschera di Zucca", 85, true);
        AddEffect("Maschera di Spaventapasseri", 86, true);
        AddEffect("Maschera di Lupo", 87, true);
        AddEffect("Maschera di Zombie", 88, true);
        AddEffect("Coltello sulla Schiena", 89, true);
        AddEffect("Ali di Farfalla 1", 90, true);
        AddEffect("Ali di Fata 1", 91, true);
        AddEffect("Foglio Fantasma", 92, true);
        AddEffect("Senza Testa", 93, true);
        AddEffect("Camion di Liisu", 94, true);
        AddEffect("Snow warred", 95, true);
        AddEffect("Snow warred blu", 96, true);
        AddEffect("Snowboard Fluttuante", 97, true);
        AddEffect("Snow storm Crosshair", 98, true);
        AddEffect("Niko", 99, true);
        AddEffect("Drago", 100, true);
        AddEffect("Pistola ad Effetto Sonoro", 101, true);
        AddEffect("Staff", 102, false);
        AddEffect("Sella da Cavalcare 2", 103, true);
        AddEffect("Torta", 104, true);
        AddEffect("Bibita Frizzante", 105, true);
        AddEffect("Torcia Blu", 106, true);
        AddEffect("Sedia in Plastica", 107, true);
        AddEffect("Ninja", 108, true);
        AddEffect("Medaglia 1", 109, true);
        AddEffect("Medaglia 2", 110, true);
        AddEffect("Medaglia 3", 111, true);
        AddEffect("SpiderMan", 112, true);
        AddEffect("Nuvola di Pioggia)", 113, true);
        AddEffect("Bicipiti da Hulk", 114, true);
        AddEffect("Ringmaster", 115, true);
        AddEffect("Testa di mosca", 116, true);
        AddEffect("Massone", 117, true);
        AddEffect("Pagliaccio Malvagio", 118, true);
        AddEffect("Maschera di Alien Scura", 119, true);
        AddEffect("Bocca Gigante 2", 120, true);
        AddEffect("Maschera Creatura delle Paludi", 121, true);
        AddEffect("Maschera Macchia d'Inchiostro", 122, true);
        AddEffect("Maschera Mimo", 123, true);
        AddEffect("Maschera Mummia", 124, true);
        AddEffect("Maschera Zucca Ghiacciata", 125, true);
        AddEffect("Maschera Spaventapasseri", 126, true);
        AddEffect("Maschera Lupo Malvagio", 127, true);
        AddEffect("Maschera Zombie", 128, true);
        AddEffect("Coltello di Spalle 2", 129, true);
        AddEffect("Ali di Farfalla 2", 130, true);
        AddEffect("Ali di Fata 2", 131, true);
        AddEffect("Telo Fantasma Luminoso", 132, true);
        AddEffect("Senza Testa 2", 133, true);
        AddEffect("Corpo di Serpente", 134, true);
        AddEffect("Maestro dei Burattini", 135, true);
        AddEffect("Moonwalk alternativo", 136, true);
        AddEffect("Faretto d'Oro", 137, true);
        AddEffect("Occhiali d'Oro", 138, true);
        AddEffect("Microfono d'Oro", 139, true);
        AddEffect("Danza Habnam", 140, true);
        AddEffect("Maschera di Fiori", 141, true);
        AddEffect("Maschera di Capra", 142, true);
        AddEffect("Maschera di Piume", 143, true);
        AddEffect("Maschera di Piume", 144, true);
        AddEffect("Maschera di Piume", 145, true);
        AddEffect("Maschera di Piume", 146, true);
        AddEffect("Maschera di Pavone", 147, true);
        AddEffect("Maschera di Pavone", 148, true);
        AddEffect("Maschera di Pavone", 149, true);
        AddEffect("Maschera di Pavone", 150, true);
        AddEffect("Maschera di Capra", 151, true);
        AddEffect("Maschera di Capra", 152, true);
        AddEffect("Maschera di Fiori", 153, true);
        AddEffect("Uovo Enorme", 154, true);
        AddEffect("Cristallo Blu", 155, true);
        AddEffect("Cristallo Verde", 156, true);
        AddEffect("Brufolo", 157, true);
        AddEffect("Pinata", 158, true);
        AddEffect("Barile", 159, true);
        AddEffect("Pirata", 160, true);
        AddEffect("Equipaggio da Pirata", 161, true);
        AddEffect("Spada da Pirata", 162, true);
        AddEffect("Testa per Terra", 163, true);
        AddEffect("Pistola da Pirata", 164, true);
        AddEffect("Pollice in Su che Gira sulla testa", 165, true);
        AddEffect("Goblin", 166, true);
        AddEffect("Mutante", 167, true);
        AddEffect("Cuori che Girano sulla testa", 168, true);
        AddEffect("Melma", 169, true);
        AddEffect("Paperella", 170, true);
        AddEffect("Coniglio Spaziale", 171, true);
        AddEffect("Vichingo Rosso", 172, true);
        AddEffect("Vichingo Verde", 173, true);
        AddEffect("Hoverboard Verde", 174, true);
        AddEffect("Aereo Rosso", 175, true);
        AddEffect("Aereo Blu", 176, true);
        AddEffect("Catrame e Piume", 177, true);
        AddEffect("Stemma", 178, true);
        AddEffect("Lottatore di Lucha Libre 1", 179, true);
        AddEffect("Lottatore di Lucha Libre 2", 180, true);
        AddEffect("Lottatore di Lucha Libre 3", 181, true);
        AddEffect("Pistola Cyberpunk", 182, true);
        AddEffect("Martello Fossile", 183, true);
        AddEffect("Pozzanghera Malvagia", 184, true);
        AddEffect("Nuoto Malvagio", 185, true);
        AddEffect("Bacchetta Magica", 186, true);
        AddEffect("Bot", 187, false);
        AddEffect("Robot", 188, true);
        AddEffect("Omino Piccolo", 189, true);
        AddEffect("Omino Femminile Piccolo", 190, true);
        AddEffect("Hoverplank", 191, true);
        AddEffect("Annaffiatoio", 192, true);
        AddEffect("Salto in alto", 193, true);
        AddEffect("Camminata continua", 194, true);
        AddEffect("Cyclette", 195, true);
        AddEffect("Spada laser", 196, true);
        AddEffect("Tappeto Volante", 197, true);
        AddEffect("clb", 199, false);
        AddEffect("cpl", 200, false);
        AddEffect("frz", 201, false);
        AddEffect("Homer Simpson", 202, true);
        AddEffect("gay", 203, false);
        AddEffect("Bicipiti", 204, true);
        AddEffect("FSpinn", 205, false);
        AddEffect("Bicipiti Neri", 206, true);
        AddEffect("Mitra Blu", 207, true);
        AddEffect("Mitra Viola", 208, true);
        AddEffect("Mitra Grigia", 209, true);
        AddEffect("Mitra Verde", 210, true);
        AddEffect("Mitra Arancio", 211, true);
        AddEffect("Falce del tristo mietitore", 212, true);
        AddEffect("Freccia Gialla", 213, true);
        AddEffect("Utente Noob", 214, true);
        AddEffect("Emoction Movibile", 215, true);
        AddEffect("Mazza da Baseball", 216, true);
        AddEffect("Fiamma Viola", 217, true);
        AddEffect("Fiamma Scura", 218, true);
        AddEffect("Mela", 219, true);
        AddEffect("Tartaruga volante", 220, true);
        AddEffect("Carlino", 221, true);
        AddEffect("Macchinina", 222, true);
        AddEffect("Personaggio ruotante", 223, true);
        AddEffect("Spada laser blu", 224, true);
        AddEffect("Spada laser rossa", 225, true);
        AddEffect("Spada laser gialla", 226, true);
        AddEffect("Bicicletta", 227, true);
        AddEffect("Fidget spinner", 228, true);
        AddEffect("Fiamma grigia", 229, true);
        AddEffect("Elicottero verde", 230, true);
        AddEffect("Elicottero arancio", 231, true);
        AddEffect("Drink", 232, true);
        AddEffect("Elicottero giallo", 233, true);
        AddEffect("Macchina nera", 234, true);
        AddEffect("Macchina verde", 235, true);
        AddEffect("Spada laser gialla 2", 236, true);
        AddEffect("Personaggio ruotante 2", 237, true);
        AddEffect("Scuoti la testa", 238, true);
        AddEffect("Buco nero", 239, true);
        AddEffect("Personaggio triggered", 240, true);
        AddEffect("Total white", 241, true);
        AddEffect("Total black", 242, true);
        AddEffect("Elicottero giallo 2", 243, true);
        AddEffect("Piccolo circo", 244, true);
        AddEffect("Pila di lingotti", 245, true);
        AddEffect("Salto lento", 246, true);
        AddEffect("Personaggio schiacciato", 247, true);
        AddEffect("Pezzo di torta", 248, true);
        AddEffect("Pezzo di torta 2", 249, true);
        AddEffect("Thè", 250, true);
        AddEffect("Jack daniel's", 251, true);
        AddEffect("Coca cola", 252, true);
        AddEffect("Bottiglia di super alcolico", 253, true);
        AddEffect("Gomma da masticare", 254, true);
        AddEffect("Bevi velocemente", 255, true);
        AddEffect("Personaggio ruotante veloce", 256, true);
        AddEffect("Fantasma blu", 257, true);
        AddEffect("Chiave del tesoro", 258, true);
        AddEffect("Freccia gialla 2", 259, true);
        AddEffect("Mitra oro 2", 260, true);
        AddEffect("Ascia dorata", 261, true);
        AddEffect("Ascia normale", 262, true);
        AddEffect("Alcetta", 263, true);
        AddEffect("Mazza insanguinata", 264, true);
        AddEffect("Spada", 265, true);
        AddEffect("Armatura casuale", 266, true);
        AddEffect("Mazza bianca", 267, true);
        AddEffect("Maza rosa di san valentino", 268, true);
        AddEffect("Mazza di san valentino azzurra", 269, true);
        AddEffect("Racchetta da tennis verde", 270, true);
        AddEffect("Racchetta da tennis gialla", 271, true);
        AddEffect("Racchetta da tennis blu", 272, true);
        AddEffect("Racchetta da tennis rossa", 273, true);
        AddEffect("Monopattino", 274, true);
        AddEffect("Fiamma del drago", 275, true);
        AddEffect("Mazza volante da strega 1", 276, true);
        AddEffect("Mazza volante da strega 2", 277, true);
        AddEffect("Aereo rosa", 278, true);
        AddEffect("Carro armato", 279, true);
        AddEffect("Dragon ball", 280, true);
        AddEffect("Aereo vecchio", 281, true);
        AddEffect("Gomma da masticare 2", 282, true);
        AddEffect("Gomma da masticare 3", 283, true);
        AddEffect("Gomma da masticare 4", 284, true);
        AddEffect("Spada laser 1", 285, true);
        AddEffect("Spada laser 2", 286, true);
        AddEffect("Spada laser 3", 287, true);
        AddEffect("Spada laser 4", 288, true);
        AddEffect("Spada samurai 1", 289, true);
        AddEffect("Spada samurai 2", 290, true);
        AddEffect("Coltello", 291, true);
        AddEffect("Mariokart 1", 292, true);
        AddEffect("Mariokart 2", 293, true);
        AddEffect("Mariokart 3", 294, true);
        AddEffect("Mariokart 4", 295, true);
        AddEffect("Mariokart 5", 296, true);
        AddEffect("Ufo volante", 297, true);
        AddEffect("Macchina taxi", 298, true);
        AddEffect("Zucca di halloween", 299, true);
        AddEffect("Arma di naruto", 300, true);
        AddEffect("Sfera di naruto", 301, true);
        AddEffect("Personaggio incandescente", 302, true);
        AddEffect("Sputa fuoco", 303, true);
        AddEffect("Aquilone 1", 304, true);
        AddEffect("Linguaccia", 305, true);
        AddEffect("Marshmallow arrosto", 306, true);
        AddEffect("Bagno di sangue", 307, true);
        AddEffect("Segnale di pericolo", 308, true);
        AddEffect("Occhio gigante", 309, true);
        AddEffect("Aquilone 2", 310, true);
        AddEffect("Aquilone 3", 311, true);
        AddEffect("Aquilone 4", 312, true);
        AddEffect("Aquilone 5", 313, true);
        AddEffect("Spada 2", 314, true);
        AddEffect("Spada 3", 315, true);
        AddEffect("Shisha 1", 316, true);
        AddEffect("Shisha 2", 317, true);
        AddEffect("Shisha 3", 318, true);
        AddEffect("Shisha 4", 319, true);
        AddEffect("Shisha 5", 320, true);
        AddEffect("Shisha 6", 321, true);
        AddEffect("Nuvola 1", 322, true);
        AddEffect("Nuvola 2", 323, true);
        AddEffect("AmongUs 1", 324, true);
        AddEffect("AmongUs 2", 325, true);
        AddEffect("AmongUs 3", 326, true);
        AddEffect("AmongUs 4", 327, true);
        AddEffect("AmongUs 5", 328, true);
        AddEffect("AmongUs 6", 329, true);
        AddEffect("AmongUs 7", 330, true);
        AddEffect("AmongUs 8", 331, true);
        AddEffect("AmongUs 9", 332, true);
        AddEffect("AmongUs 10", 333, true);
        AddEffect("AmongUs 11", 334, true);
        AddEffect("AmongUs Kil l1", 335, true);
        AddEffect("AmongUs Kill 2", 336, true);
        AddEffect("AmongUs Kill 3", 337, true);
        AddEffect("AmongUs Kill 4", 338, true);
        AddEffect("AmongUs Kill 5", 339, true);
        AddEffect("AmongUs Kill 6", 340, true);
        AddEffect("AmongUs Kill 7", 341, true);
        AddEffect("AmongUs Kill 8", 342, true);
        AddEffect("AmongUs Kill 9", 343, true);
        AddEffect("AmongUs Kill 10", 344, true);
        AddEffect("AmongUs Kill 11", 345, true);
        AddEffect("AmongUs Kill 12", 346, true);
        AddEffect("Applausi", 347, true);
        AddEffect("Tridente animato", 348, true);
        AddEffect("Tridente infuocato", 349, true);
        AddEffect("Sfera genkidama", 350, true);
        AddEffect("Caricamento", 351, true);
        AddEffect("Zombie", 352, true);
        AddEffect("Pensando ai rari", 353, true);
        AddEffect("Cartello cuore", 354, true);
        AddEffect("Pacco regalo", 355, true);
        AddEffect("Compleanno", 356, true);
        AddEffect("Bagliore di luce", 357, true);
        AddEffect("Tablet", 358, true);
        AddEffect("Testa bolla", 394, true);
        AddEffect("Scintillante", 395, true);
        AddEffect("Maschera nera", 396, true);
        AddEffect("Bastone da chiesa", 400, true);
        AddEffect("Dito medio", 401, true);
        AddEffect("Coronavirus", 402, true);
        AddEffect("Faccia shockata", 403, true);
        AddEffect("Fantasmino 1", 404, true);
        AddEffect("Fantasmino 2", 405, true);
        AddEffect("Pastorale", 406, true);
        AddEffect("Cartello lime", 407, true);
        AddEffect("Cartello arancio", 408, true);
        AddEffect("Cartello giallo 2", 409, true);
        AddEffect("Cartello bianco", 410, true);
        AddEffect("Cartello blu", 411, true);
        AddEffect("Cartello celeste 2", 412, true);
        AddEffect("Cartello fucsia", 413, true);
        AddEffect("Cartello giada", 414, true);
        AddEffect("Cartello giallo", 415, true);
        AddEffect("Cartello grigio", 416, true);
        AddEffect("Cartello rosso", 417, true);
        AddEffect("Cartello verde", 418, true);
        AddEffect("Cartello viola", 419, true);
        AddEffect("Cartello celeste", 420, true);
        AddEffect("Cartello dito medio", 421, true);
        AddEffect("Kamehameha", 422, true);
        AddEffect("Luce bianca", 423, true);
        AddEffect("Giardiniere 1", 424, true);
        AddEffect("Giardiniere 2", 425, true);
        AddEffect("Fiamme nei capelli", 426, true);
        AddEffect("IronMan", 427, true);
        AddEffect("Cioccolata 1", 428, true);
        AddEffect("Cioccolata 2", 429, true);
        AddEffect("Holo", 430, true);
        AddEffect("Drago Olografico 1", 431, true);
        AddEffect("Drago Olografico 2", 432, true);
        AddEffect("Drago Olografico 2", 432, true);
        AddEffect("Personaggio ruotante 3", 500, true);
        AddEffect("Personaggio ruotante 4", 501, true);
        AddEffect("Salto continuo", 502, true);
        AddEffect("Hoverboard 1", 504, true);
        AddEffect("Hoverboard 2", 505, true);
        AddEffect("Hoverboard 3", 506, true);
        AddEffect("Salto continuo 2", 509, true);
        AddEffect("Pistola nera", 519, true);
        AddEffect("Mitra oro 2", 520, true);
        AddEffect("Braccio muscoloso", 521, true);
        AddEffect("Saiyan", 522, false);
        AddEffect("Cane al guinzaglio", 539, true);
        AddEffect("Monopattino elettrico 1", 540, true);
        AddEffect("Monopattino elettrico 2", 541, true);
        AddEffect("Monopattino elettrico 3", 542, true);
        AddEffect("Pikachu", 547, true);
        AddEffect("Borushiki", 619, false);
        AddEffect("MadaraSusanoo", 620, false);
        AddEffect("ItachiSusanoo", 621, false);
        AddEffect("Byakugan", 622, false);
        AddEffect("Rinnegan", 623, false);
        AddEffect("Sharingan", 624, false);
        AddEffect("MangekyuSharingan", 625, false);
        AddEffect("Jogan", 626, false);
        AddEffect("Cage_Thoracique_Shisui", 627, false);
        AddEffect("Cage_Thoracique_KakashiObito", 628, false);
        AddEffect("Cage_Thoracique_Madara", 629, false);
        AddEffect("Cage_Thoracique_Itachi", 630, false);
        AddEffect("Cage_Thoracique_Sasuke", 631, false);
        AddEffect("I'am gay", 642, true);
        AddEffect("I'am lesbo", 643, true);
        AddEffect("Orchidee", 700, true);
        AddEffect("Denti grossi", 701, true);
        AddEffect("Arpia", 702, true);
        AddEffect("pok", 777, false);
        AddEffect("arb", 778, false);
        AddEffect("radio", 779, false);
        AddEffect("builder", 780, false);
        AddEffect("AFK", 781, true);
        AddEffect("Corona", 961, true);
        AddEffect("Maggie Simpson", 999, true);
        AddEffect("Pistola", 1000, true);
        AddEffect("Doppia pistola", 1001, true);
        AddEffect("Molotov", 1002, true);
        AddEffect("Pistola oro", 1003, true);
        AddEffect("Pistola grigia", 1004, true);
        AddEffect("Pistola viola", 1005, true);
        AddEffect("Decappottabile 1", 1006, true);
        AddEffect("Decappottabile 2", 1007, true);
        AddEffect("Moto rossa", 1008, true);
        AddEffect("Moto blu", 1009, true);
        AddEffect("Moto gialla", 1010, true);
        AddEffect("Moto verde", 1011, true);
        AddEffect("Moto nera", 1012, true);
        AddEffect("Gesto con le mani", 1013, true);
        AddEffect("Avada Kedavra", 2000, true);
        AddEffect("Stupeficium", 2001, true);
        AddEffect("Wingardium Leviosa", 2002, true);
        AddEffect("Flipendo", 2003, true);
        AddEffect("Dolohferio", 2004, true);
        AddEffect("Aguamenti", 2005, true);
        AddEffect("Expelliarmus", 2006, true);
        AddEffect("Rictusempra", 2007, true);
        AddEffect("Bacchetta", 2008, true);
        AddEffect("Lamborghini police", 2999, true);
        AddEffect("Lamborghini rossa", 3000, true);
        AddEffect("Lamborghini arancio", 3001, true);
        AddEffect("Lamborghini ciano", 3002, true);
        AddEffect("Lamborghini blu", 3003, true);
        AddEffect("Lamborghini gialla", 3004, true);
        AddEffect("Lamborghini grigia", 3005, true);
        AddEffect("Lamborghini marrone", 3006, true);
        AddEffect("Lamborghini purple", 3007, true);
        AddEffect("Lamborghini verde base", 3008, true);
        AddEffect("Lamborghini verde chiaro", 3009, true);
        AddEffect("Lamborghini verde scuro", 3010, true);
        AddEffect("Lamborghini viola", 3011, true);
        AddEffect("Decappottabile 3", 3012, true);
        AddEffect("Decappottabile 4", 3013, true);
        AddEffect("Decappottabile 5", 3014, true);
        AddEffect("Decappottabile 6", 3015, true);
        AddEffect("Decappottabile 7", 3016, true);
        AddEffect("Formula Uno Arancio", 3017, true);
        AddEffect("Formula Uno Rossa", 3018, true);
        AddEffect("Formula Uno Celeste", 3019, true);
        AddEffect("Formula Uno Blu", 3020, true);
        AddEffect("Formula Uno Gialla", 3021, true);
        AddEffect("Formula Uno Marrone", 3022, true);
        AddEffect("Formula Uno Verde", 3023, true);
        AddEffect("Formula Uno Viola", 3024, true);
        AddEffect("Glitch", 4000, true);
        AddEffect("Il supereroe", 5000, true);
        AddEffect("Staff Animato", 5001, false);
        AddEffect("Stemma Corvonero", 5002, true);
        AddEffect("Stemma Tassorosso", 5003, true);
        AddEffect("Stemma Grifondoro", 5004, true);
        AddEffect("Stemma Serpeverde", 5005, true);

    }
}
