namespace Domain.Shared.Common;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }

    public PagedList(IList<T> items, long count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData(pageNumber, pageSize, count);

        AddRange(items);
    }

    public PagedList(IList<T> items, MetaData metaData)
    {
        MetaData = metaData;
        AddRange(items);
    }
}
