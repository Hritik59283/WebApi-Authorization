﻿namespace CrudAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
