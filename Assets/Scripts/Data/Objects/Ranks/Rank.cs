namespace Data.Objects.Ranks
{
    public sealed class Rank
    {
        public string Id { get; }
        public int Min { get; }
        public int Max { get; }

        public Rank(string id, int min, int max)
        {
            Id = id;
            Min = min;
            Max = max;
        }

        public override string ToString() => $"{Id} [{Min}, {Max}]";
    }
}