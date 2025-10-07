using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FiapCloudGames.Games.Infrastructure.ElasticSearch.Models;
using System.Linq.Expressions;
using Query = Elastic.Clients.Elasticsearch.QueryDsl.Query;

namespace FiapCloudGames.Games.Infrastructure.ElasticSearch;

public interface IElasticClient<T>
{
    Task<bool> DeleteByIdAsync(int id);
    Task<IReadOnlyCollection<T>> GetWithComplexFiltersSearchAsync(int page,
        int size,
        Func<QueryDescriptor<T>, ICollection<Query>>? queries = default,
        Expression<Func<T, object>>? orderBy = default,
        SortOrder? sortOrder = default
    );
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Hit<T>>> SearchWithMostMatchQueryAsync(string field, string mostMatchedValue, int page, int size);
    Task<bool> UpinsertAsync(T entity);
}

public class ElasticClient<T>(ElasticsearchClient elasticsearchClient) : IElasticClient<T> where T : ElasticSearchModel
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
    private readonly string _indexName = typeof(T).Name.Replace(nameof(ElasticSearchModel), string.Empty).ToLower();

    public async Task<bool> DeleteByIdAsync(int id)
    {
        DeleteResponse response = await _elasticsearchClient.DeleteAsync<T>(id, d => d.Index(_indexName));
        return response.Result is Result.Deleted;
    }

    public async Task<IReadOnlyCollection<T>> GetWithComplexFiltersSearchAsync(
        int page,
        int size,
        Func<QueryDescriptor<T>, ICollection<Query>>? queries = default,
        Expression<Func<T, object>>? orderBy = default,
        SortOrder? sortOrder = default
    )
    {
        SearchRequestDescriptor<T> searchRequestDescriptor = new(_indexName);

        if (queries is not null)
            searchRequestDescriptor.Query(q => q.Bool(b => b.Must(mq => queries(mq))));

        if (orderBy is not null && sortOrder is not null)
            searchRequestDescriptor.Sort(sort => sort.Field(orderBy!, sortOrder.Value));

        searchRequestDescriptor.From((page - 1) * size).Size(size);

        SearchResponse<T> response = await _elasticsearchClient.SearchAsync<T>(searchRequestDescriptor);
        return response.Documents;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        GetResponse<T> response = await _elasticsearchClient.GetAsync<T>(id, i => i.Index(_indexName));
        return response.Source;
    }

    public async Task<IReadOnlyCollection<Hit<T>>> SearchWithMostMatchQueryAsync(string field, string mostMatchedValue, int page, int size)
    {
        SearchResponse<T> response = await _elasticsearchClient.SearchAsync<T>(_indexName, s => s
            .From((page - 1) * size)
            .Size(size)
            .Query(q => q.Term(t => t
                .Field(field)
                .Value(mostMatchedValue)
                .CaseInsensitive(true)
            )));
        return response.Hits;
    }

    public async Task<bool> UpinsertAsync(T entity)
    {
        IndexResponse response = await _elasticsearchClient.IndexAsync(entity, i => i.Index(_indexName).Id(entity.Id));
        return response.IsValidResponse;
    }
}