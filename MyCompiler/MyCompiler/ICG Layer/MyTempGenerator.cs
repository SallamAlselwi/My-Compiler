
namespace MyCompiler.ICG
{
    public class MyTempGenerator
    {
        private int counter = 0;

        public string NewTemp()
        {
            counter++;

            return $"t{counter}";
        }

        public void Reset()
        {
            counter = 0;
        }
    }
}