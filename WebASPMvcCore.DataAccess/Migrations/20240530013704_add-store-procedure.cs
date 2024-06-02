using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebASPMvcCore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addstoreprocedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            AddGetAllProductsStoreProcedure(migrationBuilder);
            AddGetAllOrderStoreProcedure(migrationBuilder);
            AddGetOrderDetailsByIdStoreProcedure(migrationBuilder);
        }
        private void AddGetAllProductsStoreProcedure(MigrationBuilder migrationBuilder)
        {
            string query = $@"
						IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetALLProductByPagination')
						BEGIN
							EXEC('CREATE PROCEDURE GetALLProductByPagination
                                        @keyword NVARCHAR(500),
                                        @categoryId UNIQUEIDENTIFIER = NULL,
                                        @priceMin decimal(18, 2),
                                        @priceMax decimal(18, 2),
                                        @pageIndex INT,
                                        @pageSize INT,
                                        @totalRecords INT OUT
                                    AS
                                    BEGIN
                                        -- Temporary table for pagination
                                        SELECT 
                                            a.Id, 
                                            a.Name, 
                                            a.Price, 
                                            a.Discount, 
                                            a.Quantity, 
                                            a.SoldItem, 
                                            a.Description, 
                                            a.MetaTitle, 
                                            a.MetaKeywords, 
                                            a.MetaDescription, 
                                            a.MetaLink, 
                                            a.MetaImage, 
                                            b.Name AS CategoryName,
                                            b.MetaLink AS CategoryMetaLink,
                                            ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS RowNo
                                        INTO #TempProductPagination
                                        FROM Products a  
                                        JOIN Categories b ON a.CategoryId = b.Id
                                        WHERE (ISNULL(@keyword, '''') = '''' OR b.Name LIKE ''%'' + @keyword + ''%''
                                               OR a.Name LIKE ''%'' + @keyword + ''%''
                                               OR a.MetaKeywords LIKE ''%'' + @keyword + ''%'')
                                          AND (@categoryId IS NULL OR b.Id = @categoryId)
                                          AND (@priceMin IS NULL OR a.Price >= @priceMin)
                                          AND (@priceMax IS NULL OR a.Price <= @priceMax)
                                          AND a.IsActive = 1;

                                        -- Set the total records
                                        SELECT @totalRecords = COUNT(*) 
                                        FROM #TempProductPagination;
    
                                        -- Adjust the page index if it''s zero
                                        IF(@pageIndex = 0)
                                            SET @pageIndex = 1;

                                        -- Select the paginated results
                                        SELECT *
                                        FROM #TempProductPagination
                                        WHERE RowNo BETWEEN ((@pageIndex - 1) * @pageSize + 1) AND (@pageIndex * @pageSize);

                                        -- Drop the temporary table
                                        DROP TABLE #TempProductPagination;
                                    END')
						END
					";

            migrationBuilder.Sql(query, suppressTransaction: true);
        }
        private void AddGetAllOrderStoreProcedure(MigrationBuilder migrationBuilder)
        {
            string query = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetAllOrder')
        BEGIN
            EXEC('
            CREATE PROCEDURE GetAllOrder
            AS
            BEGIN
                SELECT 
                    o.Id,
                    o.CreateOn,
                    o.Province,
                    o.District,
                    o.Town,
                    o.Address,
                    o.TotalAmount,
                    o.PaymentMethod,
                    o.StatusProcessing,
                    u.Fullname
                FROM 
                    Orders o
                JOIN 
                    ApplicationUser u ON o.UserId = u.Id
            END')
        END
    ";

            migrationBuilder.Sql(query, suppressTransaction: true);
        }
        private void AddGetOrderDetailsByIdStoreProcedure(MigrationBuilder migrationBuilder)
        {
            string query = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetOrderDetailsById')
        BEGIN
            EXEC('
            CREATE PROCEDURE GetOrderDetailsById
                @OrderId UNIQUEIDENTIFIER
            AS
            BEGIN
                SELECT 
                    od.OrderId,
                    od.ProductId,
                    od.Quanlity,
                    od.UnitPrice,
                    p.Name as ProductName,
                    p.MetaImage as ProductLink
                FROM
                    OrderDetails od
                JOIN  
                    Products p ON p.Id = od.ProductId
                WHERE
                    od.OrderId = @OrderId;
            END')
        END
    ";

            migrationBuilder.Sql(query, suppressTransaction: true);
        }

    }
}
