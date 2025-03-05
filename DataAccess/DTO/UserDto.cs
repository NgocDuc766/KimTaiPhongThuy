namespace KimTaiPhongThuy.DataAccess.DTO
{

        public class UserDto
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string ContactName { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public bool Gender { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string PhoneNumber { get; set; }
        }
   
}
