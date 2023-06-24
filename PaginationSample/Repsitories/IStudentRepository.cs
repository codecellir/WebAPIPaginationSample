using Microsoft.EntityFrameworkCore;
using PaginationSample.Data;
using PaginationSample.Entities;
using PaginationSample.Models;
using System.Linq.Expressions;

namespace PaginationSample.Repsitories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<PagedList<Student>> GetAllAsync(GetStudentQuery request);
    }
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }

        public async Task<PagedList<Student>> GetAllAsync(GetStudentQuery request)
        {
            IQueryable<Student> query = _context.Students;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(d => d.Name.Contains(request.SearchTerm));
            }

            if (request.SortOrder?.ToLower() == "desc")
                query = query.OrderByDescending(GetSortProperty(request));
            else
                query = query.OrderBy(GetSortProperty(request));

            //var result = await query
            //    .Skip((request.Page - 1) * request.PageSize)
            //    .Take(request.PageSize)
            //    .ToListAsync();

            var result = await PagedList<Student>.CreateAsync(query, request.Page, request.PageSize);

            return result;
        }

        private Expression<Func<Student, object>> GetSortProperty(GetStudentQuery request)
            => request.SortColumn?.ToLower() switch
            {
                "name" => Student => Student.Name,
                "gender" => Student => Student.Gender,
                "debtoramount" => Student => Student.DebtorAmount,
                _ => Student => Student.Id,
            };
    }
}
