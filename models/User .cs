namespace Task.models
{
    public class taskUser
    {
        public long UserId {get;set;}
        public string Username { get; set; }

        public string Password { get; set; }

        public bool TaskManager {get;set;}
    }
}
