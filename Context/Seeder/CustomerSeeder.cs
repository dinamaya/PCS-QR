using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Models.Entities.Main;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCIMS.Web.Context.Seeder
{
    public class CustomerSeeder : Seeder
    {
        public override async Task Seed(IServiceProvider serviceProvider)
        {
            using var context = new MainDbContext(serviceProvider.GetRequiredService<DbContextOptions<MainDbContext>>());
            var date = DateTime.Now;

                Customer customer1 = new Customer
                {
                    FirstName = "Maria",
                    LastName = "Santos",
                    Address = "123 Rizal St., Makati City, Metro Manila",
                    ContactNumber = "09171234567",
                    Email = "maria.santos@example.com",
                    DateCreated = date,
                    ModifiedBy = "",
                    DateModified = date,
                    IsActive = true
                };
                Customer customer2 = new Customer
                {
                    FirstName = "Juan",
                    LastName = "Reyes",
                    Address = "456 Bonifacio Ave., Quezon City, Metro Manila",
                    ContactNumber = "09189876543",
                    Email = "juan.reyes@example.com",
                    DateCreated = date,
                    ModifiedBy = "",
                    DateModified = date,
                    IsActive = true
                };
                Customer customer3 = new Customer
                {
                    FirstName = "Rosa",
                    LastName = "Cruz",
                    Address = "789 Aguinaldo St., Cebu City, Cebu",
                    ContactNumber = "09279876543",
                    Email = "rosa.cruz@example.com",
                    DateCreated = date,
                    ModifiedBy = "",
                    DateModified = date,
                    IsActive = true
                };

                List<Customer> additionalCustomers = new List<Customer>
                {
                    new Customer
                    {
                        FirstName = "Antonio",
                        LastName = "Garcia",
                        Address = "101 Mabini St., Davao City, Davao del Sur",
                        ContactNumber = "09171234568",
                        Email = "antonio.garcia@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Elena",
                        LastName = "Bautista",
                        Address = "202 Luna Ave., Iloilo City, Iloilo",
                        ContactNumber = "09189876542",
                        Email = "elena.bautista@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Ramon",
                        LastName = "Lim",
                        Address = "303 Del Pilar St., Baguio City, Benguet",
                        ContactNumber = "09279876544",
                        Email = "ramon.lim@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Luzviminda",
                        LastName = "Mendoza",
                        Address = "404 Burgos Ave., Zamboanga City, Zamboanga del Sur",
                        ContactNumber = "09331234567",
                        Email = "luzviminda.mendoza@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Eduardo",
                        LastName = "Aquino",
                        Address = "505 Laurel St., Cagayan de Oro, Misamis Oriental",
                        ContactNumber = "09269876543",
                        Email = "eduardo.aquino@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Josefina",
                        LastName = "Villanueva",
                        Address = "606 Legaspi Blvd., Batangas City, Batangas",
                        ContactNumber = "09171234569",
                        Email = "josefina.villanueva@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = false
                    },
                    new Customer
                    {
                        FirstName = "Ricardo",
                        LastName = "Tan",
                        Address = "707 Quezon St., Naga City, Camarines Sur",
                        ContactNumber = "09189876545",
                        Email = "ricardo.tan@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Teresita",
                        LastName = "Gonzales",
                        Address = "808 Magsaysay Rd., Tacloban City, Leyte",
                        ContactNumber = "09279876546",
                        Email = "teresita.gonzales@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Danilo",
                        LastName = "Ramos",
                        Address = "909 Roxas Ave., General Santos City, South Cotabato",
                        ContactNumber = "09331234568",
                        Email = "danilo.ramos@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Rosario",
                        LastName = "Fernandez",
                        Address = "1010 Sampaguita St., Laoag City, Ilocos Norte",
                        ContactNumber = "09269876544",
                        Email = "rosario.fernandez@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Manuel",
                        LastName = "Torres",
                        Address = "11 Dizon St., San Fernando, Pampanga",
                        ContactNumber = "09171234570",
                        Email = "manuel.torres@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Margarita",
                        LastName = "Navarro",
                        Address = "12 Agoncillo Blvd., Taguig City, Metro Manila",
                        ContactNumber = "09189876546",
                        Email = "margarita.navarro@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Roberto",
                        LastName = "Yap",
                        Address = "13 Magallanes St., Dagupan City, Pangasinan",
                        ContactNumber = "09279876547",
                        Email = "roberto.yap@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Carolina",
                        LastName = "Dizon",
                        Address = "14 Padre Faura St., Pasay City, Metro Manila",
                        ContactNumber = "09331234569",
                        Email = "carolina.dizon@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Andres",
                        LastName = "Castro",
                        Address = "15 Manalili St., Angeles City, Pampanga",
                        ContactNumber = "09269876545",
                        Email = "andres.castro@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Milagros",
                        LastName = "Pascual",
                        Address = "16 Aurora Blvd., Lucena City, Quezon",
                        ContactNumber = "09171234571",
                        Email = "milagros.pascual@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Reynaldo",
                        LastName = "Mercado",
                        Address = "17 Lacson St., Bacolod City, Negros Occidental",
                        ContactNumber = "09189876547",
                        Email = "reynaldo.mercado@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = false
                    },
                    new Customer
                    {
                        FirstName = "Corazon",
                        LastName = "Robles",
                        Address = "18 Abad Santos Ave., San Juan City, Metro Manila",
                        ContactNumber = "09279876548",
                        Email = "corazon.robles@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Ernesto",
                        LastName = "Santos",
                        Address = "19 España Blvd., Caloocan City, Metro Manila",
                        ContactNumber = "09331234570",
                        Email = "ernesto.santos@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Gloria",
                        LastName = "Velasco",
                        Address = "20 Taft Ave., Manila City, Metro Manila",
                        ContactNumber = "09269876546",
                        Email = "gloria.velasco@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Pedro",
                        LastName = "Gutierrez",
                        Address = "21 Katipunan Road, Marikina City, Metro Manila",
                        ContactNumber = "09171234572",
                        Email = "pedro.gutierrez@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Teresa",
                        LastName = "Aguilar",
                        Address = "22 Governor's Drive, Cavite City, Cavite",
                        ContactNumber = "09189876548",
                        Email = "teresa.aguilar@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Alfredo",
                        LastName = "Dee",
                        Address = "23 Banawe St., Cabanatuan City, Nueva Ecija",
                        ContactNumber = "09279876549",
                        Email = "alfredo.dee@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Lourdes",
                        LastName = "Gomez",
                        Address = "24 MacArthur Highway, Olongapo City, Zambales",
                        ContactNumber = "09331234571",
                        Email = "lourdes.gomez@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Rolando",
                        LastName = "Chua",
                        Address = "25 Session Road, Tagaytay City, Cavite",
                        ContactNumber = "09269876547",
                        Email = "rolando.chua@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = false
                    },
                    new Customer
                    {
                        FirstName = "Imelda",
                        LastName = "Lao",
                        Address = "26 Araneta Ave., Parañaque City, Metro Manila",
                        ContactNumber = "09171234573",
                        Email = "imelda.lao@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Rodolfo",
                        LastName = "Miranda",
                        Address = "27 Alabang-Zapote Rd., Las Piñas City, Metro Manila",
                        ContactNumber = "09189876549",
                        Email = "rodolfo.miranda@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Anita",
                        LastName = "Soriano",
                        Address = "28 Mayon St., Muntinlupa City, Metro Manila",
                        ContactNumber = "09279876550",
                        Email = "anita.soriano@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Feliciano",
                        LastName = "Ong",
                        Address = "29 Ortigas Ave., Pasig City, Metro Manila",
                        ContactNumber = "09331234572",
                        Email = "feliciano.ong@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Victoria",
                        LastName = "Alcantara",
                        Address = "30 Visayas Ave., Mandaluyong City, Metro Manila",
                        ContactNumber = "09269876548",
                        Email = "victoria.alcantara@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Francisco",
                        LastName = "Domingo",
                        Address = "31 Panay Ave., Malabon City, Metro Manila",
                        ContactNumber = "09171234574",
                        Email = "francisco.domingo@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Dolores",
                        LastName = "Salvador",
                        Address = "32 Kamias Rd., Navotas City, Metro Manila",
                        ContactNumber = "09189876550",
                        Email = "dolores.salvador@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = false
                    },
                    new Customer
                    {
                        FirstName = "Julio",
                        LastName = "Valencia",
                        Address = "33 Ayala Ave., Makati City, Metro Manila",
                        ContactNumber = "09279876551",
                        Email = "julio.valencia@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Norma",
                        LastName = "Santiago",
                        Address = "34 EDSA, Quezon City, Metro Manila",
                        ContactNumber = "09331234573",
                        Email = "norma.santiago@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Rogelio",
                        LastName = "Borja",
                        Address = "35 C5 Road, Taguig City, Metro Manila",
                        ContactNumber = "09269876549",
                        Email = "rogelio.borja@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Carmela",
                        LastName = "Fajardo",
                        Address = "36 Roxas Blvd., Manila City, Metro Manila",
                        ContactNumber = "09171234575",
                        Email = "carmela.fajardo@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Virgilio",
                        LastName = "Rosario",
                        Address = "37 Commonwealth Ave., Quezon City, Metro Manila",
                        ContactNumber = "09189876551",
                        Email = "virgilio.rosario@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Lilia",
                        LastName = "Tolentino",
                        Address = "38 Shaw Blvd., Mandaluyong City, Metro Manila",
                        ContactNumber = "09279876552",
                        Email = "lilia.tolentino@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Jaime",
                        LastName = "Marquez",
                        Address = "39 Buendia Ave., Makati City, Metro Manila",
                        ContactNumber = "09331234574",
                        Email = "jaime.marquez@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = false
                    },
                    new Customer
                    {
                        FirstName = "Zenaida",
                        LastName = "Delos Santos",
                        Address = "40 P. Tuazon Blvd., Quezon City, Metro Manila",
                        ContactNumber = "09269876550",
                        Email = "zenaida.delossantos@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Teodoro",
                        LastName = "Ignacio",
                        Address = "41 Kalayaan Ave., Makati City, Metro Manila",
                        ContactNumber = "09171234576",
                        Email = "teodoro.ignacio@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Aurora",
                        LastName = "Estrella",
                        Address = "42 Timog Ave., Quezon City, Metro Manila",
                        ContactNumber = "09189876552",
                        Email = "aurora.estrella@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Isabelo",
                        LastName = "Luna",
                        Address = "43 Mabini St., Batangas City, Batangas",
                        ContactNumber = "09279876553",
                        Email = "isabelo.luna@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Remedios",
                        LastName = "Concepcion",
                        Address = "44 National Highway, San Pablo City, Laguna",
                        ContactNumber = "09331234575",
                        Email = "remedios.concepcion@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Isidro",
                        LastName = "Romero",
                        Address = "45 A. Bonifacio Ave., Cainta, Rizal",
                        ContactNumber = "09269876551",
                        Email = "isidro.romero@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Florencia",
                        LastName = "Espiritu",
                        Address = "46 E. Rodriguez Sr. Ave., Antipolo City, Rizal",
                        ContactNumber = "09171234577",
                        Email = "florencia.espiritu@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = false
                    },
                    new Customer
                    {
                        FirstName = "Benito",
                        LastName = "Enriquez",
                        Address = "47 J.P. Rizal St., Biñan City, Laguna",
                        ContactNumber = "09189876553",
                        Email = "benito.enriquez@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Consolacion",
                        LastName = "Villanueva",
                        Address = "48 Marcos Highway, Taytay, Rizal",
                        ContactNumber = "09279876554",
                        Email = "consolacion.villanueva@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Benjamin",
                        LastName = "Perez",
                        Address = "49 Circumferential Road, Bacoor City, Cavite",
                        ContactNumber = "09331234576",
                        Email = "benjamin.perez@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    },
                    new Customer
                    {
                        FirstName = "Natividad",
                        LastName = "Samson",
                        Address = "50 General Trias Drive, Imus City, Cavite",
                        ContactNumber = "09269876552",
                        Email = "natividad.samson@example.com",
                        DateCreated = date,
                        ModifiedBy = "",
                        DateModified = date,
                        IsActive = true
                    }
                };

                // Add all customers to the database
                await context.Customers.AddRangeAsync(customer1, customer2, customer3);
                await context.Customers.AddRangeAsync(additionalCustomers.ToArray());
                await context.SaveChangesAsync();
            
        }
        public static async Task Run(IServiceProvider serviceProvider) => await new CustomerSeeder().Seed(serviceProvider);
    }
}