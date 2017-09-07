using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;

namespace ASTRA.EMSG.Business.Services.Identity
{
    public class IdentityServiceFake : IIdentityService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public IdentityServiceFake(ITransactionScopeProvider transactionScopeProvider)
        {
            this.transactionScopeProvider = transactionScopeProvider;
        }

        public bool IsUserExists(string username)
        {
            return transactionScopeProvider.Queryable<TestUserInfo>().Any(tui => tui.UserName == username);
        }
        public List<UserRole> GetRole(string username, string applicationname)
        {
            return transactionScopeProvider.Queryable<TestUserInfo>().Where(tui => tui.UserName == username).Select(tui => new UserRole { MandatorName = tui.Mandator, Rolle = tui.Rolle }).ToList();
        }

        public List<UserRole> GetRoleMandator(string mandatorname, string username, string applicationname)
        {
            return GetTestUserInfosForMandantAndUserName(mandatorname, username)
                .ToList()
                .Select(tui => new UserRole { MandatorName = applicationname, Rolle = tui.Rolle })
                .ToList();
        }

        public List<IdentityMandatorShort> GetMandatorShort(string username, string applicationname)
        {
            var ldapMandatorShorts = GetTestUserInfosForUserName(username)
                .Select(tui => new IdentityMandatorShort { Applicationname = applicationname, Mandatorname = tui.Mandator }).ToArray()
                .Distinct(new LambdaComparer<IdentityMandatorShort>((x, y) => x.Mandatorname == y.Mandatorname));

            List<IdentityMandatorShort> mandatorShorts = ldapMandatorShorts.OrderBy(lms => lms.Mandatorname).ToList();

            return mandatorShorts;
        }
        public IQueryable<TestUserInfo> GetTestUserInfosForUserName(string userName)
        {
            return transactionScopeProvider.Queryable<TestUserInfo>().Where(tui => tui.UserName == userName);
        }

        public IQueryable<TestUserInfo> GetTestUserInfosForMandant(string mandantName)
        {
            return transactionScopeProvider.Queryable<TestUserInfo>().Where(tui => tui.Mandator == mandantName);
        }

        public IQueryable<TestUserInfo> GetTestUserInfosForMandantAndUserName(string mandantName, string userName)
        {
            return transactionScopeProvider.Queryable<TestUserInfo>().Where(tui => tui.Mandator == mandantName && tui.UserName == userName);
        }
    }

    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _lambdaComparer;
        private readonly Func<T, int> _lambdaHash;

        public LambdaComparer(Func<T, T, bool> lambdaComparer) :
            this(lambdaComparer, o => 0)
        {
        }

        public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
        {
            if (lambdaComparer == null)
                throw new ArgumentNullException("lambdaComparer");
            if (lambdaHash == null)
                throw new ArgumentNullException("lambdaHash");

            _lambdaComparer = lambdaComparer;
            _lambdaHash = lambdaHash;
        }

        public bool Equals(T x, T y)
        {
            return _lambdaComparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _lambdaHash(obj);
        }
    }
}
