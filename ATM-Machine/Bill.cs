namespace ATM_Machine
{
    public class Bill
    {
        public int Value { get; private set; }


        public Bill(int value)
        {
            Value = value;
        }
    }
}