﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
  
    public interface IRepositoryBase<T> where T : class
    {
     
        Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;


      
        Task<List<T>> ListAsync(CancellationToken cancellationToken = default);


  
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

       
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

       
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }



}

