namespace SchematBlokowy
{
    public abstract class Entity
    {
        public int Id { get; set; }
    }
    public abstract class Entity<T> where T : class
    {
        public T Id { get; set; }
    }
}
