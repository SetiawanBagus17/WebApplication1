namespace WebApplication1.Models
{
    public class UserModel
    {
        public List<TaskModel> tasks { get; set; }
        public int pk_users_id { get; set; }
        public string? name { get; set; }

    }
}
