using CRM_App.Models;
using CRM_App.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CRM_App.Data
{
    public class DatabaseHelper
    {
        public static async Task InitializeDatabase()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            if (!File.Exists(databasePath))
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CRM_App.Resources.CRM.db3"))
                {
                    if (stream != null)
                    {
                        using (var fileStream = new FileStream(databasePath, FileMode.Create, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
            }

            using (var dbContext = new AppDbContext(databasePath))
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
        }

        #region User

        public static async Task<int> AddUserAsync(User user)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            dbContext.Users.Add(user);
            return await dbContext.SaveChangesAsync();
        }
        public static async Task<User> GetUserByUsernameAsync(string username)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            return await dbContext.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
        }
        public static async Task<int> DeleteUserAsync(int userId)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            var user = await dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                return await dbContext.SaveChangesAsync();
            }

            return 0;
        }
        public static async Task<int> UpdateUserPasswordAsync(int userId, string newPassword)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            var user = await dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.Password = newPassword;
                dbContext.Users.Update(user);
                return await dbContext.SaveChangesAsync();
            }

            return 0;
        }

        #endregion

        #region Customer

        public static async Task<int> AddCustomerAsync(Customer customer)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            //check if customer exists before adding
            var existingCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Name == customer.Name && c.Email == customer.Email);
            if (existingCustomer != null)
            {
                string message = "Acest client exista deja!" + Environment.NewLine + Environment.NewLine
                                 + $"Nume: existingCustomer.Name" + Environment.NewLine
                                 + $"Email: existingCustomer.Email" + Environment.NewLine
                                 + $"Telefon: existingCustomer.Phone" + Environment.NewLine
                                 + $"Adresa: existingCustomer.Address";
                await Shell.Current.DisplayAlert("Eroare", "Acest client exista deja!", "Ok");
                return 0;
            }

            dbContext.Customers.Add(customer);
            return await dbContext.SaveChangesAsync();
        }
        public static async Task<List<Customer>> GetCustomersByNameAsync(string customerName)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            return await dbContext.Customers.Where(c=>c.Name.ToLower().Contains(customerName.ToLower().Trim())).ToListAsync();
        }
        public static async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            return await dbContext.Customers.FindAsync(customerId);
        }
        public static async Task<int> UpdateCustomerAsync(Customer customer)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            dbContext.Customers.Update(customer);
            return await dbContext.SaveChangesAsync();
        }
        public static async Task<int> DeleteCustomerAsync(int customerID)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
            using var dbContext = new AppDbContext(databasePath);
            var customer = await dbContext.Customers.FindAsync(customerID);
            if (customer != null)
            {
                dbContext.Customers.Remove(customer);
                return await dbContext.SaveChangesAsync();
            }

            return 0;
        }

        #endregion

        #region Admin

        public static async Task<List<User>> GetAllUsersAsync()
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Users.ToListAsync();
            }
        }
        public static async Task<int> ResetUserPasswordAsync(int userId, string newPassword)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserID == userId);
                if (user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); 
                    dbContext.Users.Update(user);
                    return await dbContext.SaveChangesAsync();
                }

                return 0; 
            }
        }
        public static async Task<List<Product>> GetAllProductsAsync()
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Product.ToListAsync();
            }
        }
        public static async Task<int> AddProductAsync(Product product)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Product.Add(product);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static async Task<int> UpdateProductAsync(Product product)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Product.Update(product);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static async Task<int> DeleteProductAsync(int productId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                var product = await dbContext.Product.FirstOrDefaultAsync(p => p.ProductID == productId);
                if (product != null)
                {
                    dbContext.Product.Remove(product);
                    return await dbContext.SaveChangesAsync();
                }

                return 0;
            }
        }
        public static async Task<Product> GetProductByIdAsync(int productId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Product.FirstOrDefaultAsync(p => p.ProductID == productId);
            }
        }

        #endregion

        #region Sale

        public static async Task<List<Status>> GetAllStatusesAsync()
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Statuses.ToListAsync();
            }
        }
        public static async Task<Product> GetProductByNameAsync(string productName)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Product.FirstOrDefaultAsync(p => p.Name == productName);
            }
        }
        public static async Task<Status> GetStatusByNameAsync(string statusName)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Statuses.FirstOrDefaultAsync(s=>s.StatusName == statusName);
            }
        }
        public static async Task<int> AddSaleAsync(Sale sale)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Sales.Add(sale);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static Sale GetSaleById(int saleId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return dbContext.Sales.FirstOrDefault(s => s.SaleID == saleId);
            }
        }
        public static async Task<int> UpdateSaleAsync(Sale sale)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Sales.Update(sale);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static async Task<List<Sale>> GetAllSalesForCustomerAsync(int customerID)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Sales.Where(s=>s.CustomerID == customerID && s.Status=="finalizat").ToListAsync();
            }
        }
        public static async Task<List<Sale>> GetAllSalesForUserAsync(int userId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Sales.Where(s => s.UserID == userId && (s.Status == "in desfasurare" || s.Status == "anulat")).OrderByDescending(s => s.Status).ToListAsync();
            }
        }
        public static async Task<List<Sale>> GetAllCompletedSalesForCustomerAsync(int customerID)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Sales.Where(s => s.CustomerID == customerID && s.Status=="completed").ToListAsync();
            }
        }
        public static async Task<int> DeleteSaleAsync(int saleId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                var sale = await dbContext.Sales.FirstOrDefaultAsync(s => s.SaleID == saleId);
                if (sale != null)
                {
                    dbContext.Sales.Remove(sale);
                    return await dbContext.SaveChangesAsync();
                }

                return 0;
            }
        }
        public static async Task<int> DeleteSalesAsync(List<Sale> salesList)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Sales.RemoveRange(salesList);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static async Task<List<SaleReportLine>> GetSalesReportForCustomer(int customerID)
        {
            List<SaleReportLine> list = new List<SaleReportLine>();
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                string customerName = dbContext.Customers.Where(c => c.CustomerID == customerID).FirstOrDefault().Name;
                var salesList =  await GetAllSalesForCustomerAsync(customerID);
                if (salesList.Count>0)
                {
                    foreach (var sale in salesList)
                    {
                        string productName = dbContext.Product.Where(p => p.ProductID == sale.ProductID).FirstOrDefault().Name;
                        list.Add(new SaleReportLine
                        {
                            SaleID = sale.SaleID,
                            CustomerName = customerName,
                            ProductName = productName,
                            TotalAmount = sale.TotalAmount,
                            Status = sale.Status,
                            Description = sale.Description,
                        });
                    }
                }
            }
            return list;
        }
        public static async Task<List<SaleReportLine>> GetSalesReportForUser(int userId)
        {
            List<SaleReportLine> list = new List<SaleReportLine>();
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                var salesList = await GetAllSalesForUserAsync(userId);
                if (salesList.Count > 0)
                {
                    foreach (var sale in salesList)
                    {
                        string customerName = dbContext.Customers.Where(c => c.CustomerID == sale.CustomerID).FirstOrDefault().Name;
                        string productName = dbContext.Product.Where(p => p.ProductID == sale.ProductID).FirstOrDefault().Name;
                        list.Add(new SaleReportLine
                        {
                            SaleID = sale.SaleID,
                            CustomerName = customerName,
                            ProductName = productName,
                            TotalAmount = sale.TotalAmount,
                            Status = sale.Status,
                            Description = sale.Description,
                        });
                    }
                }
            }
            return list;
        }

        #endregion

        #region Request

        public static async Task<int> AddRequestAsync(Request request)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Requests.Add(request);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static async Task<List<Request>> GetAllRequestsForCustomerAsync(int customerID)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Requests.Where(r => r.CustomerID == customerID).ToListAsync();
            }
        }
        public static async Task<List<Request>> GetAllRequestsForUserAsync(int userId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Requests.Where(r => r.UserID == userId).ToListAsync();
            }
        }
        public static async Task<int> DeleteRequestAsync(int requestId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                var request = await dbContext.Requests.FirstOrDefaultAsync(r => r.RequestID == requestId);
                if (request != null)
                {
                    dbContext.Requests.Remove(request);
                    return await dbContext.SaveChangesAsync();
                }

                return 0;
            }
        }
        public static async Task<int> DeleteRequestsAsync(List<Request> requestList)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Requests.RemoveRange(requestList);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static Request GetRequestById(int requestId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return dbContext.Requests.FirstOrDefault(r => r.RequestID == requestId);
            }
        }
        public static async Task<int> UpdateRequestAsync(Request request)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Requests.Update(request);
                return await dbContext.SaveChangesAsync();
            }
        }

        #endregion

        #region Comment

        public static async Task<int> AddCommentAsync(Comment comment)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                dbContext.Comments.Add(comment);
                return await dbContext.SaveChangesAsync();
            }
        }
        public static async Task<List<Comment>> GetAllCommentsByRequestId(int requestId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                return await dbContext.Comments.Where(c => c.RequestID == requestId).ToListAsync();
            }
        }
        public static async Task<int> DeleteCommentsAsync(int requestId)
        {
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                var comments = await dbContext.Comments.FirstOrDefaultAsync(c => c.RequestID == requestId);
                if (comments != null)
                {
                    dbContext.Comments.RemoveRange(comments);
                    return await dbContext.SaveChangesAsync();
                }

                return 0;
            }
        }
        public static async Task<List<CommentReportLine>> GetCommentsReportForRequest(int requestId)
        {
            List<CommentReportLine> list = new List<CommentReportLine>();
            using (var dbContext = new AppDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3")))
            {
                var commentsList = await GetAllCommentsByRequestId(requestId);
                if (commentsList.Count > 0)
                {
                    foreach (var comment in commentsList)
                    {
                        string userName = "user neregasit";
                        try
                        {
                            userName = dbContext.Users.Where(u => u.UserID == comment.UserID).FirstOrDefault().Username;
                        }
                        catch
                        {
                        }
                        list.Add(new CommentReportLine
                        {
                            CommentID = comment.CommentID,
                            CommentText = comment.CommentText,
                            UserName = userName,
                            Timestamp=comment.Timestamp
                        });
                    }
                }
            }
            return list;
        }

        #endregion   

    }
}
