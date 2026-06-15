using AspNetCoreMongoApi.Entities;

namespace AspNetCoreMongoApi.Extensions
{
    public static class TodoFilteringExtensions
    {
        //IQueryable is immutable - methods like Where() don't modify the original query. They return a new IQueryable with the filter applied.
        public static IQueryable<Todo> AddFilters(
            this IQueryable<Todo> todosQuery,
            DateTime? minTo = null,
            DateTime? maxTo = null,
            DateTime? minFrom = null,
            DateTime? maxFrom = null,
            DateTime? minCreatedAt = null,
            DateTime? maxCreatedAt = null,
            DateTime? minUpdatedAt = null,
            DateTime? maxUpdatedAt = null,
            string? searchTerm = null)
        {
            if (minTo != null)
            {
                todosQuery = todosQuery.Where(w => w.To >= minTo);
            }

            if (maxTo != null)
            {
                todosQuery = todosQuery.Where(w => w.To <= maxTo);
            }


            if (minFrom != null)
            {
                todosQuery = todosQuery.Where(w => w.From >= minFrom);
            }

            if (maxFrom != null)
            {
                todosQuery = todosQuery.Where(w => w.From <= maxFrom);
            }


            if (minCreatedAt != null)
            {
                todosQuery = todosQuery.Where(w => w.CreatedAt >= minCreatedAt);
            }

            if (maxCreatedAt != null)
            {
                todosQuery = todosQuery.Where(w => w.CreatedAt<= maxCreatedAt);
            }


            if (minUpdatedAt != null)
            {
                todosQuery = todosQuery.Where(w => w.UpdatedAt >= minUpdatedAt);
            }

            if (maxUpdatedAt != null)
            {
                todosQuery = todosQuery.Where(w => w.UpdatedAt <= maxUpdatedAt);
            }


            if (searchTerm != null)
            {
                todosQuery = todosQuery.Where(w =>(w.Title.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||(w.Description != null && w.Description.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))));
            }

            return todosQuery;
        }
    }
}
