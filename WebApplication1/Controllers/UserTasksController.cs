using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class UserTasksController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserTasksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("AddUserTask")]

        public IActionResult AddUserTask([FromBody] InputModel? input)
        {
            using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                string queryInsetUser = "Insert Into Users (`name`) Values('" + input.name + "'); SELECT LAST_INSERT_ID();";
                MySqlCommand cmd = new MySqlCommand(queryInsetUser, conn);

                var insertedID = cmd.ExecuteScalar();
                foreach (TaskModel cModel in input.tasks)
                {
                    string queryInsertCourse = "Insert Into Tasks (`task_detail`, `fk_user_id`) Values ('" + cModel.task_detail + "', " + insertedID + ")";
                    cmd = new MySqlCommand(queryInsertCourse, conn);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return Ok("Item Added");
            }
        }

        [HttpGet]
        [Route("GetTasks")]
        public IActionResult GetTasks()
        {
            List<UserModel> users = new List<UserModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();

                    string query = @"
                        SELECT u.pk_users_id, u.name, t.pk_tasks_id, t.task_detail
                        FROM Users u
                        LEFT JOIN Tasks t ON u.pk_users_id = t.fk_user_id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int currentUserId = 0;
                        UserModel currentUser = null;

                        while (reader.Read())
                        {
                            int userId = reader.GetInt32("pk_users_id");

                            if (userId != currentUserId)
                            {
                                currentUser = new UserModel
                                {
                                    pk_users_id = userId,
                                    name = reader.GetString("name"),
                                    tasks = new List<TaskModel>()
                                };
                                users.Add(currentUser);
                                currentUserId = userId;
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("pk_tasks_id")))
                            {
                                currentUser.tasks.Add(new TaskModel
                                {
                                    pk_tasks_id = reader.GetInt32("pk_tasks_id"),
                                    task_detail = reader.GetString("task_detail")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }

            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserWithTask")]
        public IActionResult GetTasks(string? name)
        {
            List<UserModel> users = new List<UserModel>();

            if (string.IsNullOrEmpty(name)) {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        conn.Open();

                        string query = @"
                        SELECT u.pk_users_id, u.name, t.pk_tasks_id, t.task_detail
                        FROM Users u
                        LEFT JOIN Tasks t ON u.pk_users_id = t.fk_user_id";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int currentUserId = 0;
                            UserModel currentUser = null;

                            while (reader.Read())
                            {
                                int userId = reader.GetInt32("pk_users_id");

                                if (userId != currentUserId)
                                {
                                    currentUser = new UserModel
                                    {
                                        pk_users_id = userId,
                                        name = reader.GetString("name"),
                                        tasks = new List<TaskModel>()
                                    };
                                    users.Add(currentUser);
                                    currentUserId = userId;
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("pk_tasks_id")))
                                {
                                    currentUser.tasks.Add(new TaskModel
                                    {
                                        pk_tasks_id = reader.GetInt32("pk_tasks_id"),
                                        task_detail = reader.GetString("task_detail")
                                    });
                                }
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
                }

                return Ok(users);
            } else
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        conn.Open();

                        string query = @"
                        SELECT u.pk_users_id, u.name, t.pk_tasks_id, t.task_detail
                        FROM Users u
                        LEFT JOIN Tasks t ON u.pk_users_id = t.fk_user_id
                        WHERE u.name = @userName";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@userName", name);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int currentUserId = 0;
                            UserModel currentUser = null;

                            while (reader.Read())
                            {
                                int userId = reader.GetInt32("pk_users_id");

                                if (userId != currentUserId)
                                {
                                    currentUser = new UserModel
                                    {
                                        pk_users_id = userId,
                                        name = reader.GetString("name"),
                                        tasks = new List<TaskModel>()
                                    };
                                    users.Add(currentUser);
                                    currentUserId = userId;
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("pk_tasks_id")))
                                {
                                    currentUser.tasks.Add(new TaskModel
                                    {
                                        pk_tasks_id = reader.GetInt32("pk_tasks_id"),
                                        task_detail = reader.GetString("task_detail")
                                    });
                                }
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
                }
                return Ok(users);
            }
        }
    }
}
