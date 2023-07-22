namespace ContactsAPI.Models
{
    public class Contact // Domain model olusturuldu -> Etki alani
    {
        public Guid ID { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Addess { get; set; }
    }
}
