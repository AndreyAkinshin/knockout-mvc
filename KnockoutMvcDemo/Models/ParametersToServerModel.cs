namespace KnockoutMvcDemo.Models
{
    public class ParametersToServerModel
    {
        public int Number { get; set; }
        public string String { get; set; }

        public void Increment(int value)
        {
            Number += value;
        }

        public void AddToString(string s, int count)
        {
            for (int i = 0; i < count; i++)
                String += s;
        }
    }
}