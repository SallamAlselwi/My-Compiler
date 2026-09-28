
namespace MyCompiler.ICG
{
    public class MyLabelGenerator
    {
        private int counter = 0;

        public string NewLabel()
        {
            counter++;

            return $"L{counter}";
        }

        public void Reset()
        {
            counter = 0;
        }
    }
}