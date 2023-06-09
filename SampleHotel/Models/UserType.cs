namespace SampleHotel.Models
{
    public class UserType : Enumeration
    {
        public static UserType Admin = new(1, "Admin");
        public static UserType Client = new(2, "Client");

        public UserType(int id, string name) 
            : base(id, name)
        {
        }

    }
}
