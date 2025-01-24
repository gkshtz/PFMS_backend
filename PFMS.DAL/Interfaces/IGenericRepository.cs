using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Interfaces
{
    public interface IGenericRepository<Dto>
        where Dto: class, IIdentifiable
    {
        /// <summary>
        /// Created a new resource
        /// </summary>
        /// <param name="dto">Dto object</param>
        /// <returns></returns>
        public Task AddAsync(Dto dto);

        /// <summary>
        /// Reads the list of all resources
        /// </summary>
        /// <returns>List of Dto objects</returns>
        public Task<List<Dto>> GetAllAsync();

        /// <summary>
        /// Reads a particular resource by its unique Id
        /// </summary>
        /// <param name="id">Unique Id of the recource</param>
        /// <returns>Dto object</returns>
        public Task<Dto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates a resource
        /// </summary>
        /// <param name="dto">Dto object</param>
        /// <returns>Resut of updation in form of boolean</returns>
        public Task<bool> UpdateAsync(Dto dto);

        /// <summary>
        /// Deletes a resource, if it exists
        /// </summary>
        /// <param name="id">Unique Id of the resource</param>
        /// <returns></returns>
        public Task DeleteAsync(Guid id);
    }
}
