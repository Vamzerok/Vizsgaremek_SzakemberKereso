using Microsoft.AspNetCore.Identity;
using SzakemberKereso.Models;

namespace SzakemberKereso.Utils
{
    public static class DummyDataGenerator
    {
        class DummyUser
        {
            public User UserObj { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        private static readonly List<Role> _roles = new()
        {
            new Role() { Name = "Generic" },
            new Role() { Name = "Expert"  },
        };

        private static readonly List<DummyUser> _dummyUsers = new()
        {
            new DummyUser { UserObj = new User { Id = 1, UserName = "verhas.gergo@hbsz.edu.hu", Email = "verhas.gergo@hbsz.edu.hu", FirstName = "Gergő", LastName = "Verhás" }, Password = "asdf1234A!", Role = "Generic" },
            new DummyUser { UserObj = new User { Id = 2, UserName = "varga.david@hbsz.edu.hu", Email = "varga.david@hbsz.edu.hu", FirstName = "Dávid", LastName = "Varga" }, Password = "asdf1234A!", Role = "Generic" },
            new DummyUser { UserObj = new User { Id = 3, UserName = "csaszar.balint@hbsz.edu.hu", Email = "csaszar.balint@hbsz.edu.hu", FirstName = "Bálint", LastName = "Császár" }, Password = "asdf1234A!", Role = "Generic" },
            new DummyUser { UserObj = new User { Id = 4, UserName = "teszt.elek@gmail.com", Email = "teszt.elek@gmail.com", FirstName = "Elek", LastName = "Teszt" }, Password = "asdf1234A!", Role = "Generic" },
            new DummyUser { UserObj = new User { Id = 5, UserName = "horvath.peter@gmail.com", Email = "horvath.peter@gmail.com", FirstName = "Péter", LastName = "Horváth" }, Password = "asdf1234A!", Role = "Generic" },
            new DummyUser { UserObj = new User { Id = 6, UserName = "molnar.andrea@gmail.com", Email = "molnar.andrea@gmail.com", FirstName = "Andrea", LastName = "Molnár" }, Password = "asdf1234A!", Role = "Generic" },
            new DummyUser { UserObj = new User { Id = 7, UserName = "mekk.elek@ezermester.hu", Email = "mekk.elek@ezermester.hu", FirstName = "Elek", LastName = "Mekk" }, Password = "asdf1234A!", Role = "Expert" },
            new DummyUser { UserObj = new User { Id = 8, UserName = "csavaros.csaba@gmail.com", Email = "csavaros.csaba@gmail.com", FirstName = "Csaba", LastName = "Csavaros" }, Password = "asdf1234A!", Role = "Expert" },
            new DummyUser { UserObj = new User { Id = 9, UserName = "kalapacs.karcsi@ezermester.hu", Email = "kalapacs.karcsi@ezermester.hu", FirstName = "Karcsi", LastName = "Kalapács" }, Password = "asdf1234A!", Role = "Expert" },
            new DummyUser { UserObj = new User { Id = 10, UserName = "flexes.feri@gmail.com", Email = "flexes.feri@gmail.com", FirstName = "Feri", LastName = "Flexes" }, Password = "asdf1234A!", Role = "Expert" },
            new DummyUser { UserObj = new User { Id = 11, UserName = "csotoro.csopi@vizman.hu", Email = "csotoro.csopi@vizman.hu", FirstName = "Csöpi", LastName = "Csőtörő" }, Password = "asdf1234A!", Role = "Expert" },
            new DummyUser { UserObj = new User { Id = 12, UserName = "szigszalagos.sanyi@gmail.com", Email = "szigszalagos.sanyi@gmail.com", FirstName = "Sanyi", LastName = "Szigszalagos"}, Password = "asdf1234A!", Role = "Expert" },
        };

        private static readonly Dictionary<int, int> _expertWorkLocations = new()
        {
            { 7, 9700 }, //Mekk Elek: Szombathely
            { 8, 9798 }, //Csavaros: Ják
            { 9, 8300 }, //Kalapács: Tapolca
            { 10, 8019 }, //Flexes Feri: Székesfehérvár
            { 11, 4042 }, //Csőtörő: Debrecen
            { 12, 8308 }, //Szigszalagos: Zalahaláp
        };

        private static readonly Dictionary<int, string> _expertBiographies = new()
        {
            { 7,  "Mekk Elek vagyok, szombathelyi szakember. Kőműves, lakatos, kovács és kályhásmesteri munkákat vállalok. Harminc éve dolgozom ezen a területen. " +
                "Az ügyfeleimnek, a munkáim eredményeiről, megosztott véleményen vannak, nem is értem miért." },
            { 8,  "Csavaros Csaba vagyok. Vízvezeték-, gáz- és villanyszerelési munkákat vállalok. Gyors és megbízható munkavégzéssel." },
            { 9,  "Kalapács Karcsi, kőműves és ács. Tapolcán és környékén vállalok kisebb nagyobb felújítási munkákat." },
            { 10, "Flexes Feri, székesfehérvári festő és burkoló. Minőségi belső és külső festési, illetve burkolási munkák." },
            { 11, "Csőtörő Csöpi debreceni vízszerelő. Gyors duguláselhárítás, csőjavítás, szerelvénycserék." },
            { 12, "Szigszalagos Sanyi, zalahalápi tetőfedő és bútorasztalos. Megbízható munkavégzés kedvező áron." },
        };

        private static readonly Dictionary<int, string> _expertPhoneNumbers = new()
        {
            { 7,  "+36 30 123 4567" }, //Mekk Elek
            { 8,  "+36 70 234 5678" }, //csavaros csaba
            { 9,  "+36 20 345 6789" }, //Kalapács Karcsi
            { 10, "+36 30 456 7890" }, //Flexes Feri
            { 11, "+36 70 567 8901" }, //csőtörő csöpi
            { 12, "+36 20 678 9012" }, //szigszalagos sanyi
        };

        private static readonly Dictionary<int, string> _expertCompanyEmails = new()
        {
            { 7, "mekk.elek@ezermester.hu" }, //Mekk Elek
            { 8, "csavaros.csaba@gmail.com" }, //Csavaros Csaba
            { 9, "kalapacs.karcsi@ezermester.hu" }, //Kalapács Karcsi
            { 10, "flexes.feri@gmail.com" }, //Flexes Feri
            { 11, "csotoro.csopi@vizman.hu" }, //csőtörő csöpi
            { 12, "szigszalagos.sanyi@gmail.com" }, //szigszalagos sanyi
        };

        private static readonly List<ExpertSpecialty> _specialties = new()
        {
            new ExpertSpecialty { Id = 1, ExpertId = 7, OccupationId = 7511 }, //Mekk Elek: kőműves
            new ExpertSpecialty { Id = 2, ExpertId = 7, OccupationId = 7321 }, //Mekk Elek: lakatos
            new ExpertSpecialty { Id = 3, ExpertId = 7, OccupationId = 7326 }, //Mekk Elek: kovács
            new ExpertSpecialty { Id = 4, ExpertId = 7, OccupationId = 7537 }, //Mekk Elek: kályha és kandallóépítő
            new ExpertSpecialty { Id = 5, ExpertId = 8, OccupationId = 7521 }, //Csavaros Csaba: víz gáz 
            new ExpertSpecialty { Id = 6, ExpertId = 8, OccupationId = 7524 }, //Csavaros Csaba: villanyszerelő
            new ExpertSpecialty { Id = 7, ExpertId = 9, OccupationId = 7511 }, //Kalapács Karcsi: kőműves
            new ExpertSpecialty { Id = 8, ExpertId = 9, OccupationId = 7513 }, //Kalapác Karcsi: ács
            new ExpertSpecialty { Id = 9, ExpertId = 10, OccupationId = 7535 }, //Flexes Feri: festő
            new ExpertSpecialty { Id = 10, ExpertId = 10, OccupationId = 7534 }, //Flexes Feri: burkoló
            new ExpertSpecialty { Id = 11, ExpertId = 11, OccupationId = 7521 }, //Csőtörő Csöpi: víz gáz 
            new ExpertSpecialty { Id = 12, ExpertId = 12, OccupationId = 7532 }, //Szigszalagos Sanyi: tetőfedő
            new ExpertSpecialty { Id = 13, ExpertId = 12, OccupationId = 7223 }, //Szigszalagos Sanyi: bútorasztalos
        };

        private static readonly List<Job> _jobs = new()
        {
            //Császár Bálint
            new Job
            {
                Id = 1,
                Status = JobStatus.Completed,
                Rating = 4,
                InitiatingUserId = 3, 
                ServiceId = 16, 
                LocationId = 14,
                Title = "Dugulás a fürdőszobában",
                Description = "A fürdőszobai lefolyó és a WC is eldugult, sürgős segítségre van szükség.",
                Pricing = OfferPricing(PricingType.Fixed, 7000),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 3, 10,  8, 0, 12, 0),
                    Interval(TimeIntervalType.Available, 2025, 3, 11,  9, 0, 17, 0),
                    Interval(TimeIntervalType.Offered,   2025, 3, 10,  9, 0, 11, 0),
                }
            },
            new Job
            {
                Id = 2,
                Status = JobStatus.Accepted,
                InitiatingUserId = 3, 
                ServiceId = 13, 
                LocationId = 8,
                Title = "Nappali kifestése",
                Description = "A nappali falait szeretném kifesteni fehérre. A falak összterülete kb. 45 m2.",
                Pricing = OfferPricing(PricingType.FixedAndUnitBased, 10000, 1000, "m^2"),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 4, 1, 7, 0, 15, 0),
                    Interval(TimeIntervalType.Available, 2025, 4, 2, 7, 0, 15, 0),
                    Interval(TimeIntervalType.Available, 2025, 4, 3, 7, 0, 15, 0),
                    Interval(TimeIntervalType.Offered, 2025, 4, 2, 8, 0, 14, 0),
                }
            },
            new Job
            {
                Id = 3,
                Status = JobStatus.Offered,
                InitiatingUserId = 3,
                ServiceId = 15,
                LocationId = 8,
                Title = "Fürdőszoba burkolása",
                Description = "A fürdőszoba padlóját kell csempézni, a meglévő csempéket már felszedtem. Kb. 6 m^2-es területről beszélünk.",
                Pricing = OfferPricing(PricingType.FixedAndUnitBased, 5000, 2500, "m^2"),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 5, 5,  8, 0, 17, 0),
                    Interval(TimeIntervalType.Available, 2025, 5, 6,  8, 0, 17, 0),
                    Interval(TimeIntervalType.Offered,   2025, 5, 5,  9, 0, 13, 0),
                }
            },
            new Job
            {
                Id = 4,
                Status = JobStatus.Pending,
                InitiatingUserId = 3,
                ServiceId = 6, 
                LocationId = 8,
                Title = "Három sarokszelep csere",
                Description = "A konyhai mosdó alatt lévő három sarokszelep tömít, ezeket cserélni kéne.",
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 5, 15,  8, 0, 12, 0),
                    Interval(TimeIntervalType.Available, 2025, 5, 16,  8, 0, 12, 0),
                }
            },
            new Job
            {
                Id = 5, 
                Status = JobStatus.Cancelled,
                CancelledFromStatus = JobStatus.Pending,
                InitiatingUserId = 3,
                ServiceId = 19, 
                LocationId = 14,
                Title = "Tető javítás",
                Description = "A tető egy kisebb részén cserepek hiányoznak. Az eső teljesen szétáztatja a tetőszerkezetet. Gyors segítségre lenne szükségem.",
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 4, 20, 10, 0, 16, 0),
                }
            },

            //Mekk Elek
            new Job
            {
                Id = 6, 
                Status = JobStatus.Pending,
                InitiatingUserId = 1, 
                ServiceId = 1, 
                LocationId = 8,
                Title = "Repedések a nappaliban",
                Description = "A nappali falán több helyen repedés látható a vakolatban, kb. 3–4 repedés összesen.",
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 5, 10,  9, 0, 17, 0),
                    Interval(TimeIntervalType.Available, 2025, 5, 12,  9, 0, 17, 0),
                }
            },
            new Job
            {
                Id = 7,
                Status = JobStatus.Offered,
                InitiatingUserId = 2,
                ServiceId = 3,
                LocationId = 9,
                Title = "Főbejárat zárbetét csere",
                Description = "A lakás főbejárati ajtajának zárbetétje elromlott, kulcsra már nem nyit.",
                Pricing = OfferPricing(PricingType.Fixed, 8000),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 5, 8,  8, 0, 12, 0),
                    Interval(TimeIntervalType.Available, 2025, 5, 9,  8, 0, 12, 0),
                    Interval(TimeIntervalType.Offered,   2025, 5, 8,  9, 0, 11, 0),
                }
            },
            new Job
            {
                Id = 8,
                Status = JobStatus.Accepted,
                InitiatingUserId = 5,
                ServiceId = 5,
                LocationId = 9,
                Title = "Kandalló felújítása",
                Description = "Régi falazott kandallónk felújíttatást igényel: a vakolat omlik, a tömítés megrepedt.",
                Pricing = OfferPricing(PricingType.Fixed, 50000),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 5, 20,  8, 0, 14, 0),
                    Interval(TimeIntervalType.Available, 2025, 5, 21,  8, 0, 14, 0),
                    Interval(TimeIntervalType.Offered,   2025, 5, 20,  9, 0, 13, 0),
                }
            },
            new Job
            {
                Id = 9,
                Status = JobStatus.Completed,
                Rating = 3,
                InitiatingUserId = 4,
                ServiceId = 1,
                LocationId = 13,
                Title = "Garázsajtó melletti fal javítása",
                Description = "A garázs melletti külső fal vakolata több helyen lepergett, javítást igényel.",
                Pricing = OfferPricing(PricingType.FixedAndUnitBased, 15000, 2000, "m2"),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 2, 15,  8, 0, 17, 0),
                    Interval(TimeIntervalType.Offered,   2025, 2, 15,  9, 0, 15, 0),
                }
            },
            new Job
            {
                Id = 10,
                Status = JobStatus.Cancelled,
                CancelledFromStatus = JobStatus.Offered,
                InitiatingUserId = 6,
                ServiceId = 4,
                LocationId = 13,
                Title = "Kerti kerítés javítás",
                Description = "A kerti kerítés kapuja nem nyílik megfelelően, néhány hegesztési ponton utánmunkálat szükséges.",
                Pricing = OfferPricing(PricingType.FixedAndUnitBased, 10000, 5000, "óra"),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 4, 1, 10, 0, 16, 0),
                    Interval(TimeIntervalType.Offered, 2025, 4, 1, 11, 0, 15, 0),
                }
            },

            //others
            new Job
            {
                Id = 11,
                Status = JobStatus.Pending,
                InitiatingUserId = 1, //Verhás Gergő
                ServiceId = 11,
                LocationId = 12,
                Title = "Fa terasz építése a kertbe",
                Description = "A ház mögötti kertben egy kb. 12 m^2-es fa teraszt szeretnék építtetni.",
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 6, 1, 7, 0, 16, 0),
                    Interval(TimeIntervalType.Available, 2025, 6, 2, 7, 0, 16, 0),
                    Interval(TimeIntervalType.Available, 2025, 6, 3, 7, 0, 16, 0),
                }
            },
            new Job
            {
                Id = 12,
                Status = JobStatus.Completed,
                Rating = 5,
                InitiatingUserId = 2,
                ServiceId = 7,
                LocationId = 10,
                Title = "Vízóra csere",
                Description = "A régi vízórát cserélni kell, már jó ideje nem mér pontosan.",
                Pricing = OfferPricing(PricingType.Fixed, 15000),
                TimeIntervals = new List<TimeInterval>
                {
                    Interval(TimeIntervalType.Available, 2025, 1, 20, 8, 0, 12, 0),
                    Interval(TimeIntervalType.Offered, 2025, 1, 20, 9, 0, 11, 0),
                }
            },
        };

        private static readonly List<Service> _services = new()
        {
            //Mekk Elek : Kőműves (1)
            new Service { 
                Id = 1,
                ExpertSpecialtyId = 1,
                Name = "Falrepedések javítása",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 15000, UnitPrice = 2000, UnitName = "m^2" },
                Description = "Vakolati és falrepedések javítása beltéren és kültéren. A pontos árajánlathoz kérem adja meg a javítandó felület közelítő területét m^2-ben." 
            },

            new Service { 
                Id = 2,
                ExpertSpecialtyId = 1, 
                Name = "Alapozó falazat készítése",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 25000, UnitPrice = 8000, UnitName = "m^2" },
                Description = "Válaszfalak és alapozó falazatok elkészítése téglából vagy gázbetonból. Az anyagköltség külön számolódik. Kérem adja meg a tervezett falfelület területét m^2-ben." 
            },

            //Mekk Elek : Lakatos (2)
            new Service { 
                Id = 3, 
                ExpertSpecialtyId = 2, 
                Name = "Zárbetét csere",
                Pricing = new Pricing { PricingType = PricingType.Fixed, FixedPrice = 8000 },
                Description = "Ajtózárak és zárbetétek cseréje. Az ár tartalmazza a munkadíjat; az alkatrész ára külön számolódik." 
            },

            new Service { 
                Id = 4, 
                ExpertSpecialtyId = 2, 
                Name = "Kapu- és kerítésjavítás",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 10000, UnitPrice = 5000, UnitName = "m" },
                Description = "Fém kapuk, kerítések hegesztéssel elvégzett javítása, megerősítése. Kérem adja meg a javítandó kerítés hosszát méterben." 
            },

            //Mekk Elek : Kályha és kandallóépítő (specialty 4); kovács (specialty 3) has services
            new Service { 
                Id = 5,  
                ExpertSpecialtyId = 4,
                Name = "Kandalló felújítása",
                Pricing = new Pricing { PricingType = PricingType.Fixed, FixedPrice = 50000 },
                Description = "Meglévő kandallószerkezetek felújítása, tömítése, vakolása. Az ár a munkadíjat tartalmazza; anyagköltség külön megállapodás szerint." 
            },

            //Csavaros Csaba : Víz/gáz/fűtés (5)
            new Service { 
                Id = 6,
                ExpertSpecialtyId = 5, 
                Name = "Sarokszelep csere",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 2000, UnitPrice = 1000, UnitName = "sarokszelep" },
                Description = "Elöregedett sarokszelepek gyors cseréje. Kérem adja meg a cserélendő szelepek számát." 
            },

            new Service { 
                Id = 7,
                ExpertSpecialtyId = 5, 
                Name = "Vízóra csere",
                Pricing = new Pricing { PricingType = PricingType.Fixed, FixedPrice = 15000 },
                Description = "Elavult vízórák cseréje és hitelesítése. Tartalmazza a helyszíni felmérést és a tömítettség ellenőrzését." 
            },

            new Service { 
                Id = 8,
                ExpertSpecialtyId = 5, 
                Name = "Radiátor beüzemelés",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 8000, UnitPrice = 4000, UnitName = "db" },
                Description = "Radiátorok bekötése, légtelenítése és nyomáspróba elvégzése. Kérem adja meg a beüzemelni kívánt radiátorok számát." 
            },

            //Csavaros Csaba : Villanyszerelő (6)
            new Service { 
                Id = 9, 
                ExpertSpecialtyId = 6, 
                Name = "Konnektorcsere",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 2500, UnitPrice = 800, UnitName = "db" },
                Description = "Meglévő konnektorok cseréje, villamos folytonosság ellenőrzése. Kérem adja meg a cserélendő konnektorok számát." 
            },

            //Kalapács Karcsi : Kőműves (7)
            new Service { 
                Id = 10,
                ExpertSpecialtyId = 7, 
                Name = "Vakolat javítása",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 5000, UnitPrice = 1500, UnitName = "m^2" },
                Description = "Beltéri és kültéri vakolathibák javítása. Kérem adja meg a javítandó felület közelítő területét m^2-ben." 
            },

            //Kalapács Karcsi : Ács (8)
            new Service { 
                Id = 11,
                ExpertSpecialtyId = 8, 
                Name = "Terasz építése",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 30000, UnitPrice = 15000, UnitName = "m^2" },
                Description = "Fa szerkezetű terasz tervezése és megépítése. Az anyagköltség külön kalkulálandó. Kérem adja meg a tervezett terasz alapterületét m^2-ben." 
            },

            new Service { 
                Id = 12,
                ExpertSpecialtyId = 8,
                Name = "Falépcső javítása",
                Pricing = new Pricing { PricingType = PricingType.Fixed, FixedPrice = 20000 },
                Description = "Nyikorgó, törött vagy korhadt lépcsőfokok javítása, cseréje. Tartalmazza a helyszíni felmérést és az elvégzett munkát." 
            },

            //Flexes Feri : Festő és mázoló (9)
            new Service { 
                Id = 13,
                ExpertSpecialtyId = 9, 
                Name = "Beltéri falak festése",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 10000, UnitPrice = 1000, UnitName = "m^2" },
                Description = "Szobák és folyosók beltéri falainak festése diszperziós festékkel. Kérem adja meg a festendő falfelület összesített területét m^2-ben." 
            },

            new Service { 
                Id = 14,
                ExpertSpecialtyId = 9, 
                Name = "Homlokzatfestés",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 15000, UnitPrice = 1500, UnitName = "m^2" },
                Description = "Épületek külső homlokzatának festése. Az alapozás ára külön megállapodás szerint. Kérem adja meg a homlokzatfelület közelítő területét m^2-ben." 
            },

            //Flexes Feri : Burkoló (10)
            new Service { 
                Id = 15,
                ExpertSpecialtyId = 10,
                Name = "Aljzatcsempe lerakása",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 5000, UnitPrice = 2500, UnitName = "m^2" },
                Description = "Padlócsempe lerakása ragasztós technológiával, fugázással. Az anyagköltség külön számolódik. Kérem adja meg a burkolandó terület m^2-es méretét." 
            },

            //Csőtörő Csöpi : Víz/gáz/fűtés (11)
            new Service { 
                Id = 16,
                ExpertSpecialtyId = 11,
                Name = "Duguláselhárítás",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 5000, UnitPrice = 2000, UnitName = "alkalom" },
                Description = "Lefolyó-, szennyvíz- és csatornavezeték-dugulások elhárítása. Ismételt beavatkozás esetén az egységár ismételten érvényes." 
            },

            new Service { 
                Id = 17,
                ExpertSpecialtyId = 11,
                Name = "Mosdó/WC csere",
                Pricing = new Pricing { PricingType = PricingType.Fixed, FixedPrice = 20000 },
                Description = "Mosdó, WC-csésze cseréje. Tartalmazza a leszedést, az újbeüzemelést és a tömítettség ellenőrzését. A szerelvény ára külön számolódik." 
            },

            new Service { 
                Id = 18,
                ExpertSpecialtyId = 11,
                Name = "Vízvezeték javítás",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 8000, UnitPrice = 3000, UnitName = "óra" },
                Description = "Csőtörések és szivárgások javítása. Kérem adja meg a becsült munkaidőt és a jelenség rövid leírását." 
            },

            //Szigszalagos Sanyi : Tetőfedő (12)
            new Service { 
                Id = 19, 
                ExpertSpecialtyId = 12, 
                Name = "Tetőcserép csere",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 12000, UnitPrice = 500, UnitName = "cserép" },
                Description = "Törött, megrepedezett vagy eltolódott tetőcserepek cseréje. Az anyagköltség külön számítandó. Kérem adja meg a cserélendő cserepek becsült számát." 
            },

            //Szigszalagos Sanyi : Bútorasztalos (13)
            new Service { 
                Id = 20, 
                ExpertSpecialtyId = 13, 
                Name = "Szekrény javítás",
                Pricing = new Pricing { PricingType = PricingType.Fixed, FixedPrice = 20000 },
                Description = "Beépített és szabadon álló szekrények ajtajinak, fiókjainak javítása. Tartalmazza a helyszíni felmérést és az elvégzett munkát." 
            },

            new Service { 
                Id = 21, 
                ExpertSpecialtyId = 13, 
                Name = "Polc felszerelése",
                Pricing = new Pricing { PricingType = PricingType.FixedAndUnitBased, FixedPrice = 3000, UnitPrice = 1000, UnitName = "db" },
                Description = "Falra rögzített polcok felszerelése. Kérem adja meg a felszerelendő polcok számát." 
            },
        };

        public static async Task GenerateDummyData(Context context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            await CreateDummyRoles(context, roleManager);
            await CreateDummyUsers(context, userManager);
            await AssignRolesToUsers(context, userManager);
            await CreateExperts(context);
            await CreateExpertSpecialtiesAndServices(context);
            await CreateJobs(context);
        }

        public static async Task CreateDummyRoles(Context context, RoleManager<Role> roleManager)
       
        {
            foreach (var role in _roles)
            {
                if (await roleManager.RoleExistsAsync(role.Name)) continue;
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                    throw new Exception($"Failed to create role {role.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                await context.SaveChangesAsync();
            }
        }

        public static async Task CreateDummyUsers(Context context, UserManager<User> userManager)
        {
            foreach (var user in _dummyUsers)
            {
                if(user.UserObj == null) continue;
                if(user.UserObj.UserName == null) continue;
                if (await userManager.FindByNameAsync(user.UserObj.UserName) != null) continue;

                var result = await userManager.CreateAsync(user.UserObj, user.Password);
                if (!result.Succeeded)
                    throw new Exception($"Failed to create user {user.UserObj.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        public static async Task AssignRolesToUsers(Context context, UserManager<User> userManager)
        {
            var users = context.Users.ToList();
            foreach (var user in users)
            {
                var dummyUser = _dummyUsers.Find(d => d.UserObj.UserName == user.UserName);
                if (dummyUser == null) continue;
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains(dummyUser.Role)) continue;
                await userManager.AddToRoleAsync(user, dummyUser.Role);
            }
        }

        public static async Task CreateExperts(Context context)
        {
            var users = context.Users.ToList();
            foreach (var user in users)
            {
                if (await context.Experts.FindAsync(user.Id) != null) continue;

                var dummyUser = _dummyUsers.Find(d => d.UserObj.UserName == user.UserName);
                if (dummyUser?.Role != "Expert") continue;

                int? workLocationId = null;
                if (_expertWorkLocations.TryGetValue(user.Id, out int postalCode))
                {
                    var settlement = context.Settlements.FirstOrDefault(s => s.PostalCode == postalCode);
                    workLocationId = settlement?.Id;
                }

                _expertBiographies.TryGetValue(user.Id, out string? bio);
                _expertPhoneNumbers.TryGetValue(user.Id, out string? phone);
                _expertCompanyEmails.TryGetValue(user.Id, out string? companyEmail);

                context.Experts.Add(new Expert
                {
                    UserId = user.Id,
                    Biography = bio,
                    WorkLocationId = workLocationId!.Value,
                    CompanyPhoneNumber = phone!,
                    CompanyEmail = companyEmail!,
                });
            }
            await context.SaveChangesAsync();
        }

        public static async Task CreateExpertSpecialtiesAndServices(Context context)
        {
            foreach (var s in _specialties)
            {
                if (await context.ExpertSpecialties.FindAsync(s.Id) != null) continue;
                context.ExpertSpecialties.Add(s);
            }
            await context.SaveChangesAsync();

            foreach (var s in _services)
            {
                if (await context.Services.FindAsync(s.Id) != null) continue;
                context.Services.Add(s);
            }
            await context.SaveChangesAsync();
        }

        public static async Task CreateJobs(Context context)
        {
            if (context.Jobs.Any()) return;
            context.Jobs.AddRange(_jobs);
            await context.SaveChangesAsync();
        }

        //helper
        private static Pricing OfferPricing(PricingType type, decimal fixedPrice, decimal? unitPrice = null, string? unitName = null)
        {
            return new Pricing 
            { 
                PricingType = type, 
                FixedPrice = fixedPrice,
                UnitPrice = unitPrice,
                UnitName = unitName 
            };
        }

        private static TimeInterval Interval(TimeIntervalType type, int year, int month, int day, int startHour, int startMinute, int endHour, int endMinute)
        {
            return new TimeInterval 
            { 
                Type = type,
                Date = new DateOnly(year, month, day),
                StartTime = new TimeOnly(startHour, startMinute),
                EndTime = new TimeOnly(endHour, endMinute)
            };
        }
    }
}
