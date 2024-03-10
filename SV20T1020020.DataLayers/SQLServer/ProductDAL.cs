using SV20T1020020.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Azure;
using System.Buffers;

namespace SV20T1020020.DataLayers.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into Products(ProductName,ProductDescription,SupplierID,CategoryID,Unit,Price,Photo, IsSelling)
                                        values(@ProductName,@ProductDescription,@SupplierID,@CategoryID,@Unit,@Price,@Photo, @IsSelling);
                                        select @@identity;
                                  ";
                var parameters = new
                {
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierId,
                    CategoryID = data.CategoryId,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into ProductAttributes(ProductId, AttributeName,AttributeValue,DisplayOrder)
                                        values(@ProductId,@AttributeName,@AttributeValue,@DisplayOrder);
                                        select @@identity;
                                  ";
                var parameters = new
                {
                    ProductId = data.ProductId,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            long id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into ProductPhotos(ProductId,Photo,Description,DisplayOrder,IsHidden)
                                        values(@ProductId,@Photo,@Description,@DisplayOrder,@IsHidden);
                                        select @@identity;
                                 ";
                var parameters = new
                {
                    ProductId = data.ProductId,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };
                id = connection.ExecuteScalar<long>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*) from Products 
                                  where (@searchValue = N'') or (ProductName like @searchValue)";
                var parameters = new
                {
                    searchValue = searchValue ?? "",
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int productId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Products where ProductId = @ProductId";
                var parameters = new
                {
                    ProductId = productId,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteAttribute(long attributeId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes where AttributeId = @AttributeId";
                var parameters = new
                {
                    AttributeId = attributeId,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos where PhotoId = @PhotoId";
                var parameters = new
                {
                    PhotoId = photoId,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productId)
        {
            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Products where ProductId = @ProductId";
                var parameters = new
                {
                    ProductId = productId
                };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long attributeId)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes where AttributeId = @AttributeId";
                var parameters = new
                {
                    AttributeId = attributeId
                };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long photoId)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos where PhotoId = @PhotoId";
                var parameters = new
                {
                    PhotoId = photoId
                };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool IsUsed(int productId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from OrderDetails where ProductId = @ProductId)
                                    select 1
                                else 
                                    select 0";
                var parameters = new
                {
                    ProductId = productId
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data = new List<Product>();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                                (
                                    select  *,
                                            row_number() over(order by ProductName) as RowNumber
                                    from    Products
                                    where   (@SearchValue = N'' or ProductName like @SearchValue)
                                        and (@CategoryID = 0 or CategoryID = @CategoryID)
                                        and (@SupplierID = 0 or SupplierId = @SupplierID)
                                        and (Price >= @MinPrice)
                                        and (@MaxPrice <= 0 or Price <= @MaxPrice)
                                )
                                select * from cte
                                where   (@PageSize = 0)
                                    or (RowNumber between (@Page - 1)*@PageSize + 1 and @Page * @PageSize)";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue ?? "",
                    categoryId = categoryId,
                    supplierId = supplierId,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };
                data = connection.Query<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }

            return data;
        }

        public IList<ProductAttribute> ListAttributes(int productId)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes where productId = @productId 
                                Order by DisplayOrder ASC";
                var parameters = new
                {
                    ProductId = productId
                };
                data = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }

            return data;
        }

        public IList<ProductPhoto> ListPhotos(int productId)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos where productId = @productId
                                Order by DisplayOrder ASC";
                var parameters = new
                {
                    ProductId = productId
                };
                data = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }

            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Products 
                                        set ProductName = @ProductName,
                                            ProductDescription = @ProductDescription,
                                            SupplierID = @SupplierID,
                                            CategoryID = @CategoryID,
                                            Unit = @Unit,
                                            Price = @Price,
                                            Photo = @Photo,
                                            IsSelling = @IsSelling
                                        where ProductId = @ProductId
                                   ";
                var parameters = new
                {
                    ProductId = data.ProductId,
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierId,
                    CategoryID = data.CategoryId,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            };
            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update ProductAttributes 
                                        set AttributeName = @AttributeName,
                                            AttributeValue = @AttributeValue,
                                            DisplayOrder = @DisplayOrder
                                        where AttributeID = @AttributeID
                                  ";
                var parameters = new
                {
                    AttributeID = data.AttributeId,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            };
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update ProductPhotos 
                                        set Photo = @Photo,
                                            Description = @Description,
                                            DisplayOrder = @DisplayOrder,
                                            IsHidden = @IsHidden
                                        where PhotoID = @PhotoID
                                  ";
                var parameters = new
                {
                    PhotoID = data.PhotoId,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            };
            return result;
        }
    }
}
