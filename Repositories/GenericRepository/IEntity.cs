namespace GenericRepository;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}