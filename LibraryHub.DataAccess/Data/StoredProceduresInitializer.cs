using LibraryHub.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryHub.DataAccess.Data;

/// <summary>
/// Crea o actualiza los procedimientos almacenados requeridos por LibraryHub.
/// </summary>
public static class StoredProceduresInitializer
{
    /// <summary>
    /// Crea o actualiza los SPs usados por consultas paginadas.
    /// </summary>
    /// <param name="context">Contexto de datos de LibraryHub.</param>
    /// <param name="logger">Logger opcional para registrar eventos.</param>
    public static async Task EnsureCreatedAsync(LibraryHubDbContext context, ILogger? logger = null)
    {
        logger?.LogInformation("Stored procedures initialization started.");

        var hasAuthorSoftDelete = await HasColumnAsync(context, "Authors", "IsDeleted");
        var hasBookSoftDelete = await HasColumnAsync(context, "Books", "IsDeleted");

        var authorScript = hasAuthorSoftDelete
            ? GetAuthorsPagedProcedureScriptWithSoftDelete()
            : GetAuthorsPagedProcedureScriptLegacy();

        var bookScript = hasAuthorSoftDelete && hasBookSoftDelete
            ? GetBooksPagedProcedureScriptWithSoftDelete()
            : GetBooksPagedProcedureScriptLegacy();

        await context.Database.ExecuteSqlRawAsync(authorScript);
        await context.Database.ExecuteSqlRawAsync(bookScript);

        logger?.LogInformation(
            "Stored procedures initialization finished. AuthorsSoftDelete: {AuthorsSoftDelete}, BooksSoftDelete: {BooksSoftDelete}",
            hasAuthorSoftDelete,
            hasBookSoftDelete);
    }

    private static async Task<bool> HasColumnAsync(LibraryHubDbContext context, string tableName, string columnName)
    {
        var result = await context.Database.SqlQuery<int>(
            $"""
            SELECT CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = 'dbo'
                      AND TABLE_NAME = {tableName}
                      AND COLUMN_NAME = {columnName}
                ) THEN 1
                ELSE 0
            END AS [Value]
            """).SingleAsync();

        return result == 1;
    }

    private static string GetAuthorsPagedProcedureScriptLegacy()
    {
        return
            """
            CREATE OR ALTER PROCEDURE dbo.sp_Authors_GetPaged
                @PageNumber INT,
                @PageSize INT,
                @SearchTerm NVARCHAR(200) = NULL
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                DECLARE @NormalizedSearch NVARCHAR(200) = NULL;

                IF @SearchTerm IS NOT NULL AND LTRIM(RTRIM(@SearchTerm)) <> ''
                BEGIN
                    SET @NormalizedSearch = LOWER(LTRIM(RTRIM(@SearchTerm)));
                END

                SELECT
                    A.Id,
                    A.FullName,
                    A.BirthDate,
                    A.City,
                    A.Email,
                    COUNT(1) OVER() AS TotalCount
                FROM dbo.Authors AS A
                WHERE @NormalizedSearch IS NULL
                    OR LOWER(A.FullName) LIKE '%' + @NormalizedSearch + '%'
                    OR LOWER(A.City) LIKE '%' + @NormalizedSearch + '%'
                    OR LOWER(A.Email) LIKE '%' + @NormalizedSearch + '%'
                ORDER BY A.Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;
            END;
            """;
    }

    private static string GetAuthorsPagedProcedureScriptWithSoftDelete()
    {
        return
            """
            CREATE OR ALTER PROCEDURE dbo.sp_Authors_GetPaged
                @PageNumber INT,
                @PageSize INT,
                @SearchTerm NVARCHAR(200) = NULL
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                DECLARE @NormalizedSearch NVARCHAR(200) = NULL;

                IF @SearchTerm IS NOT NULL AND LTRIM(RTRIM(@SearchTerm)) <> ''
                BEGIN
                    SET @NormalizedSearch = LOWER(LTRIM(RTRIM(@SearchTerm)));
                END

                SELECT
                    A.Id,
                    A.FullName,
                    A.BirthDate,
                    A.City,
                    A.Email,
                    COUNT(1) OVER() AS TotalCount
                FROM dbo.Authors AS A
                WHERE A.IsDeleted = 0
                    AND (
                        @NormalizedSearch IS NULL
                        OR LOWER(A.FullName) LIKE '%' + @NormalizedSearch + '%'
                        OR LOWER(A.City) LIKE '%' + @NormalizedSearch + '%'
                        OR LOWER(A.Email) LIKE '%' + @NormalizedSearch + '%'
                    )
                ORDER BY A.Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;
            END;
            """;
    }

    private static string GetBooksPagedProcedureScriptLegacy()
    {
        return
            """
            CREATE OR ALTER PROCEDURE dbo.sp_Books_GetPaged
                @PageNumber INT,
                @PageSize INT,
                @SearchTerm NVARCHAR(200) = NULL
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                DECLARE @NormalizedSearch NVARCHAR(200) = NULL;

                IF @SearchTerm IS NOT NULL AND LTRIM(RTRIM(@SearchTerm)) <> ''
                BEGIN
                    SET @NormalizedSearch = LOWER(LTRIM(RTRIM(@SearchTerm)));
                END

                SELECT
                    B.Id,
                    B.Title,
                    B.[Year],
                    B.Genre,
                    B.Pages,
                    B.AuthorId,
                    A.FullName AS AuthorFullName,
                    COUNT(1) OVER() AS TotalCount
                FROM dbo.Books AS B
                INNER JOIN dbo.Authors AS A
                    ON A.Id = B.AuthorId
                WHERE @NormalizedSearch IS NULL
                    OR LOWER(B.Title) LIKE '%' + @NormalizedSearch + '%'
                    OR LOWER(B.Genre) LIKE '%' + @NormalizedSearch + '%'
                    OR LOWER(A.FullName) LIKE '%' + @NormalizedSearch + '%'
                ORDER BY B.Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;
            END;
            """;
    }

    private static string GetBooksPagedProcedureScriptWithSoftDelete()
    {
        return
            """
            CREATE OR ALTER PROCEDURE dbo.sp_Books_GetPaged
                @PageNumber INT,
                @PageSize INT,
                @SearchTerm NVARCHAR(200) = NULL
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                DECLARE @NormalizedSearch NVARCHAR(200) = NULL;

                IF @SearchTerm IS NOT NULL AND LTRIM(RTRIM(@SearchTerm)) <> ''
                BEGIN
                    SET @NormalizedSearch = LOWER(LTRIM(RTRIM(@SearchTerm)));
                END

                SELECT
                    B.Id,
                    B.Title,
                    B.[Year],
                    B.Genre,
                    B.Pages,
                    B.AuthorId,
                    A.FullName AS AuthorFullName,
                    COUNT(1) OVER() AS TotalCount
                FROM dbo.Books AS B
                INNER JOIN dbo.Authors AS A
                    ON A.Id = B.AuthorId
                WHERE B.IsDeleted = 0
                    AND A.IsDeleted = 0
                    AND (
                        @NormalizedSearch IS NULL
                        OR LOWER(B.Title) LIKE '%' + @NormalizedSearch + '%'
                        OR LOWER(B.Genre) LIKE '%' + @NormalizedSearch + '%'
                        OR LOWER(A.FullName) LIKE '%' + @NormalizedSearch + '%'
                    )
                ORDER BY B.Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;
            END;
            """;
    }
}
