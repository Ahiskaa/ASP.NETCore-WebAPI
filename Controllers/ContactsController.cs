using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller // Contacts Controller olusturuldu
    {
        // DbContext enjekte edildi
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GetContact metodu olusturuldu -> Bir adet nesne donecek
        [HttpGet]
        [Route("/GetContact/{ID:guid}")] // --> Guid olarak erisim saglayacak
        public async Task<IActionResult> GetContact([FromRoute] Guid ID)
        {
            var contact = await dbContext.Contacts.FindAsync(ID); // ID ile erisim icin arama yapilacak

            if (contact == null) // Nesne yok ise 404 donecegiz
            {
                return NotFound(); 
            }

            return Ok(contact); // var ise nesneyi donecegiz
        }



        // GetContacts metodu olusturuldu -> Kisiler List olarak donecek
        [HttpGet("/GetAllContacts")]
        public async Task<IActionResult> GetAllContacts() 
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }



        // AddContact metodu olusturuldu
        [HttpPost("/AddContacts")]
        public async Task<IActionResult> AddContacts(AddContactRequest addContactRequest)
        {
            var contact = new Contact() // Contact'tan instance olusturuldu
            {
                ID = Guid.NewGuid(), // -> Otomatik verildi
                Fullname = addContactRequest.Fullname, // Request'ten gelen alanlar esitlendi
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                Addess = addContactRequest.Addess
            };

            await dbContext.Contacts.AddAsync(contact); // dbContext'e eklendi
            await dbContext.SaveChangesAsync(); // Db'ye islendi --> Islem sonlandirildi

            return Ok(contact); // Nesne kullaniciya return edildi
        }



        // UpdateContact metodu olusturuldu
        [HttpPut]
        [Route("/UpdateContact/{ID:guid}")] // --> Guid olarak erisim saglayacak
        public async Task<IActionResult> UpdateContact([FromRoute] Guid ID, UpdateContactRequeest updateContactRequeests )
        {
            // DbContext'ten ID ile Contact nesnemize ulasmaya calisiyoruz
            var contact = await dbContext.Contacts.FindAsync(ID); 

            // Nesne var ise update ediyoruz
            if (contact != null)
            {
                contact.Fullname = updateContactRequeests.Fullname;
                contact.Addess = updateContactRequeests.Addess;
                contact.Phone = updateContactRequeests.Phone;
                contact.Email = updateContactRequeests.Email;

                await dbContext.SaveChangesAsync(); // update islemini sonlandiriyoruz -> db'ye isliyoruz

                return Ok(contact); // Nesnenin Update halini donuyoruz -> gostermek icin
            }

            // Nesne yok ise 404 hatasi donuyoruz
            return NotFound();
        }




        // DeleteContact metodu olusturuldu
        [HttpDelete]
        [Route("/DeleteContact/{ID:guid}")] // --> Guid olarak erisim saglayacak
        public async Task<IActionResult> DeleteContact([FromRoute] Guid ID)
        {
            // Nesneye erisim saglamaya calisiyoruz
            var contact = await dbContext.Contacts.FindAsync(ID);

            // Nesne erisim saglanirsa/ var ise -> remove islemi yapiyoruz
            if (contact != null)
            {
                // Remove islemini isliyoruz -> sonlandiriyoruz ve db'ye isliyoruz
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync(); 
                return Ok(contact);
            }

            // Nesne yok ise 404 hatasi donuyoruz
            return NotFound();


        }
    }
}
