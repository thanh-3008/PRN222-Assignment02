using BusinessObjects;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void CreateAccount(SystemAccount account)
        {
            _accountRepository.CreateAccount(account);
        }

        public void DeleteAccount(SystemAccount account)
        {
            _accountRepository.DeleteAccount(account);
        }

        public SystemAccount? GetAccountByEmail(string email)
        {
            return _accountRepository.GetAccountByEmail(email);
        }

        public SystemAccount? GetAccountById(short id)
        {
            return _accountRepository.GetAccountById(id);
        }

        public List<SystemAccount> GetAccounts(string? searchTerm = null)
        {
            return _accountRepository.GetAccounts(searchTerm);
        }

        public void UpdateAccount(SystemAccount account)
        {
            _accountRepository.UpdateAccount(account);
        }
    }
}