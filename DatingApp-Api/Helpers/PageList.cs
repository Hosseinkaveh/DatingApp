using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Helpers
{
    public class PageList<T> : List<T>
    {
        public PageList(IEnumerable<T> item,int currentPage,int totalCount,int pageSize)
        {
            this.PageNumber = currentPage;
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
            this.TotalPage = (int)Math.Ceiling(totalCount / (double) pageSize);
            AddRange(item);
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }


        public static async Task<PageList<T>> CreateAsync(IQueryable<T> query,
        int pageSize,int pageNumber)
        {
            int count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items,pageNumber,count,pageSize);
        }
        
    }
}