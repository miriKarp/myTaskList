
namespace Task.models{

    public class task{
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public long taskUserId { get; set; }
    }
}