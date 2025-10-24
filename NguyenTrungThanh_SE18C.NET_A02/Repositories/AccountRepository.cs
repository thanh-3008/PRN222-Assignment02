using BusinessObjects;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FUNewsManagementDbContext _context;
        public AccountRepository(FUNewsManagementDbContext context) { _context = context; }

        public SystemAccount? GetAccountByEmail(string email) => _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email);

        public List<SystemAccount> GetAccounts(string? SearchTerm = null)
        {
            var query = _context.SystemAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(a =>
                    (a.AccountName != null && a.AccountName.Contains(SearchTerm)) ||
                    (a.AccountEmail != null && a.AccountEmail.Contains(SearchTerm))
                );
            }

            return query.ToList();
        }

        public void CreateAccount(SystemAccount account) { _context.SystemAccounts.Add(account); _context.SaveChanges(); }
        public void UpdateAccount(SystemAccount account) { _context.SystemAccounts.Update(account); _context.SaveChanges(); }
        public void DeleteAccount(SystemAccount account) { _context.SystemAccounts.Remove(account); _context.SaveChanges(); }
        public SystemAccount? GetAccountById(short id) => _context.SystemAccounts.Find(id);
    }
}