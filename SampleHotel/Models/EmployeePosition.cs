namespace SampleHotel.Models
{
    public class EmployeePosition : Enumeration
    {
        public static EmployeePosition Waiter = new EmployeePosition(1, "Waiter");
        public static EmployeePosition Waitress = new EmployeePosition(2, "Waitress");
        public static EmployeePosition Chef = new EmployeePosition(3, "Chef");
        public static EmployeePosition Cleaner = new EmployeePosition(4, "Cleaner");

        public EmployeePosition(int id, string name) : base(id, name)
        {
        }
    }
}
